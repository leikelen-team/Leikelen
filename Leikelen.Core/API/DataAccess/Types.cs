using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    /// <summary>
    /// Structure for timeless data
    /// </summary>
    public struct Timeless
    {
        /// <summary>
        /// The value number (may be null)
        /// </summary>
        public double? Value;

        /// <summary>
        /// The subtitle (may be null)
        /// </summary>
        public string Subtitle;

        /// <summary>
        /// The index (may be null)
        /// </summary>
        public int? Index;
    }

    /// <summary>
    /// Structure for event data
    /// </summary>
    public struct Event
    {
        /// <summary>
        /// The time in wich the event ocurred
        /// </summary>
        public TimeSpan EventTime;
        /// <summary>
        /// The value number (may be null)
        /// </summary>
        public double? Value;
        /// <summary>
        /// The subtitle (may be null)
        /// </summary>
        public string Subtitle;
        /// <summary>
        /// The index (may be null)
        /// </summary>
        public int? Index;

        /// <summary>
        /// number to differentiate the events to which interval type
        /// </summary>
        public int toInterval;
    }

    /// <summary>
    /// Structure for interval data
    /// </summary>
    public struct Interval
    {
        /// <summary>
        /// The start time
        /// </summary>
        public TimeSpan StartTime;

        /// <summary>
        /// The end time
        /// </summary>
        public TimeSpan EndTime;

        /// <summary>
        /// The value number (may be null)
        /// </summary>
        public double? Value;

        /// <summary>
        /// The subtitle (may be null)
        /// </summary>
        public string Subtitle;

        /// <summary>
        /// The index (may be null)
        /// </summary>
        public int? Index;
    }
}
