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

namespace cl.uv.leikelen.Module.Input.MindwaveTGC
{
    public class Monitor : IMonitor
    {
        private API.DataAccess.IDataAccessFacade _dataAccessFacade = new Data.Access.DataAccessFacade();
        private TcpClient _client;
        public event EventHandler StatusChanged;
        bool _isRecording = false;
        InputStatus _status;
        private Person _person;
        private event EventHandler<EegFrameArrivedEventArgs> EegFrameArrived;

        public Monitor(Person person)
        {
            _person = person;
            _isRecording = false;
            _status = InputStatus.Unconnected;
        }

        async Task IMonitor.Close()
        {
            try
            {
                _isRecording = false;
                if (_client != null)
                    _client.Close();
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
                _client = new TcpClient("127.0.0.1", 13854);
                var stream = _client.GetStream();
                // Building command to enable JSON output from ThinkGear Connector (TGC)
                byte[] myWriteBuffer = Encoding.ASCII
                    .GetBytes(@"{ 'enableRawOutput' : true, 'format': 'Json'}");

                // Sending configuration packet to TGC
                if (stream.CanWrite)
                {
                    stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
                }
                if (stream.CanRead)
                {
                    _status = InputStatus.Connected;

                    System.Threading.Thread mithread = new System.Threading.Thread(ReadDataFrames);
                    mithread.Start(stream);
                    //ReadDataFrames(stream);
                    
                }
            }
            catch (SocketException se)
            {
                _status = InputStatus.Error;
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


                foreach (string s in packets)
                {
                    //ParseJSON(s.Trim());
                    Console.WriteLine(s.Trim());
                    if (_isRecording)
                    {
                        try
                        {
                            var jsonObj = JsonConvert.DeserializeObject<dynamic>(s.Trim());
                            if (jsonObj != null)
                            {
                                foreach (Newtonsoft.Json.Linq.JProperty item in jsonObj)
                                {
                                    switch (item.Name)
                                    {
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
                                                        meditation = jsonObj.eSense.medidation;
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
                                EegFrameArrived.Invoke(this, frame);
                            }

                        }
                        catch (Exception ex)
                        {

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
            _isRecording = true;
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
