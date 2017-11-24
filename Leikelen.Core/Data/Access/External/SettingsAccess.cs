using cl.uv.leikelen.API.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cl.uv.leikelen.Data.Access.External
{
    public class SettingsAccess : IGeneralSettings
    {
        public string GetDataDirectory()
        {
            return GeneralSettings.Instance.DataDirectory.Value;
        }

        public int GetDefaultMillisecondsThreshold()
        {
            return GeneralSettings.Instance.DefaultMillisecondsThreshold.Value;
        }

        public string GetModalDirectory(string modalTypeName)
        {
            return Path.Combine(GeneralSettings.Instance.DataDirectory.Value, Path.Combine("modal/", modalTypeName));
        }

        public string GetDataPersonsDirectory()
        {
            return Path.Combine(GeneralSettings.Instance.DataDirectory.Value, "person/");
        }

        public string GetSceneInUseDirectory()
        {
            if(!ReferenceEquals(null, Internal.SceneInUse.Instance) 
                && !ReferenceEquals(null, Internal.SceneInUse.Instance.Scene))
                return Path.Combine(Path.Combine(GeneralSettings.Instance.DataDirectory.Value, "scene/"), 
                    Internal.SceneInUse.Instance.Scene.SceneId.ToString());
            return null;

        }

        public string GetTmpDirectory()
        {
            return GeneralSettings.Instance.TmpDirectory.Value;
        }

        public string GetTmpSceneDirectory()
        {
            return GeneralSettings.Instance.TmpSceneDirectory.Value;
        }

        public string GetDataScenesDirectory()
        {
            return Path.Combine(GeneralSettings.Instance.DataDirectory.Value, "scene/");
        }

        public string GetDataModalsDirectory()
        {
            return Path.Combine(GeneralSettings.Instance.DataDirectory.Value, "modal/");
        }

        public string GetSceneDirectory(int sceneId)
        {
            if(!ReferenceEquals(null, new SceneAccess().Get(sceneId)))
                return Path.Combine(Path.Combine(GeneralSettings.Instance.DataDirectory.Value, "scene/"), sceneId.ToString());
            return null;
        }
    }
}
