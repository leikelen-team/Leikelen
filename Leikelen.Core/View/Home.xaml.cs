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
using Microsoft.Win32;
using System.IO;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.Data.Access.External;

namespace cl.uv.leikelen.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
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
        private SceneState _homeState;

        private readonly DispatcherTimer _playTimer;

        private Tuple<TimeSpan?, ImageSource> _lastColorBeforePause;
        private Tuple<TimeSpan?, ImageSource> _lastSkeletonBeforePause;

        private List<ITab> _tabs;
        private List<ITab> _generalModuleTabs;
        private List<ITab> _processingModuleTabs;
        private bool _firstLanguage = true;

        public Home()
        {
            switch (GeneralSettings.Instance.Language.Value)
            {
                case "es":
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es");
                    break;
                case "en":
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    break;
            }
            InitializeComponent();

            switch (GeneralSettings.Instance.Language.Value)
            {
                case "es":
                    SpanishLanguageRadio.IsChecked = true;
                    break;
                case "en":
                    EnglishLanguageRadio.IsChecked = true;
                    break;
                default:
                    AutoLanguageRadio.IsChecked = true;
                    break;
            }

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
            _playTimer.Interval = new TimeSpan(0, 0, 0, 1, 0); //1 second

            _playerController.Finished += PlayerFinished;
            
            InputLoader.Instance.VideoHandler.ColorImageArrived += VideoViewer_colorImageArrived;
            InputLoader.Instance.VideoHandler.SkeletonImageArrived += VideoViewer_skeletonImageArrived;

            DataAccessFacade.Instance.GetPersonAccess().PersonsChanged += Home_PersonsChanged;
            
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

            GeneralLoader.GeneralModulesHasReset += RefillGeneralModuleTabs;
            ProcessingLoader.ProcessingModulesHasReset += RefillProcessingModuleTabs;
            
            SkeletonLayerCheckbox.IsChecked = true;
            ColorLayerCheckbox.IsChecked = true;

            //Actions
            ChangeHomeState(SceneState.Base, PlayerState.Wait);
            FillMenuInputModules();
            FillMenuProccessingModules();
            FillMenuGeneralModules();
        }

        #region Tabs
        private void ResetTabs()
        {
            foreach (var tab in _tabs)
            {
                tab.Reset();
                tab.Fill();
            }
        }

        private void RemoveTabs(List<ITab> tabs)
        {
            foreach (var tab in tabs)
            {
                if (tab is TabItem tabItem)
                    Tabs.RemoveFromSource(tabItem);
            }
        }
        private void FillTabs(List<ITab> tabs, string personName)
        {
            foreach(var tab in tabs)
            {
                if (tab is TabItem tabItem)
                {
                    if (!String.IsNullOrEmpty(personName))
                        tabItem.Header = $"{tabItem.Header} ({personName})";
                    Tabs.AddToSource(tabItem);
                }
            }
        }

        private void RefillGeneralModuleTabs(object sender, EventArgs e)
        {
            MenuItems_Tools_General.Items.Clear();
            FillMenuGeneralModules();

            if (!ReferenceEquals(null, _generalModuleTabs))
            {
                foreach (var tab in _generalModuleTabs)
                {
                    if (tab is TabItem tabItem)
                        Tabs.RemoveFromSource(tabItem);
                }
            }
            _generalModuleTabs = new List<ITab>();
            foreach (var generalModule in GeneralLoader.Instance.GeneralModules)
            {
                if(generalModule.IsEnabled)
                    _generalModuleTabs.AddRange(generalModule.Tabs);
            }
            foreach(var tab in _generalModuleTabs)
            {
                if (tab is TabItem tabItem)
                    Tabs.RemoveFromSource(tabItem);
            }
        }

        private void RefillProcessingModuleTabs(object sender, EventArgs e)
        {
            MenuItems_Tools_Processing.Items.Clear();
            FillMenuProccessingModules();

            if (!ReferenceEquals(null, _processingModuleTabs))
            {
                foreach (var tab in _processingModuleTabs)
                {
                    if (tab is TabItem tabItem)
                        Tabs.RemoveFromSource(tabItem);
                }
            }
            _processingModuleTabs = new List<ITab>();
            foreach (var processingModule in GeneralLoader.Instance.GeneralModules)
            {
                if (processingModule.IsEnabled)
                    _processingModuleTabs.AddRange(processingModule.Tabs);
            }
            foreach (var tab in _processingModuleTabs)
            {
                if (tab is TabItem tabItem)
                    Tabs.RemoveFromSource(tabItem);
            }
        }
        #endregion
        
        #region states
        private bool ChangeHomeState(SceneState newHomeState, PlayerState newPlayerState)
        {
            switch (newHomeState)
            {
                case SceneState.Base:
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
                case SceneState.FromSensorWithScene:
                    if (ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
                        return false;
                    SetGuiFromSensorWithScene(newPlayerState);
                    break;
                case SceneState.FromFileWithScene:
                    if (ReferenceEquals(null, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
                        return false;
                    SetGuiFromFileWithScene(newPlayerState);
                    break;
            }

            _homeState = newHomeState;
            if (_homeState == SceneState.Base)
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
            MenuItems_Tools_PersonSensors.IsEnabled = false;
            MenuItems_Tools_Processing.IsEnabled = false;
            MenuItems_Tools_General.IsEnabled = true;
            
            MenuItem_Scene_Configure.IsEnabled = false;
            MenuItem_Scene_AddPerson.IsEnabled = false;
            MenuItem_Scene_Persons.IsEnabled = true;
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
                    ResetTabs();
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
            MenuItems_Tools_PersonSensors.IsEnabled = false;
            MenuItems_Tools_Processing.IsEnabled = false;
            
            MenuItem_Scene_Configure.IsEnabled = true;
            MenuItem_Scene_AddPerson.IsEnabled = false;
            MenuItem_Scene_Persons.IsEnabled = true;
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
                    MenuItems_Tools_PersonSensors.IsEnabled = true;
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
                    MenuItems_Tools_PersonSensors.IsEnabled = false;
                    MenuItems_Tools_Processing.IsEnabled = false;

                    MenuItem_Scene_AddPerson.IsEnabled = false;
                    MenuItem_Scene_Persons.IsEnabled = true;

                    Player_RecordButton.IsEnabled = false;
                    Player_StopButton.IsEnabled = true;
                    break;
            }
        }

        private void ResetMenuesNewScene()
        {
            Player_ActualTimeLabel.Content = "--:--:--";
            Player_RecordButton.Background = _buttonBackground;
            Player_TotalTimeLabel.Content = "--:--:--";
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Disable();
            }
            ProcessingLoader.Reset();
            ResetTabs();
            ResetMenuModules(false);
            MenuItem_Scene_PersonsInScene.Items.Clear();
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
                    ChangeHomeState(SceneState.FromSensorWithScene, PlayerState.Wait);
                    ResetMenuesNewScene();
                }
            };
        }
        
        private void MenuItem_File_LoadTestScene_Click(object sender, RoutedEventArgs e)
        {
            TestScene.LoadTest("Test");
            ChangeHomeState(SceneState.FromFileWithScene, PlayerState.Wait);
            ResetMenuesNewScene();
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
                    ChangeHomeState(SceneState.FromFileWithScene, PlayerState.Wait);
                    ResetMenuesNewScene();
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

                        ChangeHomeState(SceneState.FromFileWithScene, PlayerState.Wait);
                        ResetMenuesNewScene();
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
        }

        private void MenuItem_File_Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_File_Restart_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(Properties.GUI.SureRestartApp,
                Properties.GUI.Confirmation, 
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
            if(result== MessageBoxResult.OK)
            {
                RestartApp();
            }
        }

        private void RestartApp()
        {
            // Get file path of current process 
            var filePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //var filePath = Application.ExecutablePath;  // for WinForms

            // Start program
            Process.Start(filePath);

            Process.GetCurrentProcess().Kill();
        }
        #endregion

        #region Edit MenuItems
        private void AutoLanguage_Checked(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("auto");
        }

        private void EnglishLanguage_Checked(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("en");
        }

        private void SpanishLanguage_Checked(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("es");
        }

        private void ChangeLanguage(string languageCode)
        {
            if (_firstLanguage)
            {
                _firstLanguage = false;
                return;
            }
            GeneralSettings.Instance.Language.Write(languageCode);
            MessageBoxResult result = MessageBox.Show(Properties.GUI.MustRestartOrNo,
                Properties.GUI.RestartApp,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                RestartApp();
            }
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
            try
            {
                Process.Start(@"documentation\web\index.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Error.ErrorOcurred + "\n" + ex.Message,
                    Properties.Error.ErrorOcurredTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void MenuItem_Help_Manual_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(@"documentation\user_manual.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Error.ErrorOcurred + "\n" + ex.Message, 
                    Properties.Error.ErrorOcurredTitle, 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
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
                    ChangeHomeState(SceneState.FromFileWithScene, PlayerState.Pause);
                    Player_PlayButton_Icon.Kind = PackIconKind.Play;
                    break;

                case PlayerState.Pause:
                    _playerController.UnPause();
                    _playTimer.Start();
                    ChangeHomeState(SceneState.FromFileWithScene, PlayerState.Play);
                    Player_PlayButton_Icon.Kind = PackIconKind.Pause;
                    break;

                case PlayerState.Wait:
                    _playerController.Play();
                    _playTimer.Start();
                    ChangeHomeState(SceneState.FromFileWithScene, PlayerState.Play);
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
                await _recorderController.Stop();
                _recordTimer.Stop();
                SceneInUse.Instance.Set(DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(SceneInUse.Instance.Scene));
                Player_ActualTimeLabel.Content = "--:--:--";
                Player_TotalTimeLabel.Content = SceneInUse.Instance.Scene.Duration.ToString(@"hh\:mm\:ss");
                Player_RecordButton.Background = _buttonBackground;
                ChangeHomeState(SceneState.FromFileWithScene, PlayerState.Wait);
                ResetTabs();
            }
        }

        private void PlayerStop()
        {
            _playerController.Stop();
            _playTimer.Stop();
            //blablabla
            Player_ActualTimeLabel.Content = "00:00:00";
            Player_TotalTimeLabel.Content = SceneInUse.Instance.Scene.Duration.ToString(@"hh\:mm\:ss");
            ChangeHomeState(SceneState.FromFileWithScene, PlayerState.Wait);
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
                ChangeHomeState(SceneState.FromSensorWithScene, PlayerState.Record);
            }
        }

        private void LocationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(!ReferenceEquals(null, SceneInUse.Instance.Scene) && _playerState != PlayerState.Record)
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
                FillProcessingAndGeneralModules(input, ModuleType.InputScene, null);
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


        private void Home_PersonsChanged(object sender, Person e)
        {
            Console.WriteLine("personas cambiaron");
            MenuItem_Scene_PersonsInScene.Items.Clear();
            foreach(var pis in DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene)
            {
                Console.WriteLine("persona: "+pis.Person.Name);
                MenuItem personItem = new MenuItem
                {
                    Header = pis.Person.Name
                };
                personItem.Click += (personSender, personE) =>
                {
                    new ConfigurePerson(pis.Person).Show();
                };
                MenuItem_Scene_PersonsInScene.Items.Add(personItem);
            }

            MenuItems_Tools_PersonSensors.Items.Clear();
            foreach(var pis in DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene)
            {
                foreach (var personModule in InputLoader.Instance.PersonInputModules[pis.Person])
                {
                    FillProcessingAndGeneralModules(personModule, ModuleType.InputPerson, pis.Person);
                }
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
                    if(!ReferenceEquals(null, person))
                        FillTabs(module.Tabs, person.Name);
                    else
                        FillTabs(module.Tabs, null);

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
                    RemoveTabs(module.Tabs);
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
            moduleCheck.IsChecked = module.IsEnabled;
            moduleMenuItem.Items.Add(moduleCheck);
            //add to GUI menu according its type.
            switch (moduleType)
            {
                case ModuleType.InputScene:
                    MenuItems_Tools_Sensors.Items.Add(moduleMenuItem);
                    break;
                case ModuleType.Processing:
                    MenuItems_Tools_Processing.Items.Add(moduleMenuItem);
                    break;
                case ModuleType.General:
                    MenuItems_Tools_General.Items.Add(moduleMenuItem);
                    break;
                case ModuleType.InputPerson:
                    personItem.Items.Add(moduleMenuItem);
                    break;
            }
            if(moduleType == ModuleType.InputPerson)
                MenuItems_Tools_PersonSensors.Items.Add(personItem);
        }

        private void ResetMenuModules(bool alsoGeneralModules)
        {
            MenuItems_Tools_Sensors.Items.Clear();
            MenuItems_Tools_PersonSensors.Items.Clear();
            MenuItems_Tools_Processing.Items.Clear();

            FillMenuInputModules();
            FillMenuProccessingModules();

            if (alsoGeneralModules)
            {
                MenuItems_Tools_General.Items.Clear();
                FillMenuGeneralModules();
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

    public enum SceneState
    {
        Base,
        FromSensorWithScene,
        FromFileWithScene
    }

    public enum ModuleType
    {
        InputScene,
        Processing,
        General,
        InputPerson
    }
}
