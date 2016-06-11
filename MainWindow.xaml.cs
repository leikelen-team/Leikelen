//---------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <Description>
// This program tracks up to 6 people simultaneously.
// If a person is tracked, the associated gesture detector will determine if that person is seated or not.
// If any of the 6 positions are not in use, the corresponding gesture detector(s) will be paused
// and the 'Not Tracked' image will be displayed in the UI.
// </Description>
//----------------------------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Kinect;
    using Microsoft.Kinect.VisualGestureBuilder;
    //using AForge.Video.FFMPEG;
    using System.Drawing;
    using System.Drawing.Imaging;

    using Accord.Extensions.Imaging;
    using System.Windows.Media.Imaging;
    using System.IO;/// <summary>
                    /// Interaction logic for the MainWindow
                    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary> Active Kinect sensor </summary>
        private KinectSensor kinectSensor = null;
        
        /// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        private Body[] bodies = null;

        /// <summary> Reader for body frames </summary>
        //private BodyFrameReader bodyFrameReader = null;

        //private ColorFrameReader colorFrameReader = null;

        private MultiSourceFrameReader _reader;
        //private VideoFileWriter m_writer = null;
        private bool m_isRecording = false;
        private int m_width = 1280, m_height = 720;

        /// <summary> Current status text to display </summary>
        private string statusText = null;

        /// <summary> KinectBodyView object which handles drawing the Kinect bodies to a View box in the UI </summary>
        private KinectBodyView kinectBodyView = null;
        
        /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
        private List<GestureDetector> gestureDetectorList = null;

        

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            // only one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();
            
            // set IsAvailableChanged event notifier
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();
            //if (this.kinectSensor.IsAvailable) {
            //    m_width = this.kinectSensor.ColorFrameSource.FrameDescription.Width;
            //    m_height = this.kinectSensor.ColorFrameSource.FrameDescription.Height;
            //}

            // set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            // open the reader for the body frames
            //this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            //this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();

            //// set the BodyFramedArrived event notifier
            //this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
            //this.colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;

            _reader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
            _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            
            
            // initialize the BodyViewer object for displaying tracked bodies in the UI
            this.kinectBodyView = new KinectBodyView(this.kinectSensor);

            // initialize the gesture detection objects for our gestures
            this.gestureDetectorList = new List<GestureDetector>();

            // initialize the MainWindow
            this.InitializeComponent();

            // set our data context objects for display in UI
            this.DataContext = this;
            this.kinectBodyViewbox.DataContext = this.kinectBodyView;

            // create a gesture detector for each body (6 bodies => 6 detectors) and create content controls to display results in the UI
            //int col0Row = 0;
            int row = 0;
            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
            for (int i = 0; i < maxBodies; ++i)
            {
                GestureResultView result = new GestureResultView(i, false, false, 0.0f);
                GestureDetector detector = new GestureDetector(this.kinectSensor, result);
                this.gestureDetectorList.Add(detector);                
                
                // split gesture results across the first two columns of the content grid
                ContentControl contentControl = new ContentControl();
                contentControl.Content = this.gestureDetectorList[i].GestureResultView;

                Grid.SetColumn(contentControl, 1);
                Grid.SetRow(contentControl, row++);
                //++row;

                //if (i % 2 == 0)
                //{
                //    // Gesture results for bodies: 0, 2, 4
                //    Grid.SetColumn(contentControl, 0);
                //    Grid.SetRow(contentControl, col0Row);
                //    ++col0Row;
                //}
                //else
                //{
                //    // Gesture results for bodies: 1, 3, 5
                //    Grid.SetColumn(contentControl, 1);
                //    Grid.SetRow(contentControl, col1Row);
                //    ++col1Row;
                //} 

                this.contentGrid.Children.Add(contentControl);
            }
        }

       



        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;

                    // notify any bound elements that the text has changed
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            //if (this.bodyFrameReader != null)
            //{
            //    // BodyFrameReader is IDisposable
            //    this.bodyFrameReader.FrameArrived -= this.Reader_BodyFrameArrived;
            //    this.bodyFrameReader.Dispose();
            //    this.bodyFrameReader = null;
            //}

            if (this._reader != null)
            {
                _reader.MultiSourceFrameArrived -= Reader_MultiSourceFrameArrived;
                _reader.Dispose();
                _reader = null;
            }

            

            if (this.gestureDetectorList != null)
            {
                // The GestureDetector contains disposable members (VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader)
                foreach (GestureDetector detector in this.gestureDetectorList)
                {
                    detector.Dispose();
                }

                this.gestureDetectorList.Clear();
                this.gestureDetectorList = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.IsAvailableChanged -= this.Sensor_IsAvailableChanged;
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        /// <summary>
        /// Handles the event when the sensor becomes unavailable (e.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.SensorNotAvailableStatusText;
        }


        private void AddColorFrameToVideoRecording(ColorFrame colorFrame)
        {
            using (colorFrame)
            {
                if (colorFrame != null && m_isRecording)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;
                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        try
                        {
                            Bitmap bmap = new Bitmap(colorFrameDescription.Width, colorFrameDescription.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                            BitmapData bmapdata = bmap.LockBits(
                                new Rectangle(0, 0, colorFrameDescription.Height, colorFrameDescription.Height),
                                ImageLockMode.WriteOnly,
                                bmap.PixelFormat);
                            IntPtr ptr = bmapdata.Scan0;
                            colorFrame.CopyConvertedFrameDataToIntPtr(ptr,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);
                            bmap.UnlockBits(bmapdata);
                            string time = System.DateTime.Now.ToString("HH-mm-ss-fff", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                            //string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                            //            CultureInfo.InvariantCulture);
                            bmap.Save(@"RecImages/" + time + ".png", ImageFormat.Png);
                            //m_writer.WriteVideoFrame(bmap);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }


        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();
            

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    BitmapSource btmSource = frame.ToBitmap();
                    camera.Source = btmSource;

                    // if is recording mode, save frame as JPEG image into RecImages directory
                    if (m_isRecording) { 
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(btmSource));
                        string time = System.DateTime.Now.ToString("HH-mm-ss-fff", 
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                        using (var fs = new FileStream(@"RecImages/" + time + ".jpeg", FileMode.Create, FileAccess.Write))
                        {
                            encoder.Save(fs);
                        }
                    }


                    //mediaElement.
                }
            }

            // Body
            using (BodyFrame frame = reference.BodyFrameReference.AcquireFrame())
            {
                this.BodyFrameArrived(frame);
            }
        }

        private void BodyFrameArrived(BodyFrame bodyFrame)
        {
            bool dataReceived = false;

            using (bodyFrame)
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                // visualize the new body data
                this.kinectBodyView.UpdateBodyFrame(this.bodies);

                // we may have lost/acquired bodies, so update the corresponding gesture detectors
                if (this.bodies != null)
                {
                    // loop through all bodies to see if any of the gesture detectors need to be updated
                    int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
                    for (int i = 0; i < maxBodies; ++i)
                    {
                        Body body = this.bodies[i];
                        ulong trackingId = body.TrackingId;

                        // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        if (trackingId != this.gestureDetectorList[i].TrackingId)
                        {
                            this.gestureDetectorList[i].TrackingId = trackingId;

                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            this.gestureDetectorList[i].IsPaused = trackingId == 0;
                        }
                    }
                }
            }
        }

        //private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        //{
            

        //}

        /// <summary>
        /// Handles the body frame data arriving from the sensor and updates the associated gesture detector object for each body
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        //private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        //{
        //    bool dataReceived = false;

        //    using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
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
        //        this.kinectBodyView.UpdateBodyFrame(this.bodies);

        //        // we may have lost/acquired bodies, so update the corresponding gesture detectors
        //        if (this.bodies != null)
        //        {
        //            // loop through all bodies to see if any of the gesture detectors need to be updated
        //            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
        //            for (int i = 0; i < maxBodies; ++i)
        //            {
        //                Body body = this.bodies[i];
        //                ulong trackingId = body.TrackingId;

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
        //    }
        //}

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("test button clicked");
        }

        private void makeAvi(string imageInputfolderName, string outVideoFileName, float fps = 30.0f, string imgSearchPattern = "*.png")
        {   // reads all images in folder 
            VideoWriter w = new VideoWriter(outVideoFileName,
                new Accord.Extensions.Size(480, 640), fps, true);
            Accord.Extensions.Imaging.ImageDirectoryReader ir =
                new ImageDirectoryReader(imageInputfolderName, imgSearchPattern);
            
            while (ir.Position < ir.Length)
            {
                IImage i = ir.Read();
                w.Write(i);
            }
            w.Close();
        }
        private void grabarButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_isRecording)
            {
                m_isRecording = false;
                grabarButton.Content = "Grabar";
                //makeAvi(@"RecImages/", @"videito.avi");
            }else{
                m_isRecording = true;
                grabarButton.Content = "Detener";
            }
        }

        //private void grabarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (m_isRecording)
        //    {
        //        m_isRecording = false;
        //        m_writer.Close();
        //        grabarButton.Content = "Grabar";
        //    }
        //    else
        //    {
        //        m_writer = new VideoFileWriter();
        //        //string time = System.DateTime.Now.ToString("hh'-'mm'-'ss", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
        //        m_writer.Open("video_testing_rolo.mpeg", m_width, m_height, 30, VideoCodec.MSMPEG4v3, 12800000);
        //        m_isRecording = true;
        //        grabarButton.Content = "Detener";
        //    }
        //}
    }
}
