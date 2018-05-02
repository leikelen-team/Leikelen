using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Module;

namespace cl.uv.leikelen.Controller
{
    /// <summary>
    /// Controller to manage the sensors
    /// </summary>
    public class MediaController
    {
        /// <summary>
        /// Sets from none sensor (for initial state).
        /// </summary>
        public void SetFromNone()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Player.Close();
                input.Monitor.Close();
            }
        }
    }
}
