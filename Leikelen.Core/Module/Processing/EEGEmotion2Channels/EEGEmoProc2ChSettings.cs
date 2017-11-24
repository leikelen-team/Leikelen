using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels
{
    public class EEGEmoProc2ChSettings : SettingsContainer
    {
        public readonly Option<int> SamplingHz = new Option<int>("SamplingHz", 128);
        public readonly Option<int> m = new Option<int>("m", 2);
        public readonly Option<double> r = new Option<double>("r", 0.15);
        public readonly Option<int> secs = new Option<int>("Seconds", 9);
        public readonly Option<int> N = new Option<int>("N", 128);
        public readonly Option<int> shift = new Option<int>("shift", 1);

        public readonly Option<int> TagToTrain = new Option<int>("TagToTrain", -1);

        private static EEGEmoProc2ChSettings _instance;

        public static EEGEmoProc2ChSettings Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new EEGEmoProc2ChSettings();
                return _instance;
            }
        }


        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"config/processing/emoeeg2ch.json");
        }
    }
}
