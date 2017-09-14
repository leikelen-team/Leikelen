using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.Kinect;
using cl.uv.leikelen.API.InputModule;
using cl.uv.leikelen.ProcessingModule;
using Microsoft.Kinect;

namespace cl.uv.leikelen.InputModule.Kinect
{
    public class Monitor : IMonitor
    {
        public event EventHandler StatusChanged;

        private KinectSensor _sensor;

        private BodyFrameReader _bodyReader = null;
        private ColorFrameReader _colorReader = null;
        private AudioBeamFrameReader _audioBeamReader = null;

        private bool _isRecording;

        public SkeletonColorVideoViewer VideoViewer;


        public Monitor()
        {
            VideoViewer = new SkeletonColorVideoViewer();
            _isRecording = false;
        }

        public bool IsRecording()
        {
            return _isRecording;
        }

        public InputStatus GetStatus()
        {
            if (_sensor == null)
            {
                return InputStatus.Unconnected;
            }
            if (_sensor.IsOpen)
            {
                return InputStatus.Connected;
            }
            return InputStatus.Error;
        }

        public async Task Open()
        {
            GetSensor();
        }

        public async Task Close()
        {
            if (_sensor == null)
                return;
            _sensor.Close();
            _sensor = null;
        }

        public async Task StartRecording()
        {
            if (!GetSensor())
                return;
            //TODO: record
            foreach (var module in ProcessingLoader.Instance.ProcessingModules)
            {
                if (module.IsEnabled)
                {
                    var kinectModule = module as IKinectProcessingModule;
                    if (kinectModule != null)
                    {
                        if (kinectModule.BodyListener() != null)
                        {
                            _bodyReader.FrameArrived += kinectModule.BodyListener();
                        }

                        if (kinectModule.ColorListener() != null)
                        {
                            _colorReader.FrameArrived += kinectModule.ColorListener();
                        }

                        if (kinectModule.AudioListener() != null)
                        {
                            _audioBeamReader.FrameArrived += kinectModule.AudioListener();
                        }

                        if(kinectModule.GestureListener() != null)
                        {
                            foreach(var detector in GestureDetector.GestureDetectorList)
                            {
                                detector.KinectGestureFrameArrived += kinectModule.GestureListener();
                            }
                        }
                    }
                }
            }

            _isRecording = true;
        }

        public async Task StopRecording()
        {
            if (!GetSensor() || !_isRecording)
                return;
            //TODO: stop recording

            foreach (var module in ProcessingLoader.Instance.ProcessingModules)
            {
                var kinectModule = module as IKinectProcessingModule;
                if (kinectModule != null)
                {
                    if (kinectModule.BodyListener() != null)
                    {
                        _bodyReader.FrameArrived -= kinectModule.BodyListener();
                    }

                    if (kinectModule.ColorListener() != null)
                    {
                        _colorReader.FrameArrived -= kinectModule.ColorListener();
                    }

                    if (kinectModule.AudioListener() != null)
                    {
                        _audioBeamReader.FrameArrived -= kinectModule.AudioListener();
                    }
                }
            }

            _isRecording = false;
        }

        public Task OpenPort(string portName)
        {
            return null;
        }


        private bool GetSensor()
        {
            if(_sensor != null && _sensor.IsOpen)
            {
                return true;
            }
            else
            {
                try
                {
                    _sensor = KinectSensor.GetDefault();
                    _sensor.Open();


                    _bodyReader = _sensor.BodyFrameSource.OpenReader();
                    _bodyReader.FrameArrived += VideoViewer._bodyReader_FrameArrived;
                    _colorReader = _sensor.ColorFrameSource.OpenReader();
                    _colorReader.FrameArrived += VideoViewer._colorReader_FrameArrived;
                    _audioBeamReader = _sensor.AudioSource.OpenReader();

                    int detectorCount = _sensor.BodyFrameSource.BodyCount > 0 ? _sensor.BodyFrameSource.BodyCount : 6;
                    for (int i = 0; i < _sensor.BodyFrameSource.BodyCount; ++i)
                    {
                        GestureDetector detector = new GestureDetector(i, _sensor);
                        GestureDetector.GestureDetectorList.Add(detector);
                    }
                    _bodyReader.FrameArrived += GestureDetector._bodyReader_FrameArrived;
                    OnStatusChanged();
                    return true;
                } 
                catch(Exception)
                {
                    OnStatusChanged();
                    return false;
                }
            }
        }

        private void OnStatusChanged()
        {
            StatusChanged?.Invoke(this, new EventArgs());
        }

    }
}
