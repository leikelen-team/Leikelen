using KinectEx;
using KinectEx.DVR;
using KinectEx.Smoothing;
using Microsoft.Kinect;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.core
{
    public class BodyDetector
    {
        KinectSensor _sensor = null;
        BodyFrameReader _bodyReader = null;
        ColorFrameReader _colorReader = null;
        //DepthFrameReader _depthReader = null;
        //InfraredFrameReader _infraredReader = null;

        //FrameTypes _displayType = FrameTypes.Body;

        ColorFrameBitmap _colorBitmap = new ColorFrameBitmap();
        DepthFrameBitmap _depthBitmap = new DepthFrameBitmap();
        InfraredFrameBitmap _infraredBitmap = new InfraredFrameBitmap();

        

        List<CustomBody> _bodies = null;
        //SmoothedBodyList<KalmanSmoother> _kalmanBodies = null;
        //SmoothedBodyList<ExponentialSmoother> _exponentialBodies = null;

        // -----------------------

        //public ImageSource OutputImage { get; private set; }

        // -----------------------

        public BodyDetector()
        {
            this._sensor = Kinect.Sensor;

            _bodies = new List<CustomBody>();

            _bodyReader = _sensor.BodyFrameSource.OpenReader();
            _bodyReader.FrameArrived += _bodyReader_FrameArrived;

            _colorReader = _sensor.ColorFrameSource.OpenReader();
            _colorReader.FrameArrived += _colorReader_FrameArrived;

            //_depthReader = _sensor.DepthFrameSource.OpenReader();
            //_depthReader.FrameArrived += _depthReader_FrameArrived;

            //_infraredReader = _sensor.InfraredFrameSource.OpenReader();
            //_infraredReader.FrameArrived += _infraredReader_FrameArrived;

            
            MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
            //OutputImage = _colorBitmap.Bitmap;
        }

        private void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            //if (_displayType != FrameTypes.Body)
            //    return;

            IEnumerable<IBody> bodies = null; // to make the GetBitmap call a little cleaner
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    frame.GetAndRefreshBodyData(_bodies);
                    bodies = _bodies;
                }
            }

            if (bodies != null)
            {
                int i = 0;
                foreach (IBody body in bodies)
                {
                    ulong trackingId = body.TrackingId;
                    if (Scene.Instance != null)
                    {
                        bool personExists = Scene.Instance.Persons.Exists(p => p.TrackingId == trackingId);
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
                MainWindow.Instance().bodyImageControl.Source = bodies.GetBitmap(Colors.LightGreen, Colors.Yellow);
                //OutputImage = bodies.GetBitmap(Colors.LightGreen, Colors.Yellow);
            }
            else
            {
                MainWindow.Instance().colorImageControl.Source = null;
                //OutputImage = null;
            }
        }

        private void _colorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            //if (_displayType == FrameTypes.Color)
            //{
                _colorBitmap.Update(e.FrameReference);
                MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
            //}
        }

    }
}
