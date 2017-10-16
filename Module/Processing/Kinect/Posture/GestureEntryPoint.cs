using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.Module.Processing;
using cl.uv.leikelen.API.FrameProvider.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace cl.uv.leikelen.Module.Processing.Kinect.Posture
{
    public class GestureEntryPoint : ProcessingModule, IKinectProcessingModule
    {
        private GestureLogic _logic;
        public GestureEntryPoint()
        {
            Name = "Posture detector";
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            
            _logic = new GestureLogic();
        }

        public EventHandler<AudioBeamFrameArrivedEventArgs> AudioListener()
        {
            return null;
        }

        public EventHandler<BodyFrameArrivedEventArgs> BodyListener()
        {
            return null;
        }

        public EventHandler<ColorFrameArrivedEventArgs> ColorListener()
        {
            return null;
        }

        public override Task FunctionAfterStop()
        {
            return new Task(_logic.StopRecording);
        }

        public EventHandler<KinectGestureFrameArrivedArgs> GestureListener()
        {
            return _logic.Gesture_FrameArrived;
        }
    }
}
