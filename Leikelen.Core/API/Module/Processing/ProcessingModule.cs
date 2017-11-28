using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.API.Module.Processing
{
    /// <summary>
    /// Interface for the processing modules, that recieve data from sensors,
    /// and can insert/update data to the scene.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.AbstractModule" />
    public abstract class ProcessingModule : AbstractModule
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is active before recording.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active before recording; otherwise, <c>false</c>.
        /// </value>
        public bool IsActiveBeforeRecording { get; protected set; } = false;

        /// <summary>
        /// Functions called after the recorder stops.
        /// </summary>
        /// <returns></returns>
        public abstract Action FunctionAfterStop();
    }
}
