using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.Module.Processing;
using cl.uv.leikelen.API.FrameProvider.Kinect;
using Microsoft.Kinect;

namespace cl.uv.leikelen.Module.Processing.Kinect.Voice
{
    public class VoiceEntryPoint : ProcessingModule, IKinectProcessingModule
    {
        private readonly VoiceLogic _logic;

        public VoiceEntryPoint()
        {
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            Name = "Voice";
            
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
