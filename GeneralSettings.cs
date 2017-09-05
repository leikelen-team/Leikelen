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
        public readonly Option<string> DataDirectory = new Option<string>("DataDirectory", "data/");
        public readonly Option<string> TmpDirectory = new Option<string>("TmpDirectory", "tmp/");
        public readonly Option<string> CurrentSceneDirectory = new Option<string>("CurrentSceneDirectory", "current_scene/");
        public readonly Option<string> Extension = new Option<string>("Extension", ".leikelen");
        public readonly Option<string> ExtensionFilter = new Option<string>("ExtensionFilter", "*.leikelen");

        public readonly Option<int> DefaultMillisecondsThreshold = new Option<int>("DefaultMilliseconds", 2000);

        public readonly Option<string> Database = new Option<string>("Database", "PostgreSQL");
        public readonly Option<string> DbHost = new Option<string>("DbHost", "localhost");
        public readonly Option<int> DbPort = new Option<int>("DbPort", 0);
        public readonly Option<string> DbName = new Option<string>("DbName", "leikelen");
        public readonly Option<string> DbUser = new Option<string>("DbUser", "postgres");
        public readonly Option<string> DbPassword = new Option<string>("DbPassword", "erick1992");


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
