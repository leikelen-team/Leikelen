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
            return !ReferenceEquals(null, DbFacade.Instance.Provider.LoadScene(sceneId));
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
            var settings = DataAccessFacade.Instance.GetGeneralSettings();

            if (ReferenceEquals(null, DbFacade.Instance.Provider.LoadScene(scene.SceneId)))
                sceneReturned = DbFacade.Instance.Provider.SaveScene(scene);
            else
                sceneReturned = DbFacade.Instance.Provider.UpdateScene(scene);
            if (!ReferenceEquals(null, sceneReturned) && !System.IO.Directory.Exists(settings.GetSceneDirectory(sceneReturned.SceneId)))
            {
                System.IO.Directory.CreateDirectory(settings.GetSceneDirectory(sceneReturned.SceneId));
            }
            return sceneReturned;
        }

        public Scene SaveNew(Scene scene)
        {
            var sceneReturned = DbFacade.Instance.Provider.SaveNewScene(scene);
            var settings = DataAccessFacade.Instance.GetGeneralSettings();

            if (!ReferenceEquals(null, sceneReturned) && !System.IO.Directory.Exists(settings.GetSceneDirectory(sceneReturned.SceneId)))
            {
                System.IO.Directory.CreateDirectory(settings.GetSceneDirectory(sceneReturned.SceneId));
            }
            return sceneReturned;
        }

        public void Delete(Scene scene)
        {
            var settings = DataAccessFacade.Instance.GetGeneralSettings();

            if (!System.IO.Directory.Exists(settings.GetSceneDirectory(scene.SceneId)))
            {
                System.IO.Directory.Delete(settings.GetSceneDirectory(scene.SceneId), true);
            }
            DbFacade.Instance.Provider.DeleteScene(scene);
        }
    }
}
