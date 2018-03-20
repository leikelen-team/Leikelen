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
            SceneInUse.Instance.Scene.Duration = DateTime.Now.Subtract(SceneInUse.Instance.Scene.RecordStartedDateTime);
            SceneInUse.Instance.IsRecording = false;
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                if(input.IsEnabled)
                    await input.Monitor.StopRecording();
            }
            foreach(var person in InputLoader.Instance.PersonInputModules.Keys)
            {
                foreach(var personInput in InputLoader.Instance.PersonInputModules[person])
                {
                    if(personInput.IsEnabled)
                        await personInput.Monitor.StopRecording();
                }
            }
            Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(SceneInUse.Instance.Scene);
            foreach (var processingModule in ProcessingLoader.Instance.ProcessingModules)
            {
                if (!ReferenceEquals(null, processingModule.FunctionAfterStop()))
                {
                    if(processingModule.IsEnabled)
                        processingModule.FunctionAfterStop().Invoke();
                }
            }
        }

        public async Task Record()
        {
            Data.Access.External.ModalAccess.LoadTmpModals();
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                if (input.IsEnabled)
                {
                    if (input.Monitor.GetStatus() != API.Module.Input.InputStatus.Connected)
                        await input.Monitor.Open();
                    await input.Monitor.StartRecording();
                }
            }
            foreach (var person in InputLoader.Instance.PersonInputModules.Keys)
            {
                foreach (var personInput in InputLoader.Instance.PersonInputModules[person])
                {
                    if (personInput.IsEnabled)
                    {
                        if (personInput.Monitor.GetStatus() != API.Module.Input.InputStatus.Connected)
                            await personInput.Monitor.Open();
                        await personInput.Monitor.StartRecording();
                    }
                }
            }
            DateTime now = DateTime.Now;
            SceneInUse.Instance.Scene.RecordStartedDateTime = now;
            SceneInUse.Instance.IsRecording = true;
        }
    }
}
