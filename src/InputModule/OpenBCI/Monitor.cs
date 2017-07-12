using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API.InputModule;
using System.IO.Ports;
using System.Windows.Threading;
using System.IO;
using cl.uv.leikelen.src.API.FrameProvider.Accelerometer;
using cl.uv.leikelen.src.API.FrameProvider.EEG;

namespace cl.uv.leikelen.src.InputModule.OpenBCI
{
    public class Monitor : IMonitor
    {
        public event EventHandler StatusChanged;

        private InterpretStream _interpretStream;
        private Filter _filter;
        private SerialPort _serialPort;
        private InputStatus _status;
        private FileManage _filemanage;
        private string[] Positions;

        private bool _isRecording;


        private event EventHandler<EEGFrameArrivedEventArgs> EEGFrameArrived;
        private event EventHandler<AccelerometerFrameArrivedEventArgs> AccelerometerFrameArrived;

        public Monitor()
        {
            _interpretStream = new InterpretStream();
            _filter = new Filter();
            _status = InputStatus.Unconnected;
            Positions = new string[8];
            _isRecording = false;
        }

        #region IMonitor methods
        public async Task Close()
        {
            StopStream();
            if(_serialPort != null) _serialPort.Close();
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
            _filemanage = new FileManage(GeneralSettings.Instance.TmpDirectory.Value + GeneralSettings.Instance.CurrentSceneDirectory.Value + "openbci.csv");
            _isRecording = true;
        }

        public async Task StopRecording()
        {
            StopStream();
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
            if (_filemanage != null) _filemanage.CloseFile();
            if (_serialPort == null) return;
            char[] buff = new char[1];
            buff[0] = 's';
            _serialPort.Write(buff, 0, 1);
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = sender as SerialPort;
            byte[] buffer = new byte[port.BytesToRead];
            port.Read(buffer, 0, buffer.Length);
            for(int i = 0; i < buffer.Length; i++)
            {
                double[] data = _interpretStream.interpretBinaryStream(buffer[i]);

                if(data != null)
                {
                    foreach (double d in data)
                    {
                        Console.Write(d);
                        Console.Write(",");
                    }
                    Console.WriteLine();
                    if (data.Length >= 9)
                    {
                        var EEGArgs = new EEGFrameArrivedEventArgs()
                        {
                            //Time = ,
                            Channels = new List<EEGChannel>()
                        };
                        for (int j = 1; j < 9; j++)
                        {
                            EEGArgs.Channels.Add(new EEGChannel()
                            {
                                Filter = FilterType.None,
                                Notch = (NotchType)OpenBCI_Settings.Instance.Notch.Value,
                                PositionSystem = "10/20",
                                Position = Positions[j - 1],
                                Value = data[j]
                            });
                        }
                        OnEEGFrameArrived(EEGArgs);
                    }
                    if (data.Length == 12 && data[9] != 0 && data[10] != 0 && data[11] != 0)
                    {
                        var AccArgs = new AccelerometerFrameArrivedEventArgs()
                        {
                            Place = "Head",
                            // Time = ,
                            XAxis = data[9],
                            YAxis = data[10],
                            ZAxis = data[11]
                        };
                        OnAccelerometerArrived(AccArgs);
                    }
                    if (_filemanage != null) _filemanage.WriteFile(data);
                }
            }
        }


        private void OnEEGFrameArrived(EEGFrameArrivedEventArgs e)
        {
            EEGFrameArrived?.Invoke(this, e);
        }

        private void OnAccelerometerArrived(AccelerometerFrameArrivedEventArgs e)
        {
            AccelerometerFrameArrived?.Invoke(this, e);
        }

    }
}
