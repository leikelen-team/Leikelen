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

        private static GeneralLoader _instance;

        public static GeneralLoader Instance
        {
            get
            {
                if (_instance == null) _instance = new GeneralLoader();
                return _instance;
            }
        }

        private GeneralLoader()
        {
            GeneralModules = new List<GeneralModule>();

            GeneralModules.Add(new Processing.EEGEmotion2Channels.TrainerEntryPoint());
        }
    }
}
