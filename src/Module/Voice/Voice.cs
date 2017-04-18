using Microsoft.Kinect;
using System;

namespace cl.uv.leikelen.src.Module.Voice
{
    public class Voice : IModule
    {
        private VoiceLogic logic;

        public Voice()
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
