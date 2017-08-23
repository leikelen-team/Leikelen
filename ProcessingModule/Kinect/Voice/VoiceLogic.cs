using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace cl.uv.leikelen.ProcessingModule.Kinect.Voice
{
    public class VoiceLogic
    {
        List<int> personsId = new List<int>();

        public void _audioBeamReader_FrameArrived(object sender, AudioBeamFrameArrivedEventArgs e)
        {
            var frames = e.FrameReference.AcquireBeamFrames();

            if (frames == null) return;

            foreach (var frame in frames)
            {
                if (frame == null || frame.SubFrames == null) return;
                foreach (var subFrame in frame.SubFrames)
                {
                    if (subFrame.AudioBodyCorrelations == null) return;
                    foreach (var audioBodyCorrelation in subFrame.AudioBodyCorrelations)
                    {
                        long bodyTrackingId = (long)audioBodyCorrelation.BodyTrackingId;
                        Console.WriteLine("Tiempo: {0}, Llegó Voz de {1}", DateTime.Now, audioBodyCorrelation.BodyTrackingId);
                        if (!personsId.Exists(pid => pid == (int)audioBodyCorrelation.BodyTrackingId))
                            personsId.Add((int)audioBodyCorrelation.BodyTrackingId);
                        //TODO insertar
                    }
                }
            }
        }

        public void StopRecording()
        {
        }
    }
}
