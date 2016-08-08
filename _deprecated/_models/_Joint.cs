using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos
{
    class _Joint
    {
        //private static int idCount = 0;
        //private int _id;
        public string name { get; private set; }

        public _Joint(string name)
        {
            //this._id = ++idCount;
            this.name = name;
        }
        
    }
}
