using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.Module.Processing;
using cl.uv.leikelen.API.FrameProvider.Kinect;

namespace cl.uv.leikelen.Module.Processing.Kinect.HeadAngle
{
    public class HeadAngleEntryPoint : ProcessingModule, IKinectProcessingModule
    {
        public HeadAngleLogic _logic;

        public HeadAngleEntryPoint()
        {
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            Name = "HeadAngle";

            _logic = new HeadAngleLogic();
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
