using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.Data.Persistence;
using cl.uv.leikelen.InputModule;

namespace cl.uv.leikelen.Data.Access.External
{
    public class PersonAccess : IPersonAccess
    {
        public Person Add(string name, string photo, DateTime? birthday, char? sex)
        {
            var person = DbFacade.Instance.Provider.SavePerson(new Person()
            {
                Name = name,
                Photo = photo,
                Birthday = birthday,
                Sex = sex
            });
            InputLoader.Instance.FillPersonInputModules(person);
            return person;
        }

        public PersonInScene AddToScene(Person person, Scene scene)
        {
            return DbFacade.Instance.Provider.AddPersonToScene(person, scene);
        }

        public bool Exists(int personId)
        {
            return DbFacade.Instance.Provider.LoadPersons().Exists(p => p.PersonId == personId);
        }

        public Person Get(int personId)
        {
            return DbFacade.Instance.Provider.LoadPersons().Find(p => p.PersonId == personId);
        }

        public List<Person> GetAll()
        {
            return DbFacade.Instance.Provider.LoadPersons();
        }
    }
}
