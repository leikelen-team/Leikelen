using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.Input;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.Module.Input.Kinect
{
    public class KinectInput : InputModule
    {
        public IVideo SkeletonColorVideoViewer;

        public KinectInput()
        {
            var monitor = new Monitor();
            Name = "Kinect v2";
            Plurality = InputPlurality.Scene;
            Monitor = monitor;
            Player = new Player();
            Windows = new List<Tuple<string, WindowBuilder>>();
            
            SkeletonColorVideoViewer = monitor.VideoViewer;
        }
    }
}
