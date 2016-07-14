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
        public static List<Posture> posturesAvailables { get; private set; } = new List<Posture>();
        public static Posture none = new Posture(Properties.Resources.NonePostureName);
        
        //public enum Name
        //{
        //    Atento,
        //    Distraido,
        //    Seated,
        //    Ninguno
        //}

        public string name { get; private set; }
        //public TimeSpan relativeFrameTime { get; private set; }
        //public DateTime time { get; private set; }

        public Posture(
            //List<Joint> joints, 
            string name
            //,TimeSpan time
        //DateTime time
        )
        {
            //this.joints = joints;
            this.name = name;
            //this.relativeFrameTime = time;
            
        }

        public override string ToString()
        {
            return name;
        }

    }
}
