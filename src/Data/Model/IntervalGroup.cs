using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data.Model
{
    public class IntervalGroup
    {
        public int SubModalTypeId { get; set; }
        public List<Interval> Intervals { get; set; }

        public SubModalType SubModalType { get; set; }

        public IntervalGroup()
        {
            this.Intervals = new List<Interval>();
        }

        public IntervalGroup(SubModalType submodalType)
        {
            this.SubModalType = submodalType;
            this.Intervals = new List<Interval>();
        }
    }
}
