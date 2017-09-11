using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels.PreProcessing;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels
{
    public class LearningModel
    {
        public static TagType Classify(List<double[]> signalsList)
        {
            var featureVector = PreProcess(signalsList);
            //TODO: esto es temporal, hay que clasificar
            return 0;
        }

        public static void Train(Dictionary<TagType, List<List<double[]>>> allsignalsList)
        {
            foreach (var tag in allsignalsList.Keys)
            {
                foreach(var signalList in allsignalsList[tag])
                {
                    var featureVector = PreProcess(signalList);
                }
            }
        }

        private static List<double> PreProcess(List<double[]> signalsList)
        {
            double[] f3 = new double[signalsList.Count];
            for (int i = 0; i < f3.Length;i++)
            {
                f3[i] = signalsList[i][0];
            }

            double[] c4 = new double[signalsList.Count];
            for (int i = 0; i < c4.Length; i++)
            {
                c4[i] = signalsList[i][1];
            }

            var emdF3 = new Emd();
            var imfsF3 = emdF3.GetImfs(f3, 4, 1, 0);

            var emdC4 = new Emd();
            var imfsC4 = emdC4.GetImfs(c4, 4, 1, 0);

            List<double> features = new List<double>();
            foreach (var imfF3 in imfsF3)
            {
                features.Add(SampleEntropy.CalcSampleEntropy(imfF3, 1152, EEGEmoProc2ChSettings.Instance.m,
                    EEGEmoProc2ChSettings.Instance.r, 0));
            }

            foreach (var imfC4 in imfsC4)
            {
                features.Add(SampleEntropy.CalcSampleEntropy(imfC4, 1152, EEGEmoProc2ChSettings.Instance.m,
                    EEGEmoProc2ChSettings.Instance.r, 0));
            }
            Console.WriteLine(features);
            return features;
        }
    }
}
