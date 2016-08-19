using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    
    public class MicroPosture
    {
        public int MicroPostureId { get; set; }
        public TimeSpan SceneLocationTime { get; private set; }

        public PostureType PostureType { get; private set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        

        //public Body body { get; set; }
        public MicroPosture() { }
        public MicroPosture(PostureType postureType, TimeSpan sceneLocationTime)
        {
            this.PostureType = postureType;
            this.SceneLocationTime = sceneLocationTime;
        }
        
    }
}
