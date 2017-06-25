using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.Data.Model;

namespace cl.uv.leikelen.src.API.DataAccess
{
    public interface IModalAccess
    {
        List<ModalType> GetAll();
        void Add(string name, string description);
        bool Exists(string name);
    }
}
