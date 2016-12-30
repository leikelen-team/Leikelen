using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.true_model
{
    public class PersonData
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person person { get; set; }
        public List<DiscretePostures> discretePostures { get; set; }
        public List<ContinuousPostures> continuousPostures { get; set; }
        public List<PersonJointsDistances> jointsDistances { get; set; }
        public double distanceEntropy { get; set; }
        public double distanceSum { get; set; }
        public Voice voice { get; set; }

        public PersonData() { }
    }
}
