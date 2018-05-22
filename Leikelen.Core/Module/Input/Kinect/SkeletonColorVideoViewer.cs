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
    /// <summary>
    /// Class to send the video of skeletons and color of the kinect sensor
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Input.IVideo" />
    public class SkeletonColorVideoViewer : IVideo
    {
        /// <summary>
        /// Occurs when [color image arrived].
        /// </summary>
        public event EventHandler<ImageSource> ColorImageArrived;

        /// <summary>
        /// Occurs when [skeleton image arrived].
        /// </summary>
        public event EventHandler<ImageSource> SkeletonImageArrived;

        private bool _isColorEnabled;
        private bool _isSkeletonEnabled;

        private readonly ColorFrameBitmap _colorBitmap;
        private WriteableBitmap _bodyBitmap;
        private readonly List<CustomBody> _bodies;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonColorVideoViewer"/> class.
        /// </summary>
        public SkeletonColorVideoViewer()
        {
            _colorBitmap = new ColorFrameBitmap();
            _bodies = new List<CustomBody>();
        }

        /// <summary>
        /// Determines whether [is color enabled].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is color enabled]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsColorEnabled()
        {
            return _isColorEnabled;
        }

        /// <summary>
        /// Determines whether [is skeleton enabled].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is skeleton enabled]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSkeletonEnabled()
        {
            return _isSkeletonEnabled;
        }

        /// <summary>
        /// Enables the color layer.
        /// </summary>
        public void EnableColor()
        {
            _isColorEnabled = true;
        }

        /// <summary>
        /// Disables the color layer.
        /// </summary>
        public void DisableColor()
        {
            _isColorEnabled = false;
        }

        /// <summary>
        /// Enables the skeleton layer.
        /// </summary>
        public void EnableSkeleton()
        {
            _isSkeletonEnabled = true;
        }
        /// <summary>
        /// Disables the skeleton layer.
        /// </summary>
        public void DisableSkeleton()
        {
            _isSkeletonEnabled = false;
        }


        /// <summary>
        /// Called when [color image arrived].
        /// </summary>
        /// <param name="e">The e.</param>
        public void OnColorImageArrived(ImageSource e)
        {
            ColorImageArrived?.Invoke(this, e);
        }

        /// <summary>
        /// Called when [skeleton image arrived].
        /// </summary>
        /// <param name="e">The e.</param>
        public void OnSkeletonImageArrived(ImageSource e)
        {
            SkeletonImageArrived?.Invoke(this, e);
        }

        /// <summary>
        /// Handles the FrameArrived event of the _bodyReader control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BodyFrameArrivedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the FrameArrived event of the _colorReader control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ColorFrameArrivedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the BodyFrameArrived event of the _replay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ReplayFrameArrivedEventArgs{ReplayBodyFrame}"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the ColorFrameArrived event of the _replay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ReplayFrameArrivedEventArgs{ReplayColorFrame}"/> instance containing the event data.</param>
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
