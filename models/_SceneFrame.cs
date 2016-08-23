using System;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class _SceneFrame
    {
        public int SceneFrameId { get; set; }
        public TimeSpan Time { get; set; }
        public _SceneFrame() {}
        public _SceneFrame(TimeSpan frameTime)
        {
            this.Time = frameTime;
        }
    }
}