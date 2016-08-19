using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class Interval
    {
        public int IntervalId { get; set; }
        //public MicroPosture Item1 { get; set; }
        //public MicroPosture Item2 { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        //public int PostureTypeId { get; set; }
        //public PostureType PostureType { get; set; }
        public int PostureIntervalGroupId { get; set; }
        public PostureIntervalGroup PostureIntervalGroup { get; set; }


        public Interval() { }
        public Interval(MicroPosture startMicroPosture, MicroPosture endMicroPosture)
        {
            //this.Item1 = Item1;
            //this.Item2 = Item2;
            StartTime = startMicroPosture.SceneLocationTime;
            EndTime = endMicroPosture.SceneLocationTime;
            //PostureType = startMicroPosture.PostureType;
        }
    }
}
