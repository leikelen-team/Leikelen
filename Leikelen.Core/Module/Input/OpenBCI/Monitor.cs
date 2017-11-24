using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.Input;
using System.IO.Ports;
using System.Windows.Threading;
using System.IO;
using cl.uv.leikelen.API.FrameProvider.Accelerometer;
using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Module;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.Module.Input.OpenBCI
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
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();
        private Person _person;
        public View.LiveGraphTab GraphTab;

        private bool _isRecording;


        private event EventHandler<EegFrameArrivedEventArgs> EegFrameArrived;
        private event EventHandler<AccelerometerFrameArrivedEventArgs> AccelerometerFrameArrived;

        public Monitor(Person person)
        {
            _person = person;
            Console.WriteLine(person.Name);
            _interpretStream = new Util.InterpretStream();
            _filter = new Util.Filter();
            _status = InputStatus.Unconnected;
            GraphTab = new View.LiveGraphTab();
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
            _filemanage = new Util.FileManage(_dataAccessFacade.GetGeneralSettings().GetDataDirectory()+"scene/"+_dataAccessFacade.GetSceneInUseAccess().GetScene().SceneId + "/openbci.csv");
            _isRecording = true;

            foreach (var module in ProcessingLoader.Instance.ProcessingModules)
            {
                if (module.IsEnabled)
                {
                    if (module is IEegProcessingModule eegModule)
                    {
                        EegFrameArrived += eegModule.EegListener();
                    }

                    if (module is IAccelerometerProcessingModule accModule)
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
                if (module is IEegProcessingModule eegModule)
                {
                    EegFrameArrived -= eegModule.EegListener();
                }

                if (module is IAccelerometerProcessingModule accModule)
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
                Console.WriteLine(portName);
                if(ReferenceEquals(null, _serialPort) || _serialPort.IsOpen == false)
                {
                    _serialPort = new SerialPort(portName, 115200);
                    _serialPort.Open();
                    _serialPort.DataReceived += SerialPort_DataReceived;
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
            if (ReferenceEquals(null, _serialPort)) return;

            char[] buff = new char[1];
            buff[0] = 'b';
            try
            {
                _serialPort.Write(buff, 0, 1);
                Console.WriteLine("started stream");
            }
            catch
            {
                _status = InputStatus.Error;
            }
        }

        private void StopStream()
        {
            if (!ReferenceEquals(null, _filemanage))
            {
                _filemanage.CloseFile();
                _filemanage = null;
            }
            if (ReferenceEquals(null, _serialPort)) return;
            char[] buff = new char[1];
            buff[0] = 's';
            _serialPort.Write(buff, 0, 1);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sender is SerialPort port)
            {
                byte[] buffer = new byte[port.BytesToRead];
                port.Read(buffer, 0, buffer.Length);
                var time = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
                if (!time.HasValue) return;
                foreach (var bufferData in buffer)
                {
                    double[] data = _interpretStream.interpretBinaryStream(bufferData);

                    if (!ReferenceEquals(null, data))
                    {
                        if (data.Length >= 9)
                        {
                            double[] dataToGraph = new double[9];
                            dataToGraph[0] = data[0];
                            var eegArgs = new EegFrameArrivedEventArgs()
                            {
                                Time = time.Value,
                                PersonId = _person.PersonId,
                                Channels = new List<EegChannel>()
                            };
                            for (int j = 1; j < 9; j++)
                            {
                                double value = _filter.FiltersSelect((FilterType)(OpenBCISettings.Instance.Filter.Value),
                                    (NotchType)(OpenBCISettings.Instance.Notch.Value), data[j], j - 1);
                                dataToGraph[j] = value;
                                eegArgs.Channels.Add(new EegChannel()
                                {
                                    Filter = FilterType.None,
                                    Notch = (NotchType)OpenBCISettings.Instance.Notch.Value,
                                    Position = _positions[j - 1]?? "",
                                    Value = value
                                });
                            }
                            GraphTab.Enqueue(dataToGraph);
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
                        var actualTime = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
                        if (actualTime.HasValue)
                            _filemanage?.WriteFile(actualTime.Value, data);
                    }
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
