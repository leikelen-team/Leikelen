using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    public abstract class EfAbstractProvider : IDbProvider
    {
        protected DbDataContext Db = null;

        public bool IsConnected()
        {
            return !ReferenceEquals(null, Db);
        }

        public virtual void CreateConnection(string options)
        {
        }

        public virtual void CloseConnection()
        {
            Db?.Dispose();
            Db = null;
        }

        public void Delete()
        {
            Db.Database.EnsureDeleted();
        }

        public List<Scene> LoadScenes()
        {
            return Db.Scenes.ToList();
        }

        public Scene LoadScene(int sceneId)
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

        public Scene SaveScene(Scene instance)
        {
            var r = Db.Scenes.Add(instance).Entity;
            Db.SaveChanges();
            return r;
        }

        public Scene UpdateScene(Scene newScene)
        {
            var r = Db.Scenes.Update(newScene).Entity;
            Db.SaveChanges();
            return r;
        }

        public Scene SaveNewScene(Scene scene)
        {
            for (int iperson= 0; iperson < scene.PersonsInScene.Count; iperson++)
            {
                scene.PersonsInScene[iperson].SceneId = scene.SceneId;
                scene.PersonsInScene[iperson].Scene.SceneId = 0;
                scene.PersonsInScene[iperson].PersonId = 0;
                scene.PersonsInScene[iperson].Person.PersonId = 0;
                for (int ismtPis=0;ismtPis< scene.PersonsInScene[iperson].SubModalType_PersonInScenes.Count; ismtPis++)
                {
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

        public List<Person> LoadPersons()
        {
            return Db.Persons.ToList();
        }

        public Person SavePerson(Person person)
        {
            var r = Db.Persons.Add(person).Entity;
            Db.SaveChanges();
            return r;
        }

        public Person UpdatePerson(Person newPerson)
        {
            var r = Db.Persons.Update(newPerson).Entity;
            Db.SaveChanges();
            return r;
        }

        public List<ModalType> LoadModals()
        {
            return Db.ModalTypes
                .Include(mt => mt.SubModalTypes)
                .ToList();
        }

        public ModalType LoadModal(string name)
        {
            return Db.ModalTypes
                .ToList().Find(m => m.ModalTypeId.Equals(name));
        }

        public ModalType SaveModal(ModalType modalType)
        {
            var r = Db.ModalTypes.Add(modalType).Entity;
            Db.SaveChanges();
            return r;
        }

        public List<SubModalType> LoadSubModals(string modalTypeName)
        {
            return Db.SubModalTypes
                .Include(smt => smt.ModalType)
                .Where(mt => mt.ModalTypeId.Equals(modalTypeName)).ToList();
        }

        public SubModalType SaveSubModal(SubModalType submodalType)
        {
            var r = Db.SubModalTypes.Add(submodalType).Entity;
            Db.SaveChanges();
            return r;
        }

        public PersonInScene AddPersonToScene(Person person, Scene scene)
        {
            var r = Db.PersonInScenes.Add(new PersonInScene()
            {
                Person = person,
                Scene = scene
            }).Entity;
            Db.SaveChanges();
            return r;
        }

        public bool ExistsPersonInScene(Person person, Scene scene)
        {
            return Db.PersonInScenes.ToList().Exists(pis => pis.Person == person && pis.Scene == scene);
        }

        public void DeleteScene(Scene scene)
        {
            Db.Scenes.Remove(scene);
            Db.SaveChanges();
        }

        public void DeletePerson(Person person)
        {
            Db.Persons.Remove(person);
            Db.SaveChanges();
        }

        public void DeleteSubModal(SubModalType submodalType)
        {
            Db.SubModalTypes.Remove(submodalType);
            Db.SaveChanges();
        }

        public SubModalType UpdateSubModalType(SubModalType subModalType)
        {
            var r = Db.SubModalTypes.Update(subModalType).Entity;
            Db.SaveChanges();
            return r;
        }


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
