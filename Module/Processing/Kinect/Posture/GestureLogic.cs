using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.API.FrameProvider.Kinect;
using cl.uv.leikelen.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Module.Processing.Kinect.Posture
{
    public class GestureLogic
    {
        private IGeneralSettings _generalSettings = new GeneralSettings();
        private readonly Dictionary<ulong, int> _personsId = new Dictionary<ulong, int>();
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();
        private List<Tuple<int, string>> _discreteGestures = new List<Tuple<int, string>>();

        public void Gesture_FrameArrived(object sender, KinectGestureFrameArrivedArgs e)
        {
            CheckIfExistsPerson(e.TrackingId);
            if (e.Time.HasValue)
            {
                if(!ReferenceEquals(null, e?.Frame?.DiscreteGestureResults))
                {
                    foreach (var discreteGesture in e.Frame.DiscreteGestureResults)
                    {
                        if (discreteGesture.Value.Detected)
                        {
                            var tuple = new Tuple<int, string>(_personsId[e.TrackingId], discreteGesture.Key.Name);
                            if (!_discreteGestures.Exists(d => d.Equals(tuple)))
                                _discreteGestures.Add(tuple);
                            if (!_dataAccessFacade.GetSubModalAccess().Exists("Discrete Posture", discreteGesture.Key.Name))
                                _dataAccessFacade.GetSubModalAccess().Add("Discrete Posture", discreteGesture.Key.Name, null, null);
                            _dataAccessFacade.GetEventAccess().Add(_personsId[e.TrackingId], "Discrete Posture", discreteGesture.Key.Name, e.Time.Value, discreteGesture.Value.Confidence);
                        }
                    }
                }
                    

                if(!ReferenceEquals(null, e?.Frame?.ContinuousGestureResults))
                {
                    foreach (var continuousGesture in e.Frame.ContinuousGestureResults)
                    {
                        _dataAccessFacade.GetEventAccess().Add(_personsId[e.TrackingId], "Continuous Posture", continuousGesture.Key.Name, e.Time.Value, continuousGesture.Value.Progress);
                    }
                }
                    
                
            }
        }

        public void StopRecording()
        {
            foreach(var postureInPerson in _discreteGestures)
            {
                _dataAccessFacade.GetIntervalAccess().FromEvent(postureInPerson.Item1, "Discrete Posture", postureInPerson.Item2, _generalSettings.GetDefaultMillisecondsThreshold());
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
    }
}
