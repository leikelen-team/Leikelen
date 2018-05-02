using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.API.DataAccess
{

    /// <summary>
    /// Interface to access the scene in use
    /// </summary>
    public interface ISceneInUseAccess
    {
        /// <summary>
        /// Gets the scene in use.
        /// </summary>
        /// <returns>The scene in use</returns>
        Scene GetScene();

        /// <summary>
        /// Gets the time location, if its not recording returns Null.
        /// </summary>
        /// <returns>The time location, if its not recording returns Null</returns>
        TimeSpan? GetLocation();
    }
}
