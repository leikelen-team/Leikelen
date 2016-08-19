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
    using System.Linq;

    using Accord.Extensions.Imaging;
    using System.Windows.Media.Imaging;
    using System.IO;
    using Win32;    //using Win32;
    using pojos;    //using System.Windows.Forms;                    //using System.Windows.Forms;/// <summary>
    using System.Threading;
    using System.Windows.Shapes;
    using views;
    using windows;
    using core;
    using models;
    using utils;
    using db;

    public partial class MainWindow : Window//, INotifyPropertyChanged
    {
        ///// <summary> Active Kinect sensor </summary>
        //private KinectSensor kinectSensor = null;
        
        ///// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        //private Body[] bodies = null;

        ///// <summary> Reader for body frames </summary>
        ////private BodyFrameReader bodyFrameReader = null;

        ////private ColorFrameReader colorFrameReader = null;

        //private MultiSourceFrameReader _reader;
        ////private VideoFileWriter m_writer = null;
        ////private bool isRecording = false;
        
        ////private int m_width = 1280, m_height = 720;

        ///// <summary> Current status text to display </summary>
        //private string statusText = null;

        ///// <summary> KinectBodyView object which handles drawing the Kinect bodies to a View box in the UI </summary>
        //public static  KinectBodyView kinectBodyView = null;
        
        ///// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
        //private List<GestureDetector> gestureDetectorList = null;

        private _Escena escena;

        public KinectBody kinectBody { get; private set; }
        public KinectStudioHandler kstudio { get; private set; }
        public static ChartForm chartForm { get; private set; }
        public static TimeSpan lastCurrentTime = TimeSpan.FromSeconds(0);
        public static PostureCRUD postureCrud;


        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            // no borres esta linea de nuevo rolo!! estupida, mi código, idiota!! xD
            PostureType.none = PostureType.availablesPostureTypes.FirstOrDefault(p => p.PostureTypeId == 0);
            this.InitializeComponent();
            kinectBody = KinectBody.Instance;
            
            this.escena = new _Escena();

            this.kstudio = new KinectStudioHandler();
            this.exportButtons.IsEnabled = false;
            this.playButton.IsEnabled = false;
            this.showGraphButtons.IsEnabled = false;
            this.stopButton.IsEnabled = false;
            
            foreach (PostureType postureType in PostureType.availablesPostureTypes)
            {
                Console.WriteLine(postureType.Name + ": "+postureType.Path);// + postureType.color.ToString());
            }


        }

        public static MainWindow Instance()
        {
            return (MainWindow)System.Windows.Application.Current.MainWindow;
        }

        public void disableButtons()
        {
            //this.button_EditPerson1.IsEnabled = false;
            //this.button_EditPerson2.IsEnabled = false;
            //this.button_EditPerson3.IsEnabled = false;
            //this.button_EditPerson4.IsEnabled = false;
            //this.button_EditPerson5.IsEnabled = false;
            //this.button_EditPerson6.IsEnabled = false;
            this.exportButtons.IsEnabled = false;
            this.playButton.IsEnabled = false;
            this.showGraphButtons.IsEnabled = false;
        }

        public void enableButtons()
        {
            //this.button_EditPerson1.IsEnabled = true;
            //this.button_EditPerson2.IsEnabled = true;
            //this.button_EditPerson3.IsEnabled = true;
            //this.button_EditPerson4.IsEnabled = true;
            //this.button_EditPerson5.IsEnabled = true;
            //this.button_EditPerson6.IsEnabled = true;
            this.exportButtons.IsEnabled = true;
            this.playButton.IsEnabled = true;
            this.showGraphButtons.IsEnabled = true;
        }

        

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            KinectBody.Instance.Close();
            
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

        private void simulateButtons_Click(object sender, RoutedEventArgs e)
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
                this.kstudio.RecSimulate(dlg.FileName);
                enableButtons();
                this.Title = "Visualizador Multimodal - "+Scene.Instance.name;
                sceneTitleLabel.Content = Scene.Instance.name;
                sceneDurationLabel.Content = Scene.Instance.duration.ToString(@"hh\:mm\:ss");
            }

        }
        private void realImportButtons_Click(object sender, RoutedEventArgs e)
        {
            var db = BackupDataContext.CreateConnection(@"tmp\scene_data.db");
            //db.Database.EnsureCreated();
            //db.Scene.Add(Scene.Instance);
            //db.SaveChanges();
            Scene.CreateFromDbContext();

            if (Scene.Instance == null) return;
            foreach (Person person in Scene.Instance.Persons)
            {
                if (!person.HasBeenTracked) continue;
                //person.generatePostureIntervals();
                //StackPanel combosStackPanel = person.View.ComboStackPanel;
                //person.PosturesView = new PosturesPersonView(person);
                person.generateView();
                
                person.View.repaintIntervalGroups();
                timeLineContentGrid.Children.Add(person.View.postureGroupsGrid);
                //person.View.repaintIntervalGroups();
            }

            kstudio.Import(@"tmp\Escena_corta.xef");
            enableButtons();

        }

        private void exportButtons_Click(object sender, RoutedEventArgs e)
        {
            //string fileName = string.Empty;

            //SaveFileDialog dlg = new SaveFileDialog();
            //dlg.FileName = "";
            //dlg.DefaultExt = Properties.Resources.XefExtension; // Default file extension
            //dlg.Filter = Properties.Resources.EventFileDescription + " " + Properties.Resources.EventFileFilter; // Filter files by extension
            //dlg.Title = "Exportar escena";
            //bool? result = dlg.ShowDialog();

            //if (result == true)
            //{
            //    Console.WriteLine("filePath: " + dlg.FileName);
            //    this.kstudio.ExportScene(dlg.FileName);
            //    var db = DataAnalysisContext.CreateConnection(dlg.FileName);
            //    db.Database.EnsureCreated();
            //    db.Scene.Add(Scene.Instance);
            //    db.SaveChanges();
            //}

            //this.kstudio.ExportScene(@"tmp\scene_data.xef");
            var db = BackupDataContext.CreateConnection(@"tmp\scene_data.db");
            db.Database.EnsureCreated();
            
            db.Scene.Add(Scene.Instance);
            db.SaveChanges();


        }

        //private void button_EditPerson1_Click(object sender, RoutedEventArgs e)
        //{
        //    EditPersonForm editPersonForm = new EditPersonForm(0, ref label_sujeto1);
        //    editPersonForm.Show();
        //}

        //private void button_EditPerson2_Click(object sender, RoutedEventArgs e)
        //{
        //    EditPersonForm editPersonForm = new EditPersonForm(1, ref label_sujeto2);
        //    editPersonForm.Show();
        //}

        //private void button_EditPerson3_Click(object sender, RoutedEventArgs e)
        //{
        //    EditPersonForm editPersonForm = new EditPersonForm(2, ref label_sujeto3);
        //    editPersonForm.Show();
        //}

        //private void button_EditPerson4_Click(object sender, RoutedEventArgs e)
        //{
        //    EditPersonForm editPersonForm = new EditPersonForm(3, ref label_sujeto4);
        //    editPersonForm.Show();
        //}

        //private void button_EditPerson5_Click(object sender, RoutedEventArgs e)
        //{
        //    EditPersonForm editPersonForm = new EditPersonForm(4, ref label_sujeto5);
        //    editPersonForm.Show();
        //}

        //private void button_EditPerson6_Click(object sender, RoutedEventArgs e)
        //{
        //    EditPersonForm editPersonForm = new EditPersonForm(5, ref label_sujeto6);
        //    editPersonForm.Show();
        //}




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
                Grid.SetColumn(lineCurrentTimeRulerCursor, currentSecond); // 1seg = 1col
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

        //public StackPanel getCombosStackPanelByPerson(Person person)
        //{
        //    switch (person.BodyIndex)
        //    {
        //        case 0: return postureIntervalGroupNamesComboBox_person0;
        //        case 1: return postureIntervalGroupNamesComboBox_person1;
        //        case 2: return postureIntervalGroupNamesComboBox_person2;
        //        case 3: return postureIntervalGroupNamesComboBox_person3;
        //        case 4: return postureIntervalGroupNamesComboBox_person4;
        //        case 5: return postureIntervalGroupNamesComboBox_person5;
        //        default: return null;
        //    }
        //}

        private void analizePostures_Click(object sender, RoutedEventArgs e)
        {
            if (Scene.Instance == null) return;
            foreach (Person person in Scene.Instance.Persons)
            {
                if (!person.HasBeenTracked) continue;
                person.generatePostureIntervals();
                //StackPanel combosStackPanel = person.View.ComboStackPanel;
                //person.PosturesView = new PosturesPersonView(person);
                
                person.View.repaintIntervalGroups();
                timeLineContentGrid.Children.Add(person.View.postureGroupsGrid);
                
            }
        }


        public void TimeLineVerticalScrollsChange(object sender, ScrollChangedEventArgs e)
        {

            if (sender == personLabelsScroll)
            {
                timeLineContentScroll.ScrollToVerticalOffset(e.VerticalOffset);
                timeLineContentScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
                timeLineVerticalScrollView.ScrollToVerticalOffset(e.VerticalOffset);
                timeLineVerticalScrollView.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
            else if(sender == timeLineContentScroll)
            {
                personLabelsScroll.ScrollToVerticalOffset(e.VerticalOffset);
                personLabelsScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
                timeLineVerticalScrollView.ScrollToVerticalOffset(e.VerticalOffset);
                timeLineVerticalScrollView.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
            else if(sender == timeLineVerticalScrollView)
            {
                personLabelsScroll.ScrollToVerticalOffset(e.VerticalOffset);
                personLabelsScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
                timeLineContentScroll.ScrollToVerticalOffset(e.VerticalOffset);
                timeLineContentScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }

        private void posturesAdmin_Click(object sender, RoutedEventArgs e)
        {
            postureCrud = new PostureCRUD();
            postureCrud.Show();
        }

        
    }
}
