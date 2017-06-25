using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.Data.Model;

namespace cl.uv.leikelen.src.Data.Access.Internal
{
    public class SceneInUse
    {
        private static SceneInUse _instance;
        public Scene Scene;

        public static SceneInUse Instance
        {
            get
            {
                if (_instance == null) _instance = new SceneInUse();
                return _instance;
            }
        }

        private SceneInUse()
        {

        }

        public void Set(Scene scene)
        {
            this.Scene = scene;
        }
    }
}
