using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.FrameProvider.Accelerometer
{
    public interface IAccelerometer
    {
        EventHandler<AccelerometerFrameArrivedEventArgs> AccelerometerListener();
    }
}
