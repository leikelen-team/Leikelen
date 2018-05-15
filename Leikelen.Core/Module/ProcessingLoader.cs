using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.Processing;

namespace cl.uv.leikelen.Module
{
    /// <summary>
    /// Gets the processing modules that runs after stops the recording
    /// </summary>
    public class ProcessingLoader
    {
        /// <summary>
        /// Gets the processing modules.
        /// </summary>
        /// <value>
        /// The processing modules.
        /// </value>
        public List<ProcessingModule> ProcessingModules { get; private set; }

        /// <summary>
        /// Occurs when [processing modules has reset].
        /// </summary>
        public static event EventHandler ProcessingModulesHasReset;

        private static ProcessingLoader _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ProcessingLoader Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new ProcessingLoader();
                return _instance;
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public static void Reset()
        {
            _instance = null;
            _instance = new ProcessingLoader();
            ProcessingModulesHasReset?.Invoke(_instance, new EventArgs());
        }

        private ProcessingLoader()
        {
            ProcessingModules = new List<ProcessingModule>()
            {
                new Processing.EEGEmotion2Channels.DetectorEntryPoint(),

                new Processing.Kinect.Voice.VoiceEntryPoint(),
                new Processing.Kinect.Posture.GestureEntryPoint(),
                //new Processing.Kinect.Distance.DistanceEntryPoint(),
                new Processing.Kinect.HeadAngle.HeadAngleEntryPoint(),
                new Processing.Kinect.ShouldersAngle.ShouldersAngleEntryPoint(),
                new Processing.Kinect.Proxemic.ProxemicEntryPoint(),
                new Processing.Kinect.AccurateProxemic.AccurateProxemicEntryPoint(),
                new Processing.EEGPrint.EegPrintEntryPoint()
            };
        }
    }
}
