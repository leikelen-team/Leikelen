//using System.ComponentModel.Composition;
using cl.uv.leikelen.src.Data;
using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Model.AccessLogic;
using KinectEx.DVR;
using Microsoft.Kinect;
using System;
using cl.uv.leikelen.src.Module;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace cl.uv.leikelen.src.kinectmedia
{
    public class Recorder
    {
        private KinectSensor _sensor;
        private KinectRecorder _recorder = null;

        public Recorder()
        {
            this._sensor = KinectMediaFacade.Sensor;
        }

        public bool IsRecording
        {
            get
            {
                return _recorder != null;
            }
        }

        public TimeSpan getCurrentLocation()
        {
            if (_recorder != null) return DateTime.Now.Subtract(StaticScene.Instance.RecordStartDate);
            else
            {
                throw new Exception("_recorder null");
            }
        }

        

        public async void StopRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if(_recorder != null)
            {
                await _recorder.StopAsync();
                _recorder = null;
                MainWindow.Instance().recordButton.Background = System.Windows.Media.Brushes.White;
                KinectMediaFacade.Instance.Player.OpenFile(Properties.Paths.CurrentKdvrFile);

                StaticScene.Instance.Duration = KinectMediaFacade.Instance.Player.Duration;
                MainWindow.Instance().InitTimeLine(StaticScene.Instance.Duration);

                foreach (var module in Loader.Modules)
                {
                    if(module.FunctionAfterStop() != null)
                    {
                        module.FunctionAfterStop();
                    }
                }

                foreach(var personInScene in StaticScene.Instance.PersonsInScene)
                {
                    Person person = personInScene.Person;
                    if(!StaticScene.personsView.ContainsKey(person))
                    {
                        StaticScene.personsView[person] = new View.Classes.PersonView(personInScene, (int)(StaticScene.Instance.Duration.TotalSeconds));
                    }
                    StaticScene.personsView[person].repaintIntervalGroups();
                    MainWindow.Instance().timeLineContentGrid.Children.Add(StaticScene.personsView[person].postureGroupsGrid);
                }
                
                MainWindow.Instance().SourceComboBox.SelectedIndex = 1;
            }
        }

        public async void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (_recorder == null)
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

                MainWindow.Instance().SourceComboBox.SelectedIndex = 0;
                MainWindow.Instance().recordButton.Background = System.Windows.Media.Brushes.Red;
                
                if (KinectMediaFacade.Instance.Player.IsOpen) KinectMediaFacade.Instance.Player.Close();
                if (File.Exists(Properties.Paths.CurrentKdvrFile)) File.Delete(Properties.Paths.CurrentKdvrFile);
                if (File.Exists(Properties.Paths.CurrentDataFile)) File.Delete(Properties.Paths.CurrentDataFile);
                

                _recorder = new KinectRecorder(File.Open(Properties.Paths.CurrentKdvrFile, FileMode.Create), _sensor);
                _recorder.EnableBodyRecorder = true;
                _recorder.EnableColorRecorder = true;
                _recorder.EnableDepthRecorder = false;
                _recorder.EnableInfraredRecorder = false;

                _recorder.ColorRecorderCodec = new JpegColorCodec();
                _recorder.ColorRecorderCodec.OutputWidth = 1280;
                _recorder.ColorRecorderCodec.OutputHeight = 720;

                string sceneName = DateTime.Now.ToString("yyyy-MM-dd _ hh-mm-ss");

                StaticScene.CreateSceneFromRecord(sceneName);
                    
                _recorder.Start();
                    
            }
        }
    }
}
