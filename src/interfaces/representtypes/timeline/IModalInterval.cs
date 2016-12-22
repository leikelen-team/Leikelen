using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.multimodalvisualizer.src.interfaces.representtypes.timeline
{
    public interface IModalInterval
    {
        //List<IModalEvent> Events { get; }
        int ModalIntervalId { get; }
        TimeSpan StartTime { get; }
        TimeSpan EndTime { get; }
        TimeSpan Duration { get; }
        int IModalIntervalGroupId { get; }
        IModalIntervalGroup IModalIntervalGroup { get; }
    }
}
