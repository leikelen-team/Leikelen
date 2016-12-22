using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace cl.uv.multimodalvisualizer.src.model
{
    public class DistanceTypeList : List<Distance>
    {
        //public int DistanceTypeListId { get; set; }

        //public int BodyDistanceId { get; set; }
        //public BodyDistance BodyDistance { get; set; }
        public bool addJointType(DistanceTypes distanceType, JointType jointType, DistanceInferred inferredType)
        {
            if (this.Exists(d => d.jointType == jointType && d.DistanceType == distanceType && d.inferredType == inferredType))
            {
                return false;
            }
            else
            {
                this.Add(new Distance(distanceType, jointType, inferredType));
                return true;
            }
            
        }
        public bool setDistance(DistanceTypes distanceType, JointType jointType, DistanceInferred inferredType, double distance)
        {
            if (this.Exists(d => d.jointType == jointType && d.DistanceType == distanceType && d.inferredType == inferredType))
            {
                Distance obj = this.Find(d => d.jointType == jointType && d.DistanceType == distanceType && d.inferredType == inferredType);
                obj.distance = distance;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool containsJointType(DistanceTypes distanceType, JointType jointType, DistanceInferred inferredType)
        {
            if (this.Exists(d => d.jointType == jointType && d.DistanceType == distanceType && d.inferredType == inferredType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double getDistance(DistanceTypes distanceType, JointType jointType, DistanceInferred inferredType)
        {
            if (this.Exists(d => d.jointType == jointType && d.DistanceType == distanceType && d.inferredType == inferredType))
            {
                Distance obj = this.Find(d => d.jointType == jointType && d.DistanceType == distanceType && d.inferredType == inferredType);
                return obj.distance;
            }
            else
            {
                return 0;
            }
        }
    }
}
