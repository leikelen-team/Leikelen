using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface IPersonAccess
    {
        List<Person> GetAll();
        Person Get(int personId);
        Person Add(string name, string photo, DateTime? birthday, char? sex);
        PersonInScene AddToScene(Person person, Scene scene);
        bool Exists(int personId);
    }
}
