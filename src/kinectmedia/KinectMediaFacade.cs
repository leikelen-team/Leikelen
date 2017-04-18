using Microsoft.Kinect;
using System;
using System.Collections.Generic;

namespace cl.uv.leikelen.src.kinectmedia
{
    /// <summary>
    /// Class implementing Facade Design Pattern
    /// Works as an interface to kinect data, showing 3 classes:
    /// Monitor, Recorder and Player
    /// </summary>
    public class KinectMediaFacade
    {
        private static KinectMediaFacade _instance;
        private static KinectSensor _sensor;

        public Monitor Monitor { get; private set; }
        public Recorder Recorder { get; private set; }
        public Player Player { get; private set; }

        /// <summary>
        /// Private Constructor with 0 parameters. It Should be used Through Instance.
        /// </summary>
        private KinectMediaFacade()
        {
            Monitor = new Monitor();
            Recorder = new Recorder();
            Player = new Player();
        }
        
        /// <summary>
        /// Singleton Instance of Kinect Media Facade
        /// </summary>
        public static KinectMediaFacade Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KinectMediaFacade();
                }
                return _instance;
            }
        }
        /// <summary>
        /// Kinect Sensor Object Instance
        /// </summary>
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
