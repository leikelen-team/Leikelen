using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.EEG
{
    public interface IEegProcessingModule
    {
        EventHandler<EegFrameArrivedEventArgs> EegListener();
    }
}
