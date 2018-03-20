using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using KinectEx;
using Microsoft.Kinect;
using cl.uv.leikelen.src.API.Input;

namespace cl.uv.leikelen.src.Input.Kinect
{
    public class SkeletonColorVideoViewer : IVideoViewer
    {
        //IVideo related attributes
        private ColorFrameBitmap _colorBitmap;
        private WriteableBitmap _bodyBitmap;
        private List<CustomBody> _bodies = null;

        public void Close()
        {
            _colorBitmap = null;
            _bodyBitmap = null;
        }

        public SkeletonColorVideoViewer()
        {
            _colorBitmap = new ColorFrameBitmap();
            _bodies = new List<CustomBody>();
        }

        public event EventHandler<ImageSource> colorImageArrived;
        public event EventHandler<ImageSource> skeletonImageArrived;

        public void OnColorImageArrived(ImageSource e)
        {
            colorImageArrived?.Invoke(this, e);
        }

        public void OnSkeletonImageArrived(ImageSource e)
        {
            skeletonImageArrived?.Invoke(this, e);
        }


        public void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            IEnumerable<IBody> bodies = null; // to make the GetBitmap call a little cleaner
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    frame.GetAndRefreshBodyData(_bodies);
                    bodies = _bodies;
                }
            }

            if (bodies != null)
            {
                bodies.MapDepthPositions();
                _bodyBitmap = bodies.GetBitmap(Colors.LightGreen, Colors.Yellow);
                OnSkeletonImageArrived(_bodyBitmap);
            }
            else
            {
                _bodyBitmap = null;
            }
        }

        public void _colorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            _colorBitmap.Update(e.FrameReference);
            OnColorImageArrived(_colorBitmap.Bitmap);
        }
    }
}
