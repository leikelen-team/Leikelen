using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using KinectEx;

namespace cl.uv.leikelen.Module.Processing.Kinect.HeadAngle
{
    public class HeadAngleLogic
    {
        public List<CustomBody> _bodies;
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public HeadAngleLogic()
        {
            _bodies = new List<CustomBody>();
            _dataAccessFacade.GetModalAccess().AddIfNotExists("Neck Orientation", "Angles of neck");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Neck Orientation", "Pitch", "rotation in X axis (nodding)", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Neck Orientation", "Yaw", "rotation in Y axis", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Neck Orientation", "Roll", "rotation in Z axis", null);

            _dataAccessFacade.GetModalAccess().AddIfNotExists("Lean", "Lean of body");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Lean", "X", "Lean in X axis (right or left)", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Lean", "Y", "Lean in Y axis (back and forward)", null);
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
                foreach (IBody body in bodies)
                {
                    var orientation = body.JointOrientations[JointType.Neck].Orientation;

                    var rotationX = orientation.Pitch();
                    var rotationY = orientation.Yaw();
                    var rotationZ = orientation.Roll();
                    if (rotationX != 0 || rotationY != 0 || rotationZ != 0)
                    {
                        CheckPerson.Instance.CheckIfExistsPerson(body.TrackingId);
                        var time = _dataAccessFacade.GetSceneInUseAccess()?.GetLocation();
                        if (time.HasValue)
                        {
                            _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Neck Orientation", "Pitch", time.Value, rotationX, false);
                            _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Neck Orientation", "Yaw", time.Value, rotationY, false);
                            _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Neck Orientation", "Roll", time.Value, rotationZ, false);

                            _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Lean", "X", time.Value, body.Lean.X, false);
                            _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Lean", "Y", time.Value, body.Lean.Y, false);

                            //Console.WriteLine($"XPitch: {rotationX}\tYYaw: {rotationY}\tZRoll: {rotationZ}");
                            //Console.WriteLine($"XLean: {body.Lean.X}\tYLean: {body.Lean.Y}");
                        }

                        
                    }
                }
            }
        }
    }
}
