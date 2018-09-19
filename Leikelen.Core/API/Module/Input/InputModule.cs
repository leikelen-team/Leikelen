using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.API.Module.Input
{
    /// <summary>
    /// Module that represent a sensor or input device who save 
    /// and feed data to the different <see cref="Processing.ProcessingModule"/>
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.AbstractModule" />
    public abstract class InputModule : AbstractModule
    {
        /// <summary>
        /// Whether is a sensor that captures all the scene, 
        /// or only a person's data.
        /// </summary>
        public API.Module.Input.InputPlurality Plurality;
        /// <summary>
        /// The monitor of this sensor, that captures the data directly of the device, 
        /// and allows to record and feed data to the different <see cref="Processing.ProcessingModule"/>
        /// </summary>
        public API.Module.Input.IMonitor Monitor;
        /// <summary>
        /// The player of this sensor, that reads the saved data (if exists), 
        /// and allow to reproduce it off-line at any moment.
        /// </summary>
        public API.Module.Input.IPlayer Player;
    }
}
