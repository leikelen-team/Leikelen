using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using cl.uv.leikelen.src.Controller;
using cl.uv.leikelen.src.Data.Access.Internal;
using cl.uv.leikelen.src.InputModule;
using cl.uv.leikelen.src.ProcessingModule;
using System.Diagnostics;

namespace cl.uv.leikelen.src.View
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        /// <summary>
        /// Actual state of the player/recorder
        /// </summary>
        private PlayerState _playerState;
        /// <summary>
        /// Controller associated with the recorder
        /// </summary>
        private RecorderController _recorderController;
        /// <summary>
        /// Controller associated with the player
        /// </summary>
        private PlayerController _playerController;
        /// <summary>
        /// Controller that manage the change between the sensor (monitor/recorder) or file (player) mode
        /// </summary>
        private MediaController _mediaController;
        /// <summary>
        /// Color of a disabled button background
        /// </summary>
        private Brush _buttonBackground;
        /// <summary>
        /// Timer used when is recording to alternate the color of the button and change the actual time label
        /// </summary>
        private DispatcherTimer _recordTimer;

        public Home()
        {
            InitializeComponent();

            //Initialize properties
            _recorderController = new RecorderController();
            _playerController = new PlayerController();
            _mediaController = new MediaController();
            _playerState = PlayerState.Wait;
            _buttonBackground = Player_VolumeToggle.Background;

            //Initialize record button timer arguments
            _recordTimer = new DispatcherTimer();
            _recordTimer.Tick += _recordTimer_Tick;
            _recordTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);

            //File MenuItems
            MenuItem_File_NewScene.Click += File_NewScene_Click;
            MenuItem_File_Import.Click += MenuItem_File_Import_Click;
            MenuItem_File_Export.Click += MenuItem_File_Export_Click;
            MenuItem_File_Save.Click += MenuItem_File_Save_Click;
            MenuItem_File_Recent_More.Click += MenuItem_File_Recent_More_Click;

            //Tools MenuItems
            MenuItem_Tools_ConfigureScene.Click += Tools_ConfigureScene_Click;
            MenuItem_Tools_Preferences.Click += MenuItem_Tools_Preferences_Click;
            MenuItem_Tools_DB.Click += MenuItem_Tools_DB_Click;

            //Help MenuItems
            MenuItem_Help_AboutUs.Click += MenuItem_Help_AboutUs_Click;

            //Window related events
            this.Closed += Home_Closed;
            this.Closing += Home_Closing;

            //Player related events
            Player_PlayButton.Click += PlayPauseButton_Click;
            Player_StopButton.Click += StopButton_Click;
            Player_RecordButton.Click += RecordButton_Click;
            Player_LocationSlider.ValueChanged += LocationSlider_ValueChanged;
            Player_VolumeToggle.Checked += Player_VolumeToggle_Checked;
            Player_VolumeToggle.Unchecked += Player_VolumeToggle_Unchecked;

            //Source related events
            SourceComboBox.SelectionChanged += SourceComboBox_SelectionChanged;

            //Set initial states
            Player_LocationSlider.IsEnabled = false;

            //Actions
            SetFromNone();
            FillMenuInputModules();
            FillMenuProccessingModules();
        }

        #region Window Events
        private void Home_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            return;
        }

        private void Home_Closed(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
        #endregion

        #region MenuItems Click
        #region File MenuItems
        private void File_NewScene_Click(object sender, RoutedEventArgs e)
        {
            var configureSceneWin = new ConfigureScene();
            configureSceneWin.Show();
        }

        private void MenuItem_File_Recent_More_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuItem_File_Save_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuItem_File_Export_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuItem_File_Import_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Tools MenuItems
        private void Tools_ConfigureScene_Click(object sender, RoutedEventArgs e)
        {
            var configureSceneWin = new ConfigureScene();
            configureSceneWin.Show();
        }

        private void MenuItem_Tools_DB_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuItem_Tools_Preferences_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Help MenuItems
        private void MenuItem_Help_AboutUs_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region video viewer

        private void VideoViewer_skeletonImageArrived(object sender, System.Windows.Media.ImageSource e)
        {
            if (SkeletonLayerCheckbox.IsChecked.HasValue && SkeletonLayerCheckbox.IsChecked.Value)
            {
                Player_ImageControl_Layer1.Source = e;
            }
        }

        private void VideoViewer_colorImageArrived(object sender, System.Windows.Media.ImageSource e)
        {
            if (ColorLayerCheckbox.IsChecked.HasValue && ColorLayerCheckbox.IsChecked.Value)
            {
                Player_ImageControl_Layer2.Source = e;
            }
        }

        #endregion

        #region Player
        private async void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_playerState)
            {
                case PlayerState.Play:
                    await _playerController.Pause();
                    _playerState = PlayerState.Pause;
                    Player_PlayButton_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                    break;

                case PlayerState.Pause:
                    await _playerController.UnPause();
                    _playerState = PlayerState.Play;
                    Player_PlayButton_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                    break;

                case PlayerState.Wait:
                    await _playerController.Play();
                    _playerState = PlayerState.Play;
                    Player_PlayButton_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                    break;
            }
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if(_playerState == PlayerState.Play || _playerState == PlayerState.Pause)
            {
                await _playerController.Stop();
                _playerState = PlayerState.Wait;
                Player_PlayButton_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            }
            else if (_playerState == PlayerState.Record){
                await _recorderController.Stop();
                _playerState = PlayerState.Wait;
                _recordTimer.Stop();
                Player_ActualTimeLabel.Content = "--:--:--";
                Player_TotalTimeLabel.Content = "--:--:--";
                Player_RecordButton.Background = _buttonBackground;
                Player_RecordButton.IsEnabled = true;
                Player_StopButton.IsEnabled = false;
            }
        }

        private async void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if(_playerState == PlayerState.Wait)
            {
                await _recorderController.Record();
                Player_ActualTimeLabel.Content = "00:00:00";
                _playerState = PlayerState.Record;
                Player_RecordButton.IsEnabled = false;
                Player_StopButton.IsEnabled = true;
                _recordTimer.Start();
            }
        }

        private void LocationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void PlayerChangedLocation(object sender, EventArgs e)
        {

        }

        private void PlayerFinished(object sender, EventArgs e)
        {

        }

        private void Player_VolumeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Player_VolumeSlider.IsEnabled = true;
        }

        private void Player_VolumeToggle_Checked(object sender, RoutedEventArgs e)
        {
            Player_VolumeSlider.IsEnabled = false;
        }

        private void _recordTimer_Tick(object sender, EventArgs e)
        {
            if (_recorderController.getLocation().HasValue)
            {
                Player_ActualTimeLabel.Content = _recorderController.getLocation().Value.ToString(@"hh\:mm\:ss");
                Player_RecordButton.Background = Player_RecordButton.Background == _buttonBackground ? Brushes.Red : _buttonBackground;
            }
        }

        #endregion

        #region Source
        private void SourceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    SetFromNone();
                    break;

                case 1:
                    SetFromSensor();
                    break;

                case 2:
                    SetFromFile();
                    break;
            }
        }

        private void SetFromNone()
        {
            _mediaController.SetFromNone();
            MenuItems_Tools_Sensors.IsEnabled = false;
            MenuItems_Tools_Processing.IsEnabled = false;
            SkeletonLayerCheckbox.IsEnabled = false;
            ColorLayerCheckbox.IsEnabled = false;
            Player_LocationSlider.IsEnabled = false;
            Player_RecordButton.IsEnabled = false;
            Player_PlayButton.IsEnabled = false;
            Player_StopButton.IsEnabled = false;
        }

        private void SetFromSensor()
        {
            _mediaController.SetFromSensor();
            Player_RecordButton.IsEnabled = true;
            MenuItems_Tools_Sensors.IsEnabled = true;
            MenuItems_Tools_Processing.IsEnabled = true;
            SkeletonLayerCheckbox.IsEnabled = false;
            ColorLayerCheckbox.IsEnabled = false;
            Player_LocationSlider.IsEnabled = false;
            Player_PlayButton.IsEnabled = false;
        }

        private void SetFromFile()
        {
            if(SceneInUse.Instance != null && SceneInUse.Instance.Scene != null)
            {
                _mediaController.SetFromFile(SceneInUse.Instance.Scene.SceneId);
                MenuItems_Tools_Sensors.IsEnabled = false;
                MenuItems_Tools_Processing.IsEnabled = false;
                SkeletonLayerCheckbox.IsEnabled = true;
                ColorLayerCheckbox.IsEnabled = true;
                Player_LocationSlider.IsEnabled = true;
                Player_RecordButton.IsEnabled = false;
                Player_PlayButton.IsEnabled = true;
            }
            else
            {
                //TODO: mostrar mensaje de error
                SourceComboBox.SelectedIndex = 0;
            }
        }
        #endregion

        #region Fill Sensors and Processing modules
        private void FillMenuProccessingModules()
        {
            foreach (var process in ProcessingLoader.Instance.ProcessingModules)
            {
                MenuItem processMenuItem = new MenuItem();
                processMenuItem.Header = process.Name;
                CheckBox processCheck = new CheckBox();
                processCheck.Content = Properties.Menu.Enable;
                processCheck.Checked += (object sender, RoutedEventArgs e) =>
                {
                    process.IsEnabled = true;

                };
                processCheck.Unchecked += (object sender, RoutedEventArgs e) =>
                {
                    process.IsEnabled = false;
                };
                processMenuItem.Items.Add(processCheck);
                foreach (var window in process.Windows)
                {
                    MenuItem processWin = new MenuItem();
                    processWin.Header = window.Item1;
                    processWin.Click += (object sender, RoutedEventArgs e) => {
                        window.Item2.Show();
                    };
                    processMenuItem.Items.Add(processWin);
                }
                MenuItems_Tools_Processing.Items.Add(processMenuItem);
            }
        }

        private void FillMenuInputModules()
        {
            foreach (var input in InputLoader.Instance.InputModules)
            {
                MenuItem inputMenuItem = new MenuItem();
                inputMenuItem.Header = input.Name;
                CheckBox inputCheck = new CheckBox();
                inputCheck.Content = Properties.Menu.Enable;
                inputCheck.Checked += (object sender, RoutedEventArgs e) =>
                {
                    try
                    {
                        input.Monitor.Open();
                    }
                    catch (Exception ex)
                    {
                        inputCheck.IsChecked = false;
                    }

                };
                inputCheck.Unchecked += (object sender, RoutedEventArgs e) =>
                {
                    try
                    {
                        input.Monitor.Close();
                    }
                    catch (Exception ex)
                    {
                        inputCheck.IsChecked = true;
                    }
                };
                inputMenuItem.Items.Add(inputCheck);
                foreach (var window in input.Windows)
                {
                    MenuItem inputWin = new MenuItem();
                    inputWin.Header = window.Item1;
                    inputWin.Click += (object sender, RoutedEventArgs e) => {
                        window.Item2.Show();
                    };
                    inputMenuItem.Items.Add(inputWin);
                }
                MenuItems_Tools_Sensors.Items.Add(inputMenuItem);
            }
        }
        #endregion
    }
}
