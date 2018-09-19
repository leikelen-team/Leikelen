using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Helper;

/// <summary>
/// API for modules.
/// </summary>
namespace cl.uv.leikelen.API.Module
{
    /// <summary>
    /// Abstract class for the different modules, 
    /// see <see cref="General.GeneralModule"/>, 
    /// <see cref="Input.InputModule"/>, 
    /// and <see cref="Processing.ProcessingModule"/>.
    /// </summary>
    public abstract class AbstractModule
    {
        /// <summary>
        /// Gets or sets the name of this module.
        /// </summary>
        /// <value>
        /// The name of this module.
        /// </value>
        public string Name { get; protected set; }
        /// <summary>
        /// Gets or sets the windows.
        /// </summary>
        /// <value>
        /// A list of this module's windows.
        /// </value>
        public List<Tuple<string, API.Helper.WindowBuilder>> Windows { get; protected set; } = new List<Tuple<string, API.Helper.WindowBuilder>>();
        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled { get; private set; } = false;
        /// <summary>
        /// Gets or sets the tabs.
        /// </summary>
        /// <value>
        /// A list of this module's tabs.
        /// </value>
        public List<API.Helper.ITab> Tabs { get; protected set; } = new List<API.Helper.ITab>();

        /// <summary>
        /// Enables this instance.
        /// </summary>
        public void Enable()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Disables this instance.
        /// </summary>
        public void Disable()
        {
            IsEnabled = false;
        }
    }
}
