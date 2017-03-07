using System;

namespace cl.uv.leikelen.src.Data.Model
{
    public class Interval
    {
        public int IntervalId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public IntervalGroup IntervalGroup { get; set; }

        public Interval() { }

        public Interval(TimeSpan start, TimeSpan end, IntervalGroup intervalGroup)
        {
            this.StartTime = start;
            this.EndTime = end;
            this.IntervalGroup = intervalGroup;
        }
    }
}
