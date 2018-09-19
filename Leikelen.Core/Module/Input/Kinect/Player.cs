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
    /// <summary>
    /// Player class of kinect sensor
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Input.IPlayer" />
    public class Player : IPlayer
    {
        private KinectReplay _replay;
        private API.DataAccess.IDataAccessFacade _dataAccessFacade = new DataAccessFacade();
        
        void API.Module.Input.IPlayer.Unpause()
        {
            if (!ReferenceEquals(null, _replay))
                _replay.Start();
        }
        
        void API.Module.Input.IPlayer.Play()
        {
            if (ReferenceEquals(null, _replay) &&
                !ReferenceEquals(null, _dataAccessFacade.GetSceneInUseAccess().GetScene()))
            {
                try
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
                catch (Exception)
                {
                    Close();
                }
            }
            else
            {
                _replay.Start();
            }
        }
        
        void API.Module.Input.IPlayer.Pause()
        {
            if (!ReferenceEquals(null, _replay))
                _replay.Stop();
        }

        void API.Module.Input.IPlayer.Stop()
        {
            if (!ReferenceEquals(null, _replay))
            {
                _replay.Stop();
                _replay.ScrubTo(new TimeSpan(0));
            }
            Close();
        }

        void API.Module.Input.IPlayer.ChangeTime(TimeSpan newTime)
        {
            if (!ReferenceEquals(null, _replay))
                _replay.ScrubTo(newTime);
        }
        
        bool API.Module.Input.IPlayer.IsPlaying()
        {
            return !ReferenceEquals(null, _replay);
        }

        /// <summary>
        /// Closes the data file.
        /// </summary>
        public void Close()
        {
            if (!ReferenceEquals(null, _replay))
            {
                _replay.Stop();
                _replay.Dispose();
                _replay = null;
            }
        }


        TimeSpan? API.Module.Input.IPlayer.GetTotalDuration()
        {
            if(!ReferenceEquals(null, _replay))
                return _replay.Duration;
            return null;
        }
        
        TimeSpan? API.Module.Input.IPlayer.GetLocation()
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

        /// <summary>
        /// Occurs when playback [finished].
        /// </summary>
        public event EventHandler Finished;
        /// <summary>
        /// Occurs when playback's [location changed].
        /// </summary>
        public event EventHandler LocationChanged;
    }
}
