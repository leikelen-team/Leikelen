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
    /// <summary>
    /// General module to manage the postures
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.General.GeneralModule" />
    public class GestureConfigurationEntryPoint : GeneralModule
    {
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureConfigurationEntryPoint"/> class.
        /// </summary>
        public GestureConfigurationEntryPoint()
        {
            _dataAccessFacade.GetModalAccess().AddIfNotExists("Discrete Posture",
                "Evaluates every moment and return true or false");
            _dataAccessFacade.GetModalAccess().AddIfNotExists("Continuous Posture",
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
