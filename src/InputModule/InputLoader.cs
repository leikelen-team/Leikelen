using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.InputModule;

namespace cl.uv.leikelen.InputModule
{
    public class InputLoader
    {
        public List<API.InputModule.InputModule> InputModules { get; private set; }
        public IVideo VideoHandler { get; private set; }
        private static InputLoader _instance;

        public static InputLoader Instance
        {
            get
            {
                if (_instance == null) _instance = new InputLoader();
                return _instance;
            }
        }

        private InputLoader()
        {
            InputModules = new List<API.InputModule.InputModule>();

            InputModules.Add(new OpenBCI.OpenBCISensor());
        }
    }
}
