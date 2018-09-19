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
    /// <summary>
    /// Logic for Voice Processing Module.
    /// </summary>
    public class VoiceLogic
    {
        private API.DataAccess.IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        /// <summary>
        /// Initializes a new instance of the <see cref="VoiceLogic"/> class.
        /// </summary>
        public VoiceLogic()
        {
            _dataAccessFacade.GetModalAccess().AddIfNotExists("Voice", "When a person talks");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Voice", "Talked", "a person talked", null);

        }

        /// <summary>
        /// Handles the FrameArrived event of the _audioBeamReader control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AudioBeamFrameArrivedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Function to create the intervals from created events.
        /// </summary>
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
