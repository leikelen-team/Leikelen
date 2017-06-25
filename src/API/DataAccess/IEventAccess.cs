using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.API.DataAccess
{
    public interface IEventAccess
    {
        List<Event> GetAll(int personId, string modalName, string subModalName);

        void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value);
        void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, string subtitle);
        void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, int index);
        void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, string subtitle);
        void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, int index);
        void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, string subtitle, int index);
        void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, string subtitle, int index);
    }
}
