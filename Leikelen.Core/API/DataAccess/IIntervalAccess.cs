using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface IIntervalAccess
    {
        List<Interval> GetAll(Person person, string modalName, string subModalName);

        void Add(Person person, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value);
        void Add(Person person, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value, string subtitle);
        void Add(Person person, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, string subtitle);
        void Add(Person person, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime);

        void FromEvent(Person person, string modalName, string subModalName, int millisecondsThreshold, int which, string intervalName);
    }
}
