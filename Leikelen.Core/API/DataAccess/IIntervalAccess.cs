using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface IIntervalAccess
    {
        List<Interval> GetAll(int personId, string modalName, string subModalName);

        void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value);
        void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value, string subtitle);
        void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, string subtitle);
        void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime);

        void FromEvent(int personId, string modalName, string subModalName, int millisecondsThreshold, int which, string intervalName);
    }
}
