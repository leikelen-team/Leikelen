using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API.ProcessingModule;

namespace cl.uv.leikelen.src.ProcessingModule
{
    public class ProcessingLoader
    {
        public List<IProcessingModule> ProcessingModules { get; private set; }

        private static ProcessingLoader _instance;

        public static ProcessingLoader Instance
        {
            get
            {
                if (_instance == null) _instance = new ProcessingLoader();
                return _instance;
            }
        }

        private ProcessingLoader()
        {
            ProcessingModules = new List<IProcessingModule>();
        }
    }
}
