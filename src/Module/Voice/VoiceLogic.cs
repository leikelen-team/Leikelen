using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.Data;
using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Model.AccessLogic;
using cl.uv.leikelen.src.kinectmedia;
using Microsoft.Kinect;

namespace cl.uv.leikelen.src.Module.Voice
{
    public class VoiceLogic
    {
        public VoiceLogic()
        {

        }

        public void _audioBeamReader_FrameArrived(object sender, AudioBeamFrameArrivedEventArgs e)
        {
            var frames = e.FrameReference.AcquireBeamFrames();

            if (frames == null) return;

            foreach (var frame in frames)
            {
                if (frame == null || frame.SubFrames == null) return;
                foreach (var subFrame in frame.SubFrames)
                {
                    TimeSpan startTime = subFrame.RelativeTime;
                    TimeSpan duration = subFrame.Duration;
                    if (subFrame.AudioBodyCorrelations == null) return;
                    foreach (var audioBodyCorrelation in subFrame.AudioBodyCorrelations)
                    {
                        long bodyTrackingId = (long)audioBodyCorrelation.BodyTrackingId;
                        if (StaticScene.Instance.isPersonInScene(audioBodyCorrelation.BodyTrackingId))
                        {
                            PersonInScene pis = StaticScene.Instance.getPersonInSceneByTrackingId(audioBodyCorrelation.BodyTrackingId);
                            pis.getModalType("Voice").getSubModalTypeByName("ByEvents").addEventData(KinectMediaFacade.Instance.Recorder.getCurrentLocation());
                            pis.getModalType("Voice").getSubModalTypeByName("ByIntervals").getIG().createAndAddInterval(KinectMediaFacade.Instance.Recorder.getCurrentLocation().Subtract(duration), KinectMediaFacade.Instance.Recorder.getCurrentLocation(), 0);
                            Console.WriteLine("Tiempo: {0}, Llegó Voz de {1}", KinectMediaFacade.Instance.Recorder.getCurrentLocation(), audioBodyCorrelation.BodyTrackingId);
                        }
                    }
                }
            }
        }
    }
}
