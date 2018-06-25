using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// All processing modules that use Kinect v2 input sensor.
/// </summary>
namespace cl.uv.leikelen.Module.Processing.Kinect
{
    /// <summary>
    /// Check if the person detected by the kinect already has detected and saved.
    /// </summary>
    public class CheckPerson
    {
        /// <summary>
        /// Dictionary that match the kinect tracking id with the persons identifier.
        /// </summary>
        public Dictionary<ulong, Person> PersonsId { get; private set;} = new Dictionary<ulong, Person>();
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        private static CheckPerson _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CheckPerson Instance
        {
            get
            {
                if (ReferenceEquals(_instance, null))
                    _instance = new CheckPerson();
                return _instance;
            }
        }


        /// <summary>
        /// Check if the person is in the dictionary, if not, check if the person
        /// is in the scene (checking by the name), if yes, add it to the dictionary,
        /// if not, create a new person and adds it to the scene and dictionary.
        /// </summary>
        /// <param name="bodyTrackingId">The kinect body tracking identifier.</param>
        public void CheckIfExistsPerson(ulong bodyTrackingId)
        {
            if (!PersonsId.ContainsKey(bodyTrackingId))
            {
                bool isPersonInScene = false;
                string personName = "Kinect" + bodyTrackingId;
                var personsInScene = _dataAccessFacade.GetSceneInUseAccess()?.GetScene()?.PersonsInScene;
                foreach (var personInScene in personsInScene)
                {
                    string name = personInScene?.Person?.Name;
                    if (!ReferenceEquals(null, name) && name.Equals(personName))
                    {
                        PersonsId[bodyTrackingId] = personInScene.Person;
                        isPersonInScene = true;
                        break;
                    }
                }
                if (!isPersonInScene)
                {
                    var newPerson = new Data.Model.Person()
                    {
                        Name = personName,
                        Photo = null,
                        Birthday = null,
                        Sex = null,
                        TrackingId = (long)bodyTrackingId
                    };
                    var pis = new Data.Model.PersonInScene()
                    {
                        Person = newPerson,
                        Scene = _dataAccessFacade.GetSceneInUseAccess()?.GetScene()
                    };
                    _dataAccessFacade.GetSceneInUseAccess()?.GetScene().PersonsInScene.Add(pis);
                    PersonsId[bodyTrackingId] = newPerson;
                    /*
                     * //this writes to the database directly
                    var newPerson = _dataAccessFacade.GetPersonAccess().Add(personName, null, null, null, (long)bodyTrackingId);
                    PersonsId[bodyTrackingId] = newPerson.PersonId;
                    _dataAccessFacade.GetPersonAccess().AddToScene(newPerson, _dataAccessFacade.GetSceneInUseAccess()?.GetScene());
                    */
                }
            }
        }
    }
}
