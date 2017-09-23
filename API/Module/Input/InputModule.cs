using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.API.Module.Input
{
    public abstract class InputModule : AbstractModule
    {
        public InputPlurality Plurality;
        public IMonitor Monitor;
        public IPlayer Player;
    }
}
