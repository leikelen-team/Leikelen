using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos
{
    public class MicroPosture
    {
        public PostureType postureType { get; private set; }
        public TimeSpan sceneLocationTime { get; private set; }

        public MicroPosture(PostureType postureType, TimeSpan sceneLocationTime)
        {
            this.postureType = postureType;
            this.sceneLocationTime = sceneLocationTime;
        }
        
    }
}
