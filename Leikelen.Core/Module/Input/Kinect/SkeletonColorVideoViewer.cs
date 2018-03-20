using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using cl.uv.leikelen.API.Module.Input;
using Microsoft.Kinect;
using KinectEx;
using KinectEx.DVR;

namespace cl.uv.leikelen.Module.Input.Kinect
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
        public void DisableColor()
        {
            _isColorEnabled = false;
        }

        public void EnableSkeleton()
        {
            _isSkeletonEnabled = true;
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
            SkeletonImageArrived?.Invoke(this, e);
        }


        public void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            if (!_isSkeletonEnabled)
                return;
            IEnumerable<IBody> bodies = null; // to make the GetBitmap call a little cleaner
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (!ReferenceEquals(null, frame))
                {
                    frame.GetAndRefreshBodyData(_bodies);
                    bodies = _bodies;
                }
            }

            if (!ReferenceEquals(null, bodies))
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
            if (!_isColorEnabled)
                return;
            if(!ReferenceEquals(null, e.FrameReference))
            {
                _colorBitmap.Update(e.FrameReference);
                OnColorImageArrived(_colorBitmap.Bitmap);
            }
            
        }

        public void _replay_BodyFrameArrived(object sender, ReplayFrameArrivedEventArgs<ReplayBodyFrame> e)
        {
            if (!_isSkeletonEnabled)
                return;

            if(!ReferenceEquals(null, e.Frame))
            {
                float _width = 512;
                float _height = 424;

                Color color;
                var bitmap = BitmapFactory.New((int)_width, (int)_height);
                foreach (var body in e.Frame.Bodies)
                {
                    if (body.IsTracked)
                    {
                        color = Colors.Blue;
                        body.AddToBitmap(bitmap, color, color);
                    }
                }
                OnSkeletonImageArrived(bitmap);
            }
        }

        public void _replay_ColorFrameArrived(object sender, ReplayFrameArrivedEventArgs<ReplayColorFrame> e)
        {
            if (!_isColorEnabled)
                return;

            if (!ReferenceEquals(null, e.Frame))
            {
                _colorBitmap.Update(e.Frame);
                OnColorImageArrived(_colorBitmap.Bitmap);
            }  
        }
    }
}
