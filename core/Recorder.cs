using KinectEx;
using KinectEx.DVR;
using KinectEx.Smoothing;
using Microsoft.Kinect;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.core
{
    public class Recorder
    {
        private KinectSensor _sensor;
        public Recorder()
        {
            this._sensor = Kinect.Sensor;
        }
        KinectRecorder _recorder = null;

        public async void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (_recorder == null)
            {
                var dlg = new SaveFileDialog()
                {
                    FileName = DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss"),
                    DefaultExt = ".kdvr",
                    Filter = "KinectEx.DVR Files (*.kdvr)|*.kdvr"
                };

                if (dlg.ShowDialog().GetValueOrDefault())
                {
                    _recorder = new KinectRecorder(File.Open(dlg.FileName, FileMode.Create), _sensor);
                    _recorder.EnableBodyRecorder = true;
                    _recorder.EnableColorRecorder = true;
                    _recorder.EnableDepthRecorder = false;
                    _recorder.EnableInfraredRecorder = false;

                    // NOTE : Default ColorRecorderCodec is Raw @ 1920 x 1080. Only need to change the
                    //        bits that differ from the default.

                    _recorder.ColorRecorderCodec = new JpegColorCodec();
                    _recorder.ColorRecorderCodec.OutputWidth = 1280;
                    _recorder.ColorRecorderCodec.OutputHeight = 720;

                    //if (colorCompressionType == 1)
                    //{
                    //    _recorder.ColorRecorderCodec = new JpegColorCodec();
                    //}
                    //if (colorCompressionSize == 1) // 1280 x 720
                    //{
                    //    _recorder.ColorRecorderCodec.OutputWidth = 1280;
                    //    _recorder.ColorRecorderCodec.OutputHeight = 720;
                    //}
                    //else if (colorCompressionSize == 2) // 640 x 360
                    //{
                    //    _recorder.ColorRecorderCodec.OutputWidth = 640;
                    //    _recorder.ColorRecorderCodec.OutputHeight = 360;
                    //}

                    _recorder.Start();

                    //RecordButton.Content = "Stop Recording";
                    //BodyCheckBox.IsEnabled = false;
                    //ColorCheckBox.IsEnabled = false;
                    //DepthCheckBox.IsEnabled = false;
                    //ColorCompressionCombo.IsEnabled = false;
                }
            }
            else
            {
                //RecordButton.IsEnabled = false;

                await _recorder.StopAsync();
                _recorder = null;

                //RecordButton.Content = "Record";
                //RecordButton.IsEnabled = true;
                //BodyCheckBox.IsEnabled = true;
                //ColorCheckBox.IsEnabled = true;
                //DepthCheckBox.IsEnabled = true;
                //ColorCompressionCombo.IsEnabled = true;
            }
        }
    }
}
