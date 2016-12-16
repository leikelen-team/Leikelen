using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace cl.uv.multimodalvisualizer.models
{
    public enum DistanceTypes
    {
        Total3D,
        XTotal,
        YTotal,
        ZTotal,
        XPrevious,
        YPrevious,
        ZPrevious,

    }

    public enum DistanceInferred
    {
        WithoutInferred,
        WithInferred,
        OnlyInferred
    }
    public class Distance
    {
        public int DistanceId { get; set; }
        public JointType jointType { get; set; }
        public double distance { get; set; }
        public DistanceTypes DistanceType { get; set; }
        public DistanceInferred inferredType { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        //public int BodyDistanceId { get; set; }
        //public BodyDistance BodyDistance { get; set; }

        public Distance() { }

        public Distance(DistanceTypes distanceType, JointType jointType, DistanceInferred inferredType)
        {
            this.jointType = jointType;
            this.distance = 0;
            this.DistanceType = distanceType;
            this.inferredType = inferredType;
        }
    }
}
