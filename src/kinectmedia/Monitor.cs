//using System.ComponentModel.Composition;
using cl.uv.leikelen.src.Data;
using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Model.AccessLogic;
using cl.uv.leikelen.src.Module;
using KinectEx;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
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

        public Monitor()
        {
            Loader.addModules();
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
            foreach(var module in Loader.Modules)
            {
                if(module.BeforeRecording())
                {
                    if (module.BodyListener() != null)
                    {
                        _bodyReader.FrameArrived += module.BodyListener();
                    }

                    if (module.ColorListener() != null)
                    {
                        _colorReader.FrameArrived += module.ColorListener();
                    }

                    if (module.AudioListener() != null)
                    {
                        _audioBeamReader.FrameArrived += module.AudioListener();
                    }
                }
            }

            MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
        }

        public void Close()
        {
            _bodies = new List<CustomBody>();

            _bodyReader.FrameArrived -= _bodyReader_FrameArrived;
            _colorReader.FrameArrived -= _colorReader_FrameArrived;

            foreach (var module in Loader.Modules)
            {
                if (module.BodyListener() != null)
                {
                    _bodyReader.FrameArrived -= module.BodyListener();
                }

                if (module.ColorListener() != null)
                {
                    _colorReader.FrameArrived -= module.ColorListener();
                }

                if (module.AudioListener() != null)
                {
                    _audioBeamReader.FrameArrived -= module.AudioListener();
                }
            }

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
                }
            }

            if (bodies != null)
            {
                int i = 0;
                foreach (IBody body in bodies)
                {
                    
                    ulong trackingId = body.TrackingId;
                    if (StaticScene.Instance != null && KinectMediaFacade.Instance.Recorder.IsRecording)
                    {
                        bool personExists = StaticScene.Instance.isPersonInScene(trackingId);
                        if (!personExists && trackingId != 0)
                        {
                            Person person = new Person(
                                    trackingId,
                                    StaticScene.Instance.numberOfPersons()
                                );
                            StaticScene.Instance.addPerson(person);
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
