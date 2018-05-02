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
        public event EventHandler<Person> PersonsChanged;

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

        public Person Update(Person person)
        {
            var editedPerson = DbFacade.Instance.Provider.UpdatePerson(person);
            PersonsChanged?.Invoke(this, editedPerson);
            return editedPerson;
        }

        public PersonInScene AddToScene(Person person, Scene scene)
        {
            var pisAdded = DbFacade.Instance.Provider.AddPersonToScene(person, scene);
            PersonsChanged?.Invoke(this, pisAdded.Person);
            return pisAdded;
        }

        public bool Exists(int personId)
        {
            return DbFacade.Instance.Provider.LoadPersons().Exists(p => p.PersonId.Equals(personId));
        }

        public Person Get(int personId)
        {
            return DbFacade.Instance.Provider.LoadPersons().Find(p => p.PersonId.Equals(personId));
        }

        public List<Person> GetAll()
        {
            return DbFacade.Instance.Provider.LoadPersons();
        }

        public void Delete(Person person)
        {
            DbFacade.Instance.Provider.DeletePerson(person);
            PersonsChanged?.Invoke(this, null);
        }
    }
}
