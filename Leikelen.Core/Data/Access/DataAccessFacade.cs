﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access.External;
using cl.uv.leikelen.Data.Persistence;

namespace cl.uv.leikelen.Data.Access
{
    /// <summary>
    /// Facade to get access to the different types of data
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.DataAccess.IDataAccessFacade" />
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

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DataAccessFacade Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new DataAccessFacade();
                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessFacade"/> class.
        /// </summary>
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
            _generalSettings = new SettingsAccess();
        }

        /// <summary>
        /// Gets the event access.
        /// </summary>
        /// <returns>The object to access the event represent types</returns>
        public IEventAccess GetEventAccess()
        {
            return _eventAccess;
        }

        /// <summary>
        /// Gets the interval access.
        /// </summary>
        /// <returns>The object to access the interval represent types</returns>
        public IIntervalAccess GetIntervalAccess()
        {
            return _intervalAccess;
        }

        /// <summary>
        /// Gets the modal access.
        /// </summary>
        /// <returns>The object to access the modal types</returns>
        public IModalAccess GetModalAccess()
        {
            return _modalAccess;
        }

        /// <summary>
        /// Gets the person access.
        /// </summary>
        /// <returns>The object to access the persons</returns>
        public IPersonAccess GetPersonAccess()
        {
            return _personAccess;
        }

        /// <summary>
        /// Gets the scene access.
        /// </summary>
        /// <returns>The object to access the scenes</returns>
        public ISceneAccess GetSceneAccess()
        {
            return _sceneAccess;
        }

        /// <summary>
        /// Gets the scene in use access.
        /// </summary>
        /// <returns>The object to access the actual scene in use</returns>
        public ISceneInUseAccess GetSceneInUseAccess()
        {
            return _sceneInUseAccess;
        }

        /// <summary>
        /// Gets the sub modal access.
        /// </summary>
        /// <returns>The object to access the sub modal types</returns>
        public ISubModalAccess GetSubModalAccess()
        {
            return _subModalAccess;
        }

        /// <summary>
        /// Gets the timeless access.
        /// </summary>
        /// <returns>The object to access the array represent types</returns>
        public ITimelessAccess GetTimelessAccess()
        {
            return _timelessAccess;
        }

        /// <summary>
        /// Gets the general settings.
        /// </summary>
        /// <returns>The object to get the general settings of the app</returns>
        public IGeneralSettings GetGeneralSettings()
        {
            return _generalSettings;
        }

        //TODO: sacar de aqui ._.
        /// <summary>
        /// Deletes the database and all its tables.
        /// </summary>
        public void DeleteDatabase()
        {
            DbFacade.Instance.Provider.Delete();
        }
    }
}
