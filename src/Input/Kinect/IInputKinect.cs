using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace cl.uv.leikelen.src.Input.Kinect
{
    interface IInputKinect
    {
        EventHandler<BodyFrameArrivedEventArgs> BodyListener();
        EventHandler<ColorFrameArrivedEventArgs> ColorListener();
        EventHandler<AudioBeamFrameArrivedEventArgs> AudioListener();
    }
}
