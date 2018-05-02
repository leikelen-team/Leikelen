using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.EEG
{
    /// <summary>
    /// Filter enumeration
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// None filter is configured
        /// </summary>
        None,

        /// <summary>
        /// The band filter 1hz to 50hz
        /// </summary>
        BandFilter1HzTo50Hz,

        /// <summary>
        /// The band filter 7hz to 13hz
        /// </summary>
        BandFilter7HzTo13Hz,

        /// <summary>
        /// The band filter 15hz to 50hz
        /// </summary>
        BandFilter15HzTo50Hz,

        /// <summary>
        /// The band filter 5hz to 50hz
        /// </summary>
        BandFilter5HzTo50Hz
    }
}
