using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen.Module.Input.OpenBCI
{
    public class OpenBCISettings : SettingsContainer
    {
        public readonly Option<int> Notch = new Option<int>("Notch", 0);
        public readonly Option<int> Filter = new Option<int>("Filter", 0);

        public readonly Option<string> PositionChannel1 = new Option<string>("PositionChannel1", null);
        public readonly Option<string> PositionChannel2 = new Option<string>("PositionChannel2", null);
        public readonly Option<string> PositionChannel3 = new Option<string>("PositionChannel3", null);
        public readonly Option<string> PositionChannel4 = new Option<string>("PositionChannel4", null);
        public readonly Option<string> PositionChannel5 = new Option<string>("PositionChannel5", null);
        public readonly Option<string> PositionChannel6 = new Option<string>("PositionChannel6", null);
        public readonly Option<string> PositionChannel7 = new Option<string>("PositionChannel7", null);
        public readonly Option<string> PositionChannel8 = new Option<string>("PositionChannel8", null);

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
