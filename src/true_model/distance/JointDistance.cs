using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace cl.uv.leikelen.src.true_model
{
    public class JointDistance
    {
        public int Id { get; set; }
        public JointType jointType { get; set; }
        public double value { get; set; }
        public int PersonJointsDistancesId { get; set; }
        public PersonJointsDistances personJointsDistances { get; set; }

        public JointDistance() { }
    }
}
