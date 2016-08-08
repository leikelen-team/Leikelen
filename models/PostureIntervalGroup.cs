using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class PostureIntervalGroup
    {
        public PostureType postureType { get; private set; }
        private List<Tuple<MicroPosture, MicroPosture>> intervals; //{ get; private set; }

        public PostureIntervalGroup(PostureType postureType)
        {
            this.postureType = postureType;
            this.intervals = new List<Tuple<MicroPosture, MicroPosture>>();
        }
        
        public void addInterval(MicroPosture initialMicroPosture, MicroPosture finalMicroPosture)
        {
            if(initialMicroPosture.postureType != this.postureType || finalMicroPosture.postureType != this.postureType)
            {
                throw new Exception("You are adding an interval with diferent postureType to this IntervalPosture object");
            }
            if(initialMicroPosture.sceneLocationTime >= finalMicroPosture.sceneLocationTime)
            {
                throw new Exception("initialMicroPosture must be lower than finalMicroPosture");
            }
            bool exists = this.intervals.Exists(
                tuple =>
                    //tuple.Item1.sceneLocationTime >= initialMicroPosture.sceneLocationTime ||
                    tuple.Item2.sceneLocationTime >= initialMicroPosture.sceneLocationTime
                );
            if (exists)
            {
                throw new Exception("The initialMicroPosture must be greater than an existent finalMicroPosture interval");
            }
            intervals.Add(new Tuple<MicroPosture, MicroPosture>(initialMicroPosture, finalMicroPosture));
        }

        public IReadOnlyList<Tuple<MicroPosture, MicroPosture>> Intervals {
            get
            {
                return this.intervals;
            }
        }
    }
}
