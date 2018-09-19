using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.API.DataAccess
{
    /// <summary>
    /// /Interface to access the person
    /// </summary>
    public interface IPersonAccess
    {
        /// <summary>
        /// Occurs when [persons changed].
        /// </summary>
        event EventHandler<Data.Model.Person> PersonsChanged;

        /// <summary>
        /// Gets all persons in the database.
        /// </summary>
        /// <returns></returns>
        List<Data.Model.Person> GetAll();

        /// <summary>
        /// Gets the specified person of given identifier.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns>The person of given identifier or null</returns>
        Data.Model.Person Get(int personId);

        /// <summary>
        /// Adds the person with specified attributes.
        /// </summary>
        /// <param name="name">The name of the person.</param>
        /// <param name="photo">The photo path (optional).</param>
        /// <param name="birthday">The birthday (optional).</param>
        /// <param name="sex">The sex (0 for male, 1 for female or other number for unknown).</param>
        /// <param name="trackingId">The tracking identifier (for kinect's identified persons).</param>
        /// <returns>The new person added</returns>
        Data.Model.Person Add(string name, string photo, DateTime? birthday, int? sex, long? trackingId);

        /// <summary>
        /// Updates the specified person.
        /// </summary>
        /// <param name="person">The new person object (update the person with same id).</param>
        /// <returns>The person updated</returns>
        Person Update(Person person);

        /// <summary>
        /// Adds the given person to the scene.
        /// </summary>
        /// <param name="person">The person object.</param>
        /// <param name="scene">The scene object.</param>
        /// <returns>A PersonInScene object</returns>
        Data.Model.PersonInScene AddToScene(Data.Model.Person person, Data.Model.Scene scene);

        /// <summary>
        /// Verify if Exists the specified person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns>True if the person already exists, False if not</returns>
        bool Exists(int personId);

        /// <summary>
        /// Deletes the specified person.
        /// </summary>
        /// <param name="person">The person object.</param>
        void Delete(Data.Model.Person person);
    }
}
