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
    /// <summary>
    /// Logic for processing module to calculate angle of head.
    /// </summary>
    public class HeadAngleLogic
    {
        private List<CustomBody> _bodies;
        private API.DataAccess.IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadAngleLogic"/> class.
        /// </summary>
        public HeadAngleLogic()
        {
            _bodies = new List<CustomBody>();
            _dataAccessFacade.GetModalAccess().AddIfNotExists("Neck Orientation", "Angles of neck");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Neck Orientation", "Pitch", "rotation in X axis (nodding)", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Neck Orientation", "Yaw", "rotation in Y axis", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Neck Orientation", "Roll", "rotation in Z axis", null);

            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Neck Orientation", "Watching public", "The angle of the neck is watching to the public", null);//0

            _dataAccessFacade.GetModalAccess().AddIfNotExists("Lean", "Lean of body");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Lean", "X", "Lean in X axis (right or left)", null);
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Lean", "Y", "Lean in Y axis (back and forward)", null);


            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Lean", "Upside", "Lean in Y axis (back and forward) is to up", null);//0
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Lean", "Straight", "Lean in Y axis (back and forward) is straight", null);//1
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Lean", "Downside", "Lean in Y axis (back and forward) is to down", null);//2
        }

        /// <summary>
        /// Handles the FrameArrived event of the _bodyReader control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BodyFrameArrivedEventArgs"/> instance containing the event data.</param>
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
                            _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Neck Orientation", "Pitch", time.Value, rotationX, -1);
                            //_dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Neck Orientation", "Yaw", time.Value, rotationY, false);
                            _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Neck Orientation", "Roll", time.Value, rotationZ, -1);

                            _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Lean", "X", time.Value, body.Lean.X, -1);
                            //_dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Lean", "Y", time.Value, body.Lean.Y, false);

                            if(body.Lean.Y < (2/3) - 1) //between -1 and -0.333
                            {
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Lean", "Y", time.Value, body.Lean.Y, 0);

                            }
                            else if (body.Lean.Y < 1 - (2 / 3)) //between -0.333 and 0.333
                            {
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Lean", "Y", time.Value, body.Lean.Y, 1);

                            }
                            else //between 0.333 and 1
                            {
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Lean", "Y", time.Value, body.Lean.Y, 2);

                            }

                            //Console.WriteLine($"XPitch: {rotationX}\tYYaw: {rotationY}\tZRoll: {rotationZ}");
                            //Console.WriteLine($"XLean: {body.Lean.X}\tYLean: {body.Lean.Y}");

                            //2 is because the horizontal max y 4 mts
                            double distanceMaxOfBorder = 2 + Math.Abs(body.Joints[JointType.Neck].Position.X);
                            //4.3 is the distance between the public and the person
                            Console.WriteLine($"The distance between the sensor and person is: {body.Joints[JointType.Neck].Position.Z}");
                            double hipoten = Math.Sqrt(distanceMaxOfBorder * distanceMaxOfBorder + 4.3 * 4.3);
                            double angle = Math.Asin(distanceMaxOfBorder / hipoten);
                            if(rotationY <= angle)
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Neck Orientation", "Yaw", time.Value, body.Lean.Y, 0);
                            else
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Neck Orientation", "Yaw", time.Value, body.Lean.Y, -1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Function to create the intervals from created events.
        /// </summary>
        public async void StopRecording()
        {
            foreach (var personPair in CheckPerson.Instance.PersonsId)
            {
                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Neck Orientation", "Yaw", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold(), 0, "Watching public");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                   _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Lean", "Y" , _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold(), 0, "Upside");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Lean", "Y", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold(), 1, "Straight");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Lean", "Y", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold(), 2, "Downside");

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }
        }
    }
}
