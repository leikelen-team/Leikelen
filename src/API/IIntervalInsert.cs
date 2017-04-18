using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.API
{
    public interface IIntervalInsert
    {
        void AddInterval(int personId, string modalTypeName, string subModalTypeName, TimeSpan start, TimeSpan end);
        void AddStringInterval(int personId, string modalTypeName, string subModalTypeName, TimeSpan start, TimeSpan end, string stringData);
        void AddNumberInterval(int personId, string modalTypeName, string subModalTypeName, TimeSpan start, TimeSpan end, double numberData);
        void AddStringNumberInterval(int personId, string modalTypeName, string subModalTypeName, TimeSpan start, TimeSpan end, string stringData, double numberData);
    }
}
