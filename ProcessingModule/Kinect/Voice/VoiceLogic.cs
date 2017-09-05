using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.API.DataAccess;

namespace cl.uv.leikelen.ProcessingModule.Kinect.Voice
{
    public class VoiceLogic
    {
        private readonly Dictionary<ulong, int> _personsId = new Dictionary<ulong, int>();
        private IDataAccessFacade DataAccessFacade = new DataAccessFacade();

        public VoiceLogic()
        {
            _personsId = new Dictionary<ulong, int>();
            
            if (!DataAccessFacade.GetModalAccess().Exists("Voice"))
                DataAccessFacade.GetModalAccess().Add("Voice", "When a person talks");
            if (!DataAccessFacade.GetSubModalAccess().Exists("Voice", "Talked"))
                DataAccessFacade.GetSubModalAccess().Add("Voice", "Talked", "a person talked", null);

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
                    if (subFrame.AudioBodyCorrelations == null) return;
                    foreach (var audioBodyCorrelation in subFrame.AudioBodyCorrelations)
                    {
                        long bodyTrackingId = (long)audioBodyCorrelation.BodyTrackingId;
                        if (!_personsId.ContainsKey(audioBodyCorrelation.BodyTrackingId))
                        {
                            var newPerson = DataAccessFacade.GetPersonAccess().Add("Kinect" + audioBodyCorrelation.BodyTrackingId, null, null, null);
                            _personsId[audioBodyCorrelation.BodyTrackingId] = newPerson.PersonId;
                            DataAccessFacade.GetPersonAccess().AddToScene(newPerson, DataAccessFacade.GetSceneInUseAccess().GetScene());
                        }
                        var time = DataAccessFacade.GetSceneInUseAccess()?.GetLocation();
                        if (time.HasValue)
                            DataAccessFacade.GetEventAccess().Add(_personsId[audioBodyCorrelation.BodyTrackingId], "Voice", "Talked", time.Value);
                        
                        Console.WriteLine("Tiempo: {0}, Llegó Voz de {1}", DateTime.Now, audioBodyCorrelation.BodyTrackingId);
                        
                    }
                }
            }
        }

        public void StopRecording()
        {
            foreach(var personPair in _personsId)
            {
                DataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Voice", "Talked", GeneralSettings.Instance.DefaultMillisecondsThreshold.Value);
            }
            
        }
    }
}
