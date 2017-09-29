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
            return Db != null;
        }

        public virtual void CreateConnection(string options)
        {
        }

        public virtual void CloseConnection()
        {
            Db?.Dispose();
            Db = null;
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
            int lastSceneId = Db.Scenes.Max(sc => sc.SceneId);
            scene.SceneId = lastSceneId + 1;
            int lastPersonId = Db.Persons.Max(p => p.PersonId);
            int lastRepresentTypeId = Db.RepresentTypes.Max(rt => rt.RepresentTypeId);
            int lastSmtPisId = Db.SmtPis.Max(smtPis => smtPis.SubModalType_PersonInSceneId);
            int lastEventId = Db.EventDatas.Max(ed => ed.EventDataId);
            int lastIntervalId = Db.IntervalDatas.Max(id => id.IntervalDataId);
            foreach (var person in scene.PersonsInScene)
            {
                lastPersonId++;
                lastPersonId++;

                person.SceneId = scene.SceneId;
                person.Scene = scene;
                person.PersonId = lastPersonId;
                person.Person.PersonId = lastPersonId;
                foreach(var smtPis in person.SubModalType_PersonInScenes)
                {
                    lastSmtPisId++;
                    smtPis.SubModalType_PersonInSceneId = lastSmtPisId;
                    smtPis.PersonInScene = person;
                    foreach(var rt in smtPis.RepresentTypes)
                    {
                        lastRepresentTypeId++;
                        rt.RepresentTypeId = lastRepresentTypeId;
                        rt.SubModalType_PersonInScene = smtPis;
                        if(rt.EventData != null)
                        {
                            lastEventId++;
                            rt.EventDataId = lastEventId;
                            rt.EventData.EventDataId = lastEventId;
                            rt.RepresentTypeId = lastRepresentTypeId;
                        }
                        if(rt.IntervalData != null)
                        {
                            lastIntervalId++;
                            rt.IntervalDataId = lastIntervalId;
                            rt.IntervalData.IntervalDataId = lastIntervalId;
                            rt.RepresentTypeId = lastRepresentTypeId;
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
            return Db.ModalTypes.ToList();
        }

        public ModalType LoadModal(string name)
        {
            return Db.ModalTypes.ToList().Find(m => m.ModalTypeId.Equals(name));
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
    }
}
