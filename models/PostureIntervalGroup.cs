using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class PostureIntervalGroup
    {
        public int PostureIntervalGroupId { get; set; }
        public List<Interval> Intervals { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int PostureTypeId { get; private set; }
        public PostureType PostureType { get; private set; }

        public PostureIntervalGroup() { }

        public PostureIntervalGroup(PostureType postureType)
        {
            this.PostureType = postureType;
            this.Intervals = new List<Interval>();
        }
        
        public void addInterval(MicroPosture initialMicroPosture, MicroPosture finalMicroPosture)
        {
            if(initialMicroPosture.PostureType != this.PostureType || finalMicroPosture.PostureType != this.PostureType)
            {
                throw new Exception("You are adding an interval with diferent postureType to this IntervalPosture object");
            }
            if(initialMicroPosture.SceneLocationTime >= finalMicroPosture.SceneLocationTime)
            {
                throw new Exception("initialMicroPosture must be lower than finalMicroPosture");
            }
            bool exists = this.Intervals.Exists(
                interval =>
                    //tuple.Item1.sceneLocationTime >= initialMicroPosture.sceneLocationTime ||
                    interval.EndTime >= initialMicroPosture.SceneLocationTime
                );
            if (exists)
            {
                throw new Exception("The initialMicroPosture must be greater than an existent finalMicroPosture interval");
            }
            Intervals.Add(new Interval(initialMicroPosture, finalMicroPosture));
        }

        //public IReadOnlyList<Tuple<MicroPosture, MicroPosture>> Intervals {
        //    get
        //    {
        //        return this.intervals;
        //    }
        //}
    }
}
