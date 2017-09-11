using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.ProcessingModule;
using cl.uv.leikelen.API.FrameProvider.Kinect;
using Microsoft.Kinect;

namespace cl.uv.leikelen.ProcessingModule.Kinect.Voice
{
    public class VoiceEntryPoint : API.ProcessingModule.ProcessingModule, IKinectProcessingModule
    {
        private readonly VoiceLogic _logic;

        public VoiceEntryPoint()
        {
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            Name = "Voice";
            Plurality = ProcessingPlurality.Scene;
            
            _logic = new VoiceLogic();
        }

        public override Task FunctionAfterStop()
        {
            return new Task(_logic.StopRecording);
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
            return _logic._audioBeamReader_FrameArrived;
        }

        public EventHandler<KinectGestureFrameArrivedArgs> GestureListener()
        {
            return null;
        }
    }
}
