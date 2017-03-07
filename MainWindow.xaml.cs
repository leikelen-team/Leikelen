//---------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Universidad de Valparaíso">
//     Copyright (c) Universidad de Valparaíso.  All rights reserved.
// </copyright>
//
// <Description>
// This program tracks up to 6 people simultaneously.
// If a person is tracked, the associated gesture detector will determine if that person is seated or not.
// If any of the 6 positions are not in use, the corresponding gesture detector(s) will be paused
// and the 'Not Tracked' image will be displayed in the UI.
// </Description>
//----------------------------------------------------------------------------------------------------

using cl.uv.leikelen.src.Controller;
using cl.uv.leikelen.src.Data;
using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Persistence.MVSFile;
using cl.uv.leikelen.src.Helpers;
using cl.uv.leikelen.src.kinectmedia;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace cl.uv.leikelen
{

    /// <summary>
    /// This is the Initial and Main Window
    /// </summary>
    public partial class MainWindow : Window//, INotifyPropertyChanged
    {
        private static MainWindow _instance;

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            _instance = this;

            MVSFileManager.EnsureDirectoriesHasBeenCreated();
            
            ImportButton.Click += this.Import_Click;
            ExportButton.Click += this.Export_Click;

            recordButton.Click += KinectMediaFacade.Instance.Recorder.RecordButton_Click;
            stopButton.Click += KinectMediaFacade.Instance.Recorder.StopRecordButton_Click;
            stopButton.Click += KinectMediaFacade.Instance.Player.StopButton_Click;
            playButton.Click += KinectMediaFacade.Instance.Player.PlayButton_Click;

            sceneSlider.ValueChanged += KinectMediaFacade.Instance.Player.LocationSlider_ValueChanged;

            SourceComboBox.SelectionChanged += Source_ComboBox_SelectionChanged;

            BackgroundEnableCheckBox.IsEnabled = false;
            SkeletonsEnableCheckBox.IsEnabled = false;
            
        }

        #region importar/exportar
        public void Import_Click(object sender, RoutedEventArgs e)
        {
            MVSFileManager.Import();
        }

        public void Export_Click(object sender, RoutedEventArgs e)
        {
            MVSFileManager.Export();
        }
        #endregion

        public static MainWindow Instance()
        {
            return _instance;
        }
        
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
        }

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
        #endregion

        #region Pending Events edit Persons
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

        private void sceneAdmin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void InstanciateFromScene()
        {
            foreach (PersonInScene personInScene in StaticScene.Instance.PersonsInScene)
            {
                Person person = personInScene.Person;
                if (!person.HasBeenTracked) continue;
                //person.generateView();
                //person.View.repaintIntervalGroups();
                //MainWindow.Instance().timeLineContentGrid.Children.Add(person.View.postureGroupsGrid);
            }
        }

        public void InitTimeLine(TimeSpan duration)
        {

            ColumnDefinition rulerCol, contentCol;
            TextBlock text;

            TimeSpan frameTime = TimeSpan.FromSeconds(0);
            int colSpan = 10;
            for (int colCount = 0; frameTime < duration; colCount++)
            {
                rulerCol = new ColumnDefinition();
                rulerCol.Width = new GridLength(5, GridUnitType.Pixel);
                MainWindow.Instance().timeRulerGrid.ColumnDefinitions.Add(rulerCol);

                contentCol = new ColumnDefinition();
                contentCol.Width = new GridLength(5, GridUnitType.Pixel);
                MainWindow.Instance().timeLineContentGrid.ColumnDefinitions.Add(contentCol);

                if (colCount % colSpan == 0 && colCount != 0)
                {
                    text = new TextBlock();
                    text.Text = "|";
                    text.HorizontalAlignment = HorizontalAlignment.Left;
                    Grid.SetRow(text, 0);
                    Grid.SetColumn(text, colCount);
                    Grid.SetColumnSpan(text, colSpan);
                    MainWindow.Instance().timeRulerGrid.Children.Add(text);

                    text = new TextBlock();
                    text.Text = TimeUtils.TimeSpanToShortString(frameTime);
                    text.HorizontalAlignment = colCount == 0 ?
                        HorizontalAlignment.Left : HorizontalAlignment.Center;
                    Grid.SetRow(text, 1);
                    int offset = colCount == 0 ? 0 : (colSpan / 2);
                    Grid.SetColumn(text, colCount - offset);
                    Grid.SetColumnSpan(text, colSpan);
                    MainWindow.Instance().timeRulerGrid.Children.Add(text);
                }
                else
                {
                    text = new TextBlock();
                    text.Text = "·";
                    text.HorizontalAlignment = HorizontalAlignment.Left;
                    Grid.SetRow(text, 0);
                    Grid.SetColumn(text, colCount);
                    MainWindow.Instance().timeRulerGrid.Children.Add(text);
                }

                frameTime = frameTime.Add(TimeSpan.FromMilliseconds(1000.00));
            }
        }
    }
}
