using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.InputModule;
using cl.uv.leikelen.ProcessingModule;

namespace cl.uv.leikelen.Controller
{
    public class RecorderController
    {
        public async Task Stop()
        {
            SceneInUse.Instance.IsRecording = false;
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Monitor.StopRecording();
            }
            foreach (var processingModule in ProcessingLoader.Instance.ProcessingModules)
            {
                if (processingModule.FunctionAfterStop() != null)
                {
                    processingModule.FunctionAfterStop();
                }
            }
        }

        public async Task Record()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                await input.Monitor.StartRecording();
            }
            DateTime now = DateTime.Now;
            SceneInUse.Instance.Scene.RecordStartedDateTime = now;
            SceneInUse.Instance.IsRecording = true;
        }
    }
}
