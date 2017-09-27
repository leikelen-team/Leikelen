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
using cl.uv.leikelen.Controller;
using cl.uv.leikelen.Module;
using System.Diagnostics;
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.Data.Access;
using MaterialDesignThemes.Wpf;

using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.View
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
        private readonly RecorderController _recorderController;
        /// <summary>
        /// Controller associated with the player
        /// </summary>
        private readonly PlayerController _playerController;
        /// <summary>
        /// Controller that manage the change between the sensor (monitor/recorder) or file (player) mode
        /// </summary>
        private readonly MediaController _mediaController;
        /// <summary>
        /// Color of a disabled button background
        /// </summary>
        private readonly Brush _buttonBackground;
        /// <summary>
        /// Timer used when is recording to alternate the color of the button and change the actual time label
        /// </summary>
        private readonly DispatcherTimer _recordTimer;
        /// <summary>
        /// Get or Sets the state of the window
        /// </summary>
        private HomeState _homeState;

        private Tuple<TimeSpan?, ImageSource> _lastColorBeforePause;
        private Tuple<TimeSpan?, ImageSource> _lastSkeletonBeforePause;

        private List<ITab> _tabs;

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
            _recordTimer.Interval = new TimeSpan(0, 0, 0, 0, 500); //0.5 seconds

            //File MenuItems
            MenuItem_File_NewScene.Click += File_NewScene_Click;
            MenuItem_File_LoadTestScene.Click += MenuItem_File_LoadTestScene_Click;
            MenuItem_File_Import.Click += MenuItem_File_Import_Click;
            MenuItem_File_Export.Click += MenuItem_File_Export_Click;
            MenuItem_File_Save.Click += MenuItem_File_Save_Click;
            MenuItem_File_Recent_More.Click += MenuItem_File_Recent_More_Click;
            MenuItem_File_Quit.Click += MenuItem_File_Quit_Click;

            //Tools MenuItems
            MenuItem_Tools_Preferences.Click += MenuItem_Tools_Preferences_Click;
            MenuItem_Tools_DB.Click += MenuItem_Tools_DB_Click;

            //Scene MenuItems
            MenuItem_Scene_Configure.Click += MenuItem_Scene_Configure_Click;
            MenuItem_Scene_AddPerson.Click += MenuItem_Scene_AddPerson_Click;
            MenuItem_Scene_Persons.Click += MenuItem_Scene_Persons_Click;

            //Help MenuItems
            MenuItem_Help_DevDoc.Click += MenuItem_Help_DevDoc_Click;
            MenuItem_Help_AboutUs.Click += MenuItem_Help_AboutUs_Click;

            //Window related events
            Closed += Home_Closed;
            Closing += Home_Closing;

            //Player related events
            Player_PlayButton.Click += PlayPauseButton_Click;
            Player_StopButton.Click += StopButton_Click;
            Player_RecordButton.Click += RecordButton_Click;
            Player_LocationSlider.ValueChanged += LocationSlider_ValueChanged;
            Player_VolumeToggle.Checked += Player_VolumeToggle_Checked;
            Player_VolumeToggle.Unchecked += Player_VolumeToggle_Unchecked;

            //Source related events
            SourceComboBox.SelectionChanged += SourceComboBox_SelectionChanged;

            InputLoader.Instance.VideoHandler.ColorImageArrived += VideoViewer_colorImageArrived;
            InputLoader.Instance.VideoHandler.SkeletonImageArrived += VideoViewer_skeletonImageArrived;

            
            _tabs = new List<ITab>()
            {
                new Widget.HomeTab.TabInterval(),
                new Widget.HomeTab.TabGraph(),
                new Widget.HomeTab.TabDistance(),
                new Widget.HomeTab.TabScene()
            };

            foreach(var tab in _tabs)
            {
                if(tab is TabItem)
                    Tabs.AddToSource(tab as TabItem);
            }

            SkeletonLayerCheckbox.Checked += SkeletonLayerCheckbox_Checked;
            SkeletonLayerCheckbox.Unchecked += SkeletonLayerCheckbox_Unchecked;

            ColorLayerCheckbox.Checked += ColorLayerCheckbox_Checked;
            ColorLayerCheckbox.Unchecked += ColorLayerCheckbox_Unchecked;

            SkeletonLayerCheckbox.IsChecked = true;
            ColorLayerCheckbox.IsChecked = true;

            //Actions
            ChangeHomeState(HomeState.Initial, PlayerState.Wait);
            FillMenuInputModules();
            FillMenuProccessingModules();
            FillMenuGeneralModules();
            FillRecentScenes();
        }

        private void FillRecentScenes()
        {
            /*var scenes = DataAccessFacade.Instance.GetSceneAccess().GetAll();
            List<MenuItem> recentScenes = new List<MenuItem>();
            recentScenes.Concat(MenuItem_File_Recent.Items);

            for (int i= scenes.Count-1; i>=0 && i>scenes.Count-5; i--)
            {
                MenuItem recentScene = new MenuItem()
                {
                    Header = scenes[i].Name
                    
                };
                recentScene.Click += (sender, e) =>
                {

                };
                MenuItem_File_Recent.Items.Insert();
            }*/
        }

        

        private void MenuItem_File_LoadTestScene_Click(object sender, RoutedEventArgs e)
        {
            LoadTestScene.LoadTest();
            foreach (var tab in _tabs)
            {
                tab.Fill();
            }
            ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Wait);
        }

        #region states
        private bool ChangeHomeState(HomeState newHomeState, PlayerState newPlayerState)
        {
            switch (newHomeState)
            {
                case HomeState.Initial:
                    if (_playerState == PlayerState.Record)
                    {
                        MessageBoxResult result = MessageBox.Show(Properties.GUI.stopRecordFirst, 
                            Properties.GUI.Atention, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }
                    SceneInUse.Instance.Set(null);
                    _mediaController.SetFromNone();
                    SetGuiInitial();
                    break;
                case HomeState.FromSensor:
                    if (_homeState == HomeState.FromSensor || _homeState == HomeState.FromSensorWithScene)
                    {
                        return false;
                    }
                    _mediaController.SetFromSensor();
                    SetGuiFromScene();
                    break;
                case HomeState.FromSensorWithScene:
                    if (DataAccessFacade.Instance.GetSceneInUseAccess().GetScene() == null)
                        return false;
                    SetGuiFromSensorWithScene(newPlayerState);
                    break;
                case HomeState.FromFile:
                    if (_playerState == PlayerState.Record)
                    {
                        MessageBoxResult result = MessageBox.Show(Properties.GUI.stopRecordFirst, 
                            Properties.GUI.Atention, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }
                    SceneInUse.Instance.Set(null);
                    SetGuiFromFile();
                    break;
                case HomeState.FromFileWithScene:
                    if (DataAccessFacade.Instance.GetSceneInUseAccess().GetScene() == null)
                        return false;
                    foreach(var tab in _tabs)
                    {
                        tab.Fill();
                    }
                    SetGuiFromFileWithScene(newPlayerState);
                    break;
            }

            _homeState = newHomeState;
            if (_homeState == HomeState.Initial)
                _playerState = PlayerState.Wait;
            else
                _playerState = newPlayerState;
            return true;
        }

        private void SetGuiFromFile()
        {
            MenuItem_File_NewScene.IsEnabled = false;
            MenuItem_File_Save.IsEnabled = false;
            MenuItem_File_Recent.IsEnabled = true;
            MenuItem_File_Recent_More.IsEnabled = true;
            MenuItem_File_Import.IsEnabled = true;
            MenuItem_File_Export.IsEnabled = false;
            MenuItem_File_Quit.IsEnabled = true;

            MenuItem_Tools_Preferences.IsEnabled = true;
            MenuItem_Tools_DB.IsEnabled = true;
            MenuItem_Tools_Player.IsEnabled = false;
            MenuItems_Tools_Sensors.IsEnabled = false;
            MenuItems_Tools_Processing.IsEnabled = false;

            MenuItem_Scene.IsEnabled = false;
            MenuItem_Scene_Configure.IsEnabled = false;
            MenuItem_Scene_AddPerson.IsEnabled = false;

            Player_LocationSlider.IsEnabled = false;
            Player_RecordButton.IsEnabled = false;
            Player_PlayButton.IsEnabled = false;
            Player_StopButton.IsEnabled = false;
            Player_VolumeToggle.IsEnabled = false;
            Player_VolumeSlider.IsEnabled = false;
        }

        private void SetGuiInitial()
        {
            MenuItem_File_NewScene.IsEnabled = false;
            MenuItem_File_Save.IsEnabled = false;
            MenuItem_File_Recent.IsEnabled = false;
            MenuItem_File_Recent_More.IsEnabled = false;
            MenuItem_File_Import.IsEnabled = false;
            MenuItem_File_Export.IsEnabled = false;
            MenuItem_File_Quit.IsEnabled = true;

            MenuItem_Tools_Preferences.IsEnabled = true;
            MenuItem_Tools_DB.IsEnabled = true;
            MenuItem_Tools_Player.IsEnabled = false;
            MenuItems_Tools_Sensors.IsEnabled = false;
            MenuItems_Tools_Processing.IsEnabled = false;

            MenuItem_Scene.IsEnabled = false;
            MenuItem_Scene_Configure.IsEnabled = false;
            MenuItem_Scene_AddPerson.IsEnabled = false;

            Player_LocationSlider.IsEnabled = false;
            Player_RecordButton.IsEnabled = false;
            Player_PlayButton.IsEnabled = false;
            Player_StopButton.IsEnabled = false;
            Player_VolumeToggle.IsEnabled = false;
            Player_VolumeSlider.IsEnabled = false;
        }

        private void SetGuiFromScene()
        {
            MenuItem_File_NewScene.IsEnabled = true;
            MenuItem_File_Save.IsEnabled = false;
            MenuItem_File_Recent.IsEnabled = false;
            MenuItem_File_Recent_More.IsEnabled = false;
            MenuItem_File_Import.IsEnabled = false;
            MenuItem_File_Export.IsEnabled = false;
            MenuItem_File_Quit.IsEnabled = true;

            MenuItem_Tools_Preferences.IsEnabled = true;
            MenuItem_Tools_DB.IsEnabled = true;
            MenuItem_Tools_Player.IsEnabled = true;
            MenuItems_Tools_Sensors.IsEnabled = true;
            MenuItems_Tools_Processing.IsEnabled = true;

            MenuItem_Scene.IsEnabled = false;
            MenuItem_Scene_Configure.IsEnabled = false;
            MenuItem_Scene_AddPerson.IsEnabled = false;

            Player_LocationSlider.IsEnabled = false;
            Player_RecordButton.IsEnabled = false;
            Player_PlayButton.IsEnabled = false;
            Player_StopButton.IsEnabled = false;
            Player_VolumeToggle.IsEnabled = false;
            Player_VolumeSlider.IsEnabled = false;
        }

        private void SetGuiFromFileWithScene(PlayerState playerState)
        {
            switch (playerState)
            {
                case PlayerState.Wait:
                    Player_StopButton.IsEnabled = false;
                    break;
                case PlayerState.Play:
                    Player_StopButton.IsEnabled = true;
                    break;
                case PlayerState.Pause:
                    Player_StopButton.IsEnabled = true;
                    break;
            }
            MenuItem_File_NewScene.IsEnabled = false;
            MenuItem_File_Save.IsEnabled = true;
            MenuItem_File_Recent.IsEnabled = true;
            MenuItem_File_Recent_More.IsEnabled = true;
            MenuItem_File_Import.IsEnabled = true;
            MenuItem_File_Export.IsEnabled = true;
            MenuItem_File_Quit.IsEnabled = true;

            MenuItem_Tools_Preferences.IsEnabled = true;
            MenuItem_Tools_DB.IsEnabled = true;
            MenuItem_Tools_Player.IsEnabled = true;
            MenuItems_Tools_Sensors.IsEnabled = false;
            MenuItems_Tools_Processing.IsEnabled = false;

            MenuItem_Scene.IsEnabled = true;
            MenuItem_Scene_Configure.IsEnabled = true;
            MenuItem_Scene_AddPerson.IsEnabled = false;

            Player_LocationSlider.IsEnabled = true;
            Player_RecordButton.IsEnabled = false;
            Player_PlayButton.IsEnabled = true;
            Player_VolumeToggle.IsEnabled = true;
            Player_VolumeSlider.IsEnabled = true;
        }

        private void SetGuiFromSensorWithScene(PlayerState playerState)
        {
            switch (playerState)
            {
                case PlayerState.Wait:
                    MenuItem_File_NewScene.IsEnabled = true;
                    MenuItem_File_Save.IsEnabled = true;

                    MenuItem_Tools_DB.IsEnabled = true;
                    MenuItems_Tools_Sensors.IsEnabled = true;
                    MenuItems_Tools_Processing.IsEnabled = true;

                    Player_RecordButton.IsEnabled = true;
                    Player_StopButton.IsEnabled = false;
                    Player_VolumeToggle.IsEnabled = false;
                    Player_VolumeSlider.IsEnabled = false;
                    break;
                case PlayerState.Record:
                    MenuItem_File_NewScene.IsEnabled = false;
                    MenuItem_File_Save.IsEnabled = false;

                    MenuItem_Tools_DB.IsEnabled = false;
                    MenuItems_Tools_Sensors.IsEnabled = false;
                    MenuItems_Tools_Processing.IsEnabled = false;

                    Player_RecordButton.IsEnabled = false;
                    Player_StopButton.IsEnabled = true;
                    Player_VolumeToggle.IsEnabled = true;
                    Player_VolumeSlider.IsEnabled = true;
                    break;
            }
            MenuItem_File_Recent.IsEnabled = false;
            MenuItem_File_Recent_More.IsEnabled = false;
            MenuItem_File_Import.IsEnabled = false;
            MenuItem_File_Export.IsEnabled = true;
            MenuItem_File_Quit.IsEnabled = true;

            MenuItem_Tools_Preferences.IsEnabled = true;
            MenuItem_Tools_Player.IsEnabled = true;


            MenuItem_Scene.IsEnabled = true;
            MenuItem_Scene_Configure.IsEnabled = true;
            MenuItem_Scene_AddPerson.IsEnabled = true;

            Player_LocationSlider.IsEnabled = false;

            Player_PlayButton.IsEnabled = false;
        }

        #endregion states

        #region Window Events

        private void Home_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.GUI.AreSureExit, 
                Properties.GUI.Confirmation, MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void Home_Closed(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
        #endregion

        #region File MenuItems
        private void File_NewScene_Click(object sender, RoutedEventArgs e)
        {
            var configureSceneWin = new ConfigureScene();
            configureSceneWin.Show();
            configureSceneWin.Closed += (senderClosed, eClosed) =>
            {
                if (!Object.ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
                {
                    ChangeHomeState(HomeState.FromSensorWithScene, PlayerState.Wait);
                }
            };
        }

        private void MenuItem_File_Recent_More_Click(object sender, RoutedEventArgs e)
        {
            var allScenesWin = new AllScenes();
            allScenesWin.Show();
            allScenesWin.Closed += (senderAllScenes, eAllScenes) =>
            {
                if (!Object.ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
                {
                    ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Wait);
                }
            };
        }

        private void MenuItem_File_Save_Click(object sender, RoutedEventArgs e)
        {
            if (!Object.ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
            {
                try
                {
                    DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());
                    MessageBox.Show(Properties.GUI.SavedOk,
                    Properties.GUI.SavedOkTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(Properties.GUI.SavedError+"\n"+ex.Message,
                    Properties.GUI.SavedErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(Properties.GUI.SceneNotExist, 
                    Properties.GUI.SceneNotExistTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_File_Export_Click(object sender, RoutedEventArgs e)
        {
            var exportWin = new Export();
            exportWin.Show();
        }

        private void MenuItem_File_Import_Click(object sender, RoutedEventArgs e)
        {
            //ChangeHomeState(HomeState.FromFileWithScene);
            throw new NotImplementedException();
        }

        private void MenuItem_File_Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Tools MenuItems
        private void MenuItem_Tools_DB_Click(object sender, RoutedEventArgs e)
        {
            var preferencesWin = new Preferences();
            preferencesWin.ShowBd();
        }

        private void MenuItem_Tools_Preferences_Click(object sender, RoutedEventArgs e)
        {
            var preferencesWin = new Preferences();
            preferencesWin.Show();
        }
        #endregion

        #region Scene MenuItems
        private void MenuItem_Scene_Configure_Click(object sender, RoutedEventArgs e)
        {
            if (!Object.ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
            {
                var configureSceneWin = new ConfigureScene(DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());
                configureSceneWin.Show();
            }
        }

        private void MenuItem_Scene_AddPerson_Click(object sender, RoutedEventArgs e)
        {
            if (!Object.ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
            {
                var addPersonWin = new ConfigurePerson();
                addPersonWin.Show();
            }
        }
        
        private void MenuItem_Scene_Persons_Click(object sender, RoutedEventArgs e)
        {
            var allPersonsWin = new AllPersons();
            allPersonsWin.Show();
        }
        #endregion

        #region Help MenuItems
        private void MenuItem_Help_AboutUs_Click(object sender, RoutedEventArgs e)
        {
            var aboutWin = new AboutUs();
            aboutWin.Show();
        }


        private void MenuItem_Help_DevDoc_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"documentation\index.html");
        }
        #endregion

        #region Video viewer

        private void VideoViewer_skeletonImageArrived(object sender, ImageSource e)
        {
            Console.WriteLine("----skeleton arrived");
            Player_ImageControl_Layer1.Source = e;
        }

        private void VideoViewer_colorImageArrived(object sender, ImageSource e)
        {
            Console.WriteLine("color arrived");
            Player_ImageControl_Layer2.Source = e;
        }

        private void SkeletonLayerCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            InputLoader.Instance.VideoHandler.DisableSkeleton();
            _lastSkeletonBeforePause = new Tuple<TimeSpan?, ImageSource>(
                    DataAccessFacade.Instance.GetSceneInUseAccess().GetLocation(),
                    Player_ImageControl_Layer1.Source);
            Player_ImageControl_Layer1.Source = null;
        }

        private void SkeletonLayerCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            InputLoader.Instance.VideoHandler.EnableSkeleton();
            if (DataAccessFacade.Instance.GetSceneInUseAccess().GetLocation().HasValue && 
                _lastSkeletonBeforePause?.Item1?.CompareTo(DataAccessFacade.Instance.GetSceneInUseAccess().GetLocation().Value) == 0)
            {
                Player_ImageControl_Layer1.Source = _lastSkeletonBeforePause.Item2;
            }
        }

        private void ColorLayerCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            InputLoader.Instance.VideoHandler.DisableColor();
            _lastColorBeforePause = new Tuple<TimeSpan?, ImageSource>(
                    DataAccessFacade.Instance.GetSceneInUseAccess().GetLocation(), 
                    Player_ImageControl_Layer2.Source);
            Player_ImageControl_Layer2.Source = null;
        }

        private void ColorLayerCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            InputLoader.Instance.VideoHandler.EnableColor();
            if(DataAccessFacade.Instance.GetSceneInUseAccess().GetLocation().HasValue && 
                _lastColorBeforePause?.Item1?.CompareTo(DataAccessFacade.Instance.GetSceneInUseAccess().GetLocation().Value) == 0)
            {
                Player_ImageControl_Layer2.Source = _lastColorBeforePause.Item2;
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
                    ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Pause);
                    Player_PlayButton_Icon.Kind = PackIconKind.Play;
                    break;

                case PlayerState.Pause:
                    await _playerController.UnPause();
                    ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Play);
                    Player_PlayButton_Icon.Kind = PackIconKind.Pause;
                    break;

                case PlayerState.Wait:
                    await _playerController.Play();
                    ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Play);
                    Player_PlayButton_Icon.Kind = PackIconKind.Pause;
                    break;
            }
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if(_playerState == PlayerState.Play || _playerState == PlayerState.Pause)
            {
                await _playerController.Stop();
                ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Wait);
                Player_PlayButton_Icon.Kind = PackIconKind.Play;
            }
            else if (_playerState == PlayerState.Record){
                _recorderController.Stop();
                _recordTimer.Stop();
                SceneInUse.Instance.Set(DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(SceneInUse.Instance.Scene));
                Player_ActualTimeLabel.Content = "--:--:--";
                Player_TotalTimeLabel.Content = "--:--:--";
                Player_RecordButton.Background = _buttonBackground;
                ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Wait);
                foreach (var tab in _tabs)
                {
                    tab.Fill();
                }
            }
        }

        private async void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if(_playerState == PlayerState.Wait)
            {
                await _recorderController.Record();
                Player_ActualTimeLabel.Content = "00:00:00";
                Player_RecordButton.IsEnabled = false;
                Player_StopButton.IsEnabled = true;
                _recordTimer.Start();
                ChangeHomeState(HomeState.FromSensorWithScene, PlayerState.Record);
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
            var location = DataAccessFacade.Instance.GetSceneInUseAccess().GetLocation();
            if (location.HasValue)
            {
                Player_ActualTimeLabel.Content = location.Value.ToString(@"hh\:mm\:ss");
                Player_RecordButton.Background = Player_RecordButton.Background == _buttonBackground ? Brushes.Red : _buttonBackground;
            }
        }

        #endregion
        
        private void SourceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox==null) throw new ArgumentNullException(nameof(sender), Properties.Error.SourceComboboxIsNull);
            int lastSelectedIndex = comboBox.SelectedIndex;
            bool result = false;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    result = ChangeHomeState(HomeState.Initial, PlayerState.Wait);
                    break;

                case 1:
                    result = ChangeHomeState(HomeState.FromSensor, PlayerState.Wait);
                    break;

                case 2:
                    result = ChangeHomeState(HomeState.FromFile, PlayerState.Wait);
                    break;
            }
            if (!result)
                comboBox.SelectedIndex = lastSelectedIndex;
        }

        #region Fill modules
        private void FillMenuInputModules()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                FillProcessingAndGeneralModules(input, ModuleType.Input);
            }
        }

        private void FillMenuProccessingModules()
        {
            foreach (var process in ProcessingLoader.Instance.ProcessingModules)
            {
                FillProcessingAndGeneralModules(process, ModuleType.Processing);
            }
        }

        private void FillMenuGeneralModules()
        {
            foreach(var generalModule in GeneralLoader.Instance.GeneralModules)
            {
                FillProcessingAndGeneralModules(generalModule, ModuleType.General);
            }
        }

        private void FillProcessingAndGeneralModules(API.Module.AbstractModule module, ModuleType moduleType)
        {
            MenuItem moduleMenuItem = new MenuItem
            {
                Header = module.Name
            };
            //fill the windows of inputmodules
            List<MenuItem> windowsMenuItems = new List<MenuItem>();
            foreach (var window in module.Windows)
            {
                MenuItem moduleWin = new MenuItem
                {
                    IsEnabled = false,
                    Header = window.Item1
                };
                moduleWin.Click += (object sender, RoutedEventArgs e) => {
                    window.Item2.GetWindow().Show();
                };
                windowsMenuItems.Add(moduleWin);
                moduleMenuItem.Items.Add(moduleWin);
            }

            //fill the checkbox to enable/disable the module
            CheckBox moduleCheck = new CheckBox
            {
                Content = Properties.Menu.Enable
            };
            moduleCheck.Checked += (object sender, RoutedEventArgs e) =>
            {
                try
                {
                    var inputModule = module as API.Module.Input.InputModule;
                    if (inputModule != null)
                        inputModule.Monitor.Open();
                    module.Enable();
                    foreach (var winItem in windowsMenuItems)
                    {
                        winItem.IsEnabled = true;
                    }
                }
                catch (Exception)
                {
                    var inputModule = module as API.Module.Input.InputModule;
                    if (inputModule != null)
                        inputModule.Monitor.Close();
                    module.Disable();
                    moduleCheck.IsChecked = false;
                    foreach (var winItem in windowsMenuItems)
                    {
                        winItem.IsEnabled = false;
                    }
                }

            };
            moduleCheck.Unchecked += (object sender, RoutedEventArgs e) =>
            {
                try
                {
                    var inputModule = module as API.Module.Input.InputModule;
                    if (inputModule != null)
                        inputModule.Monitor.Close();
                    module.Disable();
                    foreach (var winItem in windowsMenuItems)
                    {
                        winItem.IsEnabled = false;
                    }
                }
                catch (Exception)
                {
                    module.Enable();
                    moduleCheck.IsChecked = true;
                    foreach (var winItem in windowsMenuItems)
                    {
                        winItem.IsEnabled = true;
                    }
                }
            };
            moduleMenuItem.Items.Add(moduleCheck);
            //add to GUI menu according its type.
            switch (moduleType)
            {
                case ModuleType.Input:
                    MenuItems_Tools_Sensors.Items.Add(moduleMenuItem);
                    break;
                case ModuleType.Processing:
                    MenuItems_Tools_Processing.Items.Add(moduleMenuItem);
                    break;
                case ModuleType.General:
                    MenuItems_Tools_General.Items.Add(moduleMenuItem);
                    break;
            }
        }
        #endregion
    }

    public enum PlayerState
    {
        Wait,
        Record,
        Play,
        Pause
    }

    public enum HomeState
    {
        Initial,
        FromSensor,
        FromSensorWithScene,
        FromFile,
        FromFileWithScene
    }

    public enum ModuleType
    {
        Input,
        Processing,
        General
    }
}
