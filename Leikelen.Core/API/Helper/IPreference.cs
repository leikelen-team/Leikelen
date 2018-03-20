using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.Helper
{
    /// <summary>
    /// Interface for the preference tabs
    /// </summary>
    /// <seealso cref="View.Preferences"/>
    public interface IPreference
    {
        /// <summary>
        /// Save the preferences of this instance.
        /// </summary>
        void Apply();
    }
}
