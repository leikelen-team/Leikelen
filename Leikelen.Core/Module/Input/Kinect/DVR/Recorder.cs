using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using KinectEx;
using Microsoft.Kinect;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;

/// <summary>
/// Derived classes of KinectEx library.
/// </summary>
namespace cl.uv.leikelen.Module.Input.Kinect.DVR
{
    /// <summary>
    /// This class is one of two primary programmatic interfaces into the 
    /// cl.uv.leikelen.Module.Input.Kinect.DVR subsystem. Created to enable recording of frames to
    /// a <c>Stream</c>.
    /// </summary>
    public class Recorder : KinectEx.DVR.KinectRecorder
    {
        private API.DataAccess.IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        /// <summary>
        /// Initializes a new instance of the <see cref="Recorder"/> class.
        /// </summary>
        /// <param name="stream">The stream to which frames will be stored.</param>
        /// <param name="sensor">If supplied, the <c>KinectSensor</c> from which frames will be automatically
        /// retrieved for recording.</param>
        public Recorder(Stream stream, KinectSensor sensor = null) : base(stream, sensor)
        {

        }

        /// <summary>
        /// Used in "Manual" mode to record a single <c>BodyFrame</c>.
        /// </summary>
        public override void RecordFrame(BodyFrame frame)
        {
            if (!_isStarted)
                throw new InvalidOperationException("Cannot record frames unless the KinectRecorder is started.");

            var time = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
            if (!ReferenceEquals(null, frame) && time.HasValue)
            {
                var replayFrame = new ReplayBodyFrameCustomTime(frame, time.Value);
                if (MapDepthPositions)
                {
                    replayFrame.MapDepthPositions();
                }
                if (MapColorPositions)
                {
                    replayFrame.MapColorPositions();
                }
                _recordQueue.Enqueue(replayFrame);
                System.Diagnostics.Debug.WriteLine("+++ Enqueued Body Frame ({0})", _recordQueue.Count);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! FRAME SKIPPED (Body in KinectRecorder)");
            }
        }

        /// <summary>
        /// Used in "Manual" mode to record a single <c>BodyFrame</c> if
        /// the body frame data has already been retrieved from the frame.
        /// </summary>
        public override void RecordFrame(BodyFrame frame, List<CustomBody> bodies)
        {
            if (!_isStarted)
                throw new InvalidOperationException("Cannot record frames unless the KinectRecorder is started.");

            var time = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
            if (!ReferenceEquals(null, frame) && time.HasValue)
            {
                var replayFrame = new ReplayBodyFrameCustomTime(frame, bodies, time.Value);
                if (MapDepthPositions)
                {
                    replayFrame.MapDepthPositions();
                }
                if (MapColorPositions)
                {
                    replayFrame.MapColorPositions();
                }
                _recordQueue.Enqueue(replayFrame);
                System.Diagnostics.Debug.WriteLine("+++ Enqueued Body Frame ({0})", _recordQueue.Count);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! FRAME SKIPPED (Body in KinectRecorder)");
            }
        }

        /// <summary>
        /// Used in "Manual" mode to record a single <c>BodyFrame</c> if
        /// the body frame data has already been retrieved from the frame.
        /// </summary>
        public override void RecordFrame(BodyFrame frame, Body[] bodies)
        {
            if (!_isStarted)
                throw new InvalidOperationException("Cannot record frames unless the KinectRecorder is started.");

            var time = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
            if (!ReferenceEquals(null, frame) && time.HasValue)
            {
                var replayFrame = new ReplayBodyFrameCustomTime(frame, bodies, time.Value);
                if (MapDepthPositions)
                {
                    replayFrame.MapDepthPositions();
                }
                if (MapColorPositions)
                {
                    replayFrame.MapColorPositions();
                }
                _recordQueue.Enqueue(replayFrame);
                System.Diagnostics.Debug.WriteLine("+++ Enqueued Body Frame ({0})", _recordQueue.Count);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! FRAME SKIPPED (Body in KinectRecorder)");
            }
        }

