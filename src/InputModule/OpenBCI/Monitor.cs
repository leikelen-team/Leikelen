using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API.InputModule;
using System.IO.Ports;
using System.IO;

namespace cl.uv.leikelen.src.InputModule.OpenBCI
{
    public class Monitor : IMonitor
    {
        public event EventHandler StatusChanged;
        public NotchType Notch;

        private InterpretStream _interpretStream;
        private Filter _filter;
        private SerialPort _serialPort;
        private InputStatus _status;
        private FileManage _filemanage;
        

        private event EventHandler<EEGFrameArrivedEventArgs> EEGFrameArrived;
        private event EventHandler<AccelerometerFrameArrivedEventArgs> AccelerometerFrameArrived;

        public Monitor()
        {
            _interpretStream = new InterpretStream();
            _filter = new Filter();
            _status = InputStatus.Unconnected;
        }

        #region IMonitor methods
        public async Task Close()
        {
            StopStream();
            _serialPort.Close();
        }

        public InputStatus GetStatus()
        {
            return _status;
        }

        public bool IsRecording()
        {
            throw new NotImplementedException();
        }

        public async Task Open()
        {
            throw new NotImplementedException();
        }

        public Task StartRecording()
        {
            throw new NotImplementedException();
        }

        public Task StopRecording()
        {
            throw new NotImplementedException();
        }
        #endregion

        private void OpenPort(String portName)
        {
            _serialPort.PortName = portName;
            _serialPort.BaudRate = 115200;
            try
            {
                _serialPort.Open();
            }
            catch
            {
                _status = InputStatus.Error;
            }
            _serialPort.DataReceived += serialPort_DataReceived;
        }
        private void StartStream()
        {
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
            char[] buff = new char[1];
            buff[0] = 's';
            _serialPort.Write(buff, 0, 1);
            if (_filemanage != null) _filemanage.CloseFile();
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = sender as SerialPort;
            byte[] buffer = new byte[port.BytesToRead];
            port.Read(buffer, 0, buffer.Length);
            for(int i = 0; i < buffer.Length; i++)
            {
                double[] data = _interpretStream.interpretBinaryStream(buffer[i]);
                if(data != null && data.Length >= 9)
                {
                    double[] dataTmp = new double[9];
                    for (int j = 0; j < 9; j++)
                    {
                        dataTmp[j] = data[j];
                    }
                    var EEGArgs = new EEGFrameArrivedEventArgs()
                    {
                        Data = dataTmp,
                        Filter = FilterType.None,
                        Notch = Notch,
                        NumberOfChannels = 8
                        //Time = 
                    };
                    OnEEGFrameArrived(EEGArgs);
                }
                if(data != null && data.Length == 12)
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
