using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos
{
    class _JointPosition
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int z { get; private set; }
        public JointType jointType { get; private set; }

    }
}
