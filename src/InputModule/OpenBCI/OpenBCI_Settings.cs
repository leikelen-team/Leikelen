using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen.src.InputModule.OpenBCI
{
    public class OpenBCI_Settings : SettingsContainer
    {
        public readonly Option<int> Notch;
        public readonly Option<int> Filter;

        private static OpenBCI_Settings _instance;

        public static OpenBCI_Settings Instance
        {
            get
            {
                if (_instance == null) _instance = new OpenBCI_Settings();
                return _instance;
            }
        }

        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"config/input/openbci.json");
        }
    }
}
