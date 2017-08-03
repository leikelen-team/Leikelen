using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.EEG
{
    public class EegFrameArrivedEventArgs
    {
        public TimeSpan Time;
        public List<EegChannel> Channels;
    }

    public struct EegChannel
    {
        public string Position;
        public string PositionSystem;
        public double Value;
        public NotchType Notch;
        public FilterType Filter;
    }
}
