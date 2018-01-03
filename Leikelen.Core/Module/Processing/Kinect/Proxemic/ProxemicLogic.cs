using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using KinectEx;

namespace cl.uv.leikelen.Module.Processing.Kinect.Proxemic
{
    public class ProxemicLogic
    {
        public List<CustomBody> _bodies;
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public ProxemicLogic()
        {
            _dataAccessFacade.GetModalAccess().AddIfNotExists("Proxemic", "distance between spinalmids of persons");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Proxemic", "Public", "Public distance", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Proxemic", "Social", "Stranger distance", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Proxemic", "Personal", "Acquaintance distance", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Proxemic", "Intimate", "Close person distance", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Proxemic", "Events", "all distances", null);

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
                CameraSpacePoint lastCentralPoint = new CameraSpacePoint();
                foreach (IBody body in bodies)
                {
                    //using only the central point
                    var centralPoint = body.Joints[JointType.SpineMid].Position;
                    if(centralPoint.X != 0 || centralPoint.Y != 0 || centralPoint.Z != 0)
                    {
                        if (lastCentralPoint.X != 0 || lastCentralPoint.Y != 0 || lastCentralPoint.Z != 0)
                        {
                            var distance = Math.Sqrt(
                                Math.Pow(lastCentralPoint.X - centralPoint.X, 2) +
                                Math.Pow(lastCentralPoint.Y - centralPoint.Y, 2) +
                                Math.Pow(lastCentralPoint.Z - centralPoint.Z, 2));
                            

                            CheckPerson.Instance.CheckIfExistsPerson(body.TrackingId);
                            var time = _dataAccessFacade.GetSceneInUseAccess()?.GetLocation();
                            if (time.HasValue)
                            {
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Proxemic", "Events", time.Value, distance, false);
                                if(distance < 0.45)
                                {
                                    _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Proxemic", "Intimate", time.Value, distance, true);
                                }
                                else if(distance < 0.65)
                                {
                                    _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Proxemic", "Personal", time.Value, distance, true);
                                }
                                else if(distance < 0.85)
                                {
                                    _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Proxemic", "Social", time.Value, distance, true);
                                }
                                else
                                {
                                    _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Proxemic", "Public", time.Value, distance, true);
                                }
                                //Console.WriteLine($"Proxemic is: {distance}");
                            }
                        }
                        lastCentralPoint = centralPoint;
                    }
                }
            }
        }

        public async void StopRecording()
        {
            foreach (var personPair in CheckPerson.Instance.PersonsId)
            {
                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Proxemic", "Intimate", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Proxemic", "Personal", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Proxemic", "Social", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Proxemic", "Public", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }
        }
    }
}
