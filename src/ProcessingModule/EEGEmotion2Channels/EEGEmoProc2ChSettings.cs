using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels
{
    public class EEGEmoProc2ChSettings : SettingsContainer
    {
        public readonly Option<int> m;
        public readonly Option<int> r;
        public readonly Option<int> secs;
        public readonly Option<int> SamplingHz;

        private static EEGEmoProc2ChSettings _instance;

        public static EEGEmoProc2ChSettings Instance
        {
            get
            {
                if (_instance == null) _instance = new EEGEmoProc2ChSettings();
                return _instance;
            }
        }


        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"config/processing/emoeeg2ch.json");
        }
    }
}
