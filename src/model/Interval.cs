using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.multimodalvisualizer.src.model
{
    public class Interval
    {
        public int IntervalId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int PostureIntervalGroupId { get; set; }
        public PostureIntervalGroup PostureIntervalGroup { get; set; }


        public Interval() { }
        public Interval(MicroPosture startMicroPosture, MicroPosture endMicroPosture)
        {
            StartTime = startMicroPosture.SceneLocationTime;
            EndTime = endMicroPosture.SceneLocationTime;
            Duration = EndTime.Subtract(StartTime);
        }
    }
}
