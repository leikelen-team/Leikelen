using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.true_model
{
    public class BooleanEvent
    {
        public int Id { get; set; }
        public TimeSpan SceneLocationTime { get; set; }
        public Person Person { get; set; }

        public BooleanEvent() { }
    }
}
