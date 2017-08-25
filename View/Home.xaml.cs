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
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.InputModule;
using cl.uv.leikelen.ProcessingModule;
using System.Diagnostics;
using cl.uv.leikelen.Data.Access.External;
using cl.uv.leikelen.Data.Persistence;
using cl.uv.leikelen.Properties;
using MaterialDesignThemes.Wpf;

using cl.uv.leikelen.Data.Model;

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



        private HomeState _homeState;

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

            //SCene MenuItems
            MenuItem_Scene_Configure.Click += MenuItem_Scene_Configure_Click;
            MenuItem_Scene_AddPerson.Click += MenuItem_Scene_AddPerson_Click;

            //Help MenuItems
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

            //Set initial states
            Player_LocationSlider.IsEnabled = false;
            MenuItem_Scene.IsEnabled = false;
            MenuItem_File_Save.IsEnabled = false;
            MenuItem_File_NewScene.IsEnabled = false;

            Tabs.AddToSource(new Widget.TabInterval());
            Tabs.AddToSource(new Widget.TabGraph());
            Tabs.AddToSource(new Widget.TabDistance());
            Tabs.AddToSource(new Widget.TabScene());

            //Actions
            SetFromNone();
            FillMenuInputModules();
            FillMenuProccessingModules();
        }

        private void MenuItem_File_LoadTestScene_Click(object sender, RoutedEventArgs e)
        {
            ChangeHomeState(HomeState.FromFileWithScene);

            var scenes = DbFacade.Instance.Provider.LoadScenes();
            if(scenes != null)
            {
                var testScene = scenes.Find(s => s.Name == "Test");
                if(testScene != null)
                {
                    SceneInUse.Instance.Set(testScene);
                    return;
                }
            }



                var scene = new Scene()
            {
                Name = "Test",
                NumberOfParticipants = 2,
                Type = "Test scene",
                Description = "This is a test scene\n only for purposes of development",
                Place = "Programmed",
                RecordRealDateTime = new DateTime(2017, 8, 25, 16, 2, 0),
                Duration = new TimeSpan(0, 2, 35)
            };
            SceneInUse.Instance.Set(scene);
            var pAccess = new PersonAccess();
            var person1 = pAccess.Add("Erick",null, null, 'M');
            var person2 = pAccess.Add("Dorotea", null, null, 'F');
            pAccess.AddToScene(person1, scene);
            pAccess.AddToScene(person2, scene);

            var mAccess = new ModalAccess();
            var smAccess = new SubModalAccess();

            var modalPosture2 = mAccess.Add("Voice", "Hablo o no de kinect");
            smAccess.Add("Voice", "Talked", "Talked or not", null);

            var modalPosture1 = mAccess.Add("Posture", "Posturas de kinect");
            smAccess.Add("Posture", "Seated", "Is Seated", null);
            smAccess.Add("Posture", "Hand On Wrist", "o<>===<", null);
            smAccess.Add("Posture", "Asking Help", "I have a question teacher plis", null);
            smAccess.Add("Posture", "CrossedArms", "Is crossing his/her arms", null);
            
            
            var rnd = new Random();
            var eAccess = new EventAccess();
            var iAccess = new IntervalAccess();
            var tAccess = new TimelessAccess();
            Console.WriteLine(mAccess.GetAll().Count);       
            foreach(var pis in SceneInUse.Instance.Scene.PersonsInScene)
            {
                var howMany = rnd.Next(1, 5);
                int i = 0;
                Console.WriteLine($"How many: {howMany}");
                foreach (var m in mAccess.GetAll())
                {
                    Console.WriteLine($"{m.ModalTypeId} tiene {m.SubModalTypes.Count} submodaltypes");
                    foreach (var s in m.SubModalTypes)
                    {
                        i++;
                        if (i > howMany) break;
                            var which = rnd.Next(1, 3);
                            Console.WriteLine($"Which: {which}");
                            var data = rnd.Next(1, 3);
                            Console.WriteLine($"Data: {data}");
                            for (int j = 0; j<rnd.Next(0, 100);j++)
                            {
                                double doubleData = rnd.NextDouble();
                                string subtitleData = "hola";
                                switch (which)
                                {
                                    case 1:
                                        switch (data)
                                        {
                                            case 1:
                                                eAccess.Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, new TimeSpan(rnd.Next(0, (int)SceneInUse.Instance.Scene.Duration.Ticks)), doubleData);
                                                break;
                                            case 2:
                                                eAccess.Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, new TimeSpan(rnd.Next(0, (int)SceneInUse.Instance.Scene.Duration.Ticks)), subtitleData);
                                                break;
                                            case 3:
                                                eAccess.Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, new TimeSpan(rnd.Next(0, (int)SceneInUse.Instance.Scene.Duration.Ticks)), 1);
                                                break;
                                        }
                                        break;
                                    case 2:
                                        int minTicks = rnd.Next(0, (int)SceneInUse.Instance.Scene.Duration.Ticks);
                                        TimeSpan min = new TimeSpan(minTicks);
                                        TimeSpan max = new TimeSpan(rnd.Next(minTicks, (int)SceneInUse.Instance.Scene.Duration.Ticks));
                                        switch (data)
                                        {
                                            case 1:
                                                iAccess.Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId,min,max, doubleData);
                                                break;
                                            case 2:
                                                iAccess.Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, min, max, subtitleData);
                                                break;
                                            case 3:
                                                iAccess.Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, min, max, 1);
                                                break;
                                        }
                                        break;
                                    case 3:
                                        switch (data)
                                        {
                                            case 1:
                                                tAccess.Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, doubleData, rnd.Next(0, 24));
                                                break;
                                            case 2:
                                                tAccess.Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, subtitleData, rnd.Next(0, 24));
                                                break;
                                            case 3:
                                                tAccess.Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, rnd.Next(0, 24));
                                                break;
                                        }
                                        break;
                                }
                            }
                            
                        
                    }
                }
            }



            
        }

        private void ChangeHomeState(HomeState newHomeState)
        {
            _homeState = newHomeState;
            switch (newHomeState)
            {
                case HomeState.Initial:
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
                    break;
                case HomeState.FromSensor:
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
                    break;
                case HomeState.FromSensorWithScene:
                    MenuItem_File_NewScene.IsEnabled = true;
                    MenuItem_File_Save.IsEnabled = true;
                    MenuItem_File_Recent.IsEnabled = false;
                    MenuItem_File_Recent_More.IsEnabled = false;
                    MenuItem_File_Import.IsEnabled = false;
                    MenuItem_File_Export.IsEnabled = true;
                    MenuItem_File_Quit.IsEnabled = true;

                    MenuItem_Tools_Preferences.IsEnabled = true;
                    MenuItem_Tools_DB.IsEnabled = true;
                    MenuItem_Tools_Player.IsEnabled = true;
                    MenuItems_Tools_Sensors.IsEnabled = true;
                    MenuItems_Tools_Processing.IsEnabled = true;

                    MenuItem_Scene.IsEnabled = true;
                    MenuItem_Scene_Configure.IsEnabled = true;
                    MenuItem_Scene_AddPerson.IsEnabled = true;

                    Player_LocationSlider.IsEnabled = false;
                    Player_RecordButton.IsEnabled = true;
                    Player_PlayButton.IsEnabled = false;
                    Player_StopButton.IsEnabled = false;
                    Player_VolumeToggle.IsEnabled = false;
                    Player_VolumeSlider.IsEnabled = false;
                    break;
                case HomeState.FromSensorRecording:
                    MenuItem_File_NewScene.IsEnabled = false;
                    MenuItem_File_Save.IsEnabled = false;
                    MenuItem_File_Recent.IsEnabled = false;
                    MenuItem_File_Recent_More.IsEnabled = false;
                    MenuItem_File_Import.IsEnabled = false;
                    MenuItem_File_Export.IsEnabled = false;
                    MenuItem_File_Quit.IsEnabled = true;

                    MenuItem_Tools_Preferences.IsEnabled = true;
                    MenuItem_Tools_DB.IsEnabled = false;
                    MenuItem_Tools_Player.IsEnabled = true;
                    MenuItems_Tools_Sensors.IsEnabled = false;
                    MenuItems_Tools_Processing.IsEnabled = false;

                    MenuItem_Scene.IsEnabled = true;
                    MenuItem_Scene_Configure.IsEnabled = true;
                    MenuItem_Scene_AddPerson.IsEnabled = true;

                    Player_LocationSlider.IsEnabled = false;
                    Player_RecordButton.IsEnabled = false;
                    Player_PlayButton.IsEnabled = false;
                    Player_StopButton.IsEnabled = true;
                    Player_VolumeToggle.IsEnabled = true;
                    Player_VolumeSlider.IsEnabled = true;
                    break;
                case HomeState.FromFile:
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
                    break;
                case HomeState.FromFileWithScene:
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
                    Player_StopButton.IsEnabled = false;
                    Player_VolumeToggle.IsEnabled = true;
                    Player_VolumeSlider.IsEnabled = true;
                    break;
            }
        }


        #region Window Events

        private void Home_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(GUI.AreSureExit, GUI.Confirmation, MessageBoxButton.YesNo,
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
                if (SceneInUse.Instance.Scene != null)
                {
                    ChangeHomeState(HomeState.FromSensorWithScene);
                }
            };
        }

        private void MenuItem_File_Recent_More_Click(object sender, RoutedEventArgs e)
        {
            var allScenesWin = new AllScenes();
            allScenesWin.Show();
            allScenesWin.Closed += (senderAllScenes, eAllScenes) =>
            {
                if (SceneInUse.Instance.Scene != null)
                {
                    ChangeHomeState(HomeState.FromFileWithScene);
                }
            };
        }

        private void MenuItem_File_Save_Click(object sender, RoutedEventArgs e)
        {
            if (SceneInUse.Instance.Scene != null)
            {
                if (DbFacade.Instance.Provider.LoadScene(SceneInUse.Instance.Scene.SceneId) == null)
                    DbFacade.Instance.Provider.SaveScene(SceneInUse.Instance.Scene);
                else
                    DbFacade.Instance.Provider.UpdateScene(SceneInUse.Instance.Scene);


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
        private void MenuItem_Scene_AddPerson_Click(object sender, RoutedEventArgs e)
        {
            if (SceneInUse.Instance.Scene != null)
            {
                var addPersonWin = new ConfigurePerson();
                addPersonWin.Show();
            }
        }

        private void MenuItem_Scene_Configure_Click(object sender, RoutedEventArgs e)
        {
            if (SceneInUse.Instance.Scene != null)
            {
                var configureSceneWin = new ConfigureScene(SceneInUse.Instance.Scene);
                configureSceneWin.Show();
            }
        }
        #endregion

        #region Help MenuItems
        private void MenuItem_Help_AboutUs_Click(object sender, RoutedEventArgs e)
        {
            var aboutWin = new AboutUs();
            aboutWin.Show();
        }
        #endregion

        #region Video viewer

        private void VideoViewer_skeletonImageArrived(object sender, System.Windows.Media.ImageSource e)
        {
            Console.WriteLine("skeleton arrived");
            if (SkeletonLayerCheckbox.IsChecked.HasValue && SkeletonLayerCheckbox.IsChecked.Value)
            {
                Player_ImageControl_Layer1.Source = e;
            }
        }

        private void VideoViewer_colorImageArrived(object sender, System.Windows.Media.ImageSource e)
        {
            Console.WriteLine("color arrived");
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
                    Player_PlayButton_Icon.Kind = PackIconKind.Play;
                    break;

                case PlayerState.Pause:
                    await _playerController.UnPause();
                    _playerState = PlayerState.Play;
                    Player_PlayButton_Icon.Kind = PackIconKind.Pause;
                    break;

                case PlayerState.Wait:
                    await _playerController.Play();
                    _playerState = PlayerState.Play;
                    Player_PlayButton_Icon.Kind = PackIconKind.Pause;
                    break;
            }
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if(_playerState == PlayerState.Play || _playerState == PlayerState.Pause)
            {
                await _playerController.Stop();
                _playerState = PlayerState.Wait;
                Player_PlayButton_Icon.Kind = PackIconKind.Play;
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
                ChangeHomeState(HomeState.FromSensorRecording);
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
            var location = SceneInUseAccess.Instance.GetLocation();
            if (location.HasValue)
            {
                Player_ActualTimeLabel.Content = location.Value.ToString(@"hh\:mm\:ss");
                Player_RecordButton.Background = Player_RecordButton.Background == _buttonBackground ? Brushes.Red : _buttonBackground;
            }
        }

        #endregion

        #region Source
        private void SourceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox==null) throw new ArgumentNullException(nameof(sender), Error.SourceComboboxIsNull);
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

            ChangeHomeState(HomeState.Initial);
        }

        private void SetFromSensor()
        {
            if (_homeState == HomeState.FromSensor || _homeState == HomeState.FromSensorWithScene ||
                _homeState == HomeState.FromSensorRecording)
            {
                return;
            }
            _mediaController.SetFromSensor();

            ChangeHomeState(HomeState.FromSensor);
        }

        private void SetFromFile()
        {
            if (_homeState == HomeState.FromFile || _homeState == HomeState.FromFileWithScene)
            {
                return;
            }
            else if (_homeState == HomeState.FromSensorRecording)
            {
                MessageBoxResult result = MessageBox.Show(GUI.stopReccordBeforeFromFile, GUI.Atention, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                SceneInUse.Instance.Set(null);
                ChangeHomeState(HomeState.FromFile);
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
                processCheck.Checked += (sender, e) =>
                {
                    process.IsEnabled = true;

                };
                processCheck.Unchecked += (sender, e) =>
                {
                    process.IsEnabled = false;
                };
                processMenuItem.Items.Add(processCheck);
                foreach (var window in process.Windows)
                {
                    MenuItem processWin = new MenuItem();
                    processWin.Header = window.Item1;
                    processWin.Click += (sender, e) => {
                        window.Item2.GetWindow().Show();
                    };
                    processMenuItem.Items.Add(processWin);
                }
                MenuItems_Tools_Processing.Items.Add(processMenuItem);
            }
        }

        private void FillMenuInputModules()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
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
                        window.Item2.GetWindow().Show();
                    };
                    inputMenuItem.Items.Add(inputWin);
                }
                MenuItems_Tools_Sensors.Items.Add(inputMenuItem);
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
        FromSensorRecording,
        FromFile,
        FromFileWithScene
    }
}
