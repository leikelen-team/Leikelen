using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.API
{
    public interface IInsertType
    {
        void CreateModalType(string modalTypeName);
        void CreateModalType(string modalTypeName, string explanation);

        void UpdateModalType(int modalTypeId, string newName, string newExplanation);

        void CreateSubModalType(string modalTypeName, string subModalTypeName);
        void CreateSubModalType(string modalTypeName, string subModalTypeName, string explanation, string path);

        void UpdateSubModalType(int subModalTypeId, string newName, string newExplanation, string path);

        bool SubModalTypeExists(string modalTypeName, string subModalTypeName);
    }
}
