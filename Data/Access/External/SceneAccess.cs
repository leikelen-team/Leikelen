using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.Data.Persistence;

namespace cl.uv.leikelen.Data.Access.External
{
    public class SceneAccess : ISceneAccess
    {
        public bool Exists(int sceneId)
        {
            return DbFacade.Instance.Provider.LoadScene(sceneId) != null;
        }

        public Scene Get(int sceneId)
        {
            return DbFacade.Instance.Provider.LoadScene(sceneId);
        }

        public List<Scene> GetAll()
        {
            return DbFacade.Instance.Provider.LoadScenes();
        }
    }
}
