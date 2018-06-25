using cl.uv.leikelen.API.Module.Input;
using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Mindwave EEG input sensor.
/// </summary>
namespace cl.uv.leikelen.Module.Input.MindwaveTGC
{
    /// <summary>
    /// Entry point for the Mindwave Eeg input sensor using the ThinkGear Connector API
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Input.InputModule" />
    public class MindwaveTGCInput : InputModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MindwaveTGCInput"/> class.
        /// </summary>
        /// <param name="person">The person.</param>
        public MindwaveTGCInput(Person person)
        {
            Plurality = InputPlurality.Person;
            var myMonitor = new Monitor(person);
            Monitor = myMonitor as IMonitor;
            Name = "Mindwave";
            Player = new Player();

        }
    }
}
