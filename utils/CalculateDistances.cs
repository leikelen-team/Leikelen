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
        private LinkedList<Body[]> bodiesInAllFrames = new LinkedList<Body[]>();

        public void AddBodies(Body[] bodiesInFrame)
        {
            this.bodiesInAllFrames.AddLast(bodiesInFrame);
        }

        public Dictionary<ulong, DistanceTypeList> calculateTotalDistance(DistanceInferred inferredType)
        {
            Dictionary<ulong, DistanceTypeList> totalDistance = new Dictionary<ulong, DistanceTypeList>();
            for(int i = 0; i < this.bodiesInAllFrames.Count; i++)
            {
                Body[] bodiesAtFrameI = this.bodiesInAllFrames.ElementAt<Body[]>(i);

                if (bodiesAtFrameI != null)
                {
                    foreach (Body body in bodiesAtFrameI)
                    {
                        if (body.IsTracked)
                        {
                            Body bodyCopy = body;
                            if (!totalDistance.ContainsKey(body.TrackingId))
                            {
                                totalDistance[body.TrackingId] = new DistanceTypeList();
                            }
                            foreach (JointType jointType in body.Joints.Keys)
                            {
                                if(body.Joints[jointType].TrackingState != TrackingState.NotTracked)
                                {
                                    if(body.Joints[jointType].TrackingState == TrackingState.Inferred)
                                    {
                                        //if(inferredType == DistanceInferred.WithInferred || inferredType == DistanceInferred.OnlyInferred)
                                        //{
                                            calculateFrameToFrame(body, ref totalDistance, jointType, inferredType, true);
                                        //}
                                    }
                                    else
                                    {
                                        //if(inferredType != DistanceInferred.OnlyInferred)
                                        //{
                                            calculateFrameToFrame(body, ref totalDistance, jointType, inferredType, false);
                                        //}
                                    }
                                }
                                
                                
                            }

                        }
                    }
                }
            }
            return totalDistance;

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

        /*
        private void calculateTotalWithFrameY(Body body, ref Dictionary<ulong, DistanceTypeList> distanceObj, JointType jointType)
        {
            if (!distanceObj[body.TrackingId].YTotal.containsJointType(jointType))
            {
                distanceObj[body.TrackingId].YTotal.addJointType(jointType);
            }
            else
            {
                distanceObj[body.TrackingId].YTotal.setDistance(jointType, distanceObj[body.TrackingId].YTotal.getDistance(jointType) + Math.Abs((body.Joints[jointType].Position.Y + 20) - (distanceObj[body.TrackingId].getDistance(DistanceTypes.YPrevious, jointType) + 20)));
            }

        }

        private void calculateTotalWithFrameZ(Body body, ref Dictionary<ulong, DistanceTypeList> distanceObj, JointType jointType)
        {
            if (!distanceObj[body.TrackingId].ZTotal.containsJointType(jointType))
            {
                distanceObj[body.TrackingId].ZTotal.addJointType(jointType);
            }
            else
            {
                distanceObj[body.TrackingId].ZTotal.setDistance(jointType, distanceObj[body.TrackingId].ZTotal.getDistance(jointType) + Math.Abs((body.Joints[jointType].Position.Z + 20) - (distanceObj[body.TrackingId].getDistance(DistanceTypes.ZPrevious, jointType) + 20)));
            }

        }
        */

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
