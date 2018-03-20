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
    public class ReplayInfraredFrameCustomTime : ReplayInfraredFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayInfraredFrame"/> class
        /// from an <c>InfraredFrame</c>.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public ReplayInfraredFrameCustomTime(InfraredFrame frame, TimeSpan relativeTime)
        {
            this.FrameType = FrameTypes.Infrared;
            this.RelativeTime = relativeTime;

            this.Width = frame.FrameDescription.Width;
            this.Height = frame.FrameDescription.Height;
            this.BytesPerPixel = frame.FrameDescription.BytesPerPixel;

            _frameData = new ushort[this.Width * this.Height];

            frame.CopyFrameDataToArray(_frameData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayInfraredFrame"/> class
        /// from an <c>InfraredFrame</c> with the data already extracted.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="frameData">The frame data.</param>
        public ReplayInfraredFrameCustomTime(InfraredFrame frame, ushort[] frameData, TimeSpan relativeTime)
        {
            this.FrameType = FrameTypes.Infrared;
            this.RelativeTime = relativeTime;

            this.Width = frame.FrameDescription.Width;
            this.Height = frame.FrameDescription.Height;
            this.BytesPerPixel = frame.FrameDescription.BytesPerPixel;

            _frameData = frameData;
        }
    }
}
