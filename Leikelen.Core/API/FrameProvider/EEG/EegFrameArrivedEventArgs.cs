using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.EEG
{
    /// <summary>
    /// Arguments of the eeg frame event
    /// </summary>
    public class EegFrameArrivedEventArgs
    {
        /// <summary>
        /// The time of the event
        /// </summary>
        public TimeSpan Time;

        /// <summary>
        /// The person of the given event
        /// </summary>
        public Person Person;

        /// <summary>
        /// The channel list
        /// </summary>
        public List<EegChannel> Channels;
    }

    /// <summary>
    /// Structure for an eeg channel
    /// </summary>
    public struct EegChannel
    {
        /// <summary>
        /// The position of the channel (in 10/20 of 10/10 system)
        /// </summary>
        public string Position;

        /// <summary>
        /// The value
        /// </summary>
        public double Value;

        /// <summary>
        /// The configured notch value (and filter) for the channel
        /// </summary>
        public NotchType Notch;

        /// <summary>
        /// The filter type configured for the channel
        /// </summary>
        public FilterType Filter;
    }
}
