using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.ProcessingModule;
using cl.uv.leikelen.API.FrameProvider.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace cl.uv.leikelen.ProcessingModule.Kinect.Posture
{
    public class GestureEntryPoint : API.ProcessingModule.ProcessingModule, IKinectProcessingModule
    {
        private GestureLogic _logic;
        protected GestureEntryPoint()
        {
            Name = "Discrete Posture detector";
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            Plurality = ProcessingPlurality.Scene;
            
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
