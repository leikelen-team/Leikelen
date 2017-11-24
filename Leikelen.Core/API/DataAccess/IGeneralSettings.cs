using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface IGeneralSettings
    {
        string GetDataDirectory();
        string GetTmpDirectory();
        string GetTmpSceneDirectory();
        string GetModalDirectory(string modalTypeName);
        string GetDataPersonsDirectory();
        string GetDataScenesDirectory();
        string GetDataModalsDirectory();
        string GetSceneInUseDirectory();
        string GetSceneDirectory(int sceneId);
        int GetDefaultMillisecondsThreshold();
    }
}
