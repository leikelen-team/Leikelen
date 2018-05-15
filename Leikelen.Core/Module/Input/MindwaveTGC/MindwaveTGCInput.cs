using cl.uv.leikelen.API.Module.Input;
using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Module.Input.MindwaveTGC
{
    public class MindwaveTGCInput : InputModule
    {
        public MindwaveTGCInput(Person person)
        {
            Plurality = InputPlurality.Person;
            var myMonitor = new Monitor(person);
            Monitor = myMonitor as IMonitor;
            Name = "Mindwave";
            Player = new Player();

        }
    }
}
