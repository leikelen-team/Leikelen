using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;

namespace cl.uv.leikelen.Module.Input.OpenBCI
{
    /// <summary>
    /// Settings of the openbci sensor.
    /// </summary>
    /// <seealso cref="Config.Net.SettingsContainer" />
    public class OpenBCISettings : SettingsContainer
    {
        /// <summary>
        /// The notch filter to use.
        /// </summary>
        public readonly Option<int> Notch = new Option<int>("Notch", 0);
        /// <summary>
        /// The filter to use.
        /// </summary>
        public readonly Option<int> Filter = new Option<int>("Filter", 0);

        /// <summary>
        /// The position of channel 1 using 10-10 or 10-20 system.
        /// </summary>
        public readonly Option<string> PositionChannel1 = new Option<string>("PositionChannel1", null);
        /// <summary>
        /// The position of channel 2 using 10-10 or 10-20 system.
        /// </summary>
        public readonly Option<string> PositionChannel2 = new Option<string>("PositionChannel2", null);
        /// <summary>
        /// The position of channel 3 using 10-10 or 10-20 system.
        /// </summary>
        public readonly Option<string> PositionChannel3 = new Option<string>("PositionChannel3", null);
        /// <summary>
        /// The position of channel 4 using 10-10 or 10-20 system.
        /// </summary>
        public readonly Option<string> PositionChannel4 = new Option<string>("PositionChannel4", null);
        /// <summary>
        /// The position of channel 5 using 10-10 or 10-20 system.
        /// </summary>
        public readonly Option<string> PositionChannel5 = new Option<string>("PositionChannel5", null);
        /// <summary>
        /// The position of channel 6 using 10-10 or 10-20 system.
        /// </summary>
        public readonly Option<string> PositionChannel6 = new Option<string>("PositionChannel6", null);
        /// <summary>
        /// The position of channel 7 using 10-10 or 10-20 system.
        /// </summary>
        public readonly Option<string> PositionChannel7 = new Option<string>("PositionChannel7", null);
        /// <summary>
        /// The position of channel 8 using 10-10 or 10-20 system.
        /// </summary>
        public readonly Option<string> PositionChannel8 = new Option<string>("PositionChannel8", null);

        private static Module.Input.OpenBCI.OpenBCISettings _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Module.Input.OpenBCI.OpenBCISettings Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new OpenBCISettings();
                return _instance;
            }
        }

        /// <summary>
        /// Called when [configure].
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"config/input/openbci.json");
        }
    }
}
