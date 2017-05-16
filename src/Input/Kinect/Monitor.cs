using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API.Input;
using cl.uv.leikelen.src.Module;
using Microsoft.Kinect;
using KinectEx.DVR;


namespace cl.uv.leikelen.src.Input.Kinect
{
    public class Monitor : IMonitor
    {
        public event EventHandler StatusChanged;

        //Kinect related attributes
        private KinectSensor _sensor;
        private KinectRecorder _recorder;

        //Kinect Frames
        private BodyFrameReader _bodyReader = null;
        private ColorFrameReader _colorReader = null;
        private AudioBeamFrameReader _audioBeamReader = null;

        public SkeletonColorVideoViewer videoViewer;

        public Monitor()
        {
            videoViewer = new SkeletonColorVideoViewer();
        }

        public InputStatus getStatus()
        {
            if (Sensor == null)
            {
                return InputStatus.Unconnected;
            }
            else if (Sensor.IsOpen)
            {
                return InputStatus.Connected;
            }
            else
            {
                return InputStatus.Error;
            }
        }

        public bool IsRecording()
        {
            return _recorder != null;
        }

        /// <summary>
        /// Open Kinect Sensor
        /// </summary>
        /// <returns>Task</returns>
        public async Task Open()
        {
            _bodyReader = Sensor.BodyFrameSource.OpenReader();
            _bodyReader.FrameArrived += videoViewer._bodyReader_FrameArrived;
            _colorReader = Sensor.ColorFrameSource.OpenReader();
            _colorReader.FrameArrived += videoViewer._colorReader_FrameArrived;
            _audioBeamReader = Sensor.AudioSource.OpenReader();

            foreach (var module in ModuleLoader.Instance.Modules)
            {
                if (module.BeforeRecording())
                {
                    IKinectModule kinectModule = module as IKinectModule;
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
                    }
                }
            }
        }

        /// <summary>
        /// Close Kinect Sensor
        /// </summary>
        /// <returns></returns>
        public async Task Close()
        {
            foreach (var module in ModuleLoader.Instance.Modules)
            {
                IKinectModule kinectModule = module as IKinectModule;
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

            _bodyReader.Dispose();
            _colorReader.Dispose();
            _audioBeamReader.Dispose();

            _bodyReader = null;
            _colorReader = null;
            _audioBeamReader = null;

            Sensor.Close();

            
        }

        public async Task StartRecording()
        {
            if (_recorder == null)
            {
                if (File.Exists(Properties.Paths.CurrentKdvrFile)) File.Delete(Properties.Paths.CurrentKdvrFile);
                if (File.Exists(Properties.Paths.CurrentDataFile)) File.Delete(Properties.Paths.CurrentDataFile);

                _recorder = new KinectRecorder(File.Open(Properties.Paths.CurrentKdvrFile, FileMode.Create), Sensor);
                _recorder.EnableBodyRecorder = true;
                _recorder.EnableColorRecorder = true;
                _recorder.EnableDepthRecorder = false;
                _recorder.EnableInfraredRecorder = false;

                _recorder.ColorRecorderCodec = new JpegColorCodec();
                _recorder.ColorRecorderCodec.OutputWidth = 1280;
                _recorder.ColorRecorderCodec.OutputHeight = 720;

                _recorder.Start();

                foreach (var module in ModuleLoader.Instance.Modules)
                {
                    if (!module.BeforeRecording())
                    {
                        IKinectModule kinectModule = module as IKinectModule;
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
                        }
                    }
                }
            }
        }

        public async Task StopRecording()
        {
            if (_recorder != null)
            {
                await _recorder.StopAsync();
                _recorder = null;
            }


            foreach (var module in ModuleLoader.Instance.Modules)
            {
                if (!module.BeforeRecording())
                {
                    IKinectModule kinectModule = module as IKinectModule;
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
            }
        }

        /// <summary>
        /// Kinect Sensor Object Instance
        /// </summary>
        public KinectSensor Sensor
        {
            get
            {
                if (_sensor == null || !_sensor.IsOpen)
                {
                    _sensor = KinectSensor.GetDefault();
                    _sensor.Open();
                }
                return _sensor;
            }
        }
    }
}
