using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.API.FrameProvider.EEG
{
    public interface IEEG
    {
        EventHandler<EEGFrameArrivedEventArgs> EEGListener();
    }
}
