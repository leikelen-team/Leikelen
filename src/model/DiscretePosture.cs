using cl.uv.multimodalvisualizer.src.interfaces;
using cl.uv.multimodalvisualizer.src.interfaces.humanmodals;
using cl.uv.multimodalvisualizer.src.interfaces.representtypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.multimodalvisualizer.src.model
{
    public class DiscretePosture: IDiscretePosture
    {
        public IRepresentType RepresentType { get; }
    }
}
