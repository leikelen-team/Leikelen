using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Data.Access.External
{
    public class SceneInUseAccess
    {
        private static SceneInUseAccess _instance;

        public static SceneInUseAccess Instance
        {
            get
            {
                if (_instance == null) _instance = new SceneInUseAccess();
                return _instance;
            }
        }

        private SceneInUseAccess()
        {

        }

        public TimeSpan? GetLocation()
        {
            if (Internal.SceneInUse.Instance.IsRecording)
                return DateTime.Now.Subtract(Internal.SceneInUse.Instance.Scene.RecordStartedDateTime);
            return null;
        }
    }
}
