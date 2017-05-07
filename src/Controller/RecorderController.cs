using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API;
using cl.uv.leikelen.src.Module;
using cl.uv.leikelen.src.Input.Kinect; //TODO: generalizar inputs
using cl.uv.leikelen.src.View.Procedural;
using cl.uv.leikelen.src.Data;

namespace cl.uv.leikelen.src.Controller
{
    public class RecorderController
    {
        private List<IRecorder> _recorders;

        public RecorderController()
        {
            _recorders = new List<IRecorder>();
            _recorders.Add(KinectMediaFacade.Instance.Recorder);
        }

        public async Task Stop()
        {
            foreach(var recorder in _recorders)
            {
                await recorder.Stop();
            }
            foreach (var module in Loader.Modules)
            {
                if (module.FunctionAfterStop() != null)
                {
                    module.FunctionAfterStop();
                }
            }

            TimeLine.InitTimeLine(StaticScene.Instance.Duration);
        }

        public async Task Record()
        {
            foreach (var recorder in _recorders)
            {
                await recorder.Record();
            }
        }
    }
}
