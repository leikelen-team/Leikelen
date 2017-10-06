using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels.PreProcessing
{
    public class FilterRLC
    {
        public static double[] LCLP(double[] input, int sampleRate, double frequency, double Q)
        {
            double O = 2.0 * Math.PI * frequency / sampleRate;
            double C = Q / O;
            double L = 1 / Q / O;
            double V = 0, I = 0, T;
            double[] output = new double[input.Length];
            for (int s = 0; s < input.Length; s++)
            {
                T = (I - V) / C;
                I += (input[s] * O - V) / L;
                V += T;
                output[s] = V / O;
            }
            return output;
        }

        public static double[] LCHP(double[] input, int sampleRate, double Frequency, double Q)
        {
            double O = 2.0 * Math.PI * Frequency / sampleRate;
            double C = Q / O;
            double L = 1 / Q / O;
            double V = 0, I = 0, T;
            double[] output = new double[input.Length];
            for (int s = 0; s < input.Length; s++)
            {
                T = input[s] * O - V;
                V += (I + T) / C;
                I += T / L;
                output[s] -= V / O;
            }
            return output;
        }
    }
}
