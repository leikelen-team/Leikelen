using Microsoft.Kinect;

namespace cl.uv.leikelen.src.Data.Model
{
    public class JointData
    {
        public int JointDataId { get; set; }
        public JointType JointType { get; set; }
        public double Value { get; set; }

        public JointArray JointArray { get; set; }

        public JointData() { }
    }
}
