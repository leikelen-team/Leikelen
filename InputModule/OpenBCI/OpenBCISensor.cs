using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cl.uv.leikelen.API.InputModule;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.InputModule.OpenBCI
{
    public sealed class OpenBCISensor : API.InputModule.InputModule
    {
        public OpenBCISensor()
        {
            Plurality = InputPlurality.Person;
            Monitor = new Monitor();
            Name = Properties.OpenBCI.SensorName;
            Player = new Player();
            Windows = new List<Tuple<string, WindowBuilder>>();
            Windows.Add(new Tuple<string, WindowBuilder>(Properties.OpenBCI.ConfigWindowTitle, new WindowBuilder(new View.OpenBCIWindow(Monitor))));
        }
    }
}
