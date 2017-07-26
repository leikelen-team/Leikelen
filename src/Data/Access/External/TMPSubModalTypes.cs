using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Data.Access.External
{
    public class TmpSubModalTypes
    {
        public List<Tuple<string, string>> TemporalSubmodals;

        private static TmpSubModalTypes _instance;

        public static TmpSubModalTypes Instance
        {
            get
            {
                if (_instance == null) _instance = new TmpSubModalTypes();
                return _instance;
            }
        }

        private TmpSubModalTypes()
        {
            TemporalSubmodals = new List<Tuple<string, string>>();
        }

        public void CheckSubmodals()
        {
            var smAccess = new SubModalAccess();
            var mAccess = new ModalAccess();
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
