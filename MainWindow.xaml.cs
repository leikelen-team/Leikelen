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
    //using Microsoft.Kinect.Tools;
    using System.IO;
    using Win32;    //using Win32;
    using pojos;    //using System.Windows.Forms;                    //using System.Windows.Forms;/// <summary>
    using System.Threading;                    /// Interaction logic for the MainWindow
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
        //private bool isRecording = false;
        
        //private int m_width = 1280, m_height = 720;

        /// <summary> Current status text to display </summary>
        private string statusText = null;

        /// <summary> KinectBodyView object which handles drawing the Kinect bodies to a View box in the UI </summary>
        public static  KinectBodyView kinectBodyView = null;
        
        /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
        private List<GestureDetector> gestureDetectorList = null;

        private _Escena escena;
        private KinectStudioHandler kstudio;
        public static ChartForm chartForm { get; private set; }
        public static TimeSpan lastCurrentTime = TimeSpan.FromSeconds(0);
        private bool sceneSliderUserDragging = false;
        //private Timer timer;

        //private KinectBodyView kinectBodyView;

        //public static KinectBodyView KinectBodyView
        //{
        //    get { return kinectBodyView; }
        //    set { kinectBodyView = value; }
        //}


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
            kinectBodyView = new KinectBodyView(this.kinectSensor);

            // initialize the gesture detection objects for our gestures
            this.gestureDetectorList = new List<GestureDetector>();

            // initialize the MainWindow
            this.InitializeComponent();

            // set our data context objects for display in UI
            this.DataContext = this;
            //this.kinectBodyViewbox.DataContext = kinectBodyView;
            _VisualizerXamlView.Instance().InitColorFrame(ref this.colorImageControl);
            _VisualizerXamlView.Instance().InitBodyFrame(ref this.bodyImageControl);

            // create a gesture detector for each body (6 bodies => 6 detectors) and create content controls to display results in the UI
            //int col0Row = 0;
            int row = 2;
            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
            for (int i = 0; i < maxBodies; ++i)
            {
                GestureResultView result = new GestureResultView(i, false, false, 0.0f);
                GestureDetector detector = new GestureDetector(i, this.kinectSensor, result);
                this.gestureDetectorList.Add(detector);                
                
                // split gesture results across the first two columns of the content grid
                ContentControl contentControl = new ContentControl();
                contentControl.Content = this.gestureDetectorList[i].GestureResultView;

                Grid.SetColumn(contentControl, 0);
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
                this.timeLineGrid.Children.Add(contentControl);
                

                //this.contentGrid.Children.Add(contentControl);
            }
            this.escena = new _Escena();
            this.kstudio = new KinectStudioHandler();
            //escena.SetColorImageControl(ref this.colorImageControl);
            this.button_EditPerson1.IsEnabled = false;
            this.button_EditPerson2.IsEnabled = false;
            this.button_EditPerson3.IsEnabled = false;
            this.button_EditPerson4.IsEnabled = false;
            this.button_EditPerson5.IsEnabled = false;
            this.button_EditPerson6.IsEnabled = false;
            this.exportButtons.IsEnabled = false;
            this.playButton.IsEnabled = false;
            this.showGraphButtons.IsEnabled = false;
            this.stopButton.IsEnabled = false;

            //ColumnDefinition col;
            //TextBlock text;
            //for (int i = 0; i < 60; i++)
            //{
            //    col = new ColumnDefinition();
            //    col.Width = new GridLength(20, GridUnitType.Pixel);
            //    timeLineGrid.ColumnDefinitions.Add(col);

            //    text = new TextBlock();
            //    text.Text = "|";

            //    Grid.SetRow(text, 0);
            //    Grid.SetColumn(text, i);
            //    timeLineGrid.Children.Add(text);
            //}

        }

        public static MainWindow Instance()
        {
            return (MainWindow)System.Windows.Application.Current.MainWindow;
        }

        public void disableButtons()
        {
            this.button_EditPerson1.IsEnabled = false;
            this.button_EditPerson2.IsEnabled = false;
            this.button_EditPerson3.IsEnabled = false;
            this.button_EditPerson4.IsEnabled = false;
            this.button_EditPerson5.IsEnabled = false;
            this.button_EditPerson6.IsEnabled = false;
            this.exportButtons.IsEnabled = false;
            this.playButton.IsEnabled = false;
            this.showGraphButtons.IsEnabled = false;
        }

        public void enableButtons()
        {
            this.button_EditPerson1.IsEnabled = true;
            this.button_EditPerson2.IsEnabled = true;
            this.button_EditPerson3.IsEnabled = true;
            this.button_EditPerson4.IsEnabled = true;
            this.button_EditPerson5.IsEnabled = true;
            this.button_EditPerson6.IsEnabled = true;
            this.exportButtons.IsEnabled = true;
            this.playButton.IsEnabled = true;
            this.showGraphButtons.IsEnabled = true;
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



        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            
            var reference = e.FrameReference.AcquireFrame();
            
            using (ColorFrame colorFrame = reference.ColorFrameReference.AcquireFrame())
            {
                
                if (colorFrame != null)
                {
                    TimeSpan currentTime = kstudio.playback.CurrentRelativeTime;
                    if (currentTime > lastCurrentTime || currentTime < lastCurrentTime.Subtract(TimeSpan.FromSeconds(2)) )
                    {
                        sceneSlider.Value = currentTime.TotalMilliseconds;
                        this.sceneCurrentTimeLabel.Content = currentTime.ToString(@"hh\:mm\:ss");
                        lastCurrentTime = currentTime;
                    }
                    
                    if(this.fondoCheckBox.IsChecked != false)
                    {
                        BitmapSource btmSource = colorFrame.ToBitmap();
                        this.colorImageControl.Source = btmSource;
                        //if (escena.IsRecording)
                        //    escena.AddColorFrame(btmSource);
                    }
                    
                }
            }

            using (BodyFrame bodyFrame = reference.BodyFrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    this.PrintBodyFrame(bodyFrame);
                    this.bodyImageControl.Source = kinectBodyView.DrawingImage;
                    //if (escena.IsRecording)
                    //    escena.AddBodyFrame(kinectBodyView.DrawingImage);
                }
            }

            

        }

        private void PrintBodyFrame(BodyFrame bodyFrame)
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
                kinectBodyView.UpdateBodyFrame(this.bodies);
                
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


        private void grabarButton_Click(object sender, RoutedEventArgs e)
        {

            if (this.kstudio.isRecording)
            {
                this.kstudio.StopRecording(); 
                recordButton.Content = Properties.Buttons.StartRecording;
                this.enableButtons();
            }
            else
            {
                this.kstudio.StartRecording();
                recordButton.Content = Properties.Buttons.StopRecording;
                this.disableButtons();
            }

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {

            if (this.kstudio.isPlaying)
            {
                this.kstudio.PausePlaying();
                this.playButton.Content = Properties.Buttons.StartPlaying;
            }
            else
            {
                //if (!this.kstudio.IsRecordedOrImported)
                //{
                //    MessageBox.Show(Properties.Messages.NotRecordedOrImportedScene);
                //    return;
                //}
                this.kstudio.StartOrResumePlaying();
                this.playButton.Content = Properties.Buttons.PausePlaying;
                this.stopButton.IsEnabled = true;
            }
        }
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            this.stopButton.IsEnabled = false;
            if (this.kstudio.isRecording)
            {
                this.kstudio.StopRecording();
                this.recordButton.Content = Properties.Buttons.StartRecording;
            }else
            {
                this.kstudio.StopPlaying();
                this.playButton.Content = Properties.Buttons.StartPlaying;
            }
        }

        private void importButtons_Click(object sender, RoutedEventArgs e)
        {
            Win32.OpenFileDialog dlg = new Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = Properties.Resources.XefExtension; // Default file extension
            dlg.Filter = Properties.Resources.EventFileDescription + " " + Properties.Resources.EventFileFilter; // Filter files by extension
            dlg.Title = "Importar escena";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                Console.WriteLine("filePath: "+dlg.FileName);
                this.kstudio.ImportScene(dlg.FileName);
                enableButtons();
                this.Title = "Visualizador Multimodal - "+Scene.Instance.name;
                sceneTitleLabel.Content = Scene.Instance.name;
                sceneDurationLabel.Content = Scene.Instance.duration.ToString(@"hh\:mm\:ss");
            }

            

            //ColumnDefinition c1 = new ColumnDefinition();
            //c1.Width = new GridLength(5, GridUnitType.Pixel);
            //timeLineGrid.ColumnDefinitions.Add(c1);

            //ColumnDefinition c2 = new ColumnDefinition();
            //c2.Width = new GridLength(5, GridUnitType.Pixel);
            //timeLineGrid.ColumnDefinitions.Add(c2);

            //ColumnDefinition c3 = new ColumnDefinition();
            //c3.Width = new GridLength(5, GridUnitType.Pixel);
            //timeLineGrid.ColumnDefinitions.Add(c3);

            //TextBlock text1 = new TextBlock();
            //text1.Text = "|";
            //TextBlock text2 = new TextBlock();
            //text2.Text = "|";
            //TextBlock text3 = new TextBlock();
            //text3.Text = "|";

            //Grid.SetRow(text1, 0);
            //Grid.SetColumn(text1, 0);
            //timeLineGrid.Children.Add(text1);

            //Grid.SetRow(text2, 0);
            //Grid.SetColumn(text2, 1);
            //timeLineGrid.Children.Add(text2);

            //Grid.SetRow(text3, 0);
            //Grid.SetColumn(text3, 3);
            //timeLineGrid.Children.Add(text3);

        }

        private void exportButtons_Click(object sender, RoutedEventArgs e)
        {
            string fileName = string.Empty;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = Properties.Resources.XefExtension; // Default file extension
            dlg.Filter = Properties.Resources.EventFileDescription + " " + Properties.Resources.EventFileFilter; // Filter files by extension
            dlg.Title = "Exportar escena";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                Console.WriteLine("filePath: " + dlg.FileName);
                this.kstudio.ExportScene(dlg.FileName);
            }
        }

        private void button_EditPerson1_Click(object sender, RoutedEventArgs e)
        {
            EditPersonForm editPersonForm = new EditPersonForm(0, ref label_sujeto1);
            editPersonForm.Show();
        }

        private void button_EditPerson2_Click(object sender, RoutedEventArgs e)
        {
            EditPersonForm editPersonForm = new EditPersonForm(1, ref label_sujeto2);
            editPersonForm.Show();
        }

        private void button_EditPerson3_Click(object sender, RoutedEventArgs e)
        {
            EditPersonForm editPersonForm = new EditPersonForm(2, ref label_sujeto3);
            editPersonForm.Show();
        }

        private void button_EditPerson4_Click(object sender, RoutedEventArgs e)
        {
            EditPersonForm editPersonForm = new EditPersonForm(3, ref label_sujeto4);
            editPersonForm.Show();
        }

        private void button_EditPerson5_Click(object sender, RoutedEventArgs e)
        {
            EditPersonForm editPersonForm = new EditPersonForm(4, ref label_sujeto5);
            editPersonForm.Show();
        }

        private void button_EditPerson6_Click(object sender, RoutedEventArgs e)
        {
            EditPersonForm editPersonForm = new EditPersonForm(5, ref label_sujeto6);
            editPersonForm.Show();
        }




        private void showGraphButtons_Click(object sender, RoutedEventArgs e)
        {
            chartForm = new ChartForm();
            chartForm.Show();
            chartForm.updateCharts();
        }

        private void fondoCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.fondoCheckBox.IsChecked == false)
            {
                this.colorImageControl.Source = null;
            }
        }

        private int lastCurrentSecondForTimeLineCursor = 0;
        private void sceneSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //int col = 0;
            //Grid.SetColumn(lineCurrentTimeCursor, col);
            int currentSecond = (int)kstudio.playback.CurrentRelativeTime.TotalSeconds;

            if(lastCurrentSecondForTimeLineCursor != currentSecond)
            {
                Console.WriteLine(currentSecond);
                Grid.SetColumn(lineCurrentTimeCursor, currentSecond); // 1seg = 1col
                lastCurrentSecondForTimeLineCursor = currentSecond;
                //timeLineScroll.
            }

            
        }

        private void sceneSlider_DragStarted(object sender, RoutedEventArgs e)
        {
            //sceneSliderUserDragging = true;
            if (kstudio.playback != null)
                if (kstudio.playback.State == Microsoft.Kinect.Tools.KStudioPlaybackState.Playing)
                {
                    kstudio.playback.UserState = "SCENE_SLIDER_DRAG_MUST_RESUME";
                }
                else
                {
                    kstudio.playback.UserState = "madafaka :D";
                } 
            kstudio.PausePlaying();
        }

        private void sceneSlider_DragCompleted(object sender, RoutedEventArgs e)
        {
            //sceneSliderUserDragging = false;
            if (kstudio.playback != null)
            {
                kstudio.playback.SeekByRelativeTime(TimeSpan.FromMilliseconds(sceneSlider.Value));
                kstudio.ResumePlaying();
                Thread.Sleep(kstudio.PausedStartMillisTime);
                if ( (string)kstudio.playback.UserState != "SCENE_SLIDER_DRAG_MUST_RESUME")
                    kstudio.PausePlaying();
            }
            
            
        }

        private void RowDefinition_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Console.WriteLine("Delta: " + e.Delta);
            //Console.WriteLine("Delta: " + e.);
        }
    }
}
