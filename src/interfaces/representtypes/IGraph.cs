using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.interfaces.representtypes
{
    interface IGraph : IRepresentType
    {
        int Id { get; }
        TimeSpan time { get; }
        double value { get; }
    }
}