        /// <summary>
        /// Used in "Manual" mode to record a single <c>ColorFrame</c>.
        /// </summary>
        public override void RecordFrame(ColorFrame frame)
        {
            if (!_isStarted)
                throw new InvalidOperationException("Cannot record frames unless the KinectRecorder is started.");

            var time = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
            if (!ReferenceEquals(null, frame) && time.HasValue)
            {
                _recordQueue.Enqueue(new ReplayColorFrameCustomTime(frame, time.Value));
                System.Diagnostics.Debug.WriteLine("+++ Enqueued Color Frame ({0})", _recordQueue.Count);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! FRAME SKIPPED (Color in KinectRecorder)");
            }
        }

        /// <summary>
        /// Used in "Manual" mode to record a single <c>ColorFrame</c> if
        /// the color frame data has already been retrieved from the frame.
        /// Note that the frame data must have been converted to BGRA format.
        /// </summary>
        public override void RecordFrame(ColorFrame frame, byte[] bytes)
        {
            if (!_isStarted)
                throw new InvalidOperationException("Cannot record frames unless the KinectRecorder is started.");

            var time = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
            if (!ReferenceEquals(null, frame) && time.HasValue)
            {
                _recordQueue.Enqueue(new ReplayColorFrameCustomTime(frame, bytes, time.Value));
                System.Diagnostics.Debug.WriteLine("+++ Enqueued Color Frame ({0})", _recordQueue.Count);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! FRAME SKIPPED (Color in KinectRecorder)");
            }
        }

        /// <summary>
        /// Used in "Manual" mode to record a single <c>DepthFrame</c>.
        /// </summary>
        public override void RecordFrame(DepthFrame frame)
        {
            if (!_isStarted)
                throw new InvalidOperationException("Cannot record frames unless the KinectRecorder is started.");

            var time = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
            if (!ReferenceEquals(null, frame) && time.HasValue)
            {
                _recordQueue.Enqueue(new ReplayDepthFrameCustomTime(frame, time.Value));
                System.Diagnostics.Debug.WriteLine("+++ Enqueued Depth Frame ({0})", _recordQueue.Count);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! FRAME SKIPPED (Depth in KinectRecorder)");
            }
        }

        /// <summary>
        /// Used in "Manual" mode to record a single <c>DepthFrame</c> if
        /// the depth frame data has already been retrieved from the frame.
        /// </summary>
        public override void RecordFrame(DepthFrame frame, ushort[] frameData)
        {
            if (!_isStarted)
                throw new InvalidOperationException("Cannot record frames unless the KinectRecorder is started.");

            var time = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
            if (!ReferenceEquals(null, frame) && time.HasValue)
            {
                _recordQueue.Enqueue(new ReplayDepthFrameCustomTime(frame, frameData, time.Value));
                System.Diagnostics.Debug.WriteLine("+++ Enqueued Depth Frame ({0})", _recordQueue.Count);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! FRAME SKIPPED (Depth in KinectRecorder)");
            }
        }

        /// <summary>
        /// Used in "Manual" mode to record a single <c>InfraredFrame</c>.
        /// </summary>
        public override void RecordFrame(InfraredFrame frame)
        {
            if (!_isStarted)
                throw new InvalidOperationException("Cannot record frames unless the KinectRecorder is started.");

            var time = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
            if (!ReferenceEquals(null, frame) && time.HasValue)
            {
                _recordQueue.Enqueue(new ReplayInfraredFrameCustomTime(frame, time.Value));
                System.Diagnostics.Debug.WriteLine("+++ Enqueued Infrared Frame ({0})", _recordQueue.Count);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! FRAME SKIPPED (Infrared in KinectRecorder)");
            }
        }

        /// <summary>
        /// Used in "Manual" mode to record a single <c>InfraredFrame</c> if
        /// the infrared frame data has already been retrieved from the frame.
        /// </summary>
        public override void RecordFrame(InfraredFrame frame, ushort[] frameData)
        {
            if (!_isStarted)
                throw new InvalidOperationException("Cannot record frames unless the KinectRecorder is started.");

            var time = _dataAccessFacade.GetSceneInUseAccess().GetLocation();
            if (!ReferenceEquals(null, frame) && time.HasValue)
            {
                _recordQueue.Enqueue(new ReplayInfraredFrameCustomTime(frame, frameData, time.Value));
                System.Diagnostics.Debug.WriteLine("+++ Enqueued Infrared Frame ({0})", _recordQueue.Count);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! FRAME SKIPPED (Infrared in KinectRecorder)");
            }
        }
    }
}
