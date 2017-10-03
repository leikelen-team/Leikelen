using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using KinectEx;
using KinectEx.DVR;

namespace cl.uv.leikelen.Module.Input.Kinect.DVR
{
    public class ReplayBodyFrameCustomTime : ReplayBodyFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayBodyFrameCustomTime"/> class
        /// based on the specified <c>BodyFrame</c>.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public ReplayBodyFrameCustomTime(BodyFrame frame, TimeSpan relativeTime)
        {
            this.FrameType = FrameTypes.Body;
            this.RelativeTime = relativeTime;
            this.BodyCount = frame.BodyCount;
            this.FloorClipPlane = frame.FloorClipPlane;
            this.Bodies = new List<CustomBody>(frame.BodyCount);
            frame.GetAndRefreshBodyData(this.Bodies);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayBodyFrameCustomTime"/> class
        /// based on the specified <c>BodyFrame</c> and list of <c>CustomBody</c> objects.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="bodies">The bodies.</param>
        public ReplayBodyFrameCustomTime(BodyFrame frame, List<CustomBody> bodies, TimeSpan relativeTime)
        {
            this.FrameType = FrameTypes.Body;
            this.RelativeTime = relativeTime;
            this.BodyCount = frame.BodyCount;
            this.FloorClipPlane = frame.FloorClipPlane;
            this.Bodies = bodies;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayBodyFrameCustomTime"/> class
        /// based on the specified <c>BodyFrame</c> and array of <c>Body</c> objects.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="bodies">The bodies.</param>
        public ReplayBodyFrameCustomTime(BodyFrame frame, Body[] bodies, TimeSpan relativeTime)
        {
            this.FrameType = FrameTypes.Body;
            this.RelativeTime = relativeTime;
            this.BodyCount = frame.BodyCount;
            this.FloorClipPlane = frame.FloorClipPlane;
            var bodyList = new List<CustomBody>(bodies.Length);
            bodyList.RefreshFromBodyArray(bodies);
            this.Bodies = bodyList;
        }
    }
}
