using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cl.uv.leikelen.Helper;

namespace cl.uv.leikelen.API.InputModule
{
    public abstract class InputModule
    {
        public InputPlurality Plurality;
        public IMonitor Monitor;
        public IPlayer Player;
        public string Name;
        public List<Tuple<string, WindowBuilder>> Windows;
    }
}
