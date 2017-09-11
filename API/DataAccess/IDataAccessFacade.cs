using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface IDataAccessFacade
    {
        IEventAccess GetEventAccess();
        IIntervalAccess GetIntervalAccess();
        IModalAccess GetModalAccess();
        IPersonAccess GetPersonAccess();
        ISceneAccess GetSceneAccess();
        ISubModalAccess GetSubModalAccess();
        ITimelessAccess GetTimelessAccess();
        ISceneInUseAccess GetSceneInUseAccess();
        IGeneralSettings GetGeneralSettings();
    }
}
