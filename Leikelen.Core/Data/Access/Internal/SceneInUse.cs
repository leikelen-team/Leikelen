using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

/// <summary>
/// Access classes that are not visible to the modules
/// </summary>
namespace cl.uv.leikelen.Data.Access.Internal
{
    /// <summary>
    /// Class for the static instance of the scene in use
    /// </summary>
    public class SceneInUse
    {
        private static Data.Access.Internal.SceneInUse _instance;

        /// <summary>
        /// Gets the scene in use.
        /// </summary>
        /// <value>
        /// The scene in use.
        /// </value>
        public Data.Model.Scene Scene { get; private set; }

        /// <summary>
        /// Verify is the scene is being is recording
        /// </summary>
        public bool IsRecording = false;

        /// <summary>
        /// Gets or sets the time where start to play (or null).
        /// </summary>
        /// <value>
        /// The time where start to play (or null).
        /// </value>
        public DateTime? PlayStartTime { get; set; }

        /// <summary>
        /// Gets or sets the time of the last time was paused (if its paused, otherwise null).
        /// </summary>
        /// <value>
        /// The last time was paused (if its paused, otherwise null).
        /// </value>
        public DateTime? PlayPausedTime { get; set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Data.Access.Internal.SceneInUse Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new SceneInUse();
                return _instance;
            }
        }

        private SceneInUse()
        {

        }

        /// <summary>
        /// Sets the scene in use.
        /// </summary>
        /// <param name="scene">The scene to use.</param>
        public void Set(Data.Model.Scene scene)
        {
            Scene = scene;
        }
    }
}
