using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.Accelerometer
{
    /// <summary>
    /// The interface to create processing modules for the accelerometer sensor
    /// </summary>
    public interface IAccelerometerProcessingModule
    {
        /// <summary>
        /// Get the handler for accelerometer events.
        /// </summary>
        /// <returns>the handler for accelerometer events</returns>
        EventHandler<AccelerometerFrameArrivedEventArgs> AccelerometerListener();
    }
}
