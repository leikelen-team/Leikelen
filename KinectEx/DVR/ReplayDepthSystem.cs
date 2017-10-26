﻿using System;
using System.IO;

namespace KinectEx.DVR
{
    /// <summary>
    /// public class that provides the services necessary to decode and playback
    /// a <c>ReplayDepthFrame</c>.
    /// </summary>
    public class ReplayDepthSystem : ReplaySystem
    {
        /// <summary>
        /// Occurs when a new frame is ready to be displayed.
        /// </summary>
        public event Action<ReplayDepthFrame> FrameArrived;

        /// <summary>
        /// Adds a frame to the Frames list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void AddFrame(BinaryReader reader)
        {
            var frame = ReplayDepthFrame.FromReader(reader);
            if (frame != null)
                this.Frames.Add(frame);
        }

        /// <summary>
        /// Pushes the current frame.
        /// </summary>
        public override void PushCurrentFrame()
        {
            if (this.FrameCount == 0)
                return;

            var frame = (ReplayDepthFrame)this.Frames[CurrentFrame];
            if (FrameArrived != null)
                FrameArrived(frame);
        }
    }
}
