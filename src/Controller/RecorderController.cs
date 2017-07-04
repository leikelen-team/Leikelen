using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.Data.Access.Internal;
using cl.uv.leikelen.src.InputModule;
using cl.uv.leikelen.src.ProcessingModule;

namespace cl.uv.leikelen.src.Controller
{
    public class RecorderController
    {
        public bool IsRecording = false;

        public TimeSpan? getLocation()
        {
            if (IsRecording) return DateTime.Now.Subtract(SceneInUse.Instance.Scene.RecordStartedDateTime);
            else return null;
        }

        public async Task Stop()
        {
            IsRecording = false;
            foreach (var input in InputLoader.Instance.InputModules)
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
            
            foreach (var input in InputLoader.Instance.InputModules)
            {
                input.Monitor.StartRecording();
            }
            DateTime now = DateTime.Now;
            string sceneName = now.ToString("yyyy-MM-dd _ hh-mm-ss");
            SceneInUse.Instance.Set(new Data.Model.Scene() { Name = sceneName, RecordStartedDateTime = now });
            IsRecording = true;
        }
    }
}
