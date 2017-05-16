using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.Input.Kinect;
using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Model.AccessLogic;
using cl.uv.leikelen.src.Data;
using KinectEx;
using Microsoft.Kinect;

namespace cl.uv.leikelen.src.Module.AddPerson
{
    public class AddPersonEntryPoint : IModule, IKinectModule
    {
        private List<CustomBody> _bodies;
        public EventHandler<AudioBeamFrameArrivedEventArgs> AudioListener()
        {
            return null;
        }

        public bool BeforeRecording()
        {
            return false;
        }

        public EventHandler<BodyFrameArrivedEventArgs> BodyListener()
        {
            return _bodyReader_FrameArrived;
        }

        public EventHandler<ColorFrameArrivedEventArgs> ColorListener()
        {
            return null;
        }

        public Action FunctionAfterStop()
        {
            return null;
        }

        public AddPersonEntryPoint()
        {
            _bodies = new List<CustomBody>();
        }


        private void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
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
                foreach (IBody body in bodies)
                {
                    ulong trackingId = body.TrackingId;
                    bool personExists = StaticScene.Instance.isPersonInScene(trackingId);
                    if (!personExists && trackingId != 0)
                    {
                        Person person = new Person(
                                (int)trackingId,
                                trackingId,
                                StaticScene.Instance.numberOfPersons()
                            );
                        StaticScene.Instance.addPerson(person);
                    }
                }
            }
        }
    }
}
