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

/// <summary>
/// OpenBCI EEG input sensor.
/// </summary>
namespace cl.uv.leikelen.Module.Input.OpenBCI
{
    /// <summary>
    /// Monitor for the OpenBCI sensor
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Input.IMonitor" />
    public class Monitor : IMonitor
    {
        /// <summary>
        /// Occurs when current sensor's [status changed].
        /// </summary>
        public event EventHandler StatusChanged;
        /// <summary>
        /// The graph tab of actual signals in real time.
        /// </summary>
        public Module.Input.OpenBCI.View.LiveGraphTab GraphTab;

        private readonly Module.Input.OpenBCI.Util.InterpretStream _interpretStream;
        private Module.Input.OpenBCI.Util.Filter _filter;
        private SerialPort _serialPort;
        private API.Module.Input.InputStatus _status;
        private Module.Input.OpenBCI.Util.FileManage _filemanage;
        private readonly string[] _positions;
        private API.DataAccess.IDataAccessFacade _dataAccessFacade = new DataAccessFacade();
        private Data.Model.Person _person;

        private bool _isRecording;


        private event EventHandler<API.FrameProvider.EEG.EegFrameArrivedEventArgs> EegFrameArrived;
        private event EventHandler<API.FrameProvider.Accelerometer.AccelerometerFrameArrivedEventArgs> AccelerometerFrameArrived;

        /// <summary>
        /// Initializes a new instance of the <see cref="Monitor"/> class.
        /// </summary>
        /// <param name="person">The person.</param>
        public Monitor(Data.Model.Person person)
        {
            _person = person;
            Console.WriteLine(person.Name);
            _interpretStream = new Util.InterpretStream();
            _filter = new Util.Filter();
            _status = InputStatus.Unconnected;
            GraphTab = new View.LiveGraphTab(person.MainColor);
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
        async Task API.Module.Input.IMonitor.Close()
        {
            StopStream();
            _serialPort?.Close();
        }

        API.Module.Input.InputStatus API.Module.Input.IMonitor.GetStatus()
        {
            return _status;
        }

        bool API.Module.Input.IMonitor.IsRecording()
        {
            return _isRecording;
        }

        async Task API.Module.Input.IMonitor.Open()
        {
            //TODO: delete this
            var mr = new Random();
            var recordTimer = new DispatcherTimer();
            recordTimer.Interval = new TimeSpan(0, 0, 0, 0, 200); //0.5 seconds
            recordTimer.Tick += (sender, e) =>
            {
                double[] data = new double[]{
                    mr.NextDouble(),
                    mr.NextDouble(),
                    mr.NextDouble(),
                    mr.NextDouble(),
                    mr.NextDouble(),
                    mr.NextDouble(),
                    mr.NextDouble(),
                    mr.NextDouble(),
                    mr.NextDouble(),
                    mr.NextDouble(),
                    mr.NextDouble(),
                    mr.NextDouble()
                };
                GraphTab.Enqueue(data);
            };
            recordTimer.Start();
            //to here
            StartStream();
        }

        async Task API.Module.Input.IMonitor.StartRecording()
        {
            StartStream();
            _filemanage = new Util.FileManage(
                Path.Combine(
                    _dataAccessFacade.GetGeneralSettings().GetSceneInUseDirectory(),
                    "openbci_"+_person.PersonId+".csv"));
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

        async Task API.Module.Input.IMonitor.StopRecording()
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

        async Task API.Module.Input.IMonitor.OpenPort(string portName)
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
                            GraphTab.Enqueue(data);
                            //double[] dataToGraph = new double[9];
                            //dataToGraph[0] = data[0];
                            var eegArgs = new EegFrameArrivedEventArgs()
                            {
                                Time = time.Value,
                                Person = _person,
                                Channels = new List<EegChannel>()
                            };
                            for (int j = 1; j < 9; j++)
                            {
                                double value = _filter.FiltersSelect((FilterType)(OpenBCISettings.Instance.Filter.Value),
                                    (NotchType)(OpenBCISettings.Instance.Notch.Value), data[j], j - 1);
                                //dataToGraph[j] = value;
                                eegArgs.Channels.Add(new EegChannel()
                                {
                                    Filter = FilterType.None,
                                    Notch = (NotchType)OpenBCISettings.Instance.Notch.Value,
                                    Position = _positions[j - 1]?? "",
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
                        var actualTime = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
                        if (actualTime.HasValue)
                            _filemanage?.WriteFile(actualTime.Value, data);
                    }
                }
            }
        }


        private void OnEegFrameArrived(API.FrameProvider.EEG.EegFrameArrivedEventArgs e)
        {
            EegFrameArrived?.Invoke(this, e);
        }

        private void OnAccelerometerArrived(API.FrameProvider.Accelerometer.AccelerometerFrameArrivedEventArgs e)
        {
            AccelerometerFrameArrived?.Invoke(this, e);
        }

    }
}
