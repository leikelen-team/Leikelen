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


            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Proxemic", "Intimate", "Close person distance", null);//0
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Proxemic", "Personal", "Acquaintance distance", null);//1
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Proxemic", "Social", "Stranger distance", null);//2
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Proxemic", "Public", "Public distance", null);//3

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
                                //_dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Proxemic", "Events", time.Value, distance, false);
                                if(distance < 0.45)
                                {
                                    _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Proxemic", "Events", time.Value, distance, 0);
                                }
                                else if(distance < 0.65)
                                {
                                    _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Proxemic", "Events", time.Value, distance, 1);
                                }
                                else if(distance < 0.85)
                                {
                                    _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Proxemic", "Events", time.Value, distance, 2);
                                }
                                else
                                {
                                    _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Proxemic", "Events", time.Value, distance, 3);
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
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Proxemic", "Events", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold(), 0, "Intimate");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Proxemic", "Events", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold(), 1, "Personal");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Proxemic", "Events", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold(), 2, "Social");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Proxemic", "Events", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold(), 3, "Public");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }
        }
    }
}
