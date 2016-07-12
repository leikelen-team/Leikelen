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

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal
{

    class KinectStudioHandler
    {
        private KStudioClient client = KStudio.CreateClient();
        public KStudioPlayback playback { get; private set; }
        //private BackgroundWorker playerWorker;
        private BackgroundWorker recWorker;
        private string recordingFilePath;
        private string playingFilePath;

        private KStudioRecording recorder;
        public int PausedStartMillisTime { get; private set; } = 150;


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

            Scene.Create(fileInfo.FilePath, fileInfo.CreationUtcFileTime, playback.Duration);

            Console.WriteLine("relative time millis timespan: " + playback.StartRelativeTime.ToString());
            Console.WriteLine("relative time millis double: " + playback.StartRelativeTime.TotalMilliseconds.ToString());

            int millisStart = (int)this.playback.StartRelativeTime.TotalMilliseconds;
            Console.WriteLine("relative time millis int: " + millisStart.ToString());

            MainWindow.lastCurrentTime = TimeSpan.FromSeconds(0);
            MainWindow.Instance().sceneSlider.Maximum = playback.Duration.TotalMilliseconds;

            //this.playback.AddPausePointByRelativeTime(TimeSpan.FromMilliseconds(PausedStartMillisTime));
            this.playback.Start();
            
            Thread.Sleep(PausedStartMillisTime);
            this.playback.Pause();




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
            recWorker.RunWorkerAsync();
            //}
        }

        public void StopRecording()
        {
            if(recorder!=null && recorder.State != KStudioRecordingState.Recording)
            {
                recorder.Stop();
                //isRecording = false;
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
