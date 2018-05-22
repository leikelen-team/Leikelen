using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen.Module.Input.Kinect
{
    /// <summary>
    /// Setting access for kinect input module
    /// </summary>
    /// <seealso cref="Config.Net.SettingsContainer" />
    public class KinectSettings : SettingsContainer
    {
        private static KinectSettings _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static KinectSettings Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new KinectSettings();
                return _instance;
            }
        }

        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"config/input/kinect.json");
        }
    }
}
