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
        string GetSceneInUseDirectory();
        int GetDefaultMillisecondsThreshold();
    }
}
