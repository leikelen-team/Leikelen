using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API.InputModule;

namespace cl.uv.leikelen.src.InputModule
{
    public class InputLoader
    {
        public List<InputType> InputModules { get; private set; }
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
            InputModules = new List<InputType>();

            InputModules.Add(new OpenBCI.OpenBCISensor());
        }
    }
}
