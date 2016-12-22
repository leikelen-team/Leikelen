namespace cl.uv.multimodalvisualizer.src.controller
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
            kinectmedia.KinectMediaFacade.Instance.Player.Disable();
            kinectmedia.KinectMediaFacade.Instance.Monitor.Open();
            MainWindow.Instance().BackgroundEnableCheckBox.IsEnabled = false;
            MainWindow.Instance().SkeletonsEnableCheckBox.IsEnabled = false;
        }
        public static void SetFromScene()
        {
            Source = SourceType.Scene;
            kinectmedia.KinectMediaFacade.Instance.Monitor.Close();
            kinectmedia.KinectMediaFacade.Instance.Player.Enable();
            MainWindow.Instance().BackgroundEnableCheckBox.IsEnabled = true;
            MainWindow.Instance().SkeletonsEnableCheckBox.IsEnabled = true;
        }
    }

    
}
