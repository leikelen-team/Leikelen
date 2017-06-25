using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.Data.Access.External
{
    public class TMPSubModalTypes
    {
        public List<Tuple<string, string>> TemporalSubmodals;

        private static TMPSubModalTypes _instance;

        public static TMPSubModalTypes Instance
        {
            get
            {
                if (_instance == null) _instance = new TMPSubModalTypes();
                return _instance;
            }
        }

        private TMPSubModalTypes()
        {
            TemporalSubmodals = new List<Tuple<string, string>>();
        }

        public void CheckSubmodals()
        {
            SubModalAccess smAccess = new SubModalAccess();
            ModalAccess mAccess = new ModalAccess();
            foreach (var subModalElement in TemporalSubmodals)
            {
                if (!mAccess.Exists(subModalElement.Item1))
                {
                    //TODO: hacer algo si no hay modal
                }
                else if (!smAccess.Exists(subModalElement.Item1, subModalElement.Item2))
                {
                    //TODO: hacer algo si no hay submodal para el modal
                }
            }
        }
    }
}
