using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access.External;

namespace cl.uv.leikelen.Data.Access
{
    public class DataAccessFacade : IDataAccessFacade
    {
        private static DataAccessFacade _instance;

        private IEventAccess _eventAccess;
        private IIntervalAccess _intervalAccess;
        private IModalAccess _modalAccess;
        private IPersonAccess _personAccess;
        private ISceneAccess _sceneAccess;
        private ISubModalAccess _subModalAccess;
        private ITimelessAccess _timelessAccess;
        private ISceneInUseAccess _sceneInUseAccess;
        private IGeneralSettings _generalSettings;

        public static DataAccessFacade Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new DataAccessFacade();
                return _instance;
            }
        }

        public DataAccessFacade()
        {
            _eventAccess = new EventAccess();
            _intervalAccess = new IntervalAccess();
            _modalAccess = new ModalAccess();
            _personAccess = new PersonAccess();
            _sceneAccess = new SceneAccess();
            _subModalAccess = new SubModalAccess();
            _timelessAccess = new TimelessAccess();
            _sceneInUseAccess = new SceneInUseAccess();
            _generalSettings = new GeneralSettings();
        }

        public IEventAccess GetEventAccess()
        {
            return _eventAccess;
        }

        public IIntervalAccess GetIntervalAccess()
        {
            return _intervalAccess;
        }

        public IModalAccess GetModalAccess()
        {
            return _modalAccess;
        }

        public IPersonAccess GetPersonAccess()
        {
            return _personAccess;
        }

        public ISceneAccess GetSceneAccess()
        {
            return _sceneAccess;
        }

        public ISceneInUseAccess GetSceneInUseAccess()
        {
            return _sceneInUseAccess;
        }

        public ISubModalAccess GetSubModalAccess()
        {
            return _subModalAccess;
        }

        public ITimelessAccess GetTimelessAccess()
        {
            return _timelessAccess;
        }

        public IGeneralSettings GetGeneralSettings()
        {
            return _generalSettings;
        }
    }
}
