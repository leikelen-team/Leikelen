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
        private readonly Dictionary<ulong, int> _personsId = new Dictionary<ulong, int>();
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public VoiceLogic()
        {
            _personsId = new Dictionary<ulong, int>();
            
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
                        CheckIfExistsPerson(audioBodyCorrelation.BodyTrackingId);
                        var time = _dataAccessFacade.GetSceneInUseAccess()?.GetLocation();
                        if (time.HasValue)
                            _dataAccessFacade.GetEventAccess().Add(_personsId[audioBodyCorrelation.BodyTrackingId], "Voice", "Talked", time.Value, true);
                        
                        Console.WriteLine("Tiempo: {0}, Llegó Voz de {1}", DateTime.Now, audioBodyCorrelation.BodyTrackingId);
                        
                    }
                }
            }
        }

        /// <summary>
        /// Check if the person is in the dictionary, if not, check if the person
        /// is in the scene (checking by the name), if yes, add it to the dictionary,
        /// if not, create a new person and adds it to the scene and dictionary
        /// </summary>
        /// <param name="bodyTrackingId">The kinect body tracking identifier.</param>
        private void CheckIfExistsPerson(ulong bodyTrackingId)
        {
            if (!_personsId.ContainsKey(bodyTrackingId))
            {
                bool isPersonInScene = false;
                string personName = "Kinect" + bodyTrackingId;
                var personsInScene = _dataAccessFacade.GetSceneInUseAccess()?.GetScene()?.PersonsInScene;
                foreach (var personInScene in personsInScene)
                {
                    string name = personInScene?.Person?.Name;
                    if (!ReferenceEquals(null, name) && name.Equals(personName))
                    {
                        _personsId[bodyTrackingId] = personInScene.Person.PersonId;
                        isPersonInScene = true;
                        break;
                    }
                }
                if (!isPersonInScene)
                {
                    var newPerson = _dataAccessFacade.GetPersonAccess().Add(personName, null, null, null);
                    _personsId[bodyTrackingId] = newPerson.PersonId;
                    _dataAccessFacade.GetPersonAccess().AddToScene(newPerson, _dataAccessFacade.GetSceneInUseAccess()?.GetScene());
                }
            }
        }

        public void StopRecording()
        {
            foreach(var personPair in _personsId)
            {
                _dataAccessFacade.GetIntervalAccess().FromEvent(personPair.Value, "Voice", "Talked", _dataAccessFacade.GetGeneralSettings().GetDefaultMillisecondsThreshold());
            }
            
        }
    }
}
