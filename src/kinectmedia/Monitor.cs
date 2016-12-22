using KinectEx;
using Microsoft.Kinect;
using cl.uv.leikelen.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace cl.uv.leikelen.src.kinectmedia
{
    public class Monitor
    {
        KinectSensor _sensor = null;
        BodyFrameReader _bodyReader = null;
        ColorFrameReader _colorReader = null;
        AudioBeamFrameReader _audioBeamReader = null;


        ColorFrameBitmap _colorBitmap;
        List<CustomBody> _bodies = null;

        bool _bodyFrameEnable = true;
        bool _colorFrameEnable = true;

        public static PostureType voice = new PostureType("Voz", "");

        public Monitor()
        {
            Open();
        }

        public bool IsOpen
        {
            get
            {
                return _colorReader != null;
            }
        }

        public void Open()
        {
            _colorBitmap = new ColorFrameBitmap();

            this._sensor = KinectMediaFacade.Sensor;
            _bodies = new List<CustomBody>();

            _bodyReader = _sensor.BodyFrameSource.OpenReader();
            
            _bodyReader.FrameArrived += _bodyReader_FrameArrived;

            _colorReader = _sensor.ColorFrameSource.OpenReader();
            _colorReader.FrameArrived += _colorReader_FrameArrived;

            _audioBeamReader = _sensor.AudioSource.OpenReader();
            _audioBeamReader.FrameArrived += _audioBeamReader_FrameArrived;

            MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
        }

        private void _audioBeamReader_FrameArrived(object sender, AudioBeamFrameArrivedEventArgs e)
        {
            System.Console.WriteLine("Frame de audio llego :v");
            if (Scene.Instance == null) return;
            var frames = e.FrameReference.AcquireBeamFrames();

            if (frames == null) return;

            foreach (var frame in frames)
            {
                if (frame == null || frame.SubFrames == null) return;
                foreach (var subFrame in frame.SubFrames)
                {
                    TimeSpan startTime = subFrame.RelativeTime;
                    TimeSpan duration = subFrame.Duration;
                    if (subFrame.AudioBodyCorrelations == null) return;
                    foreach (var audioBodyCorrelation in subFrame.AudioBodyCorrelations)
                    {
                        long bodyTrackingId = (long)audioBodyCorrelation.BodyTrackingId;
                        if (Scene.Instance.Persons.Exists(p => p.TrackingId == bodyTrackingId))
                        {
                            Person pers = Scene.Instance.Persons.Find(p => p.TrackingId == bodyTrackingId);
                            pers.pigVoz.addAudioBeamInterval(KinectMediaFacade.Instance.Recorder.getCurrentLocation().Subtract(duration), duration);

                        }
                    }
                }
            }
            
        }

        public void Close()
        {

            
            _bodies = new List<CustomBody>();

            _bodyReader.FrameArrived -= _bodyReader_FrameArrived;
            _colorReader.FrameArrived -= _colorReader_FrameArrived;

            _bodyReader.Dispose();
            _colorReader.Dispose();

            _bodyReader = null;
            _colorReader = null;

            MainWindow.Instance().colorImageControl.Source = null;
            MainWindow.Instance().bodyImageControl.Source = null;

            _colorBitmap = null;

            this._sensor.Close();
        }


        private void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            if (!_bodyFrameEnable) return;
            IEnumerable<IBody> bodies = null; // to make the GetBitmap call a little cleaner
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    frame.GetAndRefreshBodyData(_bodies);
                    bodies = _bodies;

                    Body[] bodiesInFrame = new Body[frame.BodyCount];
                    frame.GetAndRefreshBodyData(bodiesInFrame);
                    if(Scene.Instance != null)
                    Scene.Instance.calculateDistances.AddBodies(bodiesInFrame, KinectMediaFacade.Instance.Recorder.getCurrentLocation());
                }
            }

            if (bodies != null)
            {
                int i = 0;
                foreach (IBody body in bodies)
                {
                    
                    ulong trackingId = body.TrackingId;
                    if (Scene.Instance != null && KinectMediaFacade.Instance.Recorder.IsRecording)
                    {
                        bool personExists = Scene.Instance.Persons.Exists(p => p.TrackingId == (long)trackingId);
                        if (!personExists && trackingId != 0)
                        {
                            Person person = new Person(
                                    trackingId,
                                    Scene.Instance.Persons.Count
                                );
                            Scene.Instance.Persons.Add(person);
                        }

                        // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        if (trackingId != KinectMediaFacade.Instance.gestureDetectorList[i].TrackingId)
                        {
                            KinectMediaFacade.Instance.gestureDetectorList[i].TrackingId = trackingId;

                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            KinectMediaFacade.Instance.gestureDetectorList[i].IsPaused = trackingId == 0;
                        }
                    }
                    i++;
                }

                bodies.MapDepthPositions();

                MainWindow.Instance().bodyImageControl.Source = bodies.GetBitmap(Colors.LightGreen, Colors.Yellow);
                
                
            }
            else
            {
                MainWindow.Instance().bodyImageControl.Source = null;
            }
        }

        private void _colorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (!_colorFrameEnable) return;

            _colorBitmap.Update(e.FrameReference);
            MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
        }

    }
}
