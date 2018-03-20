using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.API.DataAccess;

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels
{
    public class EEGReceiver
    {
        private List<double[]> _signaList;
        private bool _signalGet = false;
        private int _lastSecond;
        private double lastF3;
        private double lastC4;
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public EEGReceiver()
        {
            _dataAccessFacade.GetModalAccess().AddIfNotExists("Emotion", "Affects or feels of a person");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Emotion", "LALV", "Low arousal Low Valence", "emotionmodel.svm");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Emotion", "LAHV", "Low arousal High Valence", "emotionmodel.svm");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Emotion", "HALV", "High arousal Low Valence", "emotionmodel.svm");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Emotion", "HAHV", "High arousal High Valence", "emotionmodel.svm");

            _signaList = new List<double[]>();
            _signalGet = true;
            _lastSecond = 0;
        }

        public void DataReceiver(object sender, EegFrameArrivedEventArgs e)
        {
            //if samplingHz is 128, then alternates if the signal is processed or no
            if (EEGEmoProc2ChSettings.Instance.SamplingHz == 256)
                _signalGet = true;
            else if (EEGEmoProc2ChSettings.Instance.SamplingHz == 128)
                _signalGet = !_signalGet;
                

            double[] values = new double[2];
            var channelCounter = 0;
            foreach (var channel in e.Channels)
            {
                if (channel.Position.Equals("F3"))
                {
                    if (EEGEmoProc2ChSettings.Instance.SamplingHz == 256)
                        values[0] = channel.Value;
                    else if (_signalGet)
                        values[0] = (lastF3 + channel.Value) / 2;
                    else
                        lastF3 = channel.Value;
                    channelCounter++;
                }
                if (channel.Position.Equals("C4"))
                {
                    if (EEGEmoProc2ChSettings.Instance.SamplingHz == 256)
                        values[1] = channel.Value;
                    else if (_signalGet)
                        values[1] = (lastC4 + channel.Value) / 2;
                    else
                        lastC4 = channel.Value;
                    channelCounter++;
                }
            }
            if(channelCounter == 2)
                _signaList.Add(values);

            //if the window ends, sends the signal to process
            var sceneLocation = e.Time;
            if (_signaList.Count >= 0 
                && _lastSecond != (int)sceneLocation.TotalSeconds 
                && (int)sceneLocation.TotalSeconds % 9 == 0)
            {
                _lastSecond = (int) sceneLocation.TotalSeconds;

                try
                {
                    var tag = LearningModel.Classify(_signaList);
                    Console.WriteLine(tag.ToString());
                    _dataAccessFacade.GetEventAccess().Add(e.Person, "Emotion", tag.ToString(), e.Time, 1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error: " + ex.Message);
                }
                _signaList = new List<double[]>();
            }
        }
    }
}
