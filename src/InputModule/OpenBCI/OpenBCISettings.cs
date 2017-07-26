using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen.InputModule.OpenBCI
{
    public class OpenBCISettings : SettingsContainer
    {
        public readonly Option<int> Notch;
        public readonly Option<int> Filter;

        private static OpenBCISettings _instance;

        public static OpenBCISettings Instance
        {
            get
            {
                if (_instance == null) _instance = new OpenBCISettings();
                return _instance;
            }
        }

        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"config/input/openbci.json");
        }
    }
}
