using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.interfaces.representtypes.timeline
{
    public interface IModalIntervalGroup
    {
        
        List<IModalInterval> Intervals { get; }
        IModalType ModalType { get; }
    }
}
