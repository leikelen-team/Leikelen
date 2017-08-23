using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.InputModule;
using System.IO.Ports;
using System.Windows.Threading;
using System.IO;
using cl.uv.leikelen.API.FrameProvider.Accelerometer;
using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.Data.Access.External;
using cl.uv.leikelen.ProcessingModule;

namespace cl.uv.leikelen.InputModule.OpenBCI
{
    public class Monitor : IMonitor
    {
        public event EventHandler StatusChanged;

        private readonly Util.InterpretStream _interpretStream;
        private Util.Filter _filter;
        private SerialPort _serialPort;
        private InputStatus _status;
        private Util.FileManage _filemanage;
        private readonly string[] _positions;

        private bool _isRecording;


        private event EventHandler<EegFrameArrivedEventArgs> EegFrameArrived;
        private event EventHandler<AccelerometerFrameArrivedEventArgs> AccelerometerFrameArrived;

        public Monitor()
        {
            _interpretStream = new Util.InterpretStream();
            _filter = new Util.Filter();
            _status = InputStatus.Unconnected;
            _positions = new string[8]
            {
                OpenBCISettings.Instance.PositionChannel1.Value,
                OpenBCISettings.Instance.PositionChannel2.Value,
                OpenBCISettings.Instance.PositionChannel3.Value,
                OpenBCISettings.Instance.PositionChannel4.Value,
                OpenBCISettings.Instance.PositionChannel5.Value,
                OpenBCISettings.Instance.PositionChannel6.Value,
                OpenBCISettings.Instance.PositionChannel7.Value,
                OpenBCISettings.Instance.PositionChannel8.Value,
            };
            _isRecording = false;
        }

        #region IMonitor methods
        public async Task Close()
        {
            StopStream();
            _serialPort?.Close();
        }

        public InputStatus GetStatus()
        {
            return _status;
        }

        public bool IsRecording()
        {
            return _isRecording;
        }

        public async Task Open()
        {
            StartStream();
        }

        public async Task StartRecording()
        {
            StartStream();
            _filemanage = new Util.FileManage(GeneralSettings.Instance.TmpDirectory.Value + GeneralSettings.Instance.CurrentSceneDirectory.Value + "openbci.csv");
            _isRecording = true;

            foreach (var module in ProcessingLoader.Instance.ProcessingModules)
            {
                if (module.IsEnabled)
                {
                    var eegModule = module as IEegProcessingModule;
                    if (eegModule != null)
                    {
                        EegFrameArrived += eegModule.EegListener();
                    }

                    var accModule = module as IAccelerometerProcessingModule;
                    if (accModule != null)
                    {
                        AccelerometerFrameArrived += accModule.AccelerometerListener();
                    }
                }
            }
        }

        public async Task StopRecording()
        {
            StopStream();

            foreach (var module in ProcessingLoader.Instance.ProcessingModules)
            {
                var eegModule = module as IEegProcessingModule;
                if (eegModule != null)
                {
                    EegFrameArrived -= eegModule.EegListener();
                }

                var accModule = module as IAccelerometerProcessingModule;
                if (accModule != null)
                {
                    AccelerometerFrameArrived -= accModule.AccelerometerListener();
                }
            }
        }
        #endregion

        public async Task OpenPort(string portName)
        {
            try
            {
                if(_serialPort == null || _serialPort.IsOpen == false)
                {
                    _serialPort = new SerialPort(portName, 115200);
                    _serialPort.Open();
                    _serialPort.DataReceived += serialPort_DataReceived;
                    _status = InputStatus.Connected;
                }
            }
            catch
            {
                _serialPort = null;
                _status = InputStatus.Error;
            }
        }
        private void StartStream()
        {
            if (_serialPort == null) return;

            char[] buff = new char[1];
            buff[0] = 'b';
            try
            {
                _serialPort.Write(buff, 0, 1);
            }
            catch
            {
                _status = InputStatus.Error;
            }
        }

        private void StopStream()
        {
            if (_filemanage != null)
            {
                _filemanage.CloseFile();
                _filemanage = null;
            }
            if (_serialPort == null) return;
            char[] buff = new char[1];
            buff[0] = 's';
            _serialPort.Write(buff, 0, 1);
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = sender as SerialPort;
            if (port == null) return;
            byte[] buffer = new byte[port.BytesToRead];
            port.Read(buffer, 0, buffer.Length);
            foreach(var bufferData in buffer)
            {
                double[] data = _interpretStream.interpretBinaryStream(bufferData);

                if(data != null)
                {
                    if (data.Length >= 9)
                    {
                        var eegArgs = new EegFrameArrivedEventArgs()
                        {
                            //Time = ,
                            Channels = new List<EegChannel>()
                        };
                        for (int j = 1; j < 9; j++)
                        {
                            double value = _filter.FiltersSelect((FilterType) (OpenBCISettings.Instance.Filter.Value),
                                (NotchType) (OpenBCISettings.Instance.Notch.Value), data[j], j - 1);
                            eegArgs.Channels.Add(new EegChannel()
                            {
                                Filter = FilterType.None,
                                Notch = (NotchType)OpenBCISettings.Instance.Notch.Value,
                                Position = _positions[j - 1],
                                Value = value
                            });
                        }
                        OnEegFrameArrived(eegArgs);
                    }
                    if (data.Length == 12 && data[9] != 0 && data[10] != 0 && data[11] != 0)
                    {
                        var accArgs = new AccelerometerFrameArrivedEventArgs()
                        {
                            Place = "Head",
                            // Time = ,
                            XAxis = data[9],
                            YAxis = data[10],
                            ZAxis = data[11]
                        };
                        OnAccelerometerArrived(accArgs);
                    }
                    var actualTime = SceneInUseAccess.Instance.GetLocation();
                    if (actualTime.HasValue)
                        _filemanage?.WriteFile(actualTime.Value, data);
                }
            }
        }


        private void OnEegFrameArrived(EegFrameArrivedEventArgs e)
        {
            EegFrameArrived?.Invoke(this, e);
        }

        private void OnAccelerometerArrived(AccelerometerFrameArrivedEventArgs e)
        {
            AccelerometerFrameArrived?.Invoke(this, e);
        }

    }
}
