using KinectEx;
using KinectEx.DVR;
using KinectEx.Smoothing;
using Microsoft.Kinect;
using cl.uv.multimodalvisualizer.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace cl.uv.multimodalvisualizer.core
{
    public class Monitor
    {
        KinectSensor _sensor = null;
        BodyFrameReader _bodyReader = null;
        ColorFrameReader _colorReader = null;
        
        ColorFrameBitmap _colorBitmap;
        List<CustomBody> _bodies = null;

        bool _bodyFrameEnable = true;
        bool _colorFrameEnable = true;

        public Monitor()
        {
            Open();
        }

        //public void Enable()
        //{
        //    _bodyFrameEnable = true;
        //    _colorFrameEnable = true;
        //}

        //public void Disable()
        //{
        //    _bodyFrameEnable = false;
        //    _colorFrameEnable = false;
        //}

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
            //DepthFrameBitmap _depthBitmap = new DepthFrameBitmap();
            //InfraredFrameBitmap _infraredBitmap = new InfraredFrameBitmap();

            this._sensor = Kinect.Sensor;
            _bodies = new List<CustomBody>();

            _bodyReader = _sensor.BodyFrameSource.OpenReader();
            
            _bodyReader.FrameArrived += _bodyReader_FrameArrived;

            _colorReader = _sensor.ColorFrameSource.OpenReader();
            _colorReader.FrameArrived += _colorReader_FrameArrived;

            MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
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
                    Scene.Instance.calculateDistances.AddBodies(bodiesInFrame);
                }
            }

            if (bodies != null)
            {
                int i = 0;
                foreach (IBody body in bodies)
                {
                    
                    ulong trackingId = body.TrackingId;
                    if (Scene.Instance != null && Kinect.Instance.Recorder.IsRecording)
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
                        if (trackingId != Kinect.Instance.gestureDetectorList[i].TrackingId)
                        {
                            Kinect.Instance.gestureDetectorList[i].TrackingId = trackingId;

                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            Kinect.Instance.gestureDetectorList[i].IsPaused = trackingId == 0;
                        }
                    }
                    i++;
                }

                bodies.MapDepthPositions();
                //bodies.MapColorPositions();



                

                //AddBodyToContext(body, context, boneColors[body.TrackingId], jointColor);

                //MainWindow.Instance().bodyImageControl.Source = bitmap;
                MainWindow.Instance().bodyImageControl.Source = bodies.GetBitmap(Colors.LightGreen, Colors.Yellow);
                
                
            }
            else
            {
                MainWindow.Instance().bodyImageControl.Source = null;
                //OutputImage = null;
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
