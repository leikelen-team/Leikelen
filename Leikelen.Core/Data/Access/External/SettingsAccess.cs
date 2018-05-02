using cl.uv.leikelen.API.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cl.uv.leikelen.Data.Access.External
{
    /// <summary>
    /// Class to get the settings
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.DataAccess.IGeneralSettings" />
    public class SettingsAccess : IGeneralSettings
    {
        /// <summary>
        /// Gets the data directory from settings file.
        /// </summary>
        /// <returns>the data directory</returns>
        public string GetDataDirectory()
        {
            return GeneralSettings.Instance.DataDirectory.Value;
        }

        /// <summary>
        /// Gets the default milliseconds threshold, used to create the intervals from events.
        /// </summary>
        /// <returns>the default milliseconds threshold</returns>
        public int GetDefaultMillisecondsThreshold()
        {
            return GeneralSettings.Instance.DefaultMillisecondsThreshold.Value;
        }

        /// <summary>
        /// Gets the directory of the modal type.
        /// </summary>
        /// <param name="modalTypeName">Name of the modal type.</param>
        /// <returns>the directory of the modal type</returns>
        public string GetModalDirectory(string modalTypeName)
        {
            return Path.Combine(GeneralSettings.Instance.DataDirectory.Value, Path.Combine("modal/", modalTypeName));
        }

        /// <summary>
        /// Gets the data directory of the persons.
        /// </summary>
        /// <returns>the directory of data of all persons</returns>
        public string GetDataPersonsDirectory()
        {
            return Path.Combine(GeneralSettings.Instance.DataDirectory.Value, "person/");
        }

        /// <summary>
        /// Gets the directory of the scene in use.
        /// </summary>
        /// <returns>the directory of the scene in use</returns>
        public string GetSceneInUseDirectory()
        {
            if(!ReferenceEquals(null, Internal.SceneInUse.Instance) 
                && !ReferenceEquals(null, Internal.SceneInUse.Instance.Scene))
                return Path.Combine(Path.Combine(GeneralSettings.Instance.DataDirectory.Value, "scene/"), 
                    Internal.SceneInUse.Instance.Scene.SceneId.ToString());
            return null;

        }

        /// <summary>
        /// Gets the temporary directory from settings file.
        /// </summary>
        /// <returns>the temporary directory</returns>
        public string GetTmpDirectory()
        {
            return GeneralSettings.Instance.TmpDirectory.Value;
        }

        /// <summary>
        /// Gets the temporary scene directory from settings file.
        /// </summary>
        /// <returns>the temporary scene directory</returns>
        public string GetTmpSceneDirectory()
        {
            return GeneralSettings.Instance.TmpSceneDirectory.Value;
        }

        /// <summary>
        /// Gets the data directory of the scenes.
        /// </summary>
        /// <returns>the data directory of the scenes</returns>
        public string GetDataScenesDirectory()
        {
            return Path.Combine(GeneralSettings.Instance.DataDirectory.Value, "scene/");
        }

        /// <summary>
        /// Gets the data directory of modal types.
        /// </summary>
        /// <returns>the data directory of modal types</returns>
        public string GetDataModalsDirectory()
        {
            return Path.Combine(GeneralSettings.Instance.DataDirectory.Value, "modal/");
        }

        /// <summary>
        /// Gets the scene directory.
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns>the scene directory</returns>
        public string GetSceneDirectory(int sceneId)
        {
            if(new SceneAccess()?.Get(sceneId) != null)
                return Path.Combine(Path.Combine(GeneralSettings.Instance.DataDirectory.Value, "scene/"), sceneId.ToString());
            return null;
        }

        /// <summary>
        /// Gets the default index of a scene in an imported file.
        /// </summary>
        /// <returns>the default index of an imported scene</returns>
        public int GetIndexOfSceneInFile()
        {
            return GeneralSettings.Instance.IndexOfSceneInFile.Value;
        }
    }
}
