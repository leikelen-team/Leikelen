using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{

    /// <summary>
    /// interface to get the settings
    /// </summary>
    public interface IGeneralSettings
    {
        /// <summary>
        /// Gets the data directory from settings file.
        /// </summary>
        /// <returns>the data directory</returns>
        string GetDataDirectory();

        /// <summary>
        /// Gets the temporary directory from settings file.
        /// </summary>
        /// <returns>the temporary directory</returns>
        string GetTmpDirectory();

        /// <summary>
        /// Gets the temporary scene directory from settings file.
        /// </summary>
        /// <returns>the temporary scene directory</returns>
        string GetTmpSceneDirectory();

        /// <summary>
        /// Gets the directory of the modal type.
        /// </summary>
        /// <param name="modalTypeName">Name of the modal type.</param>
        /// <returns>the directory of the modal type</returns>
        string GetModalDirectory(string modalTypeName);

        /// <summary>
        /// Gets the data directory of the persons.
        /// </summary>
        /// <returns>the directory of data of all persons</returns>
        string GetDataPersonsDirectory();

        /// <summary>
        /// Gets the data directory of the scenes.
        /// </summary>
        /// <returns>the data directory of the scenes</returns>
        string GetDataScenesDirectory();

        /// <summary>
        /// Gets the data directory of modal types.
        /// </summary>
        /// <returns>the data directory of modal types</returns>
        string GetDataModalsDirectory();

        /// <summary>
        /// Gets the directory of the scene in use.
        /// </summary>
        /// <returns>the directory of the scene in use</returns>
        string GetSceneInUseDirectory();

        /// <summary>
        /// Gets the scene directory.
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns>the scene directory</returns>
        string GetSceneDirectory(int sceneId);

        /// <summary>
        /// Gets the default milliseconds threshold, used to create the intervals from events.
        /// </summary>
        /// <returns>the default milliseconds threshold</returns>
        int GetDefaultMillisecondsThreshold();

        /// <summary>
        /// Gets the default index of a scene in an imported file.
        /// </summary>
        /// <returns>the default index of an imported scene</returns>
        int GetIndexOfSceneInFile();
    }
}
