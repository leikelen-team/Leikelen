using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.multimodalvisualizer.src.interfaces.representtypes.timeline
{
    public interface IModalEvent
    {
        int ModalEventId { get; }
        TimeSpan SceneLocationTime { get; }
        IModalType ModalType { get; }
        IPerson Person { get; }
    }
}
