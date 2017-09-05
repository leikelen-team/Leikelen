using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using cl.uv.leikelen.API.InputModule;
using Microsoft.Kinect;
using KinectEx;

namespace cl.uv.leikelen.InputModule.Kinect
{
    public class SkeletonColorVideoViewer : IVideo
    {
        public event EventHandler<ImageSource> ColorImageArrived;
        public event EventHandler<ImageSource> SkeletonImageArrived;

        private bool _isColorEnabled;
        private bool _isSkeletonEnabled;

        private readonly ColorFrameBitmap _colorBitmap;
        private WriteableBitmap _bodyBitmap;
        private readonly List<CustomBody> _bodies;

        public SkeletonColorVideoViewer()
        {
            _colorBitmap = new ColorFrameBitmap();
            _bodies = new List<CustomBody>();
        }

        public bool IsColorEnabled()
        {
            return _isColorEnabled;
        }

        public bool IsSkeletonEnabled()
        {
            return _isSkeletonEnabled;
        }

        public void EnableColor()
        {
            _isColorEnabled = true;
        }

        public void EnableSkeleton()
        {
            _isSkeletonEnabled = false;
        }

        public void DisableColor()
        {
            _isColorEnabled = false;
        }

        public void DisableSkeleton()
        {
            _isSkeletonEnabled = false;
        }



        public void OnColorImageArrived(ImageSource e)
        {
            ColorImageArrived?.Invoke(this, e);
        }

        public void OnSkeletonImageArrived(ImageSource e)
        {
            Console.WriteLine("boody en on skeleton image arrived");
            SkeletonImageArrived?.Invoke(this, e);
        }


        public void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            Console.WriteLine("llego body en video handler");
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
            Console.WriteLine("llego color en video handler");
            _colorBitmap.Update(e.FrameReference);
            OnColorImageArrived(_colorBitmap.Bitmap);
        }
    }
}
