using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.src.kinectmedia;

namespace cl.uv.leikelen.src.Module.Distance
{
    public class DistanceLogic
    {
        private List<Tuple<TimeSpan, Body[]>> bodiesInAllFrames;

        public DistanceLogic()
        {
            bodiesInAllFrames = new List<Tuple<TimeSpan, Body[]>>();
        }

        public void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    Body[] bodiesInFrame = new Body[frame.BodyCount];
                    frame.GetAndRefreshBodyData(bodiesInFrame);
                    //TODO: quitar kinectmediafacade de aqui y hacerlo de otra forma
                    bodiesInAllFrames.Add(new Tuple<TimeSpan, Body[]>(KinectMediaFacade.Instance.Recorder.getLocation().Value, bodiesInFrame));
                }
            }
        }
    }
}
