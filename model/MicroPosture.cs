using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect.VisualGestureBuilder;

namespace cl.uv.multimodalvisualizer.models
{
    
    public class MicroPosture
    {
        public int MicroPostureId { get; set; }
        public TimeSpan SceneLocationTime { get; private set; }

        public PostureType PostureType { get; private set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public float Progress { get; set; }
        public GestureType GestureType { get; set; }

        //public Body body { get; set; }
        public MicroPosture() { }
        public MicroPosture(PostureType postureType, TimeSpan sceneLocationTime, GestureType GestureType)
        {
            this.PostureType = postureType;
            this.SceneLocationTime = sceneLocationTime;
            this.GestureType = GestureType;
        }
        public MicroPosture(PostureType postureType, TimeSpan sceneLocationTime, float Progress, GestureType GestureType)
        {
            this.PostureType = postureType;
            this.SceneLocationTime = sceneLocationTime;
            this.Progress = Progress;
            this.GestureType = GestureType;
        }

    }
}
