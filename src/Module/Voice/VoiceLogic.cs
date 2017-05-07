using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.Data;
using cl.uv.leikelen.src.Data.Access;
using cl.uv.leikelen.src.Data.Model.AccessLogic;
using cl.uv.leikelen.src.Input.Kinect;
using Microsoft.Kinect;

namespace cl.uv.leikelen.src.Module.Voice
{
    public class VoiceLogic
    {
        List<int> personsId = new List<int>();
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
                        Console.WriteLine("Tiempo: {0}, Llegó Voz de {1}", DateTime.Now, audioBodyCorrelation.BodyTrackingId);
                        if (StaticScene.Instance.isPersonInScene(audioBodyCorrelation.BodyTrackingId))
                        {
                            personsId.Add((int)audioBodyCorrelation.BodyTrackingId);
                            StaticScene.EventInsert.AddEvent((int)audioBodyCorrelation.BodyTrackingId, "Voice", "Talked", KinectMediaFacade.Instance.Recorder.getLocation().Value, true);
                            //PersonInScene pis = StaticScene.Instance.getPersonInSceneByTrackingId(audioBodyCorrelation.BodyTrackingId);
                            //pis.getModalType("Voice").getSubModalTypeByName("Talked").addEventData(KinectMediaFacade.Instance.Recorder.getLocation().Value);
                            //TODO: Crear clase accesora y creadora de datos
                            //pis.getModalType("Voice").getSubModalTypeByName("ByIntervals").getIG().createAndAddInterval(KinectMediaFacade.Instance.Recorder.getCurrentLocation().Subtract(duration), KinectMediaFacade.Instance.Recorder.getCurrentLocation(), 0);
                            
                        }
                    }
                }
            }
        }


        public void StopRecording()
        {
            foreach(int personid in this.personsId)
            {
                StaticScene.IntervalInsert.generateFromEvent(personid, "Voice", "Talked", "Voice", "Talking", 2000);
            }
            
        }
    }
}
