using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.EEG
{
    /// <summary>
    /// Notch filter enumeration
    /// </summary>
    public enum NotchType
    {
        /// <summary>
        /// No notch filter is configured
        /// </summary>
        None,

        /// <summary>
        /// The notch is at 50hz
        /// </summary>
        Notch50Hz,

        /// <summary>
        /// The notch is at 60hz
        /// </summary>
        Notch60Hz
    }
}
