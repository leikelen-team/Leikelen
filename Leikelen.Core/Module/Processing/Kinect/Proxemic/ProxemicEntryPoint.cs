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
/// Processing module that calculates the proxemic.
/// </summary>
namespace cl.uv.leikelen.Module.Processing.Kinect.Proxemic
{
    /// <summary>
    /// Entry point for processing module that calculates the proxemic.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Processing.ProcessingModule" />
    /// <seealso cref="cl.uv.leikelen.API.FrameProvider.Kinect.IKinectProcessingModule" />
    public class ProxemicEntryPoint : ProcessingModule, IKinectProcessingModule
    {
        private Module.Processing.Kinect.Proxemic.ProxemicLogic _logic;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxemicEntryPoint"/> class.
        /// </summary>
        public ProxemicEntryPoint()
        {
            Name = "Proxemic";

            _logic = new ProxemicLogic();
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
            return _logic.StopRecording;
        }

        EventHandler<API.FrameProvider.Kinect.KinectGestureFrameArrivedArgs> IKinectProcessingModule.GestureListener()
        {
            return null;
        }
    }
}
