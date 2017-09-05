using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.InputModule;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.InputModule.Kinect
{
    public class KinectInput : API.InputModule.InputModule
    {
        public IVideo SkeletonColorVideoViewer;

        public KinectInput()
        {
            var monitor = new Monitor();
            Name = Properties.Kinect.SensorName;
            Plurality = InputPlurality.Scene;
            Monitor = monitor;
            Player = new Player();
            Windows = new List<Tuple<string, WindowBuilder>>();
            

            SkeletonColorVideoViewer = monitor.VideoViewer;
        }
    }
}
