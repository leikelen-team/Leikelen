using cl.uv.leikelen.API.Module.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Module
{
    public class GeneralLoader
    {
        public List<GeneralModule> GeneralModules { get; private set; }
        public static event EventHandler GeneralModulesHasReset;

        private static GeneralLoader _instance;

        public static GeneralLoader Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new GeneralLoader();
                return _instance;
            }
        }

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
