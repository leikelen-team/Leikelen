using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.Module;

namespace cl.uv.leikelen.Controller
{
    public class RecorderController
    {
        public async Task Stop()
        {
            SceneInUse.Instance.IsRecording = false;
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                await input.Monitor.StopRecording();
            }
            foreach (var processingModule in ProcessingLoader.Instance.ProcessingModules)
            {
                if (!ReferenceEquals(null, processingModule.FunctionAfterStop()))
                {
                    await processingModule.FunctionAfterStop();
                }
            }
        }

        public async Task Record()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                if (input.Monitor.GetStatus() != API.Module.Input.InputStatus.Connected)
                    await input.Monitor.Open();
                await input.Monitor.StartRecording();
            }
            DateTime now = DateTime.Now;
            SceneInUse.Instance.Scene.RecordStartedDateTime = now;
            SceneInUse.Instance.IsRecording = true;
        }
    }
}
