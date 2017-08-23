using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen
{
    public class GeneralSettings : SettingsContainer
    {
        public readonly Option<string> TmpDirectory = new Option<string>("TmpDirectory", "tmp/");
        public readonly Option<string> CurrentSceneDirectory = new Option<string>("CurrentSceneDirectory", "current_scene/");
        public readonly Option<string> Extension = new Option<string>("Extension", ".leikelen");
        public readonly Option<string> ExtensionFilter = new Option<string>("ExtensionFilter", "*.leikelen");

        public readonly Option<string> Database = new Option<string>("Database", "memory");
        public readonly Option<string> DbConectionString = new Option<string>("conectionString", null);

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
