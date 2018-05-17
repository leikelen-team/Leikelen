using cl.uv.leikelen.API.Module.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.API.FrameProvider.EEG;
using System.Windows;

namespace cl.uv.leikelen.Module.Input.MindwaveTGC
{
    public class Monitor : IMonitor
    {
        private API.DataAccess.IDataAccessFacade _dataAccessFacade = new Data.Access.DataAccessFacade();
        private TcpClient _client;
        public event EventHandler StatusChanged;
        private StreamWriter _sw;
        bool _isRecording = false;
        InputStatus _status;
        private Person _person;
        private event EventHandler<EegFrameArrivedEventArgs> EegFrameArrived;
        System.Threading.Thread _readThread;
        private bool _waitingStoppped = false;

        public Monitor(Person person)
        {
            _person = person;
            _isRecording = false;
            _waitingStoppped = false;
            _status = InputStatus.Unconnected;
        }

        async Task IMonitor.Close()
        {
            try
            {
                _isRecording = false;
                if (_client != null)
                    _client.Close();
                if (_readThread != null)
                {
                    try
                    {
                        _readThread.Interrupt();
                        _readThread.Abort();
                    }
                    /*catch(Exception ex)
                    {

                    }*/
                    finally
                    {
                        _readThread = null;
                    }
                    
                }
                    
            }
            catch (SocketException se)
            {
                _status = InputStatus.Error;
            }
        }

        InputStatus IMonitor.GetStatus()
        {
            return _status;
        }

        bool IMonitor.IsRecording()
        {
            return _isRecording;
        }

        async Task IMonitor.Open()
        {
            try
            {
                if(_client == null)
                    _client = new TcpClient("127.0.0.1", 13854);
                var stream = _client.GetStream();
                // Building command to enable JSON output from ThinkGear Connector (TGC)
                byte[] myWriteBuffer = Encoding.ASCII
                    .GetBytes(@"{ 'enableRawOutput' : true, 'format': 'Json'}");

                //var hash = System.Security.Cryptography.HashAlgorithm.Create("SHA1");

                //hash.Initialize();
                //hash.ComputeHash("Lelikelen".Base64DecodeAsBytes());
                
                byte[] myWriteBufferApp = Encoding.ASCII
                    .GetBytes(@"{ 'appName' : 'Lelikelen', 'appKey': '"+ Util.SHA1Util.SHA1HashStringForUTF8String("Lelikelen") + "'}");
                // Sending configuration packet to TGC
                if (stream.CanWrite)
                {
                    stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
                    stream.Write(myWriteBufferApp, 0, myWriteBufferApp.Length);
                }
                if (stream.CanRead)
                {
                    _status = InputStatus.Connected;

                    _readThread = new System.Threading.Thread(ReadDataFrames);
                    _readThread.Start(stream);
                    //ReadDataFrames(stream);
                    
                }
            }
            catch (SocketException se)
            {
                _status = InputStatus.Error;
                Console.WriteLine("exeption at opening mindwave: " + se.Message);
            }
        }

