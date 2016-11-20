using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.multimodalvisualizer.models;

namespace cl.uv.multimodalvisualizer.utils
{
    public class CalculateDistances
    {
        private LinkedList<Tuple<TimeSpan, Body[]>> bodiesInAllFrames = new LinkedList<Tuple<TimeSpan, Body[]>>();
        
        public void AddBodies(Body[] bodiesInFrame, TimeSpan RelativeTime)
        {
            if(bodiesInFrame != null)
            {
                this.bodiesInAllFrames.AddLast(new Tuple<TimeSpan, Body[]>(RelativeTime, bodiesInFrame));
            }
        }
        
        public Dictionary<TimeSpan, Dictionary<ulong, DistanceTypeList>> calculateTotalDistanceIntervals(DistanceInferred inferredType, int interval)
        {
            Dictionary<TimeSpan, Dictionary<ulong, DistanceTypeList>> distsInTime = new Dictionary<TimeSpan, Dictionary<ulong, DistanceTypeList>>();
            Dictionary<ulong, DistanceTypeList> intervalDistance = new Dictionary<ulong, DistanceTypeList>();
            TimeSpan StartTime = new TimeSpan(0);// this.bodiesInAllFrames.ElementAt<Tuple<TimeSpan, Body[]>>(0).Item1;
            distsInTime[StartTime] = new Dictionary<ulong, DistanceTypeList>();
            for (int i = 0; i < this.bodiesInAllFrames.Count; i++)
            {
                Tuple<TimeSpan, Body[]> bodiesInTime = this.bodiesInAllFrames.ElementAt<Tuple<TimeSpan, Body[]>>(i);
                if(bodiesInTime.Item1.Subtract(StartTime).TotalSeconds < interval && i < this.bodiesInAllFrames.Count - 1)
                {
                    distanceAtFrame(bodiesInTime.Item2, ref intervalDistance, inferredType);
                }
                else
                {
                    distsInTime[StartTime] = new Dictionary<ulong, DistanceTypeList>(intervalDistance);
                    StartTime = bodiesInTime.Item1;
                    distsInTime[StartTime] = new Dictionary<ulong, DistanceTypeList>();
                    intervalDistance = new Dictionary<ulong, DistanceTypeList>();
                    distanceAtFrame(bodiesInTime.Item2, ref intervalDistance, inferredType);
                }
                
                /*
                if (/*i == 0 || bodiesInTime.Item1.Subtract(StartTime).TotalSeconds >= interval || i == this.bodiesInAllFrames.Count -1)
                {
                    
                }*/
            }
            return distsInTime;
        }

        public Dictionary<ulong, DistanceTypeList> calculateTotalDistance(DistanceInferred inferredType)
        {
            Dictionary<ulong, DistanceTypeList> totalDistance = new Dictionary<ulong, DistanceTypeList>();
            for(int i = 0; i < this.bodiesInAllFrames.Count; i++)
            {
                Body[] bodiesAtFrameI = this.bodiesInAllFrames.ElementAt<Tuple<TimeSpan, Body[]>>(i).Item2;
                distanceAtFrame(bodiesAtFrameI, ref totalDistance, inferredType);
            }
            return totalDistance;
        }

        public void distanceAtFrame(Body[] bodies, ref Dictionary<ulong, DistanceTypeList> distanceObj , DistanceInferred inferredType)
        {
            foreach (Body body in bodies)
            {
                if (body.IsTracked)
                {
                    Body bodyCopy = body;
                    if (!distanceObj.ContainsKey(body.TrackingId))
                    {
                        distanceObj[body.TrackingId] = new DistanceTypeList();
                    }
                    foreach (JointType jointType in body.Joints.Keys)
                    {
                        if (body.Joints[jointType].TrackingState != TrackingState.NotTracked)
                        {
                            if (body.Joints[jointType].TrackingState == TrackingState.Inferred)
                            {
                                calculateFrameToFrame(body, ref distanceObj, jointType, inferredType, true);
                            }
                            else
                            {
                                calculateFrameToFrame(body, ref distanceObj, jointType, inferredType, false);
                            }
                        }
                    }
                }
            }
        }

        private void calculateFrameToFrame(Body body, ref Dictionary<ulong, DistanceTypeList> distanceObj, JointType jointType, DistanceInferred inferredType, bool inferred)
        {
            if(inferredType == DistanceInferred.WithInferred || (inferredType == DistanceInferred.WithoutInferred && inferred == false) || (inferredType == DistanceInferred.OnlyInferred && inferred == true)) //calcula distancia
            {
                calculateTotalWithFrame3D(body, ref distanceObj, jointType, inferredType, inferred);
                calculateTotalWithFrameOneCoordinate(body, ref distanceObj, 'X', jointType, inferredType, inferred);
                calculateTotalWithFrameOneCoordinate(body, ref distanceObj, 'Y', jointType, inferredType, inferred);
                calculateTotalWithFrameOneCoordinate(body, ref distanceObj, 'Z', jointType, inferredType, inferred);
            }
            setPrevious(body, ref distanceObj, jointType, inferredType);
        }

