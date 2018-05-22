using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.Data.Persistence;
using cl.uv.leikelen.Module;

namespace cl.uv.leikelen.Data.Access.External
{
    /// <summary>
    /// Class that implements the interface to access the person
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.DataAccess.IPersonAccess" />
    public class PersonAccess : IPersonAccess
    {
        /// <summary>
        /// Occurs when [persons changed].
        /// </summary>
        public event EventHandler<Person> PersonsChanged;

        /// <summary>
        /// Adds the person with specified attributes.
        /// </summary>
        /// <param name="name">The name of the person.</param>
        /// <param name="photo">The photo path (optional).</param>
        /// <param name="birthday">The birthday (optional).</param>
        /// <param name="sex">The sex (0 for male, 1 for female or other number for unknown).</param>
        /// <param name="trackingId">The tracking identifier (for kinect's identified persons).</param>
        /// <returns>The new person added</returns>
        public Person Add(string name, string photo, DateTime? birthday, int? sex, long? trackingId)
        {
            var person = DbFacade.Instance.Provider.SavePerson(new Person()
            {
                Name = name,
                Photo = photo,
                Birthday = birthday,
                Sex = sex,
                TrackingId = trackingId
            });
            InputLoader.Instance.FillPersonInputModules(person);
            PersonsChanged?.Invoke(this, person);
            return person;
        }

        /// <summary>
        /// Updates the specified person.
        /// </summary>
        /// <param name="person">The new person object (update the person with same id).</param>
        /// <returns>The person updated</returns>
        public Person Update(Person person)
        {
            var editedPerson = DbFacade.Instance.Provider.UpdatePerson(person);
            PersonsChanged?.Invoke(this, editedPerson);
            return editedPerson;
        }

        /// <summary>
        /// Adds the given person to the scene.
        /// </summary>
        /// <param name="person">The person object.</param>
        /// <param name="scene">The scene object.</param>
        /// <returns>A PersonInScene object</returns>
        public PersonInScene AddToScene(Person person, Scene scene)
        {
            var pisAdded = DbFacade.Instance.Provider.AddPersonToScene(person, scene);
            PersonsChanged?.Invoke(this, pisAdded.Person);
            return pisAdded;
        }

        /// <summary>
        /// Verify if Exists the specified person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns>True if the person already exists, False if not</returns>
        public bool Exists(int personId)
        {
            return DbFacade.Instance.Provider.LoadPersons().Exists(p => p.PersonId.Equals(personId));
        }

        /// <summary>
        /// Gets all persons in the database.
        /// </summary>
        /// <returns></returns>
        public Person Get(int personId)
        {
            return DbFacade.Instance.Provider.LoadPersons().Find(p => p.PersonId.Equals(personId));
        }

        /// <summary>
        /// Gets the specified person of given identifier.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns>The person of given identifier or null</returns>
        public List<Person> GetAll()
        {
            return DbFacade.Instance.Provider.LoadPersons();
        }

        /// <summary>
        /// Deletes the specified person.
        /// </summary>
        /// <param name="person">The person object.</param>
        public void Delete(Person person)
        {
            DbFacade.Instance.Provider.DeletePerson(person);
            PersonsChanged?.Invoke(this, null);
        }
    }
}
