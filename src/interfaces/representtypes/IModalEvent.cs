using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.interfaces.representtypes
{
    public interface IModalEvent
    {
        int Id { get; }
        TimeSpan SceneLocationTime { get; }
        IModalType ModalType { get; }
        IPerson Person { get; }
    }
}
