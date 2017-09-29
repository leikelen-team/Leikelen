using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;
using cl.uv.leikelen.API.DataAccess;

namespace cl.uv.leikelen
{
    public class GeneralSettings : SettingsContainer, IGeneralSettings
    {
        public readonly Option<string> DataDirectory = new Option<string>("DataDirectory", "data/");
        public readonly Option<string> TmpDirectory = new Option<string>("SceneInUseDirectory", "tmp/current_scene/");
        public readonly Option<string> TmpSceneDirectory = new Option<string>("SceneInUseDirectory", "tmp/current_scene/");
        public readonly Option<string> Extension = new Option<string>("Extension", ".leikelen");
        public readonly Option<string> ExtensionFilter = new Option<string>("ExtensionFilter", "Leikelen scene file (*.leikelen)|*.leikelen");

        public readonly Option<int> DefaultMillisecondsThreshold = new Option<int>("DefaultMilliseconds", 2000);

        public readonly Option<string> Database = new Option<string>("Database", "PostgreSQL");
        public readonly Option<string> DbHost = new Option<string>("DbHost", "localhost");
        public readonly Option<int> DbPort = new Option<int>("DbPort", -1);
        public readonly Option<string> DbName = new Option<string>("DbName", "leikelen");
        public readonly Option<string> DbUser = new Option<string>("DbUser", "postgres");
        public readonly Option<string> DbPassword = new Option<string>("DbPassword", "erick1992");

        public readonly Option<int> IntervalsGraphMinHeight = new Option<int>("IntervalsGraphMinHeight", 60);
        public readonly Option<int> EventsGraphMinHeight = new Option<int>("EventsGraphMinHeight", 200);


        private static GeneralSettings _instance;

        public static GeneralSettings Instance
        {
            get
            {
                if (_instance == null) _instance = new GeneralSettings();
                return _instance;
            }
        }

        public GeneralSettings()
        {

        }

        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"config/general.json");
        }

        public string GetDataDirectory()
        {
            return DataDirectory.Value;
        }

        public string GetTmpDirectory()
        {
            return TmpDirectory.Value;
        }

        public int GetDefaultMillisecondsThreshold()
        {
            return DefaultMillisecondsThreshold.Value;
        }

        public string GetTmpSceneDirectory()
        {
            return TmpSceneDirectory.Value;
        }
    }
}
