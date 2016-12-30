using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.true_model
{
    public class DiscretePostures
    {
        public int Id { get; set; }
        public int PersonDataId { get; set; }
        public PostureType postureType { get; set; }
        public bool isPosture { get; set; }
        public IntervalGroup discreteInterval { get; set; }
    }
}
