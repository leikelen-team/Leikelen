using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cl.uv.leikelen.API.Module.Input;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Module.Input.OpenBCI
{
    /// <summary>
    /// Entry point for OpenBCI sensor input for a person.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Input.InputModule" />
    public sealed class OpenBCIInput : InputModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenBCIInput"/> class.
        /// </summary>
        /// <param name="person">The person.</param>
        public OpenBCIInput(Person person)
        {
            Plurality = InputPlurality.Person;
            var myMonitor = new Monitor(person);
            Tabs.Add(myMonitor.GraphTab);
            Monitor = myMonitor as IMonitor;
            Name = Properties.OpenBCI.SensorName;
            Player = new Player();
            Windows = new List<Tuple<string, WindowBuilder>>
            {
                new Tuple<string, WindowBuilder>(Properties.OpenBCI.ConfigWindowTitle,
                new WindowBuilder(new View.OpenBCIWindow(Monitor)))
            };


        }
    }
}
