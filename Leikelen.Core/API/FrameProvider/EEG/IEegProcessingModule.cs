using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.EEG
{
    /// <summary>
    /// Interface to implement a processing module for the EEG sensor
    /// </summary>
    public interface IEegProcessingModule
    {
        /// <summary>
        /// Gets the handler for the EEG frame events.
        /// </summary>
        /// <returns>the handler for the EEG frame events</returns>
        EventHandler<API.FrameProvider.EEG.EegFrameArrivedEventArgs> EegListener();
    }
}
