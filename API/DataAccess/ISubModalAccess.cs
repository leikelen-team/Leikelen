using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface ISubModalAccess
    {
        List<SubModalType> GetAll(string modalName);
        void Add(string modalName, string name, string description, string path);
        bool Exists(string modalName, string subModalName);
    }
}
