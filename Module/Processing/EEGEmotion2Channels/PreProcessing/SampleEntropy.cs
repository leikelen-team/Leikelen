using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels.PreProcessing
{
    public static class SampleEntropy
    {
        /// <summary>
        /// Calculates the sample entropy.
        /// </summary>
        /// <param name="data">Input sequence (double). Nx1 in matlab.</param>
        /// <param name="numSamples">(N) Length of input.</param>
        /// <param name="wlen">(m) Length of window (normally called m).</param>
        /// <param name="r">Tolerance for "similarity".</param>
        /// <param name="shift">Shift between samples (for subsampling). Shift=1 corresponds to _no_ subsampling.</param>
        /// <returns>-log (A / B), where A is the number of |D_m(i,j) < r|, and B is |D_m+1(i,j) < r|.
        /// See wikipedia: https://en.wikipedia.org/wiki/Sample_entropy </returns>
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
                        m = Math.Max(m, DoubleAbs(data[i + k * shift] - data[j + k * shift]));
                    /* first case, distance lower in first wlen positions */
                    if (m < r) B++;
                    /* Second case, distance lower if we add the next element */
                    if (Math.Max(m, DoubleAbs(data[i + wlen * shift] - data[j + wlen * shift])) < r) A++;
                }
            }
            /* return -log A/B */
            if (A > 0 && B > 0)
                return (-1 * Math.Log(((double)A) / ((double)B)));
            else
                return 0;
        }

        public static double DoubleAbs(double value)
        {
            if(value < 0)
            {
                return value * -1;
            }
            else
            {
                return value;
            }
        }
    }
}
