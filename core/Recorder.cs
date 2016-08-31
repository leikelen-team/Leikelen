using KinectEx;
using KinectEx.DVR;
using KinectEx.Smoothing;
using Microsoft.Kinect;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.db;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.core
{
    public class Recorder
    {
        private KinectSensor _sensor;
        private KinectRecorder _recorder = null;

        public Recorder()
        {
            this._sensor = Kinect.Sensor;
        }

        public bool IsRecording
        {
            get
            {
                return _recorder != null;
            }
        }
        //public string Path { get; private set; }


        public TimeSpan getCurrentLocation()
        {
            if (_recorder != null) return DateTime.Now.Subtract(Scene.Instance.StartDate);
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

                //File.Copy(Properties.Paths.CurrentKdvrFile, Properties.Paths.CurrentReplayKdvrFile);
                Kinect.Instance.Player.OpenFile(Properties.Paths.CurrentKdvrFile);

                Scene.Instance.Duration = Kinect.Instance.Player.Duration;
                Scene.Instance.InitTimeLine();

                foreach (Person person in Scene.Instance.Persons)
                {
                    if (!person.HasBeenTracked) continue;
                    person.generatePostureIntervals();
                    person.generateView();
                    person.View.repaintIntervalGroups();
                    MainWindow.Instance().timeLineContentGrid.Children.Add(person.View.postureGroupsGrid);
                }
                
                var db = BackupDataContext.CreateConnection(Properties.Paths.CurrentDataFile);
                db.Database.EnsureCreated();
                db.Scene.Add(Scene.Instance);
                db.SaveChanges();

                MainWindow.Instance().FromSceneRadioButton.IsChecked = true;
            }
        }

        public async void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (_recorder == null)
            {
                
                if (Scene.Instance != null)
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

                MainWindow.Instance().FromSensorRadioButton.IsChecked = true;

                //if (File.Exists(Properties.Paths.asd)) File.Delete(Properties.Paths.asd);
                if (Kinect.Instance.Player.IsOpen) Kinect.Instance.Player.Close();
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

                //Form testDialog = new Form();
                
                //if (testDialog.ShowDialog() == DialogResult.OK)
                //{
                //    // Read the contents of testDialog's TextBox.
                //    sceneName = testDialog.Text;
                //}
                //else
                //{
                //    sceneName = "Cancelled";
                //}
                //testDialog.Dispose();

                Scene.CreateFromRecord(sceneName);
                    
                _recorder.Start();
                    
            }
        }
    }
}
