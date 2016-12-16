using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.multimodalvisualizer.models
{
    public class BodyDistance
    {
        public int BodyDistanceId { get; set; }

        public int asdf { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public List<Distance> Distances { get; set; }

        public BodyDistance()
        {
            //this.DistancesWithInferred = new List<Distance>();
            //this.DistancesWithoutInferred = new List<Distance>();

            this.asdf = 4;
        }

        /*
        public DistanceTypeList XPrevious = new DistanceTypeList();

        public DistanceTypeList YPrevious = new DistanceTypeList();

        public DistanceTypeList ZPrevious = new DistanceTypeList();

        public DistanceTypeList Total3D = new DistanceTypeList();

        public DistanceTypeList XTotal = new DistanceTypeList();

        public DistanceTypeList YTotal = new DistanceTypeList();

        public DistanceTypeList ZTotal = new DistanceTypeList();

        public static object GetPropValue(object target, string propName)
        {
            return target.GetType().GetProperty(propName).GetValue(target, null);
        }
        */
    }
}
