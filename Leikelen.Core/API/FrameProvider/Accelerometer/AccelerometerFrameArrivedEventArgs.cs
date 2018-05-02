using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.Accelerometer
{
    /// <summary>
    /// Arguments of accelerometer frame event
    /// </summary>
    public class AccelerometerFrameArrivedEventArgs
    {
        /// <summary>
        /// The time of the event
        /// </summary>
        public TimeSpan Time;

        /// <summary>
        /// The z axis value
        /// </summary>
        public double ZAxis;

        /// <summary>
        /// The y axis value
        /// </summary>
        public double YAxis;

        /// <summary>
        /// The x axis value
        /// </summary>
        public double XAxis;

        /// <summary>
        /// The place of the person in which the accelerometer is
        /// </summary>
        public string Place;
    }
}
