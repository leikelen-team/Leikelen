using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.core;
namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.views
{
    public static class MediaView
    {
        public enum SourceType { Sensor, Scene };

        public static SourceType Source { get; private set; }
        
        static MediaView()
        {
            Source = SourceType.Sensor;
        }

        public static void SetFromSensor()
        {
            Source = SourceType.Sensor;
            core.Kinect.Instance.Player.Disable();
            core.Kinect.Instance.Monitor.Open();
            MainWindow.Instance().BackgroundEnableCheckBox.IsEnabled = false;
            MainWindow.Instance().SkeletonsEnableCheckBox.IsEnabled = false;
        }
        public static void SetFromScene()
        {
            Source = SourceType.Scene;
            core.Kinect.Instance.Monitor.Close();
            core.Kinect.Instance.Player.Enable();
            MainWindow.Instance().BackgroundEnableCheckBox.IsEnabled = true;
            MainWindow.Instance().SkeletonsEnableCheckBox.IsEnabled = true;
        }
    }

    
}
