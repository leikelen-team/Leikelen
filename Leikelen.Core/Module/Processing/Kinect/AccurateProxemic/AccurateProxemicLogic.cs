using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using KinectEx;
using Microsoft.Kinect;

namespace cl.uv.leikelen.Module.Processing.Kinect.AccurateProxemic
{
    public class AccurateProxemicLogic
    {
        public List<CustomBody> _bodies;
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public AccurateProxemicLogic()
        {
            _dataAccessFacade.GetModalAccess().AddIfNotExists("AccProxemic", "distance between spinalmids of persons");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("AccProxemic", "Public", "Public distance", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("AccProxemic", "Social", "Stranger distance", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("AccProxemic", "Personal", "Acquaintance distance", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("AccProxemic", "Intimate", "Close person distance", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("AccProxemic", "Events", "all distances", null);

            _bodies = new List<CustomBody>();
        }

        public void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            IEnumerable<IBody> bodies = null; // to make the GetBitmap call a little cleaner
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (!ReferenceEquals(null, frame))
                {
                    frame.GetAndRefreshBodyData(_bodies);
                    bodies = _bodies;
                }
            }

            if (!ReferenceEquals(null, bodies))
            {
                List<CameraSpacePoint> lastPersonsJoints = new List<CameraSpacePoint>();
                foreach (IBody body in bodies)
                {
                    //using all points
                    List<CameraSpacePoint> actualPersonJoints = new List<CameraSpacePoint>();
                    double minDistance = double.PositiveInfinity;
                    foreach (var joint in body.Joints)
                    {
                        if (joint.Value.Position.X != 0 || joint.Value.Position.Y != 0 || joint.Value.Position.Z != 0)
                        {
                            actualPersonJoints.Add(joint.Value.Position);
                            foreach (var lastJoint in lastPersonsJoints)
                            {
                                var distance = Math.Sqrt(
                                    Math.Pow(lastJoint.X - joint.Value.Position.X, 2) +
                                    Math.Pow(lastJoint.Y - joint.Value.Position.Y, 2) +
                                    Math.Pow(lastJoint.Z - joint.Value.Position.Z, 2));
                                if (distance < minDistance)
                                {
                                    minDistance = distance;
                                }
                            }
                        }
                    }
                    if (actualPersonJoints.Count > 0)
                        lastPersonsJoints = actualPersonJoints;
                    if (!double.IsInfinity(minDistance))
                    {
                        CheckPerson.Instance.CheckIfExistsPerson(body.TrackingId);
                        var time = _dataAccessFacade.GetSceneInUseAccess()?.GetLocation();
                        if (time.HasValue)
                        {
                            _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "AccProxemic", "Events", time.Value, minDistance, false);
                            if (minDistance < 0.45)
                            {
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "AccProxemic", "Intimate", time.Value, minDistance, true);
                            }
                            else if (minDistance < 0.65)
                            {
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "AccProxemic", "Personal", time.Value, minDistance, true);
                            }
                            else if (minDistance < 0.85)
                            {
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "AccProxemic", "Social", time.Value, minDistance, true);
                            }
                            else
                            {
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "AccProxemic", "Public", time.Value, minDistance, true);
                            }
                            //Console.WriteLine($"Proxemic real min is: {minDistance}");
                        }
                    }  
                }
            }
        }

        public void StopRecording()
        {
            foreach (var personPair in CheckPerson.Instance.PersonsId)
            {
                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "AccProxemic", "Intimate", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "AccProxemic", "Personal", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "AccProxemic", "Social", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "AccProxemic", "Public", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }
        }
    }
}
