using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.API.DataAccess;

namespace cl.uv.leikelen.Module.Processing.Kinect.Voice
{
    public class VoiceLogic
    {
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public VoiceLogic()
        {
            _dataAccessFacade.GetModalAccess().AddIfNotExists("Voice", "When a person talks");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Voice", "Talked", "a person talked", null);

        }

        public void _audioBeamReader_FrameArrived(object sender, AudioBeamFrameArrivedEventArgs e)
        {
            var frames = e.FrameReference.AcquireBeamFrames();

            if (ReferenceEquals(null, frames)) return;

            foreach (var frame in frames)
            {
                if (ReferenceEquals(null, frame) || ReferenceEquals(null, frame.SubFrames)) return;
                foreach (var subFrame in frame.SubFrames)
                {
                    if (ReferenceEquals(null, subFrame.AudioBodyCorrelations)) return;
                    foreach (var audioBodyCorrelation in subFrame.AudioBodyCorrelations)
                    {
                        CheckPerson.Instance.CheckIfExistsPerson(audioBodyCorrelation.BodyTrackingId);
                        var time = _dataAccessFacade.GetSceneInUseAccess()?.GetLocation();
                        if (time.HasValue)
                            _dataAccessFacade.GetEventAccess().Add(CheckPerson.Instance.PersonsId[audioBodyCorrelation.BodyTrackingId], "Voice", "Talked", time.Value, 1);
                        
                        //Console.WriteLine("Tiempo: {0}, Llegó Voz de {1}", DateTime.Now, audioBodyCorrelation.BodyTrackingId);
                        
                    }
                }
            }
        }

        public async void StopRecording()
        {
            foreach(var personPair in CheckPerson.Instance.PersonsId)
            {
                try
                {
                    _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Voice", "Talked", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold(), 1, "Talked");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                
            }
        }
    }
}
