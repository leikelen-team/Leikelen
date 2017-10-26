using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.Helper
{
    /// <summary>
    /// Interface for the tabs in the main window
    /// </summary>
    /// <seealso cref="View.Home"/>
    public interface ITab
    {
        /// <summary>
        /// Fills this instance. Called when a new scene is set,
        /// or the scene has stopped recording
        /// </summary>
        void Fill();
        /// <summary>
        /// Resets and clear this instance. 
        /// Called before fill the actual tab.
        /// </summary>
        void Reset();
    }
}
