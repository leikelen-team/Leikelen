using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using KinectEx;
using cl.uv.leikelen.API.FrameProvider.Kinect;

namespace cl.uv.leikelen.Module.Input.Kinect
{
    public class GestureDetector : IDisposable
    {
        public static List<GestureDetector> GestureDetectorList { get; set; } = new List<GestureDetector>();
        private static List<CustomBody> _bodies = new List<CustomBody>();

        /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
        public VisualGestureBuilderFrameReader VgbFrameReader = null;

        /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
        private VisualGestureBuilderFrameSource _vgbFrameSource = null;
        
        /// <summary> The body index (0-5) associated with the current gesture detector </summary>
        public int BodyIndex { get; private set; }

        public event EventHandler<KinectGestureFrameArrivedArgs> KinectGestureFrameArrived;

        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        /// <summary>
        /// Gets or sets the body tracking ID associated with the current detector
        /// The tracking ID can change whenever a body comes in/out of scope
        /// </summary>
        public ulong TrackingId
        {
            get
            {
                return this._vgbFrameSource.TrackingId;
            }

            set
            {
                if (this._vgbFrameSource.TrackingId != value)
                {
                    this._vgbFrameSource.TrackingId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the detector is currently paused
        /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return this.VgbFrameReader.IsPaused;
            }

            set
            {
                if (this.VgbFrameReader.IsPaused != value)
                {
                    this.VgbFrameReader.IsPaused = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureDetector"/> class.
        /// </summary>
        /// <param name="bodyIndex">Index of the body.</param>
        /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with.</param>
        public GestureDetector(int bodyIndex, KinectSensor kinectSensor)
        {
            BodyIndex = bodyIndex;
            if (kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSensor is null");
            }

            // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
            this._vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);

            this.VgbFrameReader = this._vgbFrameSource.OpenReader();
            if (this.VgbFrameReader != null)
            {
                this.VgbFrameReader.IsPaused = true;
                this.VgbFrameReader.FrameArrived += VgbFrameReader_FrameArrived;
            }
            
            //get all discrete and continuous postures
            var discretePostures = _dataAccessFacade.GetSubModalAccess().GetAll("Discrete Posture");
            var continuousPostures = _dataAccessFacade.GetSubModalAccess().GetAll("Continuous Posture");
            
            //add all the database filenames of each posture to posturePaths list
            List<string> posturePaths = new List<string>();
            foreach (var discrete in discretePostures)
            {
                if (!ReferenceEquals(null, discrete.File))
                {
                    string internalFilePath = $"{_dataAccessFacade.GetGeneralSettings().GetDataDirectory()}" +
                    $"modal/Discrete Posture/{discrete.File}";
                    posturePaths.Add(internalFilePath);
                }
                    
            }
            foreach (var continuous in continuousPostures)
            {
                if (!ReferenceEquals(null, continuous.File))
                {
                    string internalFilePath = $"{_dataAccessFacade.GetGeneralSettings().GetDataDirectory()}" +
                    $"modal/Continuous Posture/{continuous.File}";
                    posturePaths.Add(internalFilePath);
                }
            }

            //add the database files of the postures to the detector
            foreach (var posturePath in posturePaths)
            {
                Console.WriteLine("Loading VGB Database: '{0}'", posturePath);
                using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(posturePath))
                {
                    _vgbFrameSource.AddGestures(database.AvailableGestures);
                }
            }
        }

        private void VgbFrameReader_FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            var kFrame = e.FrameReference.AcquireFrame();
            if(kFrame != null)
            {
                var frame = new KinectGestureFrameArrivedArgs()
                {
                    Time = _dataAccessFacade.GetSceneInUseAccess().GetLocation(),
                    TrackingId = this.TrackingId,
                    Frame = kFrame
                };
                OnKinectGestureFrameArrived(frame);
            }
        }

        public static void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            IEnumerable<IBody> bodies = null; // to make the GetBitmap call a little cleaner
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    frame.GetAndRefreshBodyData(_bodies);
                    bodies = _bodies;
                }
            }

            if (bodies != null)
            {
                int i = 0;
                foreach (IBody body in bodies)
                {
                    // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                    if (body.TrackingId != GestureDetectorList[i].TrackingId)
                    {
                        GestureDetectorList[i].TrackingId = body.TrackingId;

                        // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                        // if the current body is not tracked, pause its detector so we don't
                        //waste resources trying to get invalid gesture results
                        GestureDetectorList[i].IsPaused = body.TrackingId == 0;
                        Console.WriteLine($"gesture {i} pausado= {GestureDetectorList[i].IsPaused}");
                    }
                    i++;
                }
            }
        }

        private void OnKinectGestureFrameArrived(KinectGestureFrameArrivedArgs e)
        {
            KinectGestureFrameArrived?.Invoke(this, e);
        }

        public void Dispose()
        {
            if (this.VgbFrameReader != null)
            {
                this.VgbFrameReader.Dispose();
                this.VgbFrameReader = null;
            }

            if (this._vgbFrameSource != null)
            {
                this._vgbFrameSource.Dispose();
                this._vgbFrameSource = null;
            }
        }
    }
}
