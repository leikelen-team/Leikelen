using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Frame to access the EEG data.
/// </summary>
namespace cl.uv.leikelen.API.FrameProvider.EEG
{
    /// <summary>
    /// Arguments of the EEG frame event
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
        /// The quality of the signal
        /// </summary>
        public int Quality;

        /// <summary>
        /// The channel list
        /// </summary>
        public List<EegChannel> Channels;

        /// <summary>
        /// The power for each frequency band
        /// </summary>
        public Dictionary<FrequencyBand, double> BandPower;

        /// <summary>
        /// Miscellaneous attributes calculated to be included
        /// </summary>
        public Dictionary<string, double> CalculatedAttributes;
    }

    /// <summary>
    /// Structure for an EEG channel
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
        /// The resistance in ohms of the channel
        /// </summary>
        public double? Resistance;

        /// <summary>
        /// The configured notch value (and filter) for the channel
        /// </summary>
        public NotchType Notch;

        /// <summary>
        /// The filter type configured for the channel
        /// </summary>
        public FilterType Filter;
    }

    /// <summary>
    /// Enumeration of the different frequency bands in EEG
    /// </summary>
    public enum FrequencyBand
    {
        /// <summary>
        /// The delta frequency band (Usually 1-3Hz)
        /// </summary>
        Delta,
        /// <summary>
        /// The theta frequency band (Usually 4-7Hz)
        /// </summary>
        Theta,
        /// <summary>
        /// The alpha frequency band (Usually 8-14Hz)
        /// </summary>
        Alpha,
        /// <summary>
        /// The low alpha frequency band (Usually 8-9Hz)
        /// </summary>
        LowAlpha,
        /// <summary>
        /// The high alpha frequency band (Usually 10-12Hz)
        /// </summary>
        HighAlpha,
        /// <summary>
        /// The beta frequency band (Usually 13-30Hz)
        /// </summary>
        Beta,
        /// <summary>
        /// The low beta frequency band (Usually 13-17Hz)
        /// </summary>
        LowBeta,
        /// <summary>
        /// The high beta frequency band (Usually 18-30Hz)
        /// </summary>
        HighBeta,
        /// <summary>
        /// The gamma frequency band (Usually >31)
        /// </summary>
        Gamma,
        /// <summary>
        /// The low gamma frequency band (Usually 31-40Hz)
        /// </summary>
        LowGamma,
        /// <summary>
        /// The high gamma frequency band (Usually 41-50Hz)
        /// </summary>
        HighGamma
    }
}
