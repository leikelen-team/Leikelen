using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels.PreProcessing
{
    public static class SampleEntropy
    {
        public static double CalcSampleEntropy(double[] data, int numSamples/*N*/, int wlen/*m*/, double r/*r*/, int shift)
        {
            int A = 0;
            int B = 0;
            int i, j, k;
            double m;

            /* ok, now we go through all windows data_i ... data_i+wlen and calculate the 
               Chebyshev distance to all the _following_ windows. As the distance is symmetric,
               d(i,j) = d(j,i), we only need to calculate the distance for less than half the pairs:

                D(i,j) (just calculate x)
                    - x x x x
                    - - x x x
                    - - - x x
                    - - - - x 
                    - - - - -
            */
            
            for (i = 0; i < numSamples - wlen * shift - shift; i += shift)
            {
                /* compare to all following windows > i */
                for (j = i + shift; j < numSamples - wlen * shift - shift; j += shift)
                {
                    m = 0; /* maximum so far */
                    for (k = 0; k < wlen; k++)
                        /* get max cheb. distance */
                        m = Math.Max(m, Rational.Abs(new Rational(data[i + k * shift] - data[j + k * shift])).Value);
                    /* first case, distance lower in first wlen positions */
                    if (m < r) B++;
                    /* Second case, distance lower if we add the next element */
                    if (Math.Max(m, Rational.Abs( new Rational(data[i + wlen * shift] - data[j + wlen * shift])).Value) < r) A++;
                }
            }
            /* return -log A/B */
            if (A > 0 && B > 0)
                return (-1 * Math.Log(((double)A) / ((double)B)));
            else
                return 0;
        }
    }
}
