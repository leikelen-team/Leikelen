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

        public readonly Option<string> PositionChannel1;
        public readonly Option<string> PositionChannel2;
        public readonly Option<string> PositionChannel3;
        public readonly Option<string> PositionChannel4;
        public readonly Option<string> PositionChannel5;
        public readonly Option<string> PositionChannel6;
        public readonly Option<string> PositionChannel7;
        public readonly Option<string> PositionChannel8;

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
