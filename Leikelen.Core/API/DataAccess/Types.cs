using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    public struct Timeless
    {
        public double? Value;
        public string Subtitle;
        public int? Index;
    }

    public struct Event
    {
        public TimeSpan EventTime;
        public double? Value;
        public string Subtitle;
        public int? Index;
        public int toInterval;
    }

    public struct Interval
    {
        public TimeSpan StartTime;
        public TimeSpan EndTime;
        public double? Value;
        public string Subtitle;
        public int? Index;
    }
}
