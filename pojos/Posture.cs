using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos
{
    public class Posture
    {
        //JointPosition jointPosition;
        //public List<Joint> joints { get; private set; }
        public Emotion emotion { get; private set; }
        public TimeSpan relativeFrameTime { get; private set; }
        //public DateTime time { get; private set; }

        public Posture(
            //List<Joint> joints, 
            Emotion emotion,
            TimeSpan time
        //DateTime time
        )
        {
            //this.joints = joints;
            this.emotion = emotion;
            this.relativeFrameTime = time;
            
        }

    }
}
