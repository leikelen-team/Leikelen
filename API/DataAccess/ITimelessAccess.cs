using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface ITimelessAccess
    {
        List<Timeless> GetAll(int personId, string modalName, string subModalName);

        void Add(int personId, string modalName, string subModalName, int index, double value);
        void Add(int personId, string modalName, string subModalName, int index, double value, string subtitle);
        void Add(int personId, string modalName, string subModalName, int index, string subtitle);
        void Add(int personId, string modalName, string subModalName, int index);
    }
}
