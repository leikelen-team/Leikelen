﻿using System;
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
using Microsoft.Win32;
using System.IO;
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

        private readonly DispatcherTimer _playTimer;

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
            _buttonBackground = Player_RecordButton.Background;

            //Initialize record button timer arguments
            _recordTimer = new DispatcherTimer();
            _recordTimer.Tick += _recordTimer_Tick;
            _recordTimer.Interval = new TimeSpan(0, 0, 0, 0, 500); //0.5 seconds

            //Initialize player timer
            _playTimer = new DispatcherTimer();
            _playTimer.Tick += _playTimer_Tick;
            _recordTimer.Interval = new TimeSpan(0, 0, 0, 1, 0); //1 second

            _playerController.Finished += PlayerFinished;

            //File MenuItems
            MenuItem_File_NewScene.Click += File_NewScene_Click;
            MenuItem_File_LoadTestScene.Click += MenuItem_File_LoadTestScene_Click;
            MenuItem_File_Import.Click += MenuItem_File_Import_Click;
            MenuItem_File_Export.Click += MenuItem_File_Export_Click;
            MenuItem_File_Save.Click += MenuItem_File_Save_Click;
            MenuItem_File_AllScenes.Click += MenuItem_File_AllScenes_Click;
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
            
            InputLoader.Instance.VideoHandler.ColorImageArrived += VideoViewer_colorImageArrived;
            InputLoader.Instance.VideoHandler.SkeletonImageArrived += VideoViewer_skeletonImageArrived;

            DataAccessFacade.Instance.GetPersonAccess().PersonAdded += Home_PersonAdded;
            
            _tabs = new List<ITab>()
            {
                new Widget.HomeTab.TabInterval(),
                new Widget.HomeTab.TabGraph(),
                new Widget.HomeTab.TabScene()
            };

            foreach(var tab in _tabs)
            {
                if(tab is TabItem tabItem)
                    Tabs.AddToSource(tabItem);
            }

            SkeletonLayerCheckbox.Checked += SkeletonLayerCheckbox_Checked;
            SkeletonLayerCheckbox.Unchecked += SkeletonLayerCheckbox_Unchecked;

            ColorLayerCheckbox.Checked += ColorLayerCheckbox_Checked;
            ColorLayerCheckbox.Unchecked += ColorLayerCheckbox_Unchecked;

            SkeletonLayerCheckbox.IsChecked = true;
            ColorLayerCheckbox.IsChecked = true;

            //Actions
            ChangeHomeState(HomeState.Base, PlayerState.Wait);
            FillMenuInputModules();
            FillMenuProccessingModules();
            FillMenuGeneralModules();
        }

        private void MenuItem_File_LoadTestScene_Click(object sender, RoutedEventArgs e)
        {
            LoadTestScene.LoadTest("Test");
            ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Wait);
        }

        #region states
        private bool ChangeHomeState(HomeState newHomeState, PlayerState newPlayerState)
        {
            switch (newHomeState)
            {
                case HomeState.Base:
                    if (_playerState == PlayerState.Record)
                    {
                        MessageBoxResult result = MessageBox.Show(Properties.GUI.stopRecordFirst, 
                            Properties.GUI.Atention, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }
                    SceneInUse.Instance.Set(null);
                    _mediaController.SetFromNone();
                    SetGuiBase();
                    break;
                case HomeState.FromSensorWithScene:
                    if (ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
                        return false;
                    SetGuiFromSensorWithScene(newPlayerState);
                    break;
                case HomeState.FromFileWithScene:
                    if (ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
                        return false;
                    SetGuiFromFileWithScene(newPlayerState);
                    break;
            }

            _homeState = newHomeState;
            if (_homeState == HomeState.Base)
                _playerState = PlayerState.Wait;
            else
                _playerState = newPlayerState;
            return true;
        }

        private void SetGuiBase()
        {
            MenuItem_File_NewScene.IsEnabled = true;
            MenuItem_File_Save.IsEnabled = false;
            MenuItem_File_AllScenes.IsEnabled = true;
            MenuItem_File_Import.IsEnabled = true;
            MenuItem_File_Export.IsEnabled = false;
            MenuItem_File_Quit.IsEnabled = true;

            MenuItem_Tools_Preferences.IsEnabled = true;
            MenuItem_Tools_DB.IsEnabled = true;
            MenuItem_Tools_Player.IsEnabled = false;
            MenuItems_Tools_Sensors.IsEnabled = false;
            MenuItems_Tools_Processing.IsEnabled = false;
            MenuItems_Tools_General.IsEnabled = true;
            
            MenuItem_Scene_Configure.IsEnabled = false;
            MenuItem_Scene_AddPerson.IsEnabled = false;
            MenuItem_Scene_Persons.IsEnabled = false;
            MenuItem_Scene_PersonsInScene.IsEnabled = false;

            Player_LocationSlider.IsEnabled = false;
            Player_RecordButton.IsEnabled = false;
            Player_PlayButton.IsEnabled = false;
            Player_StopButton.IsEnabled = false;
        }

        private void SetGuiFromFileWithScene(PlayerState playerState)
        {
            switch (playerState)
            {
                case PlayerState.Wait:
                    Player_StopButton.IsEnabled = false;
                    foreach (var tab in _tabs)
                    {
                        tab.Reset();
                        tab.Fill();
                    }
                    break;
                case PlayerState.Play:
                    Player_StopButton.IsEnabled = true;
                    break;
                case PlayerState.Pause:
                    Player_StopButton.IsEnabled = true;
                    break;
            }
            MenuItem_File_Save.IsEnabled = true;
            MenuItem_File_Export.IsEnabled = true;
            
            MenuItem_Tools_Player.IsEnabled = true;
            MenuItems_Tools_Sensors.IsEnabled = false;
            MenuItems_Tools_Processing.IsEnabled = false;
            
            MenuItem_Scene_Configure.IsEnabled = true;
            MenuItem_Scene_AddPerson.IsEnabled = false;
            MenuItem_Scene_Persons.IsEnabled = false;
            MenuItem_Scene_PersonsInScene.IsEnabled = false;

            Player_LocationSlider.IsEnabled = true;
            Player_RecordButton.IsEnabled = false;
            Player_PlayButton.IsEnabled = true;
        }

        private void SetGuiFromSensorWithScene(PlayerState playerState)
        {
            MenuItem_File_Save.IsEnabled = true;
            MenuItem_File_Export.IsEnabled = true;
            MenuItem_Scene_Configure.IsEnabled = true;
            Player_LocationSlider.IsEnabled = false;
            Player_PlayButton.IsEnabled = false;
            switch (playerState)
            {
                case PlayerState.Wait:
                    MenuItem_Tools_Player.IsEnabled = false;
                    MenuItems_Tools_Sensors.IsEnabled = true;
                    MenuItems_Tools_Processing.IsEnabled = true;
                    MenuItem_Scene_PersonsInScene.IsEnabled = true;

                    MenuItem_Scene_AddPerson.IsEnabled = true;
                    MenuItem_Scene_Persons.IsEnabled = true;

                    Player_RecordButton.IsEnabled = true;
                    Player_StopButton.IsEnabled = false;
                    break;
                case PlayerState.Record:
                    MenuItem_Tools_Player.IsEnabled = true;
                    MenuItems_Tools_Sensors.IsEnabled = false;
                    MenuItems_Tools_Processing.IsEnabled = false;

                    MenuItem_Scene_AddPerson.IsEnabled = false;
                    MenuItem_Scene_Persons.IsEnabled = false;

                    Player_RecordButton.IsEnabled = false;
                    Player_StopButton.IsEnabled = true;
                    break;
            }
        }

        private bool StopRecordFirstMsg()
        {
            if (_playerState == PlayerState.Record)
            {
                MessageBox.Show(Properties.GUI.stopRecordFirst,
                    Properties.GUI.Atention, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            return true;
        }

        private bool AskLoseScene()
        {
            if (!ReferenceEquals(null, SceneInUse.Instance.Scene))
            {
                MessageBoxResult result = MessageBox.Show(Properties.GUI.ActualSceneWillLose,
                    Properties.GUI.Atention, MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.OK)
                    return true;
                else
                    return false;
            }
            else
                return true;
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
            if (!StopRecordFirstMsg()) return;
            if (!AskLoseScene()) return;

            var configureSceneWin = new ConfigureScene();
            configureSceneWin.Show();
            configureSceneWin.Closed += (senderClosed, eClosed) =>
            {
                if (!ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
                {
                    ChangeHomeState(HomeState.FromSensorWithScene, PlayerState.Wait);
                }
            };
        }

        private void MenuItem_File_AllScenes_Click(object sender, RoutedEventArgs e)
        {
            if (!StopRecordFirstMsg()) return;
            if (!AskLoseScene()) return;

            var allScenesWin = new AllScenes();
            allScenesWin.Show();
            allScenesWin.Closed += (senderAllScenes, eAllScenes) =>
            {
                if (!ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
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
            _playerController.Close();
            PlayerStop();
            var exportWin = new Export();
            exportWin.Show();
        }

        private void MenuItem_File_Import_Click(object sender, RoutedEventArgs e)
        {
            if (!StopRecordFirstMsg()) return;
            if (!AskLoseScene()) return;

            var dlg = new OpenFileDialog()
            {
                Filter = GeneralSettings.Instance.ExtensionFilter,
                DefaultExt = GeneralSettings.Instance.Extension
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                if (!String.IsNullOrEmpty(dlg.FileName))
                {
                    try
                    {
                        FileController.Import(dlg.FileName);
                        _playerController.Close();
                        PlayerStop();
                        MessageBox.Show(Properties.GUI.SceneImportedSuccesfully,
                            Properties.GUI.SceneImportedSuccesfullyTitle,
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Properties.GUI.SceneImportedError+"\n"+ex.Message,
                            Properties.GUI.SceneImportedErrorTitle,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    
                }
                else
                {
                    MessageBox.Show(Properties.GUI.SceneImportedNoFile,
                        Properties.GUI.SceneImportedNoFile,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Wait);
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
            Player_TotalTimeLabel.Content = SceneInUse.Instance.Scene.Duration.ToString(@"hh\:mm\:ss");
            switch (_playerState)
            {
                case PlayerState.Play:
                    _playTimer.Stop();
                    _playerController.Pause();
                    ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Pause);
                    Player_PlayButton_Icon.Kind = PackIconKind.Play;
                    break;

                case PlayerState.Pause:
                    _playerController.UnPause();
                    _playTimer.Start();
                    ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Play);
                    Player_PlayButton_Icon.Kind = PackIconKind.Pause;
                    break;

                case PlayerState.Wait:
                    _playerController.Play();
                    _playTimer.Start();
                    ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Play);
                    Player_PlayButton_Icon.Kind = PackIconKind.Pause;
                    break;
            }
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if(_playerState == PlayerState.Play || _playerState == PlayerState.Pause)
            {
                PlayerStop();
            }
            else if (_playerState == PlayerState.Record){
                _recorderController.Stop();
                _recordTimer.Stop();
                SceneInUse.Instance.Set(DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(SceneInUse.Instance.Scene));
                Player_ActualTimeLabel.Content = "--:--:--";
                Player_TotalTimeLabel.Content = SceneInUse.Instance.Scene.Duration.ToString(@"hh\:mm\:ss");
                Player_RecordButton.Background = _buttonBackground;
                ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Wait);
                foreach (var tab in _tabs)
                {
                    tab.Reset();
                    tab.Fill();
                }
            }
        }

        private void PlayerStop()
        {
            _playerController.Stop();
            _playTimer.Stop();
            //blablabla
            Player_ActualTimeLabel.Content = "00:00:00";
            Player_TotalTimeLabel.Content = SceneInUse.Instance.Scene.Duration.ToString(@"hh\:mm\:ss");
            ChangeHomeState(HomeState.FromFileWithScene, PlayerState.Wait);
            Player_PlayButton_Icon.Kind = PackIconKind.Play;
        }

        private async void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if(_playerState == PlayerState.Wait)
            {
                await _recorderController.Record();
                Player_ActualTimeLabel.Content = "00:00:00";
                Player_TotalTimeLabel.Content = "--:--:--";
                Player_RecordButton.IsEnabled = false;
                Player_StopButton.IsEnabled = true;
                _recordTimer.Start();
                ChangeHomeState(HomeState.FromSensorWithScene, PlayerState.Record);
            }
        }

        private void LocationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(_playerState != PlayerState.Record)
            {
                TimeSpan newTime = new TimeSpan(SceneInUse.Instance.Scene.Duration.Ticks * (long)(e.NewValue * 1000/1000));
                _playerController.MoveTo(newTime);
            }
        }

        private void PlayerFinished(object sender, EventArgs e)
        {
            if (_playerState != PlayerState.Record)
            {
                PlayerStop();
            }
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
        
        private void _playTimer_Tick(object sender, EventArgs e)
        {
            var location = DataAccessFacade.Instance.GetSceneInUseAccess().GetLocation();
            if (location.HasValue)
            {
                Player_ActualTimeLabel.Content = location.Value.ToString(@"hh\:mm\:ss");
            }
        }

        #endregion
        
        #region Fill modules
        private void FillMenuInputModules()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                FillProcessingAndGeneralModules(input, ModuleType.Input, null);
            }
        }

        private void FillMenuProccessingModules()
        {
            foreach (var process in ProcessingLoader.Instance.ProcessingModules)
            {
                FillProcessingAndGeneralModules(process, ModuleType.Processing, null);
            }
        }

        private void FillMenuGeneralModules()
        {
            foreach(var generalModule in GeneralLoader.Instance.GeneralModules)
            {
                FillProcessingAndGeneralModules(generalModule, ModuleType.General, null);
            }
        }


        private void Home_PersonAdded(object sender, Person e)
        {
            foreach(var personModule in InputLoader.Instance.PersonInputModules[e])
            {
                FillProcessingAndGeneralModules(personModule, ModuleType.Person, e);
            }
        }


        private void FillProcessingAndGeneralModules(API.Module.AbstractModule module, ModuleType moduleType, Person person)
        {
            MenuItem moduleMenuItem = new MenuItem
            {
                Header = module.Name
            };
            MenuItem personItem = new MenuItem();
            personItem.Header = person?.Name;
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
                    if (module is API.Module.Input.InputModule inputModule)
                        inputModule.Monitor.Open();
                    module.Enable();
                    foreach (var winItem in windowsMenuItems)
                    {
                        winItem.IsEnabled = true;
                    }
                }
                catch (Exception)
                {
                    if (module is API.Module.Input.InputModule inputModule)
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
                    if (module is API.Module.Input.InputModule inputModule)
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
                case ModuleType.Person:
                    personItem.Items.Add(moduleMenuItem);
                    break;
            }
            if(moduleType == ModuleType.Person)
                MenuItem_Scene_PersonsInScene.Items.Add(personItem);
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
        Base,
        FromSensorWithScene,
        FromFileWithScene
    }

    public enum ModuleType
    {
        Input,
        Processing,
        General,
        Person
    }
}
