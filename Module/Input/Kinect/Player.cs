using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.Input;
using KinectEx;
using KinectEx.DVR;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using System.IO;

namespace cl.uv.leikelen.Module.Input.Kinect
{
    public class Player : IPlayer
    {
        private KinectReplay _replay;
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public void Unpause()
        {
            if (!ReferenceEquals(null, _replay))
                _replay.Start();
        }

        public void Play()
        {
            if (ReferenceEquals(null, _replay) &&
                !ReferenceEquals(null, _dataAccessFacade.GetSceneInUseAccess().GetScene()))
            {
                string fileName = _dataAccessFacade.GetGeneralSettings().GetDataDirectory() +
                    "scene/" + _dataAccessFacade.GetSceneInUseAccess().GetScene().SceneId + "/kinect.dvr";
                if (File.Exists(fileName))
                {
                    Console.WriteLine($"archivo {fileName} existe, a abrirlo");
                    _replay = new KinectReplay(File.Open(fileName, FileMode.Open, FileAccess.Read));
                    _replay.PropertyChanged += _replay_PropertyChanged;
                    if (_replay.HasBodyFrames)
                    {
                        _replay.BodyFrameArrived += KinectInput.SkeletonColorVideoViewer._replay_BodyFrameArrived;
                    }
                    if (_replay.HasColorFrames)
                    {
                        _replay.ColorFrameArrived += KinectInput.SkeletonColorVideoViewer._replay_ColorFrameArrived;
                    }

                    _replay.ScrubTo(new TimeSpan(0));
                    _replay.Start();
                }
            }
            else
            {
                _replay.Start();
            }
        }

        public void Pause()
        {
            if (!ReferenceEquals(null, _replay))
                _replay.Stop();
        }

        public void Stop()
        {
            if (!ReferenceEquals(null, _replay))
            {
                _replay.Stop();
                _replay.ScrubTo(new TimeSpan(0));
            }
        }

        public void ChangeTime(TimeSpan newTime)
        {
            if (!ReferenceEquals(null, _replay))
                _replay.ScrubTo(newTime);
        }

        public bool IsPlaying()
        {
            return !ReferenceEquals(null, _replay);
        }

        public void Close()
        {
            if (!ReferenceEquals(null, _replay))
            {
                _replay.Stop();
                _replay.Dispose();
                _replay = null;
            }
        }

        public TimeSpan? GetTotalDuration()
        {
            if(!ReferenceEquals(null, _replay))
                return _replay.Duration;
            return null;
        }

        public TimeSpan? GetLocation()
        {
            if (!ReferenceEquals(null, _replay))
                return _replay.Location;
            return null;
        }

        private void _replay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == KinectReplay.IsFinishedPropertyName)
            {
                Finished?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler Finished;
        public event EventHandler LocationChanged;
    }
}
