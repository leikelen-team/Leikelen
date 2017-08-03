using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.EEG;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels
{
    public class EEGReceiver
    {
        private List<double[]> _signaList;
        private List<List<double[]>> _allsignalsList;
        private bool _signalGet;
        private readonly ClassifierType _type;

        public EEGReceiver(ClassifierType type)
        {
            _type = type;
            _allsignalsList = new List<List<double[]>>();
            _signaList = new List<double[]>();
            _signalGet = true;
        }

        public void DataReceiver(object sender, EegFrameArrivedEventArgs e)
        {
            if (EEGEmoProc2ChSettings.Instance.SamplingHz == 256)
            {
                _signalGet = true;
            }
            else if (EEGEmoProc2ChSettings.Instance.SamplingHz == 128)
            {
                _signalGet = !_signalGet;
            }
            double[] values = new double[2];
            var channelCounter = 0;
            foreach (var channel in e.Channels)
            {
                if (_signalGet)
                {
                    if (channel.Position == "F3")
                    {
                        values[0] = channel.Value;
                        channelCounter++;
                    }
                    if (channel.Position == "C4")
                    {
                        values[1] = channel.Value;
                        channelCounter++;
                    }
                }
            }
            if(channelCounter == 2)
                _signaList.Add(values);

            if (_signaList.Count >= EEGEmoProc2ChSettings.Instance.SamplingHz * EEGEmoProc2ChSettings.Instance.secs)
            {
                //TODO: arreglar cada 9 segundos
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

        public void Train()
        {
            LearningModel.Train(_allsignalsList);
            _allsignalsList = new List<List<double[]>>();
        }
    }
}
