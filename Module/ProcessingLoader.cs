using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.Processing;

namespace cl.uv.leikelen.Module
{
    public class ProcessingLoader
    {
        public List<ProcessingModule> ProcessingModules { get; private set; }

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
            ProcessingModules = new List<ProcessingModule>();
            
            ProcessingModules.Add(new Processing.EEGEmotion2Channels.DetectorEntryPoint());

            ProcessingModules.Add(new Processing.Kinect.Voice.VoiceEntryPoint());
            ProcessingModules.Add(new Processing.Kinect.Posture.GestureEntryPoint());
            ProcessingModules.Add(new Processing.Kinect.Distance.DistanceEntryPoint());
        }
    }
}
