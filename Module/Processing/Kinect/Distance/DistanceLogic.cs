using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;

namespace cl.uv.leikelen.Module.Processing.Kinect.Distance
{
    public class DistanceLogic
    {
        private readonly Dictionary<ulong, int> _personsId = new Dictionary<ulong, int>();
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();


        public void _bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
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
