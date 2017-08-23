﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace cl.uv.leikelen.API.FrameProvider.Kinect
{
    public interface IKinectProcessingModule
    {
        EventHandler<BodyFrameArrivedEventArgs> BodyListener();
        EventHandler<ColorFrameArrivedEventArgs> ColorListener();
        EventHandler<AudioBeamFrameArrivedEventArgs> AudioListener();
    }
}
