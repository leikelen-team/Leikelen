using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace cl.uv.leikelen.src.API.Input
{
    public interface IVideoViewer
    {
        event EventHandler<ImageSource> colorImageArrived;
        event EventHandler<ImageSource> skeletonImageArrived;
    }
}
