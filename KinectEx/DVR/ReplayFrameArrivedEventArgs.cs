﻿using System;

namespace KinectEx.DVR
{
    /// <summary>
    /// Generic <c>EventArgs</c> type used to pass the specific type of 
    /// <c>ReplayFrame</c> to a listener during playback when that frame 
    /// should be processed or displayed.
    /// </summary>
    public class ReplayFrameArrivedEventArgs<T> : EventArgs where T : ReplayFrame
    {
        /// <summary>
        /// Gets the frame that has arrived.
        /// </summary>
        public T Frame { get; set; }
    }
}
