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

        public static void Reset()
        {
            _instance = null;
            _instance = new ProcessingLoader();
        }

        private ProcessingLoader()
        {
            ProcessingModules = new List<ProcessingModule>()
            {
                new Processing.EEGEmotion2Channels.DetectorEntryPoint(),

                new Processing.Kinect.Voice.VoiceEntryPoint(),
                new Processing.Kinect.Posture.GestureEntryPoint(),
                new Processing.Kinect.Distance.DistanceEntryPoint()
            };
        }
    }
}
