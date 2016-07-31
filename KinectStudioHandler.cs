using Microsoft.Kinect.Tools;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal
{

    public class KinectStudioHandler
    {
        private KStudioClient client = KStudio.CreateClient();
        public KStudioPlayback playback { get; private set; }
        //private BackgroundWorker playerWorker;
        private BackgroundWorker recWorker;
        private string recordingFilePath;
        private string playingFilePath;
        private DateTime? recordStartTime = null;
        private KStudioRecording recorder;
        public int PausedStartMillisTime { get; private set; } = 150;

        public List<TimeSpan> frameTimes { get; private set; }



        //public bool isPlaying { get; private set; }
        //public bool isRecording { get; private set; }
        public bool isSceneImportedOrRecorded { get; private set; }
        public KinectStudioHandler()
        {
            this.isSceneImportedOrRecorded = false;
            this.recordingFilePath = Properties.Paths.tmpScene;
            this.playingFilePath = Properties.Paths.tmpScene;
            this.client = KStudio.CreateClient();
            this.client.ConnectToService();
            //isPlaying = false;
            //playerWorker = new BackgroundWorker();
            //playerWorker.WorkerReportsProgress = false;
            //playerWorker.WorkerSupportsCancellation = true;
            //playerWorker.DoWork += new DoWorkEventHandler(this.bw_DoWork_StartPlaying);

            recWorker = new BackgroundWorker();
            recWorker.WorkerReportsProgress = false;
            recWorker.WorkerSupportsCancellation = true;
            recWorker.DoWork += new DoWorkEventHandler(this.bw_DoWork_StartRecording);

            

            //streamCollection.Add(KStudioEventStreamDataTypeIds.Depth);
            
        }
        public TimeSpan? getSceneLocationTime()
        {
            if (playback != null && playback.State == KStudioPlaybackState.Playing) return playback.CurrentRelativeTime;
            else if (recorder != null
                && recorder.State == KStudioRecordingState.Recording
                && recordStartTime != null) return DateTime.Now.Subtract(recordStartTime.Value);
            else return null;
        }
        public void ExportScene(string path)
        {
            // copy file recordingFilePath to path

            string old_playingFilePath = this.playingFilePath;
            File.Copy(this.playingFilePath, path);
            ImportScene(path);

        }
        public void ImportScene(string path)
        {
            this.client.DisconnectFromService();
            this.client.ConnectToService();

            this.playingFilePath = path;
            this.isSceneImportedOrRecorded = true;
            this.playback = this.client.CreatePlayback(this.playingFilePath);
            KStudioFileInfo fileInfo = this.client.GetFileList(this.playingFilePath).First();

            Console.WriteLine("Scene Imported:");
            Console.WriteLine("File Name: " + fileInfo.FilePath);
            Console.WriteLine("Size: " + fileInfo.Size);
            Console.WriteLine("Creation Date: " + fileInfo.CreationUtcFileTime);
            Console.WriteLine("Duration: " + playback.Duration);
            Console.WriteLine("Path: " + playback.FilePath);
            
            if ( Scene.Instance != null)
            {
                UIElementCollection childs = MainWindow.Instance().timeRulerGrid.Children;
                for (int i= childs.Count-1; i>=0; i--)
                    if (childs[i] is TextBlock)
                        MainWindow.Instance().timeRulerGrid.Children.RemoveAt(i);
                MainWindow.Instance().timeRulerGrid.ColumnDefinitions.Clear();
                MainWindow.Instance().timeLineContentGrid.ColumnDefinitions.Clear();
                foreach (var person in Scene.Instance.persons)
                    if (person.view != null) { 
                        person.view.postureGroupsGrid.Children.Clear();
                        person.view.postureGroupsGrid.ColumnDefinitions.Clear();
                        person.view.postureGroupsGrid.RowDefinitions.Clear();
                        person.view.combosStackPanel.Children.Clear();
                    }
            }

            Scene.Create(fileInfo.FilePath, fileInfo.CreationUtcFileTime, playback.Duration);
            MainWindow.lastCurrentTime = TimeSpan.FromSeconds(0);
            MainWindow.Instance().sceneSlider.Maximum = playback.Duration.TotalMilliseconds;
            
            this.playback.Start();
            Thread.Sleep(PausedStartMillisTime);
            this.playback.Pause();


            ColumnDefinition rulerCol, contentCol;
            TextBlock text;
            frameTimes = new List<TimeSpan>();
            TimeSpan frameTime = TimeSpan.FromSeconds(0);
            //int currentSeg = 0;
            int colSpan = 10;
            for (int colCount=0; true; colCount++)
            {
                if (frameTime < playback.Duration)
                {
                    frameTimes.Add(frameTime);
                    
                    rulerCol = new ColumnDefinition();
                    rulerCol.Width = new GridLength(5, GridUnitType.Pixel);
                    MainWindow.Instance().timeRulerGrid.ColumnDefinitions.Add(rulerCol);

                    contentCol = new ColumnDefinition();
                    contentCol.Width = new GridLength(5, GridUnitType.Pixel);
                    MainWindow.Instance().timeLineContentGrid.ColumnDefinitions.Add(contentCol);

                    if (colCount % colSpan == 0 && colCount!=0)
                    {
                        text = new TextBlock();
                        text.Text = "|";
                        text.HorizontalAlignment = HorizontalAlignment.Left;
                        Grid.SetRow(text, 0);
                        Grid.SetColumn(text, colCount);
                        Grid.SetColumnSpan(text, colSpan);
                        MainWindow.Instance().timeRulerGrid.Children.Add(text);

                        text = new TextBlock();
                        //text.Text = frameTime.TotalSeconds.ToString("N0");
                        text.Text = frameTime.ToShortForm();
                        text.HorizontalAlignment = colCount == 0 ? HorizontalAlignment.Left : HorizontalAlignment.Center;
                        Grid.SetRow(text, 1);
                        int offset = colCount==0 ? 0 : (colSpan / 2);
                        Grid.SetColumn(text, colCount- offset);
                        Grid.SetColumnSpan(text, colSpan);
                        MainWindow.Instance().timeRulerGrid.Children.Add(text);


                    }
                    else
                    //if (colCount % (colSpan / 2) == 0)
                    {
                        text = new TextBlock();
                        text.Text = "·";
                        text.HorizontalAlignment = HorizontalAlignment.Left;
                        Grid.SetRow(text, 0);
                        Grid.SetColumn(text, colCount);
                        //Grid.SetColumnSpan(text, colSpan/2);
                        MainWindow.Instance().timeRulerGrid.Children.Add(text);
                    }

                    frameTime = frameTime.Add(TimeSpan.FromMilliseconds(1000.00));
                }
                else
                {
                    break;
                }
            }









            //Console.WriteLine("relative time millis timespan: " + playback.StartRelativeTime.ToString());
            //Console.WriteLine("relative time millis double: " + playback.StartRelativeTime.TotalMilliseconds.ToString());
            //int millisStart = (int)this.playback.StartRelativeTime.TotalMilliseconds;
            //Console.WriteLine("relative time millis int: " + millisStart.ToString());
            //Console.WriteLine("----MARKERS---");
            //Console.WriteLine("Markers count: "+ this.playback.Markers.Count);
            //foreach (KStudioMarker marker in this.playback.Markers)
            //{
            //    Console.WriteLine("Marker name: " + marker.Name);
            //    Console.WriteLine("Marker ToString: " + marker.ToString());
            //    Console.WriteLine("Marker RelativeTime: " + marker.RelativeTime.ToString());
            //    Console.WriteLine("--------------");
            //}

            //Console.WriteLine("----PUBLIC METADATA---");
            //KStudioMetadata publicMetadata = this.playback.GetMetadata(KStudioMetadataType.Public);
            //foreach (KeyValuePair<string, object> entry in publicMetadata)
            //{
            //    // do something with entry.Value or entry.Key
            //    Console.WriteLine(entry.Key+": "+entry.Value);
            //}

            //Console.WriteLine("----PRIVATE METADATA---");
            //KStudioMetadata privateMetadata = this.playback.GetMetadata(KStudioMetadataType.PersonallyIdentifiableInformation);
            //foreach (KeyValuePair<string, object> entry in privateMetadata)
            //{
            //    // do something with entry.Value or entry.Key
            //    Console.WriteLine(entry.Key + ": " + entry.Value);
            //}





        }


        //private void importScene()
        //{
        //    //string fileName = string.Empty;

        //    OpenFileDialog dlg = new OpenFileDialog();
        //    dlg.FileName = "";
        //    dlg.DefaultExt = Properties.Resources.XefExtension; // Default file extension
        //    dlg.Filter = Properties.Resources.EventFileDescription + " " + Properties.Resources.EventFileFilter; // Filter files by extension 
        //    bool? result = dlg.ShowDialog();

        //    if (result == true)
        //    {
        //        this.filePath = dlg.FileName;
        //    }
        //    //return this.filePath;
        //}
        //private void exportScene()
        //{
        //    //string fileName = string.Empty;

        //    SaveFileDialog dlg = new SaveFileDialog();
        //    dlg.FileName = "";
        //    dlg.DefaultExt = Properties.Resources.XefExtension; // Default file extension
        //    dlg.Filter = Properties.Resources.EventFileDescription + " " + Properties.Resources.EventFileFilter; // Filter files by extension 
        //    bool? result = dlg.ShowDialog();

        //    if (result == true)
        //    {
        //        this.filePath = dlg.FileName;
        //    }
        //    //return this.filePath;
        //}

        public bool isPlaying
        {
            get
            {
                return playback!=null && playback.State == KStudioPlaybackState.Playing;
            }
        }
        public bool isRecording
        {
            get
            {
                return recorder!=null && recorder.State == KStudioRecordingState.Recording;
            }
        }
        public void StartOrResumePlaying()
        {
            //isPlaying = true;
            if (playback != null && playback.State == KStudioPlaybackState.Paused)
                playback.Resume();
            else playback.Start(); //playerWorker.RunWorkerAsync();
            //else if (playback.State == KStudioPlaybackState.Stopped)
        }
        public void ResumePlaying()
        {
            if (playback != null && playback.State == KStudioPlaybackState.Paused)
                playback.Resume();
        }
        public void PausePlaying()
        {
            
            if(playback!=null && playback.State == KStudioPlaybackState.Playing)
            {
                //isPlaying = false;
                playback.Pause();
            }else if (playback!= null && playback.State == KStudioPlaybackState.Error)
                throw new Exception("Error. Playback failed");
            
            //playerWorker.CancelAsync();
        }
        public void StopPlaying()
        {
            
            if (playback != null && playback.State != KStudioPlaybackState.Stopped)
            {
                playback.Stop();
                //isPlaying = false;

                MainWindow.lastCurrentTime = TimeSpan.FromSeconds(0);
                //this.playback.AddPausePointByRelativeTime(TimeSpan.FromMilliseconds(PausedStartMillisTime));
                this.playback.Start();
                Thread.Sleep(PausedStartMillisTime);
                this.playback.Pause();
            }

            //playerWorker.CancelAsync();
        }
        public void StartRecording()
        {
            //isRecording = true;
            //if( recorder!=null && recorder.State != KStudioRecordingState.Recording)
            //{
            Directory.CreateDirectory(Properties.Paths.tmpDirectory);
            recordStartTime = DateTime.Now;
            recWorker.RunWorkerAsync();
            
            //}
        }

        public void StopRecording()
        {
            if(recorder!=null && recorder.State != KStudioRecordingState.Recording)
            {
                recorder.Stop();
                //isRecording = false;
                recordStartTime = null;
                this.ImportScene(this.recordingFilePath);
                
                //this.client.DisconnectFromService();
                //this.client.ConnectToService();

                //this.playingFilePath = this.recordingFilePath;
                //this.isSceneImportedOrRecorded = true;
                //this.playback = this.client.CreatePlayback(this.playingFilePath);
                //KStudioFileInfo fileInfo = this.client.GetFileList(this.playingFilePath).First();

                //Console.WriteLine("Scene Imported:");
                //Console.WriteLine("File Name: " + fileInfo.FilePath);
                //Console.WriteLine("Size: " + fileInfo.Size);
                //Console.WriteLine("Creation Date: " + fileInfo.CreationUtcFileTime);
                //Console.WriteLine("Duration: " + playback.Duration);
                //Console.WriteLine("Path: " + playback.FilePath);

                //Scene.Create(fileInfo.FilePath, fileInfo.CreationUtcFileTime, playback.Duration);

                //Console.WriteLine("relative time millis timespan: " + playback.StartRelativeTime.ToString());
                //Console.WriteLine("relative time millis double: " + playback.StartRelativeTime.TotalMilliseconds.ToString());

                //int millisStart = (int)this.playback.StartRelativeTime.TotalMilliseconds;
                //Console.WriteLine("relative time millis int: " + millisStart.ToString());

                //this.playback.AddPausePointByRelativeTime(TimeSpan.FromMilliseconds(500));
                //this.playback.Start();



            }
            
                
        }

        private void bw_DoWork_StartRecording(object sender, DoWorkEventArgs e)
        {
            this.client.DisconnectFromService();
            this.client.ConnectToService();

            File.Delete(this.recordingFilePath);

            KStudioEventStreamSelectorCollection streamCollection;
            streamCollection = new KStudioEventStreamSelectorCollection();
            streamCollection.Add(KStudioEventStreamDataTypeIds.CompressedColor);
            streamCollection.Add(KStudioEventStreamDataTypeIds.Body);
            streamCollection.Add(KStudioEventStreamDataTypeIds.BodyIndex);
            recorder = client.CreateRecording(this.recordingFilePath, streamCollection);
            this.isSceneImportedOrRecorded = true;
            recorder.Start();// (TimeSpan.FromSeconds(5));
            while (recorder.State == KStudioRecordingState.Recording)
            {
                
                Thread.Sleep(500);
            }
            

            if (recorder.State == KStudioRecordingState.Error)
            {
                throw new InvalidOperationException("Error: Recording failed!");
            }
            
        }


        //private void bw_DoWork_StartPlaying(object sender, DoWorkEventArgs e)
        //{
        //    //using (this.client)
        //    //{
        //        //client.ConnectToService();

        //        //string filePath = "C:/Users/elrod_000/Documents/Develop/Kinect/Kinect Studio/Repository/20160531_220653_00.xef";
                
                
        //        //using (playback = client.CreatePlayback(filePath))
        //        //{
        //            //playback.LoopCount = loopCount;
        //        playback.Start();


        //        while (playback.State == KStudioPlaybackState.Playing)
        //        {
        //            Thread.Sleep(500);
        //        }

        //        if (playback.State == KStudioPlaybackState.Error)
        //        {
        //            throw new InvalidOperationException("Error: Playback failed!");
        //        }
        //        //}

        //        //client.DisconnectFromService();
        //    //}
        //}
    }
}
