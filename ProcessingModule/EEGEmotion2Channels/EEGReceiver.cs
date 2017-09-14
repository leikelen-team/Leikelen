using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.API.DataAccess;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels
{
    public class EEGReceiver
    {
        private List<double[]> _signaList;
        private List<List<double[]>> _allsignalsList;
        private bool _signalGet;
        private readonly ClassifierType _type;
        private int _lastSecond;
        private IDataAccessFacade DataAccessFacade = new DataAccessFacade();

        public EEGReceiver(ClassifierType type)
        {
            if (!DataAccessFacade.GetModalAccess().Exists("Emotion"))
                DataAccessFacade.GetModalAccess().Add("Emotion", "Affects or feels of a person");
            if (!DataAccessFacade.GetSubModalAccess().Exists("Emotion", "LALV"))
                DataAccessFacade.GetSubModalAccess().Add("Emotion", "LALV", "Low arousal Low Valence", "emotionmodel.svm");
            if (!DataAccessFacade.GetSubModalAccess().Exists("Emotion", "LAHV"))
                DataAccessFacade.GetSubModalAccess().Add("Emotion", "LAHV", "Low arousal High Valence", "emotionmodel.svm");
            if (!DataAccessFacade.GetSubModalAccess().Exists("Emotion", "HALV"))
                DataAccessFacade.GetSubModalAccess().Add("Emotion", "HALV", "High arousal Low Valence", "emotionmodel.svm");
            if (!DataAccessFacade.GetSubModalAccess().Exists("Emotion", "HAHV"))
                DataAccessFacade.GetSubModalAccess().Add("Emotion", "HAHV", "High arousal High Valence", "emotionmodel.svm");

            _type = type;
            _allsignalsList = new List<List<double[]>>();
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
            if (!_signalGet)
                return;

            double[] values = new double[2];
            var channelCounter = 0;
            foreach (var channel in e.Channels)
            {
                    if (channel.Position.Equals("F3"))
                    {
                        values[0] = channel.Value;
                        channelCounter++;
                    }
                    if (channel.Position.Equals("C4"))
                    {
                        values[1] = channel.Value;
                        channelCounter++;
                    }
            }
            if(channelCounter == 2)
                _signaList.Add(values);

            //if the window ends, sends the signal to process
            var sceneLocation = DataAccessFacade.GetSceneInUseAccess().GetLocation();
            if (_signaList.Count >= 0 
                && sceneLocation.HasValue 
                && _lastSecond != (int)sceneLocation.Value.TotalSeconds 
                && (int)sceneLocation.Value.TotalSeconds % 9 == 0)
            {
                _lastSecond = (int) sceneLocation.Value.TotalSeconds;
                switch (_type)
                {
                    case ClassifierType.Detector:
                        LearningModel.Classify(_signaList);
                        break;
                    case ClassifierType.Trainer:
                        _allsignalsList.Add(_signaList);
                        break;
                }
                _signaList = new List<double[]>();
            }
        }
    }
}
