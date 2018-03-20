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
    public class ReplayDepthFrameCustomTime : ReplayDepthFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayDepthFrameCustomTime"/> class
        /// based on the specified <c>DepthFrame</c>.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public ReplayDepthFrameCustomTime(DepthFrame frame, TimeSpan relativeTime)
        {
            this.FrameType = FrameTypes.Depth;
            this.RelativeTime = relativeTime;

            this.DepthMinReliableDistance = frame.DepthMinReliableDistance;
            this.DepthMaxReliableDistance = frame.DepthMaxReliableDistance;

            this.Width = frame.FrameDescription.Width;
            this.Height = frame.FrameDescription.Height;
            this.BytesPerPixel = frame.FrameDescription.BytesPerPixel;

            _frameData = new ushort[this.Width * this.Height];

            frame.CopyFrameDataToArray(_frameData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayDepthFrameCustomTime"/> class
        /// based on the specified <c>DepthFrame</c> and array of <c>ushort</c>.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="frameData">The frame data.</param>
        public ReplayDepthFrameCustomTime(DepthFrame frame, ushort[] frameData, TimeSpan relativeTime)
        {
            this.FrameType = FrameTypes.Depth;
            this.RelativeTime = relativeTime;

            this.DepthMinReliableDistance = frame.DepthMinReliableDistance;
            this.DepthMaxReliableDistance = frame.DepthMaxReliableDistance;

            this.Width = frame.FrameDescription.Width;
            this.Height = frame.FrameDescription.Height;
            this.BytesPerPixel = frame.FrameDescription.BytesPerPixel;

            _frameData = frameData;
        }
    }
}
