using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace cl.uv.multimodalvisualizer.models
{
    public class myGesture
    {
        

        public int myGestureId { get; set; }
        public string name { get; set; }
        public PostureType startPosture;
        public PostureType continuousPosture;
        public PostureType endPosture;
        public PostureIntervalGroup gestureIntervalGroup { get; set; }



        public myGesture()
        {
        }

        public myGesture(int id, string name, PostureType start, PostureType cont, PostureType end, PostureIntervalGroup pig)
        {
            this.myGestureId = id;
            this.name = name;
            this.startPosture = start;
            this.continuousPosture = cont;
            this.endPosture = end;
            this.gestureIntervalGroup = pig;
        }
    }
}
