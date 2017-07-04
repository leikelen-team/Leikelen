using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.InputModule.OpenBCI
{
    public class AccelerometerFrameArrivedEventArgs
    {
        public TimeSpan Time;
        public double ZAxis;
        public double YAxis;
        public double XAxis;
        public string Place;
    }
}
