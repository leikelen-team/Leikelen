using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    /// <summary>
    /// A database provider (DAO instance) for any database using entity framework core.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.Data.Persistence.IDbProvider" />
    public abstract class EfAbstractProvider : IDbProvider
    {
        /// <summary>
        /// The database instance
        /// </summary>
        protected Data.Persistence.Provider.DbDataContext Db = null;

        /// <summary>
        /// Determines whether this instance is connected.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </returns>
        public bool IsConnected()
        {
            return !ReferenceEquals(null, Db);
        }

        /// <summary>
        /// Creates the connection with the database.
        /// </summary>
        /// <param name="options">The options.</param>
        public virtual void CreateConnection(string options)
        {
        }

        /// <summary>
        /// Closes the connection with the database.
        /// </summary>
        public virtual void CloseConnection()
        {
            Db?.Dispose();
            Db = null;
        }

        /// <summary>
        /// Deletes the entire database.
        /// </summary>
        public void Delete()
        {
            Db.Database.EnsureDeleted();
        }

        /// <summary>
        /// Loads the scenes.
        /// </summary>
        /// <returns></returns>
        public List<Data.Model.Scene> LoadScenes()
        {
            return Db.Scenes.ToList();
        }

        /// <summary>
        /// Loads a single scene.
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns>
        /// The scene with the id or null
        /// </returns>
        public Data.Model.Scene LoadScene(int sceneId)
        {
            return Db.Scenes
                .Include(sc => sc.PersonsInScene)
                    .ThenInclude(pis => pis.Person)
                .Include(sc => sc.PersonsInScene)
                    .ThenInclude(pis => pis.SubModalType_PersonInScenes)
                        .ThenInclude(smtPis => smtPis.RepresentTypes)
                            .ThenInclude(rt => rt.EventData)
                .Include(sc => sc.PersonsInScene)
                    .ThenInclude(pis => pis.SubModalType_PersonInScenes)
                        .ThenInclude(smtPis => smtPis.RepresentTypes)
                            .ThenInclude(rt => rt.IntervalData)
                .Include(sc => sc.PersonsInScene)
                    .ThenInclude(pis => pis.SubModalType_PersonInScenes)
                        .ThenInclude(smtPis => smtPis.SubModalType)
                            .ThenInclude(sm => sm.ModalType)
                .Where(s => s.SceneId == sceneId)
                .ToList().Find(s => s.SceneId == sceneId);
        }

        /// <summary>
        /// Saves the scene.
        /// </summary>
        /// <param name="instance">The scene to be saved.</param>
        /// <returns>
        /// The new scene saved returned by the database
        /// </returns>
        public Data.Model.Scene SaveScene(Data.Model.Scene instance)
        {
            var r = Db.Scenes.Add(instance).Entity;
            Db.SaveChanges();
            return r;
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="newScene">The modified scene.</param>
        /// <returns>
        /// The new scene returned by the database
        /// </returns>
        public Data.Model.Scene UpdateScene(Data.Model.Scene newScene)
        {
            var r = Db.Scenes.Update(newScene).Entity;
            Db.SaveChanges();
            return r;
        }

        /// <summary>
        /// Saves a new scene changing all ids recursively according to last data in database.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <returns>
        /// The new scene returned by the database
        /// </returns>
        public Data.Model.Scene SaveNewScene(Data.Model.Scene scene)
        {
            scene.SceneId = 0;
            for (int iperson= 0; iperson < scene.PersonsInScene.Count; iperson++)
            {
                scene.PersonsInScene[iperson].Scene = null;
                scene.PersonsInScene[iperson].SceneId = 0;
                //scene.PersonsInScene[iperson].Scene.SceneId = 0;
                scene.PersonsInScene[iperson].PersonId = 0;
                scene.PersonsInScene[iperson].Person.PersonId = 0;
                for (int ismtPis=0;ismtPis< scene.PersonsInScene[iperson].SubModalType_PersonInScenes.Count; ismtPis++)
                {
                    var smt = Db.SubModalTypes.Find(scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].ModalTypeId, scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].SubModalTypeId);
                    if(smt != null)
                    {
                        scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].SubModalTypeId = null;
                        scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].SubModalType = smt;
                        scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].ModalTypeId = null;
                    //scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].SubModalType.ModalTypeId = null;
                    //scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].SubModalType.SubModalType_PersonInScenes = null;
                    }
                    scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].SubModalType_PersonInSceneId = 0;
                    scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].SceneId = 0;
                    scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].PersonId = 0;
                    for(int irt=0; irt< scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes.Count;irt++)
                    {
                        scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].RepresentTypeId = 0;
                        scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].SubModalType_PersonInScene = null;
                        scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].SubModalType_PersonInSceneId = 0;
                        if(!ReferenceEquals(null, scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].EventData))
                        {
                            scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].EventDataId = 0;
                            scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].EventData.EventDataId = 0;
                            scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].EventData.RepresentType = null;
                        }
                        if(!ReferenceEquals(null, scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].IntervalData))
                        {
                            scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].IntervalDataId = 0;
                            scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].IntervalData.IntervalDataId = 0;
                            scene.PersonsInScene[iperson].SubModalType_PersonInScenes[ismtPis].RepresentTypes[irt].IntervalData.RepresentType = null;
                        }
                    }
                }
            }
            var r = Db.Scenes.Add(scene).Entity;
            Db.SaveChanges();
            return r;
        }

        /// <summary>
        /// Loads the persons.
        /// </summary>
        /// <returns>
        /// All persons (without any associated entity)
        /// </returns>
        public List<Data.Model.Person> LoadPersons()
        {
            return Db.Persons.ToList();
        }

        /// <summary>
        /// Saves a person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns>
        /// The person returned by the database
        /// </returns>
        public Data.Model.Person SavePerson(Data.Model.Person person)
        {
            var r = Db.Persons.Add(person).Entity;
            Db.SaveChanges();
            return r;
        }

        /// <summary>
        /// Updates a person.
        /// </summary>
        /// <param name="newPerson">The new person.</param>
        /// <returns>
        /// The person returned by the database
        /// </returns>
        public Data.Model.Person UpdatePerson(Data.Model.Person newPerson)
        {
            var r = Db.Persons.Update(newPerson).Entity;
            Db.SaveChanges();
            return r;
        }

        /// <summary>
        /// Loads the modal types.
        /// </summary>
        /// <returns>
        /// All modal types (without any associated entity)
        /// </returns>
        public List<Data.Model.ModalType> LoadModals()
        {
            return Db.ModalTypes
                .Include(mt => mt.SubModalTypes)
                .ToList();
        }

        /// <summary>
        /// Loads the modal type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The modal type
        /// </returns>
        public Data.Model.ModalType LoadModal(string name)
        {
            return Db.ModalTypes
                .ToList().Find(m => m.ModalTypeId.Equals(name));
        }

        /// <summary>
        /// Saves the modal type.
        /// </summary>
        /// <param name="modalType">Type of the modal.</param>
        /// <returns>
        /// The modal type returned by the database
        /// </returns>
        public Data.Model.ModalType SaveModal(Data.Model.ModalType modalType)
        {
            var r = Db.ModalTypes.Add(modalType).Entity;
            Db.SaveChanges();
            return r;
        }

        /// <summary>
        /// Loads the sub modals.
        /// </summary>
        /// <param name="modalTypeName">Name of the modal type.</param>
        /// <returns></returns>
        public List<Data.Model.SubModalType> LoadSubModals(string modalTypeName)
        {
            return Db.SubModalTypes
                .Include(smt => smt.ModalType)
                .Where(mt => mt.ModalTypeId.Equals(modalTypeName)).ToList();
        }

        /// <summary>
        /// Saves the sub modal.
        /// </summary>
        /// <param name="submodalType">Type of the submodal.</param>
        /// <returns>
        /// The submodal returned by the database
        /// </returns>
        public Data.Model.SubModalType SaveSubModal(Data.Model.SubModalType submodalType)
        {
            var r = Db.SubModalTypes.Add(submodalType).Entity;
            Db.SaveChanges();
            return r;
        }

        /// <summary>
        /// Adds the person to a scene.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="scene">The scene.</param>
        /// <returns>
        /// The entity that associates both entities
        /// </returns>
        public Data.Model.PersonInScene AddPersonToScene(Data.Model.Person person, Data.Model.Scene scene)
        {
            var r = Db.PersonInScenes.Add(new PersonInScene()
            {
                Person = person,
                Scene = scene
            }).Entity;
            Db.SaveChanges();
            return r;
        }

        /// <summary>
        /// Verify if the person is in the scene.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="scene">The scene.</param>
        /// <returns>
        /// True if the person in is the scene, false otherwise
        /// </returns>
        public bool ExistsPersonInScene(Data.Model.Person person, Data.Model.Scene scene)
        {
            return Db.PersonInScenes.ToList().Exists(pis => pis.Person == person && pis.Scene == scene);
        }

        /// <summary>
        /// Deletes a scene.
        /// </summary>
        /// <param name="scene">The scene.</param>
        public void DeleteScene(Data.Model.Scene scene)
        {
            Db.Scenes.Remove(scene);
            Db.SaveChanges();
        }

        /// <summary>
        /// Deletes a person.
        /// </summary>
        /// <param name="person">The person to be deleted.</param>
        public void DeletePerson(Data.Model.Person person)
        {
            Db.Persons.Remove(person);
            Db.SaveChanges();
        }

        /// <summary>
        /// Deletes the sub modal.
        /// </summary>
        /// <param name="submodalType">Type of the submodal.</param>
        public void DeleteSubModal(Data.Model.SubModalType submodalType)
        {
            Db.SubModalTypes.Remove(submodalType);
            Db.SaveChanges();
        }

        /// <summary>
        /// Updates the sub modal type.
        /// </summary>
        /// <param name="subModalType">Type of the sub modal.</param>
        /// <returns>
        /// The submodal returned by the database
        /// </returns>
        public Data.Model.SubModalType UpdateSubModalType(Data.Model.SubModalType subModalType)
        {
            var r = Db.SubModalTypes.Update(subModalType).Entity;
            Db.SaveChanges();
            return r;
        }

        /// <summary>
        /// Gets an objects with all events of all scenes given the submodal type names.
        /// </summary>
        /// <param name="submodal_names">The submodal names.</param>
        /// <returns>
        /// An object with the events data
        /// </returns>
        public dynamic GetEventsQuery(string[] submodal_names)
        {
            var scs = from scene in Db.Scenes
                      join sp in Db.SmtPis on scene.SceneId equals sp.SceneId
                      join p in Db.Persons on sp.PersonId equals p.PersonId
                      join rt in Db.RepresentTypes on sp.SubModalType_PersonInSceneId equals rt.SubModalType_PersonInSceneId
                      join ed in Db.EventDatas on rt.EventDataId equals ed.EventDataId
                      where submodal_names.Contains(sp.SubModalTypeId)
                      //orderby new { scene_group.Key.Type, scene_group.Key.Name, scene_group.Key.ModalTypeId, scene_group.Key.SubModalTypeId }
                      select new
                      {
                          scene.Name,
                          scene.Duration,
                          scene.Type,
                          p.PersonId,
                          p.TrackingId,
                          sp.ModalTypeId,
                          sp.SubModalTypeId,
                          ed.EventTime,
                          ed.ToInterval,
                          rt.Value
                      };

            return scs.ToArray();
        }

        /// <summary>
        /// Gets an objects with the sum of intervals of all scenes given the submodal type names..
        /// </summary>
        /// <param name="submodal_names">The submodal names.</param>
        /// <returns>
        /// An object with the intervals data resumed taking the sum
        /// </returns>
        public dynamic GetIntervalsResumeQuery(string[] submodal_names)
        {
            var scs = from scene in Db.Scenes
                      join sp in Db.SmtPis on scene.SceneId equals sp.SceneId
                      join p in Db.Persons on sp.PersonId equals p.PersonId
                      join rt in Db.RepresentTypes on sp.SubModalType_PersonInSceneId equals rt.SubModalType_PersonInSceneId
                      join ind in Db.IntervalDatas on rt.IntervalDataId equals ind.IntervalDataId
                      where submodal_names.Contains(sp.SubModalTypeId)
                      group ind by new { scene.Name, scene.Duration, scene.Type, p.PersonId, p.TrackingId, sp.ModalTypeId, sp.SubModalTypeId } into scene_group
                      //orderby new { scene_group.Key.Type, scene_group.Key.Name, scene_group.Key.ModalTypeId, scene_group.Key.SubModalTypeId }
                      select new {
                          scene_group.Key.Name,
                          scene_group.Key.Duration,
                          scene_group.Key.Type,
                          scene_group.Key.PersonId,
                          scene_group.Key.TrackingId,
                          scene_group.Key.ModalTypeId,
                          scene_group.Key.SubModalTypeId,
                          interval_duration = scene_group.Sum(s => s.EndTime.Subtract(s.StartTime).TotalSeconds),
                          interval_percetage_duration = scene_group.Sum(s => s.EndTime.Subtract(s.StartTime).TotalMilliseconds) / (scene_group.Key.Duration.TotalMilliseconds) };

            return scs.ToArray();
        }

        /// <summary>
        /// Gets an objects with all intervals of all scenes given the submodal type names..
        /// </summary>
        /// <param name="submodal_names">The submodal names.</param>
        /// <returns>
        /// An object with the intervals data
        /// </returns>
        public dynamic GetIntervalsAllQuery(string[] submodal_names)
        {
            var scs = from scene in Db.Scenes
                      join sp in Db.SmtPis on scene.SceneId equals sp.SceneId
                      join p in Db.Persons on sp.PersonId equals p.PersonId
                      join rt in Db.RepresentTypes on sp.SubModalType_PersonInSceneId equals rt.SubModalType_PersonInSceneId
                      join ind in Db.IntervalDatas on rt.IntervalDataId equals ind.IntervalDataId
                      where submodal_names.Contains(sp.SubModalTypeId)
                      //orderby new { scene_group.Key.Type, scene_group.Key.Name, scene_group.Key.ModalTypeId, scene_group.Key.SubModalTypeId }
                      select new
                      {
                          scene.Name,
                          scene.Duration,
                          scene.Type,
                          p.PersonId,
                          p.TrackingId,
                          sp.ModalTypeId,
                          sp.SubModalTypeId,
                          interval_duration = ind.EndTime.Subtract(ind.StartTime).TotalSeconds,
                          interval_percentage_duration = (ind.EndTime.Subtract(ind.StartTime).TotalMilliseconds) / (scene.Duration.TotalMilliseconds)
                      };

            return scs.ToArray();
        }
    }
}
