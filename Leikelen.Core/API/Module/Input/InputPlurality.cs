using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.Module.Input
{
    /// <summary>
    /// Plurality of the input device.
    /// </summary>
    public enum InputPlurality
    {
        /// <summary>
        /// Represent a sensor that captures data of the entire scene.
        /// </summary>
        Scene,
        /// <summary>
        /// Represent a sensor that captures data of only one person.
        /// </summary>
        Person
    }
}
