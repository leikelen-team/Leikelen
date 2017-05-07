using cl.uv.leikelen.src.Data;
using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Model.AccessLogic;
using KinectEx;
using KinectEx.DVR;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using cl.uv.leikelen.src.API;

namespace cl.uv.leikelen.src.Input.Kinect
{
    public class Player : IPlayer
    {
        private KinectReplay _replay;
        private bool _locationSetByHand = false;
        private int StartFromMillis = 200;

        private bool colorFrameEnable = true;
        private bool bodyFrameEnable = true;
        private bool viewEnable = true;

        private bool wasPlayingAtDisable = false;
        private ColorFrameBitmap _colorBitmap = null;
        private ImageSource lastBodyFrame = null;

        public event EventHandler Finished;
        public event EventHandler LocationChanged;

        public Player(){}

        public TimeSpan Duration
        {
            get
            {
                return _replay.Duration;
            }
        }

        public bool IsOpen
        {
            get
            {
                return _replay!=null;
            }
        }

        #region Open - Close

        public void OpenFile(string fullPath)
        {
            
            this.Close();
            _replay = new KinectReplay(File.Open(fullPath, FileMode.Open, FileAccess.Read));
            _replay.PropertyChanged += _replay_PropertyChanged;
            if (_replay.HasBodyFrames)
            {
                _replay.BodyFrameArrived += _replay_BodyFrameArrived;
            }
            if (_replay.HasColorFrames)
            {
                _replay.ColorFrameArrived += _replay_ColorFrameArrived;
            }
            StaticScene.Instance.Duration = _replay.Duration;
            this.sendToStartLocation();
        }

        public void Close()
        {
            if (_replay != null)
            {
                if (_replay.IsStarted)
                    _replay.Stop();

                _replay.PropertyChanged -= _replay_PropertyChanged;

                if (_replay.HasBodyFrames)
                    _replay.BodyFrameArrived -= _replay_BodyFrameArrived;
                if (_replay.HasColorFrames)
                    _replay.ColorFrameArrived -= _replay_ColorFrameArrived;
                _replay.Dispose();

                _replay = null;

            }

            _colorBitmap = null; // reset to force recreation for new file
        }

        #endregion

        
        #region Enable Disable Toggle

        public void Enable()
        {
            if (wasPlayingAtDisable)
            {
                _replay.Start();
                wasPlayingAtDisable = false;
            }
            //TODO: revisar estos enable
            //bodyFrameEnable = true;
            //colorFrameEnable = true;
            viewEnable = true;
            if(colorFrameEnable && _colorBitmap!=null) MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
            if(bodyFrameEnable) MainWindow.Instance().bodyImageControl.Source = this.lastBodyFrame;
        }

        public void Disable()
        {
            if (_replay!=null && _replay.IsStarted)
            {
                wasPlayingAtDisable = true;
                _replay.Stop();
            }

            //bodyFrameEnable = false;
            //colorFrameEnable = false;
            viewEnable = false;
            if (bodyFrameEnable) this.lastBodyFrame = MainWindow.Instance().bodyImageControl.Source;

            MainWindow.Instance().colorImageControl.Source = null;
            MainWindow.Instance().bodyImageControl.Source = null;
        }
        
        public void ToggleColorFrameEnable()
        {
            colorFrameEnable = !colorFrameEnable;

            if (colorFrameEnable && _colorBitmap != null)
            {
                MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
            }
            else
            {
                MainWindow.Instance().colorImageControl.Source = null;
            }
        }

        public void ToggleBodyFrameEnable()
        {
            bodyFrameEnable = !bodyFrameEnable;

            if (bodyFrameEnable)
            {
                MainWindow.Instance().bodyImageControl.Source = this.lastBodyFrame;
            }
            else
            {
                this.lastBodyFrame = MainWindow.Instance().bodyImageControl.Source;
                MainWindow.Instance().bodyImageControl.Source = null;
            }
        }

        #endregion

        public TimeSpan? getTotalDuration()
        {
            if (_replay != null)
            {
                return _replay.Duration;
            }
            return null;
        }

        public TimeSpan? getLocation()
        {
            if( _replay != null)
            {
                return _replay.Location;
            }
            return null;
        }

        public void Stop()
        {
            if (_replay != null && _replay.IsStarted)
            {
                _replay.Stop();
            }
            this.sendToStartLocation();
        }

        public void Pause()
        {
            if (_replay != null && _replay.IsStarted)
            {
                _replay.Stop();
            }
        }

        public void Play()
        {
            if (_replay != null && !_replay.IsStarted)
            {
                this.sendToStartLocation();
                _replay.Start();
            }
        }

        public void Unpause()
        {
            if (_replay != null && !_replay.IsStarted)
            {
                _replay.Start();
            }
        }

        public bool IsPlaying()
        {
            return _replay.IsStarted;
        }

        private void sendToStartLocation()
        {
            _replay.ScrubTo(TimeSpan.FromMilliseconds(StartFromMillis));
        }

        public void ChangeTime(TimeSpan newTime)
        {
            if (_locationSetByHand)
            {
                if (_replay != null)
                    _replay.ScrubTo(newTime);
            }
            else
            {
                _locationSetByHand = true;
            }
        }

        private void _replay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == KinectReplay.IsFinishedPropertyName)
            {
                OnFinished(EventArgs.Empty);
            }
            else if (e.PropertyName == KinectReplay.LocationPropertyName)
            {
                OnLocationChange(EventArgs.Empty);
                _locationSetByHand = false;
            }
        }

        private void _replay_BodyFrameArrived(object sender, ReplayFrameArrivedEventArgs<ReplayBodyFrame> e)
        {
            if (!bodyFrameEnable || !viewEnable || StaticScene.Instance==null) return;

            float _width = 512;
            float _height = 424;

            var bitmap = BitmapFactory.New((int)_width, (int)_height);
            foreach (var body in e.Frame.Bodies)
            {
                if (body.IsTracked)
                {
                    Person person = StaticScene.Instance.getPersonInScene(body.TrackingId).Person;
                    var boneColor = (StaticScene.boneColors[person.ListIndex] as SolidColorBrush).Color;
                    var jointColor = (StaticScene.jointColors[person.ListIndex] as SolidColorBrush).Color;
                    body.AddToBitmap(bitmap, boneColor, jointColor);
                }
            }
            MainWindow.Instance().bodyImageControl.Source = bitmap;
        }
        
        private void _replay_ColorFrameArrived(object sender, ReplayFrameArrivedEventArgs<ReplayColorFrame> e)
        {
            if (!colorFrameEnable || !viewEnable) return;
            if (_colorBitmap == null)
            {
                _colorBitmap = new ColorFrameBitmap(e.Frame);
                MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
            }
            _colorBitmap.Update(e.Frame);
            
        }


        protected virtual void OnFinished(EventArgs e)
        {
            Finished?.Invoke(this, e);
        }

        protected virtual void OnLocationChange(EventArgs e)
        {
            LocationChanged?.Invoke(this, e);
        }

    }
}
