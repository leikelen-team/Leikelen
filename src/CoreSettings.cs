using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen.src
{
    public class CoreSettings : SettingsContainer
    {
        public readonly Option<int> maxIntervalGroupsInViewPerPerson;
        public readonly Option<int> pixelsPerSecond;
        public readonly Option<string> fileExtension;
        public readonly Option<string> fileFilter;
        public readonly Option<string> appDataFile;
        public readonly Option<string> tmpDirectory;
        public readonly Option<string> currentDataFile;
        public readonly Option<string> recordedZipFile;
        public readonly Option<string> currentKdvrFile;
        public readonly Option<string> currentSceneDirectory;

        public static CoreSettings _instance;

        public static CoreSettings Instance
        {
            get
            {
                if (_instance == null) _instance = new CoreSettings();
                return _instance;
            }
        }

        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"Config/coreConfig.json");
        }
    }
}
