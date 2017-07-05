using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen.src
{
    public class GeneralSettings : SettingsContainer
    {
        public readonly Option<string> TmpDirectory;
        public readonly Option<string> CurrentSceneDirectory;

        private static GeneralSettings _instance;

        public static GeneralSettings Instance
        {
            get
            {
                if (_instance == null) _instance = new GeneralSettings();
                return _instance;
            }
        }

        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"config/general.json");
        }
    }
}
