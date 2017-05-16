using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.Module;
using cl.uv.leikelen.src.Input;
using cl.uv.leikelen.src.View.Widget;
using cl.uv.leikelen.src.Data;

namespace cl.uv.leikelen.src.Controller
{
    public class RecorderController
    {
        public bool IsRecording = false;
        public TimeSpan? getLocation()
        {
            if (IsRecording) return DateTime.Now.Subtract(StaticScene.Instance.RecordStartDate);
            else return null;
        }
        public async Task Stop()
        {
            IsRecording = false;
            foreach (var input in InputLoader.Instance.Inputs)
            {
                await input.Monitor.StopRecording();
            }
            foreach (var module in ModuleLoader.Instance.Modules)
            {
                if (module.FunctionAfterStop() != null)
                {
                    module.FunctionAfterStop();
                }
            }


            
        }

        public async Task Record()
        {
            IsRecording = true;
            foreach (var input in InputLoader.Instance.Inputs)
            {
                await input.Monitor.StartRecording();
            }
            string sceneName = DateTime.Now.ToString("yyyy-MM-dd _ hh-mm-ss");
            StaticScene.CreateSceneFromRecord(sceneName);
        }
    }
}
