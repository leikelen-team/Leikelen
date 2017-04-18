using cl.uv.leikelen.src.Data.Persistence.Interface;
using System;
using System.Collections.Generic;

namespace cl.uv.leikelen.src.Data.Model.AccessLogic
{
    public static class SceneLogic
    {
        public static void CreateFromRecord(this Scene _instance ,string name)
        {
            if (_instance != null) ClearScene();
            _instance = new Scene()
            {
                Name = name,
                RecordStartDate = DateTime.Now,
                PersonsInScene = new List<PersonInScene>(),
                Status = SceneStatus.Recorded
            };
        }

        public static void CreateFromDbContext(this Scene _instance, IBackupDataContext dataContext, string optionString)
        {
            if (_instance != null) ClearScene();
            if (!dataContext.isConnected()) dataContext.CreateConnection(optionString);
            _instance = dataContext.LoadScene();
            _instance.Status = SceneStatus.Imported;
            MainWindow.Instance().InstanciateFromScene();
        }

        public static void ClearScene()
        {

        }

        public static List<Person> getPersonsInScene(this Scene _instance)
        {
            List<Person> personsInScene = new List<Person>();
            foreach(PersonInScene pis in _instance.PersonsInScene)
            {
                personsInScene.Add(pis.Person);
            }
            return personsInScene;
        }

        public static bool isPersonInScene(this Scene _instance, Person person)
        {
            foreach (PersonInScene pis in _instance.PersonsInScene)
            {
                if(pis.Person.PersonId == person.PersonId)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isPersonInScene(this Scene _instance, ulong trackingPersonId)
        {
            foreach (PersonInScene pis in _instance.PersonsInScene)
            {
                if (pis.Person.TrackingId == trackingPersonId)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isPersonInScene(this Scene _instance, int personId)
        {
            foreach (PersonInScene pis in _instance.PersonsInScene)
            {
                if (pis.Person.PersonId == personId)
                {
                    return true;
                }
            }
            return false;
        }

        public static PersonInScene getPersonInScene(this Scene _instance, ulong trackingPersonId)
        {
            foreach (PersonInScene pis in _instance.PersonsInScene)
            {
                if (pis.Person.TrackingId == trackingPersonId)
                {
                    return pis;
                }
            }
            return null;
        }

        public static PersonInScene getPersonInScene(this Scene _instance, int personId)
        {
            foreach (PersonInScene pis in _instance.PersonsInScene)
            {
                if (pis.Person.PersonId == personId)
                {
                    return pis;
                }
            }
            return null;
        }

        public static void addPerson(this Scene _instance, Person person)
        {
            PersonInScene pis = new PersonInScene(_instance, person);
            person.PersonInScenes.Add(pis);
            _instance.PersonsInScene.Add(pis);
        }

        public static int numberOfPersons(this Scene _instance)
        {
            return _instance.PersonsInScene.Count;
        }
    }
}
