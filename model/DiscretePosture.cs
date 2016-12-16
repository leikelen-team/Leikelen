using cl.uv.multimodalvisualizer.interfaces;
using cl.uv.multimodalvisualizer.interfaces.humanmodals;
using cl.uv.multimodalvisualizer.interfaces.representtypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.multimodalvisualizer.model
{
    public class DiscretePosture: IDiscretePosture
    {
        public IRepresentType RepresentType { get; }
    }
}
