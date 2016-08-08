using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos
{
    class _Interval
    {
        //private static int idCount = 0;
        //private int _id;
        public DateTime startTime { get; private set; }
        public DateTime endTime { get; private set; }
        public TimeSpan timeSpan { get; private set; }

        public List<Person> persons { get; private set; }

        public _Interval(DateTime startTime, DateTime endTime)
        {
            //this._id = ++idCount;
            this.startTime = startTime;
            this.endTime = endTime;
            this.timeSpan = endTime.Subtract(startTime);
            
        }
    }
}
