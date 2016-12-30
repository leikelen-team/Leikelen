using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.true_model
{
    public class Interval
    {
        public int Id { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public int IntervalGroupId { get; }
        public IntervalGroup intervalGroup { get; }

        public Interval() { }
    }
}
