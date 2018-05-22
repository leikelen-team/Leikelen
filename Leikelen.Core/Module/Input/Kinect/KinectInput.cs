using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.Input;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.Module.Input.Kinect
{
    /// <summary>
    /// Entry point for the Kinect sensor input module
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Input.InputModule" />
    public class KinectInput : InputModule
    {
        /// <summary>
        /// The skeleton color video viewer
        /// </summary>
        public static SkeletonColorVideoViewer SkeletonColorVideoViewer = new SkeletonColorVideoViewer();

        /// <summary>
        /// Initializes a new instance of the <see cref="KinectInput"/> class.
        /// </summary>
        public KinectInput()
        {
            var monitor = new Monitor();
            Name = "Kinect v2";
            Plurality = InputPlurality.Scene;
            Monitor = monitor;
            Player = new Player();
            Windows = new List<Tuple<string, WindowBuilder>>();
        }
    }
}
