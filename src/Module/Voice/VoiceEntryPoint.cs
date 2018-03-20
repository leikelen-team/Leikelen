using Microsoft.Kinect;
using System;
using cl.uv.leikelen.src.Input.Kinect;

namespace cl.uv.leikelen.src.Module.Voice
{
    public class VoiceEntryPoint : IModule, IKinectModule
    {
        private VoiceLogic logic;

        public VoiceEntryPoint()
        {
            logic = new VoiceLogic();
        }
        public bool BeforeRecording()
        {
            return false;
        }
        public EventHandler<BodyFrameArrivedEventArgs> BodyListener()
        {
            return null;
        }
        public EventHandler<ColorFrameArrivedEventArgs> ColorListener()
        {
            return null;
        }
        public EventHandler<AudioBeamFrameArrivedEventArgs> AudioListener()
        {
            return logic._audioBeamReader_FrameArrived;
        }

        public Action FunctionAfterStop()
        {
            return logic.StopRecording;
        }

        
    }
}
