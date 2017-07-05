using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.API.FrameProvider.EEG
{
    public class EEGFrameArrivedEventArgs
    {
        public TimeSpan Time;
        public List<EEGChannel> Channels;
    }

    public struct EEGChannel
    {
        public string Position;
        public string PositionSystem;
        public double Value;
        public NotchType Notch;
        public FilterType Filter;
    }
}
