//------------------------------------------------------------------------------
// <copyright file="GestureDetector.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.core
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Kinect;
    using Microsoft.Kinect.VisualGestureBuilder;
    using pojos;
    using System.Linq;
    using views;
    using models;
    using db;

    //using System.Data.SQLite;
    /// <summary>
    /// Gesture Detector class which listens for VisualGestureBuilderFrame events from the service
    /// and updates the associated GestureResultView object with the latest results for the 'Seated' gesture
    /// </summary>
    public class GestureDetector : IDisposable
    {
        /// <summary> Path to the gesture database that was trained with VGB </summary>
        //private readonly string gestureDatabase = @"Database\Seated.gbd";

        /// <summary> Name of the discrete gesture in the database that we want to track </summary>
        //private readonly string seatedGestureName = "Seated";

        /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
        private VisualGestureBuilderFrameSource vgbFrameSource = null;

        /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
        private VisualGestureBuilderFrameReader vgbFrameReader = null;
        

        /// <summary>
        /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
        /// </summary>
        /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
        /// <param name="gestureResultView">GestureResultView object to store gesture results of a single body to</param>
        public GestureDetector(int bodyIndex, KinectSensor kinectSensor/*, GestureResultView gestureResultView*/)
        {
            this.BodyIndex = bodyIndex;
            if (kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSensor");
            }

            //if (gestureResultView == null)
            //{
            //    throw new ArgumentNullException("gestureResultView");
            //}
            
            //this.GestureResultView = gestureResultView;
            
            // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
            this.vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);
            //this.vgbFrameSource.TrackingIdLost += this.Source_TrackingIdLost;

            // open the reader for the vgb frames
            this.vgbFrameReader = this.vgbFrameSource.OpenReader();
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.IsPaused = true;
                this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            }


            //Console.WriteLine("-----------");

            // agrupa todos las posturas que tienen el mismo path
            var posturePaths = PostureTypeContext.db.PostureType.ToList()
                .FindAll(posture => posture != PostureType.none)
                .GroupBy(posture => posture.Path);
            foreach (var posturePath in posturePaths)
            {
                Console.WriteLine("Loading VGB Database: '{0}'", posturePath.Key);
                using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(posturePath.Key))
                {
                    vgbFrameSource.AddGestures(database.AvailableGestures);
                }
            }
            
        }

        /// <summary> Gets the GestureResultView object which stores the detector results for display in the UI </summary>
        //public GestureResultView GestureResultView { get; private set; }

        /// <summary> The body index (0-5) associated with the current gesture detector </summary>
        public int BodyIndex { get; private set; }

        /// <summary>
        /// Gets or sets the body tracking ID associated with the current detector
        /// The tracking ID can change whenever a body comes in/out of scope
        /// </summary>
        public ulong TrackingId
        {
            get
            {
                return this.vgbFrameSource.TrackingId;
            }

            set
            {
                if (this.vgbFrameSource.TrackingId != value)
                {
                    this.vgbFrameSource.TrackingId = value;
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
                return this.vgbFrameReader.IsPaused;
            }

            set
            {
                if (this.vgbFrameReader.IsPaused != value)
                {
                    this.vgbFrameReader.IsPaused = value;
                }
            }
        }

        /// <summary>
        /// Disposes all unmanaged resources for the class
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
        /// </summary>
        /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.vgbFrameReader != null)
                {
                    this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                    this.vgbFrameReader.Dispose();
                    this.vgbFrameReader = null;
                }

                if (this.vgbFrameSource != null)
                {
                    //this.vgbFrameSource.TrackingIdLost -= this.Source_TrackingIdLost;
                    this.vgbFrameSource.Dispose();
                    this.vgbFrameSource = null;
                }
            }
        }

        /// <summary>
        /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            
            if (!Kinect.Instance.Recorder.IsRecording) return;
            VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            {
                if (frame != null)
                {
                   
                    // get the discrete gesture results which arrived with the latest frame
                    IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                    if (discreteResults != null)
                    {
                        Person person = Scene.Instance.Persons
                                                .FirstOrDefault(p => p.TrackingId == (long)this.TrackingId);
                        // we only have one gesture in this source object, but you can get multiple gestures
                        bool postureDetected = false;
                        foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                        {
                            //Posture.Name[] postures = (Posture.Name[])Enum.GetValues(typeof(Posture.Name));
                            foreach (PostureType postureType in PostureTypeContext.db.PostureType.ToList())
                            {
                                //if (gesture.Name.Equals(this.seatedGestureName) && gesture.GestureType == GestureType.Discrete)
                                if (gesture.Name.Equals(postureType.Name) && gesture.GestureType == GestureType.Discrete)
                                {
                                    DiscreteGestureResult result = null;
                                    discreteResults.TryGetValue(gesture, out result);

                                    if (result != null)
                                    {
                                        // update the GestureResultView object with new gesture result values
                                        //this.GestureResultView.UpdateGestureResult(true, result.Detected, result.Confidence);
                                        if (result.Detected && Scene.Instance!=null)
                                        {
                                            //Console.WriteLine("registered posture:"+postureType.name);
                                            MicroPosture mc = new MicroPosture(
                                                        postureType,
                                                        Kinect.Instance.Recorder.getCurrentLocation()
                                                    );
                                            
                                            person.MicroPostures.Add(mc);
                                            postureDetected = true;
                                            //Scene.Instance.persons[BodyIndex].postures.
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        if (!postureDetected && Scene.Instance != null)
                        {
                            //Console.WriteLine("registered posture:" + PostureType.none.name);
                            person.MicroPostures.Add(new MicroPosture(PostureType.none, Kinect.Instance.Recorder.getCurrentLocation()));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        //private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
        //{
        //    // update the GestureResultView object to show the 'Not Tracked' image in the UI
        //    this.GestureResultView.UpdateGestureResult(false, false, 0.0f);
        //}
    }
}
