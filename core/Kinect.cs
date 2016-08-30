using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.core
{
    public class Kinect
    {
        private static Kinect _instance;
        private static KinectSensor _sensor;

        public List<GestureDetector> gestureDetectorList { get; private set; }
        public Monitor Monitor;

        public Recorder Recorder { get; private set; }
        public Player Player { get; private set; }

        private Kinect()
        {
            Monitor = new Monitor();
            Recorder = new Recorder();
            Player = new Player();
            gestureDetectorList = new List<GestureDetector>();
            for (int i = 0; i < _sensor.BodyFrameSource.BodyCount; ++i)
            {
                GestureDetector detector = new GestureDetector(i, _sensor/*, result*/);
                this.gestureDetectorList.Add(detector);
            }
        }
        
        public static Kinect Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Kinect();
                }
                return _instance;
            }
        }
        public static KinectSensor Sensor
        {
            get
            {
                if (_sensor == null || !_sensor.IsOpen)
                {
                    _sensor = KinectSensor.GetDefault();
                    _sensor.Open();
                }
                return _sensor;
            }
        }
     
    }
}
