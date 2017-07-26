using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cl.uv.leikelen.API.InputModule;

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
            Windows = new List<Tuple<string, Window>>();
            Windows.Add(new Tuple<string, Window>(Properties.OpenBCI.ConfigWindowTitle, new View.OpenBCIWindow(Monitor)));
        }
    }
}
