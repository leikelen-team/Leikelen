using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.true_model
{
    public class ContinuousPostures
    {
        public int Id { get; set; }
        public int PersonDataId { get; set; }
        public List<ValueEvent> progressList { get; set; }
        public PostureType postureType { get; set; }
    }
}
