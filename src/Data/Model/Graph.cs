using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data.Model
{
    public class Graph
    {
        public int SubModalTypeId { get; set; }
        public List<PointInTime> Points { get; set; }

        public SubModalType SubModalType { get; set; }
    }
}
