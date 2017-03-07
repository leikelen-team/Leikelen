using System;

namespace cl.uv.leikelen.src.Data.Model
{
    public class PointInTime
    {
        public int PointInTimeId { get; set; }
        public TimeSpan Time { get; set; }
        public double Value { get; set; }

        public Graph Graph { get; set; }

        public PointInTime() { }
    }
}