        private void ReadDataFrames(object stream)
        {
            var stream2 = (NetworkStream)stream;
            byte[] buffer = new byte[2048];
            int bytesRead;

            //Console.WriteLine("reading bytes");
            // This should really be in it's own thread
            while (true)
            {
                bytesRead = stream2.Read(buffer, 0, 2048);
                if (bytesRead == 0)
                    continue;
                string[] packets = Encoding.UTF8.GetString(buffer, 0,
                    bytesRead).Split('\r');
                double? rawEEg = null;
                int? quality = null;
                double? attention = null;
                double? meditation = null;
                double? mentalEffort = null;
                double? familiarity = null;
                int? blinkStrength = null;
                double? powerDelta = null;
                double? powerTheta = null;
                double? powerLowAlpha = null;
                double? powerHighAlpha = null;
                double? powerLowBeta = null;
                double? powerHighBeta = null;
                double? powerLowGamma = null;
                double? powerHighGama = null;
                string path = null;


                foreach (string s in packets)
                {
                    //ParseJSON(s.Trim());
                    Console.WriteLine(s.Trim());
                    if (_isRecording || _waitingStoppped)
                    {
                        try
                        {
                            bool stopped = false;
                            var jsonObj = JsonConvert.DeserializeObject<dynamic>(s);
                            if (jsonObj != null)
                            {
                                foreach (Newtonsoft.Json.Linq.JProperty item in jsonObj)
                                {
                                    switch (item.Name)
                                    {
                                        case "status":
                                            if(jsonObj.status == "recordingStopped")
                                            {
                                                stopped = true;
                                            }
                                            break;
                                        case "filePath":
                                            path = jsonObj.filePath;
                                            break;
                                        case "rawEeg":
                                            rawEEg = jsonObj.rawEeg;
                                            break;
                                        case "poorSignalLevel":
                                            quality = jsonObj.PoorSignalLevel;
                                            break;
                                        case "blinkStrength":
                                            blinkStrength = jsonObj.blinkStrength;
                                            break;
                                        case "mentalEffort":
                                            mentalEffort = jsonObj.mentalEffort;
                                            break;
                                        case "familiarity":
                                            familiarity = jsonObj.familiarity;
                                            break;
                                        case "eSense":
                                            if (jsonObj.eSense != null)
                                            {
                                                foreach (Newtonsoft.Json.Linq.JProperty item2 in jsonObj.eSense)
                                                {
                                                    if (item2.Name.Equals("attention"))
                                                        attention = jsonObj.eSense.attention;
                                                    if (item2.Name.Equals("meditation"))
                                                        meditation = jsonObj.eSense.meditation;
                                                }
                                            }
                                            break;
                                        case "eegPower":
                                            if (jsonObj.eegPower != null)
                                            {
                                                foreach (Newtonsoft.Json.Linq.JProperty item2 in jsonObj.eegPower)
                                                {
                                                    switch (item2.Name)
                                                    {
                                                        case "delta":
                                                            powerDelta = jsonObj.eegPower.delta;
                                                            break;
                                                        case "theta":
                                                            powerTheta = jsonObj.eegPower.theta;
                                                            break;
                                                        case "lowAlpha":
                                                            powerLowAlpha = jsonObj.eegPower.lowAlpha;
                                                            break;
                                                        case "highAlpha":
                                                            powerHighAlpha = jsonObj.eegPower.highAlpha;
                                                            break;
                                                        case "lowBeta":
                                                            powerLowBeta = jsonObj.eegPower.lowBeta;
                                                            break;
                                                        case "highBeta":
                                                            powerHighBeta = jsonObj.eegPower.highBeta;
                                                            break;
                                                        case "lowGamma":
                                                            powerLowGamma = jsonObj.eegPower.lowGamma;
                                                            break;
                                                        case "highGamma":
                                                            powerHighGama = jsonObj.eegPower.highGamma;
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }
                                if(_waitingStoppped && stopped && !String.IsNullOrEmpty(path))
                                {
                                    //TODO: show messagebox
                                    MessageBox.Show("path: "+path,"Titulo", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                                }
                                if(_isRecording && (rawEEg.HasValue || quality.HasValue ||
                                    attention.HasValue || meditation.HasValue || powerDelta.HasValue
                                    || powerTheta.HasValue || powerHighAlpha.HasValue || powerLowAlpha.HasValue
                                    || powerLowBeta.HasValue || powerHighBeta.HasValue || powerLowGamma.HasValue
                                    || powerHighGama.HasValue || powerHighAlpha.HasValue || familiarity.HasValue
                                    || mentalEffort.HasValue || blinkStrength.HasValue ))
                                {
                                    var frame = new API.FrameProvider.EEG.EegFrameArrivedEventArgs();
                                    frame.Person = _person;
                                    var miTime = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
                                    if (miTime.HasValue)
                                        frame.Time = miTime.Value;
                                    if (quality.HasValue)
                                        frame.Quality = quality.Value;
                                    if (rawEEg.HasValue)
                                        frame.Channels = new List<API.FrameProvider.EEG.EegChannel>()
                                            {
                                                new API.FrameProvider.EEG.EegChannel()
                                                {
                                                    Value = rawEEg.Value
                                                }
                                            };
                                    frame.CalculatedAttributes = new Dictionary<string, double>();
                                    if (attention.HasValue)
                                        frame.CalculatedAttributes["attention"] = attention.Value;
                                    if (meditation.HasValue)
                                        frame.CalculatedAttributes["meditation"] = meditation.Value;
                                    if (familiarity.HasValue)
                                        frame.CalculatedAttributes["familiarity"] = meditation.Value;
                                    if (mentalEffort.HasValue)
                                        frame.CalculatedAttributes["mentalEffort"] = meditation.Value;
                                    if (blinkStrength.HasValue)
                                        frame.CalculatedAttributes["blinkStrength"] = meditation.Value;
                                    frame.BandPower = new Dictionary<FrequencyBand, double>();
                                    if (powerDelta.HasValue)
                                        frame.BandPower[FrequencyBand.Delta] = powerDelta.Value;
                                    if (powerTheta.HasValue)
                                        frame.BandPower[FrequencyBand.Theta] = powerTheta.Value;
                                    if (powerHighAlpha.HasValue)
                                        frame.BandPower[FrequencyBand.HighAlpha] = powerHighAlpha.Value;
                                    if (powerLowAlpha.HasValue)
                                        frame.BandPower[FrequencyBand.LowAlpha] = powerLowAlpha.Value;
                                    if (powerLowBeta.HasValue)
                                        frame.BandPower[FrequencyBand.LowBeta] = powerLowBeta.Value;
                                    if (powerHighBeta.HasValue)
                                        frame.BandPower[FrequencyBand.HighBeta] = powerHighBeta.Value;
                                    if (powerLowGamma.HasValue)
                                        frame.BandPower[FrequencyBand.LowGamma] = powerLowGamma.Value;
                                    if (powerHighGama.HasValue)
                                        frame.BandPower[FrequencyBand.HighGamma] = powerHighGama.Value;
                                    
                                    EegFrameArrived?.Invoke(this, frame);
                                    var myPerson = new Person
                                    {
                                        Name = _person.Name,
                                        PersonId = _person.PersonId,
                                        TrackingId = _person.TrackingId,
                                        Sex = _person.Sex,
                                        Photo = _person.Photo,
                                        Birthday = _person.Birthday,
                                        PersonInScenes = null
                                    };
                                    frame.Person = myPerson;
                                    _sw.Write(frame.ToJsonString(compress: true, enumsAsStrings: true));
                                    _sw.WriteLine(",");
                                    _sw.Flush();
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error mindwave: "+ex.Message);
                        }

                    }
                }
            }
        }

        Task IMonitor.OpenPort(string portName)
        {
            return null;
        }

        async Task IMonitor.StartRecording()
        {
            if(_client != null)
            {
                _sw = new StreamWriter(Path.Combine(
                    _dataAccessFacade.GetGeneralSettings().GetSceneInUseDirectory(),
                    "mindwave_pId_" + _person.PersonId + "_pName_"+_person.Name+ ".json"));
                _sw.WriteLine("[");
                var stream = _client.GetStream();
                // Building command to enable JSON output from ThinkGear Connector (TGC)
                byte[] myWriteBuffer = Encoding.ASCII
                    .GetBytes(@"{ 'startRecording' : {'rawEeg' : true, 'poorSignalLevel': true, 'eSense': true, 'eegPower': true, 'blinkStrength': true}, 'applicationName': 'Lelikelen'}");

                // Sending configuration packet to TGC
                if (stream.CanWrite)
                {
                    stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
                }
            }
            _isRecording = true;
            _waitingStoppped = false;
            foreach (var module in ProcessingLoader.Instance.ProcessingModules)
            {
                if (module.IsEnabled)
                {
                    if (module is IEegProcessingModule eegModule)
                    {
                        EegFrameArrived += eegModule.EegListener();
                    }
                }
            }
        }

        async Task IMonitor.StopRecording()
        {
            if (_client != null)
            {
                if(_sw != null)
                {
                    _sw.WriteLine("]");
                    _sw.Flush();
                    _sw.Close();
                    _sw = null;
                }
                var stream = _client.GetStream();
                // Building command to enable JSON output from ThinkGear Connector (TGC)
                byte[] myWriteBuffer = Encoding.ASCII
                    .GetBytes(@"{ 'stopRecording' : 'Lelikelen'}");

                // Sending configuration packet to TGC
                if (stream.CanWrite)
                {
                    stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
                }
                if (stream.CanRead)
                {
                    _waitingStoppped = true;

                }
            }
            _isRecording = false;
            foreach (var module in ProcessingLoader.Instance.ProcessingModules)
            {
                if (module is IEegProcessingModule eegModule)
                {
                    EegFrameArrived -= eegModule.EegListener();
                }
            }
        }
    }
}
