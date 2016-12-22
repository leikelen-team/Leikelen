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

namespace cl.uv.leikelen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Linq;

    using src.view.window;
    using src.kinectmedia;
    using src.model;
    using src.helpers;
    using src.dbcontext;
    using src.controller;

    public partial class MainWindow : Window//, INotifyPropertyChanged
    {

        
        public static PostureCRUD postureCrud;

        private static MainWindow _instance;


        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            // no borres esta linea de nuevo rolo!! estupida, mi código, idiota!! xD
            PostureType.none = PostureTypeContext.db.PostureType.ToList().FirstOrDefault(p => p.PostureTypeId == 0);
            this.InitializeComponent();
            _instance = this;

            IO.EnsureDirectoriesHasBeenCreated();
            
            ImportButton.Click += IO.ImportButton_Click;
            ExportButton.Click += IO.ExportButton_Click;

            recordButton2.Click += KinectMediaFacade.Instance.Recorder.RecordButton_Click;
            stopButton2.Click += KinectMediaFacade.Instance.Recorder.StopRecordButton_Click;
            stopButton2.Click += KinectMediaFacade.Instance.Player.StopButton_Click;
            playButton2.Click += KinectMediaFacade.Instance.Player.PlayButton_Click;

            sceneSlider.ValueChanged += KinectMediaFacade.Instance.Player.LocationSlider_ValueChanged;

            SourceComboBox.SelectionChanged += Source_ComboBox_SelectionChanged;

            BackgroundEnableCheckBox.IsEnabled = false;
            SkeletonsEnableCheckBox.IsEnabled = false;
            
        }

        

        public static MainWindow Instance()
        {
            return _instance;
        }

        public void disableButtons()
        {

            //this.exportButtons.IsEnabled = false;
            //this.playButton2.IsEnabled = false;
            //this.showGraphButtons.IsEnabled = false;
        }

        public void enableButtons()
        {

            //this.exportButtons.IsEnabled = true;
            //this.playButton2.IsEnabled = true;
            //this.showGraphButtons.IsEnabled = true;
        }

        
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
        }

        #region Button Events

        private void pgsqlExport_Click(object sender, RoutedEventArgs e)
        {
            var db = PgsqlContext.CreateConnection();
            db.Database.EnsureCreated();

            //Scene psqlScene = db.Scene.FirstOrDefault(s => s.SceneId == Scene.Instance.SceneId);
            //if (psqlScene!=null)
            //{
            //    System.Windows.Forms.DialogResult dialogResult =
            //        System.Windows.Forms.MessageBox.Show(
            //            "May be this scene already exists in PostgreSQL database."
            //            +" Psql scene name: " + psqlScene.Name
            //            +" This scene name: " + Scene.Instance.Name
            //            +" Are you sure you want insert it?",
            //            "Think about it!",
            //            System.Windows.Forms.MessageBoxButtons.YesNo);
            //    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            //    {
            //        Console.WriteLine("testing1");
            //        Scene.Instance.SceneId = db.Scene.Last().SceneId + 1;
            //        Console.WriteLine("testing2");
            //    }
            //    else if (dialogResult == System.Windows.Forms.DialogResult.No)
            //    {
            //        return;
            //    }
                    
            //}
            
            //List<Scene> list = db.Scene.ToList();
            //Console.WriteLine("algo");

            db.Scene.Add(Scene.Instance);

            //list = db.Scene.ToList();
            //Console.WriteLine("algo");

            db.SaveChanges();
        }

        private void posturesAdmin_Click(object sender, RoutedEventArgs e)
        {
            postureCrud = new PostureCRUD();
            postureCrud.Show();
        }

        private void showGraphButtons_Click(object sender, RoutedEventArgs e)
        {
            Charts c = new Charts();
            c.Show();
            
        }

        private void showContinuousGraph_Click(object sender, RoutedEventArgs e)
        {
            ContinuousChart cc = new ContinuousChart();
            cc.Show();
        }

        private void FromSensorRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MediaView.SetFromSensor();
        }

        private void FromSceneRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MediaView.SetFromScene();
        }

        #endregion

        #region Other Events
        private void BackgroundEnableCheckBox_Click(object sender, RoutedEventArgs e)
        {
            KinectMediaFacade.Instance.Player.ToggleColorFrameEnable();
        }
        private void SkeletonsEnableCheckBox_Click(object sender, RoutedEventArgs e)
        {
            KinectMediaFacade.Instance.Player.ToggleBodyFrameEnable();
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
            else if (sender == timeLineContentScroll)
            {
                personLabelsScroll.ScrollToVerticalOffset(e.VerticalOffset);
                personLabelsScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
                timeLineVerticalScrollView.ScrollToVerticalOffset(e.VerticalOffset);
                timeLineVerticalScrollView.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
            else if (sender == timeLineVerticalScrollView)
            {
                personLabelsScroll.ScrollToVerticalOffset(e.VerticalOffset);
                personLabelsScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
                timeLineContentScroll.ScrollToVerticalOffset(e.VerticalOffset);
                timeLineContentScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }
        
        private void RowDefinition_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Console.WriteLine("Delta: " + e.Delta);
        }
        #endregion

        #region Pending Events
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
        #endregion

        private void Source_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmbox = sender as ComboBox;
            int indexSelected = cmbox.SelectedIndex;
            switch (indexSelected)
            {
                case 0:
                    MediaView.SetFromSensor();
                    break;
                case 1:
                    MediaView.SetFromScene();
                    break;
                default:
                    break;
            }
        }

        private void showDistances_Click(object sender, RoutedEventArgs e)
        {
            DistanceForm df = new DistanceForm();
            df.Show();
        }

        private void sceneAdmin_Click(object sender, RoutedEventArgs e)
        {
            ManageScene mScene = new ManageScene();
            mScene.Show();
        }
    }
}
