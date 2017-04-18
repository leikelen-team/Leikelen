using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.API
{
    public interface IEventInsert
    {
        void AddEvent(int personId, string modalTypeName, string subModalTypeName, TimeSpan time, bool create);
        void AddStringEvent(int personId, string modalTypeName, string subModalTypeName, TimeSpan time, string stringData, bool create);
        void AddNumberEvent(int personId, string modalTypeName, string subModalTypeName, TimeSpan time, double numberData, bool create);
        void AddStringNumberEvent(int personId, string modalTypeName, string subModalTypeName, TimeSpan time, string stringData, double numberData, bool create);
    }
}
