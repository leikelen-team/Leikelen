using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.API.DataAccess;

namespace cl.uv.leikelen.Data.Access.External
{
    /// <summary>
    /// Class to access the scene in use
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.DataAccess.ISceneInUseAccess" />
    public class SceneInUseAccess : ISceneInUseAccess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Access.External.SceneInUseAccess"/> class.
        /// </summary>
        public SceneInUseAccess()
        {

        }

        /// <summary>
        /// Gets the scene in use.
        /// </summary>
        /// <returns>The scene in use</returns>
        public Data.Model.Scene GetScene()
        {
            return Internal.SceneInUse.Instance.Scene;
        }

        /// <summary>
        /// Gets the time location, if its not recording returns Null.
        /// </summary>
        /// <returns>The time location, if its not recording returns Null</returns>
        public TimeSpan? GetLocation()
        {
            if (Internal.SceneInUse.Instance.IsRecording)
                return DateTime.Now.Subtract(Internal.SceneInUse.Instance.Scene.RecordStartedDateTime);
            else if (Internal.SceneInUse.Instance.PlayStartTime.HasValue)
                return DateTime.Now.Subtract(Internal.SceneInUse.Instance.PlayStartTime.Value);
            else
                return null;
        }
    }
}