        private void calculateTotalWithFrame3D(Body body, ref Dictionary<ulong, DistanceTypeList> distanceObj, JointType jointType, DistanceInferred inferredType, bool inferred)
        {
            if (!distanceObj[body.TrackingId].containsJointType(DistanceTypes.Total3D, jointType, inferredType))
            {
                distanceObj[body.TrackingId].addJointType(DistanceTypes.Total3D, jointType, inferredType);
            }
            else
            {
                double Xdist = (body.Joints[jointType].Position.X + 20) - (distanceObj[body.TrackingId].getDistance(DistanceTypes.XPrevious, jointType, inferredType) + 20);
                double Ydist = (body.Joints[jointType].Position.Y + 20) - (distanceObj[body.TrackingId].getDistance(DistanceTypes.YPrevious, jointType, inferredType) + 20);
                double Zdist = (body.Joints[jointType].Position.Z + 20) - (distanceObj[body.TrackingId].getDistance(DistanceTypes.ZPrevious, jointType, inferredType) + 20);
                distanceObj[body.TrackingId].setDistance(DistanceTypes.Total3D, jointType, inferredType, distanceObj[body.TrackingId].getDistance(DistanceTypes.Total3D, jointType, inferredType) + Math.Sqrt(Math.Pow(Xdist, 2) + Math.Pow(Ydist, 2) + Math.Pow(Zdist, 2)));
            }

        }

        private void calculateTotalWithFrameOneCoordinate(Body body, ref Dictionary<ulong, DistanceTypeList> distanceObj, char eje, JointType jointType, DistanceInferred inferredType, bool inferred)
        {
            DistanceTypes distanceType;
            DistanceTypes PreviousDistanceType;
            float pointInFrame;

            switch (eje)
            {
                case 'X':
                    distanceType = DistanceTypes.XTotal;
                    PreviousDistanceType = DistanceTypes.XPrevious;
                    pointInFrame = body.Joints[jointType].Position.X;
                    break;
                case 'Y':
                    distanceType = DistanceTypes.YTotal;
                    PreviousDistanceType = DistanceTypes.YPrevious;
                    pointInFrame = body.Joints[jointType].Position.Y;
                    break;
                case 'Z':
                    distanceType = DistanceTypes.ZTotal;
                    PreviousDistanceType = DistanceTypes.ZPrevious;
                    pointInFrame = body.Joints[jointType].Position.Z;
                    break;
                default:
                    return;
            }
            if (!distanceObj[body.TrackingId].containsJointType(distanceType, jointType, inferredType))
            {
                distanceObj[body.TrackingId].addJointType(distanceType, jointType, inferredType);
            }
            else
            {
                distanceObj[body.TrackingId].setDistance(distanceType, jointType, inferredType, distanceObj[body.TrackingId].getDistance(distanceType, jointType, inferredType) + Math.Abs((pointInFrame + 20) - (distanceObj[body.TrackingId].getDistance(PreviousDistanceType, jointType, inferredType) + 20)));
            }

        }

        private void setPrevious(Body body, ref Dictionary<ulong, DistanceTypeList> distanceObj, JointType jointType, DistanceInferred inferredType)
        {
            if (!distanceObj[body.TrackingId].containsJointType(DistanceTypes.XPrevious, jointType, inferredType))
            {
                distanceObj[body.TrackingId].addJointType(DistanceTypes.XPrevious, jointType, inferredType);
            }
            distanceObj[body.TrackingId].setDistance(DistanceTypes.XPrevious, jointType, inferredType, body.Joints[jointType].Position.X);

            if (!distanceObj[body.TrackingId].containsJointType(DistanceTypes.YPrevious, jointType, inferredType))
            {
                distanceObj[body.TrackingId].addJointType(DistanceTypes.YPrevious, jointType, inferredType);
            }
            distanceObj[body.TrackingId].setDistance(DistanceTypes.YPrevious, jointType, inferredType, body.Joints[jointType].Position.Y);

            if (!distanceObj[body.TrackingId].containsJointType(DistanceTypes.ZPrevious, jointType, inferredType))
            {
                distanceObj[body.TrackingId].addJointType(DistanceTypes.ZPrevious, jointType, inferredType);
            }
            distanceObj[body.TrackingId].setDistance(DistanceTypes.ZPrevious, jointType, inferredType, body.Joints[jointType].Position.Z);
        }
    }
}
