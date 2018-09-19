using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.API.DataAccess
{
    /// <summary>
    /// Interface to access the scene
    /// </summary>
    public interface ISceneAccess
    {
        /// <summary>
        /// Gets all scenes in the database.
        /// </summary>
        /// <returns>A list of the scenes</returns>
        List<Data.Model.Scene> GetAll();

        /// <summary>
        /// Gets the specified scene.
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns>The scene</returns>
        Data.Model.Scene Get(int sceneId);

        /// <summary>
        /// Verify if Exists the specified scene.
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns>True if exists, False if not</returns>
        bool Exists(int sceneId);

        /// <summary>
        /// Saves the scene if not exists another with the same id (or the id is 0)
        /// or update otherwise.
        /// </summary>
        /// <param name="scene">The scene object.</param>
        /// <returns>The new Scene</returns>
        Data.Model.Scene SaveOrUpdate(Data.Model.Scene scene);

        /// <summary>
        /// Saves the scene as a new object, changing all ids 
        /// (and in all related objects inside) according the lasts in database.
        /// </summary>
        /// <param name="scene">The scene object.</param>
        /// <returns>The new Scene</returns>
        Data.Model.Scene SaveNew(Data.Model.Scene scene);

        /// <summary>
        /// Deletes the specified scene.
        /// </summary>
        /// <param name="scene">The scene object.</param>
        void Delete(Data.Model.Scene scene);
    }
}
