using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using KinectEx;

namespace cl.uv.leikelen.Module.Processing.Kinect.ShouldersAngle
{
    /// <summary>
    /// Logic for processing module that calculate angle of shoulders.
    /// </summary>
    public class ShouldersAngleLogic
    {
        private List<CustomBody> _bodies;
        private const double _movementToPositive = 0;

        private readonly Dictionary<ulong, int> _personsId = new Dictionary<ulong, int>();
        private API.DataAccess.IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        /// <summary>
        /// Initializes a new instance of the <see cref="ShouldersAngleLogic"/> class.
        /// </summary>
        public ShouldersAngleLogic()
        {
            _bodies = new List<CustomBody>();

            _dataAccessFacade.GetModalAccess().AddIfNotExists("Shoulders", "Shoulders data");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Shoulders", "Angle", "angle between the shoulders", null);
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
                    try
                    {
                        var left = body.Joints[JointType.ShoulderLeft].Position;
                        var right = body.Joints[JointType.ShoulderRight].Position;
                        var spine = body.Joints[JointType.SpineShoulder].Position;

                        var v1 = new double[]
                        {
                            spine.X - left.X + _movementToPositive,
                            spine.Y - left.Y + _movementToPositive,
                            spine.Z - left.Z + _movementToPositive
                        };

                        var v2 = new double[]
                        {
                            right.X - spine.X + _movementToPositive,
                            right.Y - spine.Y + _movementToPositive,
                            right.Z - spine.Z + _movementToPositive
                        };

                        //The dot product of v1 and v2 is a function of 
                        //the cosine of the angle between them 
                        //(it's scaled by the product of their magnitudes).
                        //So first normalize v1 and v2
                        var v1mag = Math.Sqrt(v1[0] * v1[0] + v1[1] * v1[1] + v1[2] * v1[2]);
                        var v1norm = new double[] { v1[0] / v1mag, v1[1] / v1mag, v1[2] / v1mag };

                        var v2mag = Math.Sqrt(v2[0] * v2[0] + v2[1] * v2[1] + v2[2] * v2[2]);
                        var v2norm = new double[] { v2[0] / v2mag, v2[1] / v2mag, v2[2] / v2mag };

                        //Then calculate the dot product:
                        var res = v1norm[0] * v2norm[0] + v1norm[1] * v2norm[1] + v1norm[2] * v2norm[2];

                        //And finally, recover the angle:
                        var angle = Math.Acos(res) * 180.0 / Math.PI;
                        if(!double.IsNaN(angle) && !double.IsInfinity(angle))
                        {
                            CheckPerson.Instance.CheckIfExistsPerson(body.TrackingId);
                            var time = _dataAccessFacade.GetSceneInUseAccess()?.GetLocation();
                            if (time.HasValue)
                            {
                                _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[body.TrackingId], "Shoulders", "Angle", time.Value, angle, -1);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Exception obtained at try get shoulders angle: {ex.Message}");
                    }
                    
                }
            }
        }
    }
}
