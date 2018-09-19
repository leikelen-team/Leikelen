using cl.uv.leikelen.API.Module.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The different modules.
/// </summary>
namespace cl.uv.leikelen.Module
{
    /// <summary>
    /// Gets the general modules, that are active all the time.
    /// </summary>
    public class GeneralLoader
    {
        /// <summary>
        /// Gets the general modules.
        /// </summary>
        /// <value>
        /// The general modules.
        /// </value>
        public List<API.Module.General.GeneralModule> GeneralModules { get; private set; }

        /// <summary>
        /// Occurs when [general modules has reset].
        /// </summary>
        public static event EventHandler GeneralModulesHasReset;

        private static Module.GeneralLoader _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Module.GeneralLoader Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new GeneralLoader();
                return _instance;
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public static void Reset()
        {
            _instance = null;
            _instance = new GeneralLoader();
            GeneralModulesHasReset?.Invoke(_instance, new EventArgs());
        }

        private GeneralLoader()
        {
            GeneralModules = new List<GeneralModule>()
            {
                new Processing.EEGEmotion2Channels.TrainerEntryPoint(),
                new General.GestureConfiguration.GestureConfigurationEntryPoint()
            };
        }
    }
}
