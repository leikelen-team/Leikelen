using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.EEG
{
    public enum FilterType
    {
        None,
        BandFilter1HzTo50Hz,
        BandFilter7HzTo13Hz,
        BandFilter15HzTo50Hz,
        BandFilter5HzTo50Hz
    }
}
