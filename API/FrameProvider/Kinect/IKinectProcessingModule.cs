using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace cl.uv.leikelen.API.FrameProvider.Kinect
{
    /// <summary>
    /// Provides listeners to kinect frames
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.InputModule.Kinect.KinectInput"/>
    public interface IKinectProcessingModule
    {
        /// <summary>
        /// Returns the handler for the body frame, that contains each body
        /// with its joints, position, angle, and others.
        /// </summary>
        /// <returns>The event handler of the body frame</returns>
        EventHandler<BodyFrameArrivedEventArgs> BodyListener();
        /// <summary>
        /// Returns the handler for the color frame, that contains
        /// the image of the camera.
        /// </summary>
        /// <returns>The event handler of the color frame</returns>
        EventHandler<ColorFrameArrivedEventArgs> ColorListener();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The event handler of the audio frame</returns>
        EventHandler<AudioBeamFrameArrivedEventArgs> AudioListener();
    }
}
