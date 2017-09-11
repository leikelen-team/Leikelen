using Microsoft.Kinect.VisualGestureBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.Kinect
{
    public class KinectGestureFrameArrivedArgs
    {
        public TimeSpan? Time;
        public ulong TrackingId;
        public VisualGestureBuilderFrameArrivedEventArgs VisualGestureBuilderFrameArrivedEventArgs;
    }
}
