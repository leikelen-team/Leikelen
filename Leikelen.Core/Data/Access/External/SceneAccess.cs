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
    /// <summary>
    /// Class to access the scenes
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.DataAccess.ISceneAccess" />
    public class SceneAccess : ISceneAccess
    {
        /// <summary>
        /// Verify if Exists the specified scene.
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns>True if exists, False if not</returns>
        public bool Exists(int sceneId)
        {
            return !ReferenceEquals(null, DbFacade.Instance.Provider.LoadScene(sceneId));
        }

        /// <summary>
        /// Gets the specified scene.
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns>The scene</returns>
        public Data.Model.Scene Get(int sceneId)
        {
            return DbFacade.Instance.Provider.LoadScene(sceneId);
        }

        /// <summary>
        /// Gets all scenes in the database.
        /// </summary>
        /// <returns>A list of the scenes</returns>
        public List<Data.Model.Scene> GetAll()
        {
            return DbFacade.Instance.Provider.LoadScenes();
        }

        /// <summary>
        /// Saves the scene if not exists another with the same id (or the id is 0)
        /// or update otherwise.
        /// </summary>
        /// <param name="scene">The scene object.</param>
        /// <returns>The new Scene</returns>
        public Data.Model.Scene SaveOrUpdate(Data.Model.Scene scene)
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

        /// <summary>
        /// Saves the scene as a new object, changing all ids 
        /// (and in all related objects inside) according the lasts in database.
        /// </summary>
        /// <param name="scene">The scene object.</param>
        /// <returns>The new Scene</returns>
        public Data.Model.Scene SaveNew(Data.Model.Scene scene)
        {
            var sceneReturned = DbFacade.Instance.Provider.SaveNewScene(scene);
            var settings = DataAccessFacade.Instance.GetGeneralSettings();

            if (!ReferenceEquals(null, sceneReturned) && !System.IO.Directory.Exists(settings.GetSceneDirectory(sceneReturned.SceneId)))
            {
                System.IO.Directory.CreateDirectory(settings.GetSceneDirectory(sceneReturned.SceneId));
            }
            return sceneReturned;
        }

        /// <summary>
        /// Deletes the specified scene.
        /// </summary>
        /// <param name="scene">The scene object.</param>
        public void Delete(Data.Model.Scene scene)
        {
            var settings = DataAccessFacade.Instance.GetGeneralSettings();

            if (System.IO.Directory.Exists(settings.GetSceneDirectory(scene.SceneId)))
            {
                System.IO.Directory.Delete(settings.GetSceneDirectory(scene.SceneId), true);
            }
            DbFacade.Instance.Provider.DeleteScene(scene);
        }
    }
}
