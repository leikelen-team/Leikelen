using cl.uv.multimodalvisualizer.interfaces.representtypes.timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.multimodalvisualizer.interfaces.representtypes
{
    public interface ITimeLine: IRepresentType
    {
        List<IModalIntervalGroup> IntervalGroups { get; }
        List<IModalInterval> Intervals { get; }
    }
}
