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
        private IDataAccessFacade DataAccessFacade = new DataAccessFacade();
        private List<Tuple<int, string>> _discreteGestures = new List<Tuple<int, string>>();

        public void Gesture_FrameArrived(object sender, KinectGestureFrameArrivedArgs e)
        {
            CheckIfExistsPerson(e.TrackingId);
            if (e.Time.HasValue)
            {
                if(e.Frame.DiscreteGestureResults != null)
                {
                    foreach (var discreteGesture in e.Frame.DiscreteGestureResults)
                    {
                        if (discreteGesture.Value.Detected)
                        {
                            var tuple = new Tuple<int, string>(_personsId[e.TrackingId], discreteGesture.Key.Name);
                            if (!_discreteGestures.Exists(d => d.Equals(tuple)))
                                _discreteGestures.Add(tuple);
                            if (!DataAccessFacade.GetSubModalAccess().Exists("Discrete Posture", discreteGesture.Key.Name))
                                DataAccessFacade.GetSubModalAccess().Add("Discrete Posture", discreteGesture.Key.Name, null, null);
                            DataAccessFacade.GetEventAccess().Add(_personsId[e.TrackingId], "Discrete Posture", discreteGesture.Key.Name, e.Time.Value, discreteGesture.Value.Confidence);
                        }
                    }
                }
                    

                if(e.Frame.ContinuousGestureResults != null)
                {
                    foreach (var continuousGesture in e.Frame.ContinuousGestureResults)
                    {
                        DataAccessFacade.GetEventAccess().Add(_personsId[e.TrackingId], "Continuous Posture", continuousGesture.Key.Name, e.Time.Value, continuousGesture.Value.Progress);
                    }
                }
                    
                
            }
        }

        public void StopRecording()
        {
            foreach(var postureInPerson in _discreteGestures)
            {
                DataAccessFacade.GetIntervalAccess().FromEvent(postureInPerson.Item1, "Discrete Posture", postureInPerson.Item2, _generalSettings.GetDefaultMillisecondsThreshold());
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
                var personsInScene = DataAccessFacade.GetSceneInUseAccess()?.GetScene()?.PersonsInScene;
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
                    var newPerson = DataAccessFacade.GetPersonAccess().Add(personName, null, null, null);
                    _personsId[bodyTrackingId] = newPerson.PersonId;
                    DataAccessFacade.GetPersonAccess().AddToScene(newPerson, DataAccessFacade.GetSceneInUseAccess()?.GetScene());
                }
            }
        }
    }
}
