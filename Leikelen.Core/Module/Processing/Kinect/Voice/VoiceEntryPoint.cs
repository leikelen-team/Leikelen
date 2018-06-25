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

/// <summary>
/// Voice processing Module from Kinect v2.
/// </summary>
namespace cl.uv.leikelen.Module.Processing.Kinect.Voice
{
    /// <summary>
    /// Entry point for Voice processing Module from Kinect v2.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Processing.ProcessingModule" />
    /// <seealso cref="cl.uv.leikelen.API.FrameProvider.Kinect.IKinectProcessingModule" />
    public class VoiceEntryPoint : ProcessingModule, IKinectProcessingModule
    {
        private readonly VoiceLogic _logic;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoiceEntryPoint"/> class.
        /// </summary>
        public VoiceEntryPoint()
        {
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            Name = "Voice";
            
            _logic = new VoiceLogic();
        }

        /// <summary>
        /// Functions called after the recorder stops.
        /// </summary>
        /// <returns>The function to be executed at stop recording</returns>
        public override Action FunctionAfterStop()
        {
            return _logic.StopRecording;
        }

        EventHandler<BodyFrameArrivedEventArgs> IKinectProcessingModule.BodyListener()
        {
            return null;
        }

        EventHandler<ColorFrameArrivedEventArgs> IKinectProcessingModule.ColorListener()
        {
            return null;
        }

        EventHandler<AudioBeamFrameArrivedEventArgs> IKinectProcessingModule.AudioListener()
        {
            return _logic._audioBeamReader_FrameArrived;
        }

        EventHandler<KinectGestureFrameArrivedArgs> IKinectProcessingModule.GestureListener()
        {
            return null;
        }
    }
}
