using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.InputModule.OpenBCI
{
    public class EEGFrameArrivedEventArgs
    {
        public int NumberOfChannels;
        public NotchType Notch;
        public FilterType Filter;
        public double[] Data;
        public TimeSpan Time;
    }
}
