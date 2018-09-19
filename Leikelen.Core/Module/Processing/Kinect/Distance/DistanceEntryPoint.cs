using cl.uv.leikelen.API.FrameProvider.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.Module.Processing;

/// <summary>
/// Processing module to calculate distance.
/// </summary>
namespace cl.uv.leikelen.Module.Processing.Kinect.Distance
{
    /// <summary>
    /// Entry point for processing module to calculate distance.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Processing.ProcessingModule" />
    /// <seealso cref="cl.uv.leikelen.API.FrameProvider.Kinect.IKinectProcessingModule" />
    public class DistanceEntryPoint : ProcessingModule, IKinectProcessingModule
    {
        private Module.Processing.Kinect.Distance.DistanceLogic _logic;
        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceEntryPoint"/> class.
        /// </summary>
        public DistanceEntryPoint()
        {
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            Name = "Distance";

            _logic = new DistanceLogic();
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
            return null;
        }

        EventHandler<API.FrameProvider.Kinect.KinectGestureFrameArrivedArgs> IKinectProcessingModule.GestureListener()
        {
            return null;
        }
    }
}
