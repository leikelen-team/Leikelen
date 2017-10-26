using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.Module.Input
{
    /// <summary>
    /// Current sensor's status
    /// </summary>
    public enum InputStatus
    {
        /// <summary>
        /// The sensor is connected succesfully
        /// </summary>
        Connected,
        /// <summary>
        /// The sensor is unconnected
        /// </summary>
        Unconnected,
        /// <summary>
        /// The sensor has errors at connecting
        /// </summary>
        Error
    }
}
