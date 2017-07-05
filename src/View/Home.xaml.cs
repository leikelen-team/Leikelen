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

namespace cl.uv.leikelen.src.View
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private PlayerState _playerState;
        private RecorderController _recorderController;
        private PlayerController _playerController;
        private MediaController _mediaController;

        private Brush _buttonBackground;
        private DispatcherTimer _recordTimer;

        public Home()
        {
            InitializeComponent();

            _recorderController = new RecorderController();
            _playerController = new PlayerController();
            _mediaController = new MediaController();

            MenuItem_File_NewScene.Click += File_NewScene_Click;
            MenuItem_Tools_ConfigureScene.Click += Tools_ConfigureScene_Click;

            Player_PlayButton.Click += PlayPauseButton_Click;
            Player_StopButton.Click += StopButton_Click;
            Player_RecordButton.Click += RecordButton_Click;
            Player_LocationSlider.ValueChanged += LocationSlider_ValueChanged;

            SourceComboBox.SelectionChanged += SourceComboBox_SelectionChanged;

            Player_LocationSlider.IsEnabled = false;
            _buttonBackground = Player_VolumeToggle.Background;
            _recordTimer = new DispatcherTimer();
            _recordTimer.Tick += _recordTimer_Tick;
            _recordTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            SetFromNone();

            _playerState = PlayerState.Wait;
        }

        private void _recordTimer_Tick(object sender, EventArgs e)
        {
            if (_recorderController.getLocation().HasValue)
            {
                Player_ActualTimeLabel.Content = _recorderController.getLocation().Value.ToString(@"hh\:mm\:ss");
                Player_RecordButton.Background = Player_RecordButton.Background == _buttonBackground ? Brushes.Red : _buttonBackground;
            }
        }

        #region MenuItems Click
        private void File_NewScene_Click(object sender, RoutedEventArgs e)
        {
            var configureSceneWin = new ConfigureScene();
            configureSceneWin.Show();
        }

        private void Tools_ConfigureScene_Click(object sender, RoutedEventArgs e)
        {
            var configureSceneWin = new ConfigureScene();
            configureSceneWin.Show();
        }
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
            SkeletonLayerCheckbox.IsEnabled = false;
            ColorLayerCheckbox.IsEnabled = false;
            Player_LocationSlider.IsEnabled = false;
            Player_RecordButton.IsEnabled = true;
            Player_PlayButton.IsEnabled = false;
        }

        private void SetFromFile()
        {
            if(SceneInUse.Instance != null && SceneInUse.Instance.Scene != null)
            {
                _mediaController.SetFromFile(SceneInUse.Instance.Scene.SceneId);
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
    }
}
