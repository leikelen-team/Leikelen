using Microsoft.Kinect.VisualGestureBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.Kinect
{
    /// <summary>
    /// Class that represent a specific frame of kinect gestures, associated with a person
    /// </summary>
    public class KinectGestureFrameArrivedArgs
    {
        /// <summary>
        /// The time of current frame
        /// </summary>
        public TimeSpan? Time;
        /// <summary>
        /// The tracking identifier of the person which this frame belongs to
        /// </summary>
        public ulong TrackingId;
        /// <summary>
        /// Current gesture frame
        /// </summary>
        public VisualGestureBuilderFrame Frame;
    }
}
