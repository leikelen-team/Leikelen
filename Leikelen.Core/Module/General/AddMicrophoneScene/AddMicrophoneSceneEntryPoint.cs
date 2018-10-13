using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.General;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;

namespace cl.uv.leikelen.Module.General.AddMicrophoneScene
{
    public class AddMicrophoneSceneEntryPoint : API.Module.General.GeneralModule
    {
        private API.DataAccess.IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        /// <summary>
        /// Initializes a new instance of the <see cref="AddMicrophoneSceneEntryPoint"/> class.
        /// </summary>
        public AddMicrophoneSceneEntryPoint()
        {
            Name = "Add Microphone Scene";

            _dataAccessFacade.GetModalAccess().AddIfNotExists("Talk",
                "Whether is talking or not");

            Windows = new List<Tuple<string, WindowBuilder>>()
            {
                new Tuple<string, WindowBuilder>("Add Mic Data",
                    new WindowBuilder(new View.AddMicrophoneData()))
            };

        }
    }
}
