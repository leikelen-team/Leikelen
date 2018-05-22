using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Persistence
{
    /// <summary>
    /// Interface to Database providers (DAO)
    /// </summary>
    public interface IDbProvider
    {
        /// <summary>
        /// Creates the connection with the database.
        /// </summary>
        /// <param name="options">The options.</param>
        void CreateConnection(string options);

        /// <summary>
        /// Closes the connection with the database.
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Deletes the entire database.
        /// </summary>
        void Delete();

        /// <summary>
        /// Loads the scenes.
        /// </summary>
        /// <returns></returns>
        List<Scene> LoadScenes();
        /// <summary>
        /// Loads a single scene.
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns>The scene with the id or null</returns>
        Scene LoadScene(int sceneId);
        /// <summary>
        /// Saves the scene.
        /// </summary>
        /// <param name="instance">The scene to be saved.</param>
        /// <returns>The new scene saved returned by the database</returns>
        Scene SaveScene(Scene instance);
        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="newScene">The modified scene.</param>
        /// <returns>The new scene returned by the database</returns>
        Scene UpdateScene(Scene newScene);
        /// <summary>
        /// Deletes a scene.
        /// </summary>
        /// <param name="scene">The scene.</param>
        void DeleteScene(Scene scene);
        /// <summary>
        /// Saves a new scene changing all ids recursively according to last data in database.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <returns>The new scene returned by the database</returns>
        Scene SaveNewScene(Scene scene);

        /// <summary>
        /// Loads the persons.
        /// </summary>
        /// <returns>All persons (without any associated entity)</returns>
        List<Person> LoadPersons();
        /// <summary>
        /// Saves a person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns>The person returned by the database</returns>
        Person SavePerson(Person person);
        /// <summary>
        /// Updates a person.
        /// </summary>
        /// <param name="newPerson">The new person.</param>
        /// <returns>The person returned by the database</returns>
        Person UpdatePerson(Person newPerson);
        /// <summary>
        /// Deletes a person.
        /// </summary>
        /// <param name="person">The person to be deleted.</param>
        void DeletePerson(Person person);

        /// <summary>
        /// Adds the person to a scene.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="scene">The scene.</param>
        /// <returns>The entity that associates both entities</returns>
        PersonInScene AddPersonToScene(Person person, Scene scene);
        /// <summary>
        /// Verify if the person is in the scene.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="scene">The scene.</param>
        /// <returns>True if the person in is the scene, false otherwise</returns>
        bool ExistsPersonInScene(Person person, Scene scene);

        /// <summary>
        /// Loads the modal types.
        /// </summary>
        /// <returns>All modal types (without any associated entity)</returns>
        List<ModalType> LoadModals();
        /// <summary>
        /// Loads the modal type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The modal type</returns>
        ModalType LoadModal(string name);
        /// <summary>
        /// Saves the modal type.
        /// </summary>
        /// <param name="modalType">Type of the modal.</param>
        /// <returns>The modal type returned by the database</returns>
        ModalType SaveModal(ModalType modalType);

        /// <summary>
        /// Loads the sub modal of a given modal type.
        /// </summary>
        /// <param name="ModalTypeName">Name of the modal type.</param>
        /// <returns>All submodal types of the given modal type name(without any associated entity)</returns>
        List<SubModalType> LoadSubModals(string ModalTypeName);
        /// <summary>
        /// Saves the sub modal.
        /// </summary>
        /// <param name="submodalType">Type of the submodal.</param>
        /// <returns>The submodal returned by the database</returns>
        SubModalType SaveSubModal(SubModalType submodalType);
        /// <summary>
        /// Deletes the sub modal.
        /// </summary>
        /// <param name="submodalType">Type of the submodal.</param>
        void DeleteSubModal(SubModalType submodalType);
        /// <summary>
        /// Updates the sub modal type.
        /// </summary>
        /// <param name="subModalType">Type of the sub modal.</param>
        /// <returns>The submodal returned by the database</returns>
        SubModalType UpdateSubModalType(SubModalType subModalType);

        /// <summary>
        /// Gets an objects with all events of all scenes given the submodal type names.
        /// </summary>
        /// <param name="submodal_names">The submodal names.</param>
        /// <returns>An object with the events data</returns>
        dynamic GetEventsQuery(string[] submodal_names);
        /// <summary>
        /// Gets an objects with the sum of intervals of all scenes given the submodal type names..
        /// </summary>
        /// <param name="submodal_names">The submodal names.</param>
        /// <returns>An object with the intervals data resumed taking the sum</returns>
        dynamic GetIntervalsResumeQuery(string[] submodal_names);
        /// <summary>
        /// Gets an objects with all intervals of all scenes given the submodal type names..
        /// </summary>
        /// <param name="submodal_names">The submodal names.</param>
        /// <returns>An object with the intervals data</returns>
        dynamic GetIntervalsAllQuery(string[] submodal_names);
    }
}
