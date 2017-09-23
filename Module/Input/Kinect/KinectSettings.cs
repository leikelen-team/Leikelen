using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen.Module.Input.Kinect
{
    public class KinectSettings : SettingsContainer
    {
        private static KinectSettings _instance;

        public static KinectSettings Instance
        {
            get
            {
                if (_instance == null) _instance = new KinectSettings();
                return _instance;
            }
        }

        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"config/input/kinect.json");
        }
    }
}
