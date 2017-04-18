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
using cl.uv.leikelen.src.API;
using cl.uv.leikelen.src.Data;
using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Persistence.MVSFile;
using cl.uv.leikelen.src.Helper;
using cl.uv.leikelen.src.kinectmedia;
using cl.uv.leikelen.src.View.Classes;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using cl.uv.leikelen.src.Module;
using System.Timers;

namespace cl.uv.leikelen
{

    /// <summary>
    /// This is the Initial and Main Window
    /// </summary>
    public partial class MainWindow : Window//, INotifyPropertyChanged
    {
        private static MainWindow _instance;

        private PlayerState _playerState;

        private IPlayer _player;
        private IRecorder _recorder;

        private Timer _timeLabelTimer;

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            _instance = this;
            _playerState = PlayerState.Waiting;
            
            FileSystemUtils.EnsureDirectoriesHasBeenCreated();
            
            ImportButton.Click += this.Import_Click;
            ExportButton.Click += this.Export_Click;

            _player = KinectMediaFacade.Instance.Player;
            _recorder = KinectMediaFacade.Instance.Recorder;

            _player.Finished += PlayerFinished;
            _player.LocationChanged += PlayerChangedLocation;

            recordButton.Click += this.RecordButton_Click;
            stopButton.Click += this.StopButton_Click;
            playButton.Click += this.PlayPauseButton_Click;

            sceneSlider.ValueChanged += this.LocationSlider_ValueChanged;

            SourceComboBox.SelectionChanged += Source_ComboBox_SelectionChanged;

            BackgroundEnableCheckBox.IsEnabled = false;
            SkeletonsEnableCheckBox.IsEnabled = false;

            
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if(_playerState == PlayerState.Playing)
            {
                _player.Pause();
                _playerState = PlayerState.Paused;
                this.playButton.Content = Properties.Buttons.PausePlaying;
            }
            if(_playerState == PlayerState.Paused)
            {
                _player.Unpause();
                _playerState = PlayerState.Playing;
                this.playButton.Content = Properties.Buttons.StartPlaying;
            }
            if(_playerState == PlayerState.Waiting)
            {
                _player.Play();
                _playerState = PlayerState.Playing;
                this.playButton.Content = Properties.Buttons.PausePlaying;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_playerState == PlayerState.Playing)
            {
                _player.Stop();
                _playerState = PlayerState.Waiting;
                this.playButton.Content = Properties.Buttons.StartPlaying;
            }
            if(_playerState == PlayerState.Recording)
            {
                _recorder.Stop();
                //_timeLabelTimer.Stop();
                _playerState = PlayerState.Waiting;
                foreach (var module in TMPLoader.Modules)
                {
                    if (module.FunctionAfterStop() != null)
                    {
                        module.FunctionAfterStop();
                    }
                }
                
                foreach (var personInScene in StaticScene.Instance.PersonsInScene)
                {
                    Person person = personInScene.Person;
                    if (!StaticScene.personsView.ContainsKey(person))
                    {
                        StaticScene.personsView[person] = new PersonView(personInScene, (int)(StaticScene.Instance.Duration.TotalSeconds));
                    }
                    StaticScene.personsView[person].repaintIntervalGroups();
                    this.timeLineContentGrid.Children.Add(StaticScene.personsView[person].postureGroupsGrid);
                }

                this.SourceComboBox.SelectedIndex = 1;
                this.recordButton.Background = System.Windows.Media.Brushes.White;

            }
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (_playerState == PlayerState.Waiting)
            {
                if (StaticScene.Instance != null)
                {
                    System.Windows.Forms.DialogResult dialogResult =
                    System.Windows.Forms.MessageBox.Show(
                        "Are you sure to record scene?",
                        "Did you save the current scene data?",
                        System.Windows.Forms.MessageBoxButtons.YesNo);
                    if (dialogResult == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }
                _recorder.Record();
                //_timeLabelTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
                //_timeLabelTimer.Elapsed += _timeLabelTimer_Elapsed;
                //_timeLabelTimer.Start();
                _playerState = PlayerState.Recording;
                this.SourceComboBox.SelectedIndex = 0;
                this.recordButton.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void _timeLabelTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("timeeer");
            if (_recorder.getLocation().HasValue)
                this.sceneCurrentTimeLabel.Content = _recorder.getLocation().Value.ToString(@"hh\:mm\:ss");
        }

        public void LocationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_playerState == PlayerState.Playing || _playerState == PlayerState.Paused)
            {
                
                if (_player.getLocation().HasValue && _player.getTotalDuration().HasValue)
                {
                    _player.ChangeTime(TimeSpan.FromMilliseconds((e.NewValue / 100.0) * _player.getTotalDuration().Value.TotalMilliseconds));
                    this.sceneCurrentTimeLabel.Content = _player.getLocation().Value.ToString(@"hh\:mm\:ss");
                }

                int currentSecond = (int)_player.getLocation().Value.TotalSeconds;
                if (TimeSpan.FromMilliseconds(e.NewValue).TotalSeconds != currentSecond)
                {
                    Grid.SetColumn(this.lineCurrentTimeCursor, currentSecond); // 1seg = 1col
                    Grid.SetColumn(this.lineCurrentTimeRulerCursor, currentSecond); // 1seg = 1col
                }

            }
        }

        public void PlayerFinished(object sender, EventArgs e)
        {
            this.playButton.Content = Properties.Buttons.StartPlaying;
        }

        public void PlayerChangedLocation(object sender, EventArgs e)
        {
            if(_player.getTotalDuration().HasValue && _player.getLocation().HasValue)
            {
                this.sceneSlider.Value = 100 - (100 * ((_player.getTotalDuration().Value.TotalMilliseconds - _player.getLocation().Value.TotalMilliseconds) / _player.getTotalDuration().Value.TotalMilliseconds));

            }
        }

        public static MainWindow Instance()
        {
            return _instance;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
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

        #region CheckBox Events
        private void BackgroundEnableCheckBox_Click(object sender, RoutedEventArgs e)
        {
            KinectMediaFacade.Instance.Player.ToggleColorFrameEnable();
        }
        private void SkeletonsEnableCheckBox_Click(object sender, RoutedEventArgs e)
        {
            KinectMediaFacade.Instance.Player.ToggleBodyFrameEnable();
        }
        #endregion

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
    }
}
