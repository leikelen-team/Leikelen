using KinectEx;
using KinectEx.DVR;
using KinectEx.Smoothing;
using Microsoft.Kinect;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.db;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



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

                Kinect.Instance.Player.OpenFile(Properties.Paths.RecordedKdvrFilePath);

                Scene.Instance.Duration = Kinect.Instance.Player.Duration;
                Scene.Instance.InitTimeLine();

                foreach (Person person in Scene.Instance.Persons)
                {
                    if (!person.HasBeenTracked) continue;
                    person.generatePostureIntervals();
                    person.View.repaintIntervalGroups();
                    MainWindow.Instance().timeLineContentGrid.Children.Add(person.View.postureGroupsGrid);
                }
                
                var db = BackupDataContext.CreateConnection(Properties.Paths.RecordedSceneDataFile);
                db.Database.EnsureCreated();
                db.Scene.Add(Scene.Instance);
                db.SaveChanges();
                
            }
        }

        public async void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (_recorder == null)
            {
                //var dlg = new SaveFileDialog()
                //{
                //    FileName = DateTime.Now.ToString("yyyy-MM-dd _ hh-mm-ss"),
                //    DefaultExt = ".kdvr",
                //    Filter = "KinectEx.DVR Files (*.kdvr)|*.kdvr"
                //};

                //if (dlg.ShowDialog().GetValueOrDefault())
                //{
                //Path = dlg.FileName;

                if (File.Exists(Properties.Paths.RecordedKdvrFilePath)) File.Delete(Properties.Paths.RecordedKdvrFilePath);
                if (File.Exists(Properties.Paths.RecordedSceneDataFile)) File.Delete(Properties.Paths.RecordedSceneDataFile);

                _recorder = new KinectRecorder(File.Open(Properties.Paths.RecordedKdvrFilePath, FileMode.Create), _sensor);
                _recorder.EnableBodyRecorder = true;
                _recorder.EnableColorRecorder = true;
                _recorder.EnableDepthRecorder = false;
                _recorder.EnableInfraredRecorder = false;

                _recorder.ColorRecorderCodec = new JpegColorCodec();
                _recorder.ColorRecorderCodec.OutputWidth = 1280;
                _recorder.ColorRecorderCodec.OutputHeight = 720;

                Scene.CreateFromRecord();
                    
                _recorder.Start();
                    
                //}
            }
            
        }
    }
}
