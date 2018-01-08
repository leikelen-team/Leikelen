using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface ITimelessAccess
    {
        List<Timeless> GetAll(Person person, string modalName, string subModalName);

        void Add(Person person, string modalName, string subModalName, int index, double value);
        void Add(Person person, string modalName, string subModalName, int index, double value, string subtitle);
        void Add(Person person, string modalName, string subModalName, int index, string subtitle);
        void Add(Person person, string modalName, string subModalName, int index);
    }
}
