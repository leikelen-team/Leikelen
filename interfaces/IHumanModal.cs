﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.multimodalvisualizer.interfaces
{
    public interface IHumanModal
    {
        //TODO: implementar lista List<IRepresentType>
        IRepresentType RepresentType { get; }
    }
}
