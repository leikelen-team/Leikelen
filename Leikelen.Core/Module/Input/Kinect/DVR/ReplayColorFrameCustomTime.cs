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
    /// <summary>
    /// Modified version of the <see cref="KinectEx.DVR.ReplayColorFrame" />
    /// to accept relative time of the scene/>
    /// </summary>
    /// <seealso cref="KinectEx.DVR.ReplayColorFrame" />
    public class ReplayColorFrameCustomTime : ReplayColorFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayColorFrameCustomTime"/> class
        /// based on the specified <c>ColorFrame</c>.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="relativeTime">time relative to the start of the scene</param>
        public ReplayColorFrameCustomTime(ColorFrame frame, TimeSpan relativeTime)
        {
            this.Codec = ColorCodecs.Raw;

            this.FrameType = FrameTypes.Color;
            this.RelativeTime = relativeTime;

            this.Width = frame.FrameDescription.Width;
            this.Height = frame.FrameDescription.Height;

            this.FrameDataSize = this.Width * this.Height * 4; // BGRA is 4 bytes per pixel
            this._frameData = new Byte[this.FrameDataSize];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(_frameData);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(_frameData, ColorImageFormat.Bgra);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayColorFrameCustomTime"/> class
        /// based on the specified <c>ColorFrame</c> and <c>byte</c> array.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="relativeTime">time relative to the start of the scene</param>
        public ReplayColorFrameCustomTime(ColorFrame frame, byte[] bytes, TimeSpan relativeTime)
        {
            this.Codec = ColorCodecs.Raw;

            this.FrameType = FrameTypes.Color;
            this.RelativeTime = relativeTime;

            this.Width = frame.FrameDescription.Width;
            this.Height = frame.FrameDescription.Height;

            this.FrameDataSize = this.Width * this.Height * 4; // BGRA is 4 bytes per pixel
            this._frameData = bytes;
        }
    }
}
