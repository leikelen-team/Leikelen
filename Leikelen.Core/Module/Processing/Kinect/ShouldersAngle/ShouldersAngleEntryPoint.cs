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
/// Processing module that calculate angle of shoulders.
/// </summary>
namespace cl.uv.leikelen.Module.Processing.Kinect.ShouldersAngle
{
    /// <summary>
    /// Entry point for processing module that calculate angle of shoulders.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Processing.ProcessingModule" />
    /// <seealso cref="cl.uv.leikelen.API.FrameProvider.Kinect.IKinectProcessingModule" />
    public class ShouldersAngleEntryPoint : ProcessingModule, IKinectProcessingModule
    {
        public Module.Processing.Kinect.ShouldersAngle.ShouldersAngleLogic _logic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShouldersAngleEntryPoint"/> class.
        /// </summary>
        public ShouldersAngleEntryPoint()
        {
            Name = "Shoulders Angle";
            _logic = new ShouldersAngleLogic();
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
        /// <returns>The function to be executed at stop recording</returns>
        public override Action FunctionAfterStop()
        {
            return null;
        }

        EventHandler<API.FrameProvider.Kinect.KinectGestureFrameArrivedArgs> IKinectProcessingModule.GestureListener()
        {
            return null;
        }
    }
}
