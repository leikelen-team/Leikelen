using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.Kinect;
using cl.uv.leikelen.API.Module.Input;
using Microsoft.Kinect;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using System.IO;
using KinectEx.DVR;

namespace cl.uv.leikelen.Module.Input.Kinect
{
    public class Monitor : IMonitor
    {
        public event EventHandler StatusChanged;

        private KinectSensor _sensor;
        private DVR.Recorder _recorder;

        private BodyFrameReader _bodyReader = null;
        private ColorFrameReader _colorReader = null;
        private AudioBeamFrameReader _audioBeamReader = null;
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        private bool _isRecording;

        


        public Monitor()
        {
            _isRecording = false;
        }

        public bool IsRecording()
        {
            return _isRecording;
        }

        public InputStatus GetStatus()
        {
            if (ReferenceEquals(null, _sensor))
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
            if (ReferenceEquals(null, _sensor))
                return;
            _sensor.Close();
            _sensor = null;
        }

        public async Task StartRecording()
        {
            if (!GetSensor())
                return;
            
            int detectorCount = _sensor.BodyFrameSource.BodyCount > 0 ? _sensor.BodyFrameSource.BodyCount : 6;
            for (int i = 0; i < _sensor.BodyFrameSource.BodyCount; ++i)
            {
                try
                {
                    GestureDetector detector = new GestureDetector(i, _sensor);
                    GestureDetector.GestureDetectorList.Add(detector);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error at creating the gesture detector: "+ex.Message);
                }
            }
            _bodyReader.FrameArrived += GestureDetector._bodyReader_FrameArrived;

            foreach (var module in ProcessingLoader.Instance.ProcessingModules)
            {
                if (module.IsEnabled)
                {
                    if (module is IKinectProcessingModule kinectModule)
                    {
                        if (!ReferenceEquals(null, kinectModule.BodyListener()))
                        {
                            _bodyReader.FrameArrived += kinectModule.BodyListener();
                        }

                        if (!ReferenceEquals(null, kinectModule.ColorListener()))
                        {
                            _colorReader.FrameArrived += kinectModule.ColorListener();
                        }

                        if (!ReferenceEquals(null, kinectModule.AudioListener()))
                        {
                            _audioBeamReader.FrameArrived += kinectModule.AudioListener();
                        }

                        if (!ReferenceEquals(null, kinectModule.GestureListener()))
                        {
                            foreach (var detector in GestureDetector.GestureDetectorList)
                            {
                                detector.KinectGestureFrameArrived += kinectModule.GestureListener();
                            }
                        }
                    }
                }
            }

            if (ReferenceEquals(null, _recorder) && !ReferenceEquals(null, _dataAccessFacade.GetSceneInUseAccess().GetScene()))
            {
                string fileName = Path.Combine(_dataAccessFacade.GetGeneralSettings().GetSceneInUseDirectory(), "kinect.dvr");
                _recorder = new DVR.Recorder(File.Open(fileName, FileMode.Create), _sensor)
                {
                    EnableBodyRecorder = true,
                    EnableColorRecorder = true,
                    EnableDepthRecorder = false,
                    EnableInfraredRecorder = false,

                    ColorRecorderCodec = new JpegColorCodec()
                };
                //TODO: revisar estos pixeles
                //_recorder.ColorRecorderCodec.OutputWidth = 1280;
                //_recorder.ColorRecorderCodec.OutputHeight = 720;
                _recorder.Start();
            }
            _isRecording = true;
        }

        public async Task StopRecording()
        {
            if (!GetSensor() || !_isRecording)
                return;

            if (!ReferenceEquals(null, _recorder))
            {
                await _recorder.StopAsync();
                _recorder = null;
            }

            foreach (var module in ProcessingLoader.Instance.ProcessingModules)
            {
                if (module is IKinectProcessingModule kinectModule)
                {
                    if (!ReferenceEquals(null, kinectModule.BodyListener()))
                    {
                        _bodyReader.FrameArrived -= kinectModule.BodyListener();
                    }

                    if (!ReferenceEquals(null, kinectModule.ColorListener()))
                    {
                        _colorReader.FrameArrived -= kinectModule.ColorListener();
                    }

                    if (!ReferenceEquals(null, kinectModule.AudioListener()))
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
            if(!ReferenceEquals(null, _sensor) && _sensor.IsOpen)
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
                    _bodyReader.FrameArrived += KinectInput.SkeletonColorVideoViewer._bodyReader_FrameArrived;
                    _colorReader = _sensor.ColorFrameSource.OpenReader();
                    _colorReader.FrameArrived += KinectInput.SkeletonColorVideoViewer._colorReader_FrameArrived;
                    _audioBeamReader = _sensor.AudioSource.OpenReader();
                    
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
