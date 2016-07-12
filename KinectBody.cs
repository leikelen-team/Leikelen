using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal
{
    class KinectBody
    {
        /// <summary> Active Kinect sensor </summary>
        public KinectSensor kinectSensor { get; private set; }

        /// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        public Body[] bodies { get; private set; }

        public MultiSourceFrameReader _reader { get; private set; }


        public KinectBody()
        {
            // only one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();

            // set IsAvailableChanged event notifier
            //this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();
        }
    }
}
