using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace cl.uv.leikelen.src.API.InputModule
{
    public interface IVideo
    {
        event EventHandler<ImageSource> ColorImageArrived;
        event EventHandler<ImageSource> SkeletonImageArrived;

        bool IsColorEnabled();
        bool IsSkeletonEnabled();
        void EnableColor();
        void EnableSkeleton();
        void DisableColor();
        void DisableSkeleton();
    }
}
