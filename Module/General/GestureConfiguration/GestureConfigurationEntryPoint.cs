using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.General;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;

namespace cl.uv.leikelen.Module.General.GestureConfiguration
{
    public class GestureConfigurationEntryPoint : GeneralModule
    {
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public GestureConfigurationEntryPoint()
        {
            if (!_dataAccessFacade.GetModalAccess().Exists("Discrete Posture"))
                _dataAccessFacade.GetModalAccess().Add("Discrete Posture",
                    "Evaluates every moment and return true or false");
            if (!_dataAccessFacade.GetModalAccess().Exists("Continuous Posture"))
                _dataAccessFacade.GetModalAccess().Add("Continuous Posture",
                    "Evaluates every moment and return a number associated to the progress");

            Name = Properties.GestureConfiguration.ModuleName;
            Windows = new List<Tuple<string, WindowBuilder>>()
            {
                new Tuple<string, WindowBuilder>(Properties.GestureConfiguration.PostureCRUDTitle,
                    new WindowBuilder(new View.PostureCRUD()))
            };
        }
    }
}
