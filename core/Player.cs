using KinectEx;
using KinectEx.DVR;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.db;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.core
{
    public class Player
    {
        private KinectReplay _replay;
        bool _locationSetByHand = false;
        private int StartFromMillis = 200;

        //FrameTypes _displayType = FrameTypes.Body;

        ColorFrameBitmap _colorBitmap = null;
        //DepthFrameBitmap _depthBitmap = null;
        //InfraredFrameBitmap _infraredBitmap = null;

        public Player()
        {

        }

        public TimeSpan Duration
        {
            get
            {
                return _replay.Duration;
            }
        }

        public bool IsOpened
        {
            get
            {
                return _replay!=null;
            }
        }

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
            MainWindow.Instance().sceneDurationLabel.Content = _replay.Duration.ToString(@"hh\:mm\:ss");
            this.sendToStartLocation();
            //_replay.Start();
            //Thread.Sleep(150);
            //_replay.Stop();

        }
        private void sendToStartLocation()
        {
            _replay.ScrubTo(TimeSpan.FromMilliseconds(StartFromMillis));
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
        

        public void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_replay == null)
                return;
            
            if (_replay.IsStarted)
            {
                _replay.Stop();
            }
            MainWindow.Instance().playButton2.Content = Properties.Buttons.StartPlaying;
            this.sendToStartLocation();

        }

        public void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_replay == null)
                return;

            if (!_replay.IsStarted)
            {
                _replay.Start();
                MainWindow.Instance().playButton2.Content = Properties.Buttons.PausePlaying;
            }
            else
            {
                _replay.Stop();
                //PlayButton.Content = "Play";
                MainWindow.Instance().playButton2.Content = Properties.Buttons.StartPlaying;
            }
        }

        private int lastCurrentSecondForTimeLineCursor = 0;

        public void LocationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_locationSetByHand)
            {
                if (_replay != null)
                    _replay.ScrubTo(TimeSpan.FromMilliseconds((MainWindow.Instance().sceneSlider.Value / 100.0) * _replay.Duration.TotalMilliseconds));
            }
            else
            {
                _locationSetByHand = true;
            }

            int currentSecond = (int)_replay.Location.TotalSeconds;

            if (lastCurrentSecondForTimeLineCursor != currentSecond)
            {
                Grid.SetColumn(MainWindow.Instance().lineCurrentTimeCursor, currentSecond); // 1seg = 1col
                Grid.SetColumn(MainWindow.Instance().lineCurrentTimeRulerCursor, currentSecond); // 1seg = 1col
                lastCurrentSecondForTimeLineCursor = currentSecond;
                MainWindow.Instance().sceneCurrentTimeLabel.Content = _replay.Location.ToString(@"hh\:mm\:ss");
            }
            
        }

        private void _replay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == KinectReplay.IsFinishedPropertyName)
            {
                MainWindow.Instance().playButton2.Content = Properties.Buttons.StartPlaying;
            }
            else if (e.PropertyName == KinectReplay.LocationPropertyName)
            {
                _locationSetByHand = false;
                MainWindow.Instance().sceneSlider.Value = 100 - (100 * ((_replay.Duration.TotalMilliseconds - _replay.Location.TotalMilliseconds) / _replay.Duration.TotalMilliseconds));
            }
        }

        private void _replay_BodyFrameArrived(object sender, ReplayFrameArrivedEventArgs<ReplayBodyFrame> e)
        {
            MainWindow.Instance().bodyImageControl.Source = e.Frame.Bodies.GetBitmap(Colors.LightGreen, Colors.Yellow);
        }

        
        private bool _colorFrameEnable = true;
        private bool _justColorFrameEnabled = true;

        public void ToggleColorFrameEnable()
        {
            _colorFrameEnable = !_colorFrameEnable;

            if (_colorFrameEnable)
            {
                
                _justColorFrameEnabled = true;
                MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
            }
            else
            {
                MainWindow.Instance().colorImageControl.Source = null;
            }


        }

        //public bool ColorFrameEnable {
        //    get
        //    {
        //        return _colorFrameEnable;
        //    }
        //    set
        //    {
        //        _colorFrameEnable = value;
        //        if (_colorFrameEnable)
        //        {
        //            _replay.ColorFrameArrived += _replay_ColorFrameArrived;
        //            if(_colorBitmap!= null)
        //            {
        //                MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
        //            }
        //        }
        //        else
        //            _replay.ColorFrameArrived -= _replay_ColorFrameArrived;
        //    }
        //}


        private void _replay_ColorFrameArrived(object sender, ReplayFrameArrivedEventArgs<ReplayColorFrame> e)
        {
            if (_colorBitmap == null || _justColorFrameEnabled)
            {
                _colorBitmap = new ColorFrameBitmap(e.Frame);
                MainWindow.Instance().colorImageControl.Source = _colorBitmap.Bitmap;
                _justColorFrameEnabled = false;
            }
            if(_colorFrameEnable) _colorBitmap.Update(e.Frame);
            
        }


    }
}
