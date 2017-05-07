using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.Input.Kinect;
using Microsoft.Kinect;

namespace cl.uv.leikelen.src.Module.Distance
{
    public class Distance : IModule, IInputKinect
    {
        private DistanceLogic logic;
        public Distance()
        {
            logic = new DistanceLogic();
        }

        public bool BeforeRecording()
        {
            return false;
        }
        public EventHandler<BodyFrameArrivedEventArgs> BodyListener()
        {
            return logic._bodyReader_FrameArrived;
        }
        public EventHandler<ColorFrameArrivedEventArgs> ColorListener()
        {
            return null;
        }
        public EventHandler<AudioBeamFrameArrivedEventArgs> AudioListener()
        {
            return null;
        }
        public Action FunctionAfterStop()
        {
            //TODO: Ordenar bodies por tiempo, y calcular distancias
            return null;
        }
    }
}
