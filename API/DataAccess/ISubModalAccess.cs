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
        SubModalType Get(string modalName, string name);
        SubModalType Add(string modalName, string name, string description, string path);
        SubModalType AddIfNotExists(string modalName, string name, string description, string path);
        void Delete(string modalName, string name);
        bool Exists(string modalName, string subModalName);
        SubModalType Update(SubModalType subModalType);
    }
}
