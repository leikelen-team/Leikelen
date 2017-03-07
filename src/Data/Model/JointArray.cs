using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data.Model
{
    public class JointArray
    {
        public int SubModalTypeId { get; set; }
        public List<JointData> JointDatas { get; set; }

        public SubModalType SubModalType { get; set; }
    }
}
