using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cl.uv.leikelen.API.Helper
{
    /// <summary>
    /// Helper class to create clones of a determined window.
    /// </summary>
    /// <seealso cref="API.Module.AbstractModule.Windows"/>
    public class WindowBuilder
    {
        /// <summary>
        /// The window associated with this instance.
        /// </summary>
        private readonly ICloneable _window;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowBuilder"/> class.
        /// </summary>
        /// <param name="window">The clonable window, 
        /// must be an instance of <see cref="System.Windows.Window"/>.</param>
        public WindowBuilder(ICloneable window)
        {
            _window = window;
        }

        /// <summary>
        /// Gets a cloned instance of the associated window.
        /// </summary>
        /// <returns>A cloned instance of associated window</returns>
        public Window GetWindow()
        {
            return (Window) _window.Clone(); 
        }
    }
}
