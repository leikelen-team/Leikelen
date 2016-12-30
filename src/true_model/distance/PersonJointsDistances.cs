using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.true_model
{
    public class PersonJointsDistances
    {
        public int Id { get; set; }
        public int PersonDataId { get; set; }
        public DistanceType distanceType { get; set; }
        public DistanceInferredType distanceInferredType { get; set; }
        public List<JointDistance> jointDistances { get; set; }

        public PersonJointsDistances() { }
    }
}
