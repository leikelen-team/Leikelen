using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.Module.Processing;
using cl.uv.leikelen.API.FrameProvider.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

/// <summary>
/// Processing module to calculates the postures.
/// </summary>
namespace cl.uv.leikelen.Module.Processing.Kinect.Posture
{
    /// <summary>
    /// Entry point for processing module to calculates the postures.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Processing.ProcessingModule" />
    /// <seealso cref="cl.uv.leikelen.API.FrameProvider.Kinect.IKinectProcessingModule" />
    public class GestureEntryPoint : ProcessingModule, IKinectProcessingModule
    {
        private GestureLogic _logic;
        /// <summary>
        /// Initializes a new instance of the <see cref="GestureEntryPoint"/> class.
        /// </summary>
        public GestureEntryPoint()
        {
            Name = "Posture detector";
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            
            _logic = new GestureLogic();
        }

        EventHandler<AudioBeamFrameArrivedEventArgs> IKinectProcessingModule.AudioListener()
        {
            return null;
        }

        EventHandler<BodyFrameArrivedEventArgs> IKinectProcessingModule.BodyListener()
        {
            return null;
        }

        EventHandler<ColorFrameArrivedEventArgs> IKinectProcessingModule.ColorListener()
        {
            return null;
        }

        /// <summary>
        /// Functions called after the recorder stops.
        /// </summary>
        /// <returns>
        /// The function to be executed at stop recording
        /// </returns>
        public override Action FunctionAfterStop()
        {
            return _logic.StopRecording;
        }

        EventHandler<KinectGestureFrameArrivedArgs> IKinectProcessingModule.GestureListener()
        {
            return _logic.Gesture_FrameArrived;
        }
    }
}
