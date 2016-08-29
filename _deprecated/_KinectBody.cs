using Microsoft.Kinect;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.utils;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.core
{
    public class _KinectBody
    {

        ///// <summary> Active Kinect sensor </summary>
        //private KinectSensor kinectSensor { get; set; }
        //private MultiSourceFrameReader frameReader { get; set; }
        //private KinectBodyView kinectBodyView = null;

        ///// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        //private Body[] bodies { get; set; }
        //public List<GestureDetector> gestureDetectorList { get; private set; } = null;

        
        //private static _KinectBody instance = null;
        //private static _KinectBody Instance
        //{
        //    get
        //    {
        //        if (instance == null) instance = new _KinectBody();
        //        return instance;
        //    }
        //}

        //private _KinectBody()
        //{
        //    // only one sensor is currently supported
        //    this.kinectSensor = KinectSensor.GetDefault();
        //    this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;
        //    this.kinectSensor.Open();
        //    frameReader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
        //    frameReader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
        //    kinectBodyView = new KinectBodyView(this.kinectSensor);
            
        //    this.gestureDetectorList = new List<GestureDetector>();
        //    for (int i = 0; i < kinectSensor.BodyFrameSource.BodyCount; ++i)
        //    {
        //        //GestureResultView result = new GestureResultView(i, false, false, 0.0f);
        //        GestureDetector detector = new GestureDetector(i, this.kinectSensor/*, result*/);
        //        this.gestureDetectorList.Add(detector);

        //        // split gesture results across the first two columns of the content grid
        //        //ContentControl contentControl = new ContentControl();
        //        //contentControl.Content = this.gestureDetectorList[i].GestureResultView;
        //        //Grid.SetColumn(contentControl, 0);
        //        //Grid.SetRow(contentControl, row++);

        //    }


        //}

        //private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        //{
        //    // on failure, set the status text
        //    //MainWindow.Instance().StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
        //    //                                                : Properties.Resources.SensorNotAvailableStatusText;
        //}

        //private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        //{

        //    var reference = e.FrameReference.AcquireFrame();

        //    using (ColorFrame colorFrame = reference.ColorFrameReference.AcquireFrame())
        //    {

        //        if (colorFrame != null)
        //        {
        //            if (MainWindow.Instance().kstudio.playback != null)
        //            {
        //                TimeSpan currentTime = MainWindow.Instance().kstudio.playback.CurrentRelativeTime;
        //                if (currentTime > MainWindow.lastCurrentTime || currentTime < MainWindow.lastCurrentTime.Subtract(TimeSpan.FromSeconds(2)))
        //                {
        //                    MainWindow.Instance().sceneSlider.Value = currentTime.TotalMilliseconds;
        //                    MainWindow.Instance().sceneCurrentTimeLabel.Content = currentTime.ToString(@"hh\:mm\:ss");
        //                    MainWindow.lastCurrentTime = currentTime;
        //                }
        //            }
        //            if (MainWindow.Instance().fondoCheckBox2.IsChecked != false)
        //            {
        //                BitmapSource btmSource = colorFrame.ToBitmap();
        //                MainWindow.Instance().colorImageControl.Source = btmSource;
        //                //if (escena.IsRecording)
        //                //    escena.AddColorFrame(btmSource);
        //            }


        //        }
        //    }

        //    using (BodyFrame bodyFrame = reference.BodyFrameReference.AcquireFrame())
        //    {
        //        if (bodyFrame != null)
        //        {
        //            this.PrintBodyFrame(bodyFrame);
        //            MainWindow.Instance().bodyImageControl.Source = kinectBodyView.DrawingImage;
        //            //if (escena.IsRecording)
        //            //    escena.AddBodyFrame(kinectBodyView.DrawingImage);
        //        }
        //    }
        //}


        //private void PrintBodyFrame(BodyFrame bodyFrame)
        //{
        //    bool dataReceived = false;

        //    using (bodyFrame)
        //    {
        //        if (bodyFrame != null)
        //        {
        //            if (this.bodies == null)
        //            {
        //                // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
        //                this.bodies = new Body[bodyFrame.BodyCount];
        //            }

        //            // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
        //            // As long as those body objects are not disposed and not set to null in the array,
        //            // those body objects will be re-used.
        //            bodyFrame.GetAndRefreshBodyData(this.bodies);
        //            dataReceived = true;
        //        }
        //    }

        //    if (dataReceived)
        //    {
        //        // visualize the new body data
                

        //        // we may have lost/acquired bodies, so update the corresponding gesture detectors
        //        if (this.bodies != null)
        //        {
        //            // loop through all bodies to see if any of the gesture detectors need to be updated
        //            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
        //            for (int i = 0; i < maxBodies; ++i)
        //            {
        //                Body body = this.bodies[i];
        //                ulong trackingId = body.TrackingId;
        //                if (Scene.Instance != null)
        //                {
        //                    bool personExists = Scene.Instance.Persons.Exists(p => p.TrackingId == trackingId);
        //                    if (!personExists && trackingId != 0)
        //                    {
        //                        Person person = new Person(
        //                                trackingId,
        //                                Scene.Instance.Persons.Count
        //                            );
        //                        Scene.Instance.Persons.Add(person);
        //                    }
        //                }
                        
        //                // if the current body TrackingId changed, update the corresponding gesture detector with the new value
        //                if (trackingId != this.gestureDetectorList[i].TrackingId)
        //                {
        //                    this.gestureDetectorList[i].TrackingId = trackingId;

        //                    // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
        //                    // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
        //                    this.gestureDetectorList[i].IsPaused = trackingId == 0;
        //                }
        //            }
        //        }
        //        kinectBodyView.UpdateBodyFrame(this.bodies);
        //    }
        //}

        //public void Close()
        //{
        //    if (this.frameReader != null)
        //    {
        //        frameReader.MultiSourceFrameArrived -= Reader_MultiSourceFrameArrived;
        //        frameReader.Dispose();
        //        frameReader = null;
        //    }
            
        //    if (this.gestureDetectorList != null)
        //    {
        //        // The GestureDetector contains disposable members (VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader)
        //        foreach (GestureDetector detector in this.gestureDetectorList)
        //        {
        //            detector.Dispose();
        //        }

        //        this.gestureDetectorList.Clear();
        //        this.gestureDetectorList = null;
        //    }

        //    if (this.kinectSensor != null)
        //    {
        //        this.kinectSensor.IsAvailableChanged -= this.Sensor_IsAvailableChanged;
        //        this.kinectSensor.Close();
        //        this.kinectSensor = null;
        //    }
        //}

    }
}
