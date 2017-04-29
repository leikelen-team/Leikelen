//using System.ComponentModel.Composition;
using cl.uv.leikelen.src.Data;
//using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Model.AccessLogic;
using KinectEx.DVR;
using Microsoft.Kinect;
using System;
using cl.uv.leikelen.src.Module;
using cl.uv.leikelen.src.View.Procedural;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using cl.uv.leikelen.src.API;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.kinectmedia
{
    public class Recorder : IRecorder
    {
        private KinectSensor _sensor;
        private KinectRecorder _recorder = null;

        public Recorder()
        {
            this._sensor = KinectMediaFacade.Sensor;
            //StaticScene.Instance.RecordStartDate = DateTime.Now;
        }

        public bool IsRecording()
        {
            return _recorder != null;
        }

        public TimeSpan? getLocation()
        {
            if (_recorder != null) return DateTime.Now.Subtract(StaticScene.Instance.RecordStartDate);
            else return null;
        }

        public async Task Stop()
        {
            if(_recorder != null)
            {
                await _recorder.StopAsync();
                _recorder = null;
                
                KinectMediaFacade.Instance.Player.OpenFile(Properties.Paths.CurrentKdvrFile);

                StaticScene.Instance.Duration = KinectMediaFacade.Instance.Player.Duration;
                TimeLine.InitTimeLine(StaticScene.Instance.Duration);
                
            }
        }

        public void Record()
        {
            if (_recorder == null)
            {
                
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
