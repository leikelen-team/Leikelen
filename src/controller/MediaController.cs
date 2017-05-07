using cl.uv.leikelen.src.Input.Kinect;

namespace cl.uv.leikelen.src.Controller
{
    public static class MediaView
    {
        public static SourceType Source { get; private set; }
        
        static MediaView()
        {
            Source = SourceType.Sensor;
        }

        public static void SetFromSensor()
        {
            Source = SourceType.Sensor;
            KinectMediaFacade.Instance.Player.Disable();
            KinectMediaFacade.Instance.Monitor.Open();
            MainWindow.Instance().BackgroundEnableCheckBox.IsEnabled = false;
            MainWindow.Instance().SkeletonsEnableCheckBox.IsEnabled = false;
        }
        public static void SetFromScene()
        {
            Source = SourceType.Scene;
            KinectMediaFacade.Instance.Monitor.Close();
            KinectMediaFacade.Instance.Player.Enable();
            MainWindow.Instance().BackgroundEnableCheckBox.IsEnabled = true;
            MainWindow.Instance().SkeletonsEnableCheckBox.IsEnabled = true;
        }
    }

    
}
