using KinectEx;
using KinectEx.DVR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.core
{
    public class Player
    {
        KinectReplay _replay;
        bool _locationSetByHand = false;

        //FrameTypes _displayType = FrameTypes.Body;

        ColorFrameBitmap _colorBitmap = null;
        DepthFrameBitmap _depthBitmap = null;
        InfraredFrameBitmap _infraredBitmap = null;
    }
}
