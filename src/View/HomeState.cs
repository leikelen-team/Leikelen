using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.View
{
    public enum HomeState
    {
        Initial,
        FromSensor,
        FromSensorWithScene,
        FromSensorRecording,
        FromFile,
        FromFileWithScene
    }
}
