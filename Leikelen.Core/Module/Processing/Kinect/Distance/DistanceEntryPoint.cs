using cl.uv.leikelen.API.FrameProvider.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.Module.Processing;

namespace cl.uv.leikelen.Module.Processing.Kinect.Distance
{
    public class DistanceEntryPoint : ProcessingModule, IKinectProcessingModule
    {
        private DistanceLogic _logic;

        public DistanceEntryPoint()
        {
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            Name = "Distance";

            _logic = new DistanceLogic();
        }

        public EventHandler<AudioBeamFrameArrivedEventArgs> AudioListener()
        {
            return null;
        }

        public EventHandler<BodyFrameArrivedEventArgs> BodyListener()
        {
            return _logic._bodyReader_FrameArrived;
        }

        public EventHandler<ColorFrameArrivedEventArgs> ColorListener()
        {
            return null;
        }

        public override Action FunctionAfterStop()
        {
            return null;
        }

        public EventHandler<KinectGestureFrameArrivedArgs> GestureListener()
        {
            return null;
        }
    }
}
