using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.ProcessingModule;

namespace cl.uv.leikelen.ProcessingModule
{
    public class ProcessingLoader
    {
        public List<API.ProcessingModule.ProcessingModule> ProcessingModules { get; private set; }

        private static ProcessingLoader _instance;

        public static ProcessingLoader Instance
        {
            get
            {
                if (_instance == null) _instance = new ProcessingLoader();
                return _instance;
            }
        }

        private ProcessingLoader()
        {
            ProcessingModules = new List<API.ProcessingModule.ProcessingModule>();

            ProcessingModules.Add(new EEGEmotion2Channels.TrainerEntryPoint());
            ProcessingModules.Add(new EEGEmotion2Channels.DetectorEntryPoint());

            ProcessingModules.Add(new Kinect.Voice.VoiceEntryPoint());;
        }
    }
}
