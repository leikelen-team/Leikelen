using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    public class MemoryProvider : IDbProvider
    {
        private DbContext _db;

        public void CreateConnection(string options)
        {
            _db = new DbContext();
        }

        public void CloseConnection()
        {
            _db = null;
        }

        public List<Scene> LoadScenes()
        {
            return _db.Scenes;
        }

        public Scene LoadScene(int sceneId, bool timeless, bool intervals, bool events)
        {
            return _db.Scenes.Find(s => s.SceneId == sceneId);
        }

        public void SaveScene(Scene instance)
        {
            _db.Scenes.Add(instance);
        }

        public void UpdateScene(int sceneId, Scene newScene)
        {
            _db.Scenes.RemoveAll(s => s.SceneId == sceneId);
            _db.Scenes.Add(newScene);
        }

        public List<Person> LoadPersons()
        {
            return _db.Persons;
        }

        public void SavePerson(Person person)
        {
            _db.Persons.Add(person);
        }

        public void UpdatePerson(int personId, Person newPerson)
        {
            _db.Persons.RemoveAll(s => s.PersonId == personId);
            _db.Persons.Add(newPerson);
        }

        public List<ModalType> LoadModals()
        {
            return _db.ModalTypes;
        }

        public ModalType LoadModal(string name)
        {
            return _db.ModalTypes.Find(m => m.Name == name);
        }

        public void SaveModal(ModalType modalType)
        {
            _db.ModalTypes.Add(modalType);
        }

        public List<SubModalType> LoadSubModals(ModalType modalType)
        {
            return _db.ModalTypes.Find(m => m.Name == modalType.Name).SubmodalTypes;
        }

        public void SaveSubModal(string modalTypeName, SubModalType submodalType)
        {
            _db.ModalTypes.Find(m => m.Name == modalTypeName).SubmodalTypes.Add(submodalType);
        }
    }

    class DbContext
    {
        public List<Scene> Scenes { get; set; }
        public List<Person> Persons { get; set; }
        public List<PersonInScene> PersonInScenes { get; set; }
        public List<ModalType> ModalTypes { get; set; }
        public List<SubModalType> SubModalTypes { get; set; }
        public List<SubModalType_PersonInScene> SmtPis { get; set; }
        public List<RepresentType> RepresentTypes { get; set; }
        public List<EventData> EventDatas { get; set; }
        public List<IntervalData> IntervalDatas { get; set; }

        public DbContext()
        {
            Scenes = new List<Scene>();
            Persons = new List<Person>();
            PersonInScenes = new List<PersonInScene>();
            ModalTypes = new List<ModalType>();
            SubModalTypes = new List<SubModalType>();
            SmtPis = new List<SubModalType_PersonInScene>();
            RepresentTypes = new List<RepresentType>();
            EventDatas = new List<EventData>();
            IntervalDatas = new List<IntervalData>();
        }
    }
}
