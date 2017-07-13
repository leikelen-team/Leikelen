using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cl.uv.leikelen.src.API.InputModule
{
    public abstract class InputType
    {
        public InputPlurality Plurality;
        public IMonitor Monitor;
        public IPlayer Player;
        public string Name;
        public List<Tuple<string, Window>> Windows;
    }
}
