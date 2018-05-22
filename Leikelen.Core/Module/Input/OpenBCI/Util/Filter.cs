using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.EEG;

namespace cl.uv.leikelen.Module.Input.OpenBCI.Util
{
    /// <summary>
    /// Filter the data of signal of OpenBCI input sensor.
    /// </summary>
    public class Filter
    {
        double[,] prev_x_notch = new double[8, 5];
        double[,] prev_y_notch = new double[8, 5];
        double[,] prev_x_standard = new double[8, 5];
        double[,] prev_y_standard = new double[8, 5];

        /// <summary>
        /// Filters the a given data input.
        /// </summary>
        /// <param name="filter">The filter type.</param>
        /// <param name="notch">The notch type.</param>
        /// <param name="data">The data input.</param>
        /// <param name="channelNumber">The channel number.</param>
        /// <returns>The filtered data of the signal.</returns>
        public double FiltersSelect(FilterType filter, NotchType notch, double data, int channelNumber)
        {
            double[] filterB;
            double[] filterA;
            double[] notchB;
            double[] notchA;
            switch (filter)
            {
                case FilterType.BandFilter1HzTo50Hz:
                    filterB = new double[5] { 0.2001387256580675, 0, -0.4002774513161350, 0, 0.2001387256580675 };
                    filterA = new double[5] { 1, -2.355934631131582, 1.941257088655214, -0.7847063755334187, 0.1999076052968340 };
                    break;
                case FilterType.BandFilter7HzTo13Hz:
                    filterB = new double[5] { 0.005129268366104263, 0, -0.01025853673220853, 0, 0.005129268366104263 };
                    filterA = new double[5] { 1, -3.678895469764040, 5.179700413522124, -3.305801890016702, 0.8079495914209149 };
                    break;
                case FilterType.BandFilter15HzTo50Hz:
                    filterB = new double[5] { 0.1173510367246093, 0, -0.2347020734492186, 0, 0.1173510367246093 };
                    filterA = new double[5] { 1, -2.137430180172061, 2.038578008108517, -1.070144399200925, 0.2946365275879138 };
                    break;
                case FilterType.BandFilter5HzTo50Hz:
                    filterB = new double[5] { 0.1750876436721012, 0, -0.3501752873442023, 0, 0.1750876436721012 };
                    filterA = new double[5] { 1, -2.299055356038497, 1.967497759984450, -0.8748055564494800, 0.2196539839136946 };
                    break;
                default:
                    filterB = new double[5] { 1, 1, 1, 1, 1 };
                    filterA = new double[5] { 1, 1, 1, 1, 1 };
                    break;
            }

            switch (notch)
            {
                case NotchType.Notch50Hz:
                    notchB = new double[5] { 0.96508099, -1.19328255, 2.29902305, -1.19328255, 0.96508099 };
                    notchA = new double[5] { 1, -1.21449347931898, 2.29780334191380, -1.17207162934772, 0.931381682126902 };
                    break;
                case NotchType.Notch60Hz:
                    notchB = new double[5] { 0.9650809863447347, -0.2424683201757643, 1.945391494128786, -0.2424683201757643, 0.9650809863447347 };
                    notchA = new double[5] { 1, -0.2467782611297853, 1.944171784691352, -0.2381583792217435, 0.9313816821269039 };
                    break;
                default:
                    notchB = new double[5] { 1, 1, 1, 1, 1 };
                    notchA = new double[5] { 1, 1, 1, 1, 1 };
                    break;

            }
            return filterIIR(notchA, notchB, filterA, filterB, data, channelNumber);
        }

        private double filterIIR(double[] notchA, double[] notchB, double[] filterA, double[] filterB, double data, int channelNumber)
        {
            for (int j = 5 - 1; j > 0; j--)
            {
                prev_x_notch[channelNumber, j] = prev_x_notch[channelNumber, j - 1];
                prev_y_notch[channelNumber, j] = prev_y_notch[channelNumber, j - 1];
                prev_x_standard[channelNumber, j] = prev_x_standard[channelNumber, j - 1];
                prev_y_standard[channelNumber, j] = prev_y_standard[channelNumber, j - 1];
            }
            prev_x_notch[channelNumber, 0] = data;

            double gain = 0;
            for (int j = 0; j < 5; j++)
            {
                gain += notchB[j] * prev_x_notch[channelNumber, j];
                if (j > 0)
                {
                    gain -= notchA[j] * prev_y_notch[channelNumber, j];
                }
            }
            prev_y_notch[channelNumber, 0] = gain;
            prev_x_standard[channelNumber, 0] = gain;
            gain = 0;
            for (int j = 0; j < 5; j++)
            {
                gain += filterB[j] * prev_x_standard[channelNumber, j];
                if (j > 0)
                {
                    gain -= filterA[j] * prev_y_standard[channelNumber, j];
                }
            }
            prev_y_standard[channelNumber, 0] = gain;
            return gain;
        }
    }
}
