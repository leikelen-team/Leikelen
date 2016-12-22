using KinectEx;
using KinectEx.DVR;
using KinectEx.Smoothing;
using Microsoft.Kinect;
using cl.uv.multimodalvisualizer.src.dbcontext;
using cl.uv.multimodalvisualizer.src.model;
using cl.uv.multimodalvisualizer.src.view;
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

namespace cl.uv.multimodalvisualizer.src.kinectmedia
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
                MainWindow.Instance().recordButton2.Background = System.Windows.Media.Brushes.White;
                KinectMediaFacade.Instance.Player.OpenFile(Properties.Paths.CurrentKdvrFile);

                Scene.Instance.Duration = KinectMediaFacade.Instance.Player.Duration;
                Scene.Instance.InitTimeLine();

                foreach (Person person in Scene.Instance.Persons)
                {
                    if (!person.HasBeenTracked) continue;
                    person.generatePostureIntervals();
                    person.generateView();
                    person.View.repaintIntervalGroups();
                    MainWindow.Instance().timeLineContentGrid.Children.Add(person.View.postureGroupsGrid);
                }

                Dictionary<ulong, DistanceTypeList> totalDistanceWithInferred = Scene.Instance.calculateDistances.calculateTotalDistance(DistanceInferred.WithInferred);
                Dictionary<ulong, DistanceTypeList> totalDistanceWithoutInferred = Scene.Instance.calculateDistances.calculateTotalDistance(DistanceInferred.WithoutInferred);

                Dictionary<ulong, DistanceTypeList> totalDistanceOnlyInferred = Scene.Instance.calculateDistances.calculateTotalDistance(DistanceInferred.OnlyInferred);

                putDistanceInPersons(totalDistanceWithInferred);
                putDistanceInPersons(totalDistanceWithoutInferred);
                putDistanceInPersons(totalDistanceOnlyInferred);


                Dictionary<TimeSpan, Dictionary<ulong, DistanceTypeList>> intervalDistWithInferred = Scene.Instance.calculateDistances.calculateTotalDistanceIntervals(DistanceInferred.WithInferred, 20);
                Dictionary<TimeSpan, Dictionary<ulong, DistanceTypeList>> intervalDistWithoutInferred = Scene.Instance.calculateDistances.calculateTotalDistanceIntervals(DistanceInferred.WithoutInferred, 20);
                Dictionary<TimeSpan, Dictionary<ulong, DistanceTypeList>> intervalDistOnlyInferred = Scene.Instance.calculateDistances.calculateTotalDistanceIntervals(DistanceInferred.OnlyInferred, 20);

                putIntervalDistanceInPersons(intervalDistWithInferred);
                putIntervalDistanceInPersons(intervalDistWithoutInferred);
                putIntervalDistanceInPersons(intervalDistOnlyInferred);

                foreach(Person person in Scene.Instance.Persons)
                {
                    person.generateDistanceSum();
                    person.generateIntervalDistancesSum();
                }
                
                MainWindow.Instance().SourceComboBox.SelectedIndex = 1;
            }
        }

        public void putDistanceInPersons(Dictionary<ulong, DistanceTypeList> totalDistanceObj)
        {
            foreach (ulong personTrackingId in totalDistanceObj.Keys)
            {
                if (Scene.Instance.Persons.Exists(p => p.TrackingId == (long)personTrackingId))
                {
                    Person person = Scene.Instance.Persons.Find(p => p.TrackingId == (long)personTrackingId);
                    person.Distances.AddRange(totalDistanceObj[personTrackingId]);
                }
            }
        }


        public void putIntervalDistanceInPersons(Dictionary<TimeSpan, Dictionary<ulong, DistanceTypeList>> intervalDistObj)
        {
            foreach (TimeSpan startTime in intervalDistObj.Keys)
            {
                foreach (ulong personTrackingId in intervalDistObj[startTime].Keys)
                {
                    if (Scene.Instance.Persons.Exists(p => p.TrackingId == (long)personTrackingId))
                    {
                        Person person = Scene.Instance.Persons.Find(p => p.TrackingId == (long)personTrackingId);
                        if (person.IntervalDistances.ContainsKey(startTime))
                        {
                            person.IntervalDistances[startTime].AddRange(intervalDistObj[startTime][personTrackingId]);
                        }
                        else
                        {
                            person.IntervalDistances.Add(startTime, intervalDistObj[startTime][personTrackingId]);
                        }
                        
                    }
                }
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

                MainWindow.Instance().SourceComboBox.SelectedIndex = 0;
                MainWindow.Instance().recordButton2.Background = System.Windows.Media.Brushes.Red;
                
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

                Scene.CreateFromRecord(sceneName);
                    
                _recorder.Start();
                    
            }
        }
    }
}
