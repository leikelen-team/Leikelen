using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal
{
    class _Frame
    {
        public enum FrameType { Color, Body };

        private BitmapSource bitmap;
        private string path;
        private TimeSpan time;
        
        // time diff interval between previous frame and this frame.
        public TimeSpan timeWithPrevFrame { get; private set; }

        // color or body
        private FrameType type;


        public _Frame(BitmapSource bitmap, string path, TimeSpan time, TimeSpan timeWithPrevFrame, FrameType type)
        {
            this.Bitmap = bitmap;
            this.Path = path;
            this.Time = time;
            this.Type = type;
            this.timeWithPrevFrame = timeWithPrevFrame;
        }

        public BitmapSource Bitmap
        {
            get { return bitmap; }
            set { bitmap = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        public TimeSpan Time
        {
            get { return time; }
            set { time = value; }
        }
        public FrameType Type
        {
            get { return type; }
            private set { type = value; }
        }
    }
}
