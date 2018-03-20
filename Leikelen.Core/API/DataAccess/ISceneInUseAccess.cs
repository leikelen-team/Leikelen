using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.API.DataAccess
{
    public interface ISceneInUseAccess
    {
        Scene GetScene();
        TimeSpan? GetLocation();
    }
}
