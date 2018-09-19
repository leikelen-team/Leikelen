using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels.PreProcessing
{
    public class FilterButterworth
    {
        /// <summary>
        /// resonance amount, from sqrt(2) to ~ 0.1
        /// </summary>
        private readonly float resonance;

        private readonly float frequency;
        private readonly int sampleRate;
        private readonly PassType passType;

        private readonly float c, a1, a2, a3, b1, b2;

        /// <summary>
        /// Array of input values, latest are in front
        /// </summary>
        private double[] inputHistory = new double[2];

        /// <summary>
        /// Array of output values, latest are in front
        /// </summary>
        private double[] outputHistory = new double[3];

        public FilterButterworth(float frequency, int sampleRate, PassType passType, float resonance)
        {
            this.resonance = resonance;
            this.frequency = frequency;
            this.sampleRate = sampleRate;
            this.passType = passType;

            switch (passType)
            {
                case PassType.Lowpass:
                    c = 1.0f / (float)Math.Tan(Math.PI * frequency / sampleRate);
                    a1 = 1.0f / (1.0f + resonance * c + c * c);
                    a2 = 2f * a1;
                    a3 = a1;
                    b1 = 2.0f * (1.0f - c * c) * a1;
                    b2 = (1.0f - resonance * c + c * c) * a1;
                    break;
                case PassType.Highpass:
                    c = (float)Math.Tan(Math.PI * frequency / sampleRate);
                    a1 = 1.0f / (1.0f + resonance * c + c * c);
                    a2 = -2f * a1;
                    a3 = a1;
                    b1 = 2.0f * (c * c - 1.0f) * a1;
                    b2 = (1.0f - resonance * c + c * c) * a1;
                    break;
            }
        }

        public enum PassType
        {
            Highpass,
            Lowpass,
        }

        public double Update(double newInput)
        {
            double newOutput = a1 * newInput + a2 * this.inputHistory[0] + a3 * this.inputHistory[1] - b1 * this.outputHistory[0] - b2 * this.outputHistory[1];

            this.inputHistory[1] = this.inputHistory[0];
            this.inputHistory[0] = newInput;

            this.outputHistory[2] = this.outputHistory[1];
            this.outputHistory[1] = this.outputHistory[0];
            this.outputHistory[0] = newOutput;
            return newOutput;
        }

        public double Value
        {
            get { return this.outputHistory[0]; }
        }
    }
}
