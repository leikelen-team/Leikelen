using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.Module.Processing;
using cl.uv.leikelen.API.FrameProvider.Kinect;

/// <summary>
/// Processing module to calculate angle of head.
/// </summary>
namespace cl.uv.leikelen.Module.Processing.Kinect.HeadAngle
{
    /// <summary>
    /// Entry point for processing module to calculate angle of head.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Processing.ProcessingModule" />
    /// <seealso cref="cl.uv.leikelen.API.FrameProvider.Kinect.IKinectProcessingModule" />
    public class HeadAngleEntryPoint : ProcessingModule, IKinectProcessingModule
    {
        private Module.Processing.Kinect.HeadAngle.HeadAngleLogic _logic;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadAngleEntryPoint"/> class.
        /// </summary>
        public HeadAngleEntryPoint()
        {
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            Name = "HeadAngle";

            _logic = new HeadAngleLogic();
        }

        EventHandler<AudioBeamFrameArrivedEventArgs> IKinectProcessingModule.AudioListener()
        {
            return null;
        }

        EventHandler<BodyFrameArrivedEventArgs> IKinectProcessingModule.BodyListener()
        {
            return _logic._bodyReader_FrameArrived;
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

        EventHandler<API.FrameProvider.Kinect.KinectGestureFrameArrivedArgs> IKinectProcessingModule.GestureListener()
        {
            return null;
        }
    }
}
