using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.API.DataAccess;

namespace cl.uv.leikelen.Data.Access.External
{
    public class SceneInUseAccess : ISceneInUseAccess
    {
        public SceneInUseAccess()
        {

        }

        public Scene GetScene()
        {
            return Internal.SceneInUse.Instance.Scene;
        }

        public TimeSpan? GetLocation()
        {
            if (Internal.SceneInUse.Instance.IsRecording)
                return DateTime.Now.Subtract(Internal.SceneInUse.Instance.Scene.RecordStartedDateTime);
            return null;
        }
    }
}
