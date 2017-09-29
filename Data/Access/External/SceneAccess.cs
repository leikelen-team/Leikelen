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

        public Scene SaveOrUpdate(Scene scene)
        {
            Scene sceneReturned = null;
            if (DbFacade.Instance.Provider.LoadScene(scene.SceneId) == null)
                sceneReturned = DbFacade.Instance.Provider.SaveScene(scene);
            else
                sceneReturned = DbFacade.Instance.Provider.UpdateScene(scene);
            if (sceneReturned != null && !System.IO.Directory.Exists(GeneralSettings.Instance.GetDataDirectory()+"scene/"+ sceneReturned.SceneId))
            {
                System.IO.Directory.CreateDirectory(GeneralSettings.Instance.GetDataDirectory() + "scene/" + sceneReturned.SceneId);
            }
            return sceneReturned;
        }

        public Scene SaveNew(Scene scene)
        {
            var sceneReturned = DbFacade.Instance.Provider.SaveNewScene(scene);
            if (sceneReturned != null && !System.IO.Directory.Exists(GeneralSettings.Instance.GetDataDirectory() + "scene/" + sceneReturned.SceneId))
            {
                System.IO.Directory.CreateDirectory(GeneralSettings.Instance.GetDataDirectory() + "scene/" + sceneReturned.SceneId);
            }
            return sceneReturned;
        }

        public void Delete(Scene scene)
        {
            DbFacade.Instance.Provider.DeleteScene(scene);
            if (!System.IO.Directory.Exists(GeneralSettings.Instance.GetDataDirectory() + "scene/" + scene?.SceneId))
            {
                System.IO.Directory.Delete(GeneralSettings.Instance.GetDataDirectory() + "scene/" + scene?.SceneId, true);
            }
        }
    }
}
