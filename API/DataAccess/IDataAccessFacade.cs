using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Package with interfaces to access the data
/// </summary>
namespace cl.uv.leikelen.API.DataAccess
{
    /// <summary>
    /// Facade to get access to the different types of data
    /// </summary>
    /// <seealso cref="Data.Access"/>
    /// <seealso cref="Data.Model"/>
    public interface IDataAccessFacade
    {
        /// <summary>
        /// Gets the event access.
        /// </summary>
        /// <returns>The object to access the event represent types</returns>
        IEventAccess GetEventAccess();
        /// <summary>
        /// Gets the interval access.
        /// </summary>
        /// <returns>The object to access the interval represent types</returns>
        IIntervalAccess GetIntervalAccess();
        /// <summary>
        /// Gets the modal access.
        /// </summary>
        /// <returns>The object to access the modal types</returns>
        IModalAccess GetModalAccess();
        /// <summary>
        /// Gets the person access.
        /// </summary>
        /// <returns>The object to access the persons</returns>
        IPersonAccess GetPersonAccess();
        /// <summary>
        /// Gets the scene access.
        /// </summary>
        /// <returns>The object to access the scenes</returns>
        ISceneAccess GetSceneAccess();
        /// <summary>
        /// Gets the sub modal access.
        /// </summary>
        /// <returns>The object to access the sub modal types</returns>
        ISubModalAccess GetSubModalAccess();
        /// <summary>
        /// Gets the timeless access.
        /// </summary>
        /// <returns>The object to access the array represent types</returns>
        ITimelessAccess GetTimelessAccess();
        /// <summary>
        /// Gets the scene in use access.
        /// </summary>
        /// <returns>The object to access the actual scene in use</returns>
        ISceneInUseAccess GetSceneInUseAccess();
        /// <summary>
        /// Gets the general settings.
        /// </summary>
        /// <returns>The object to get the general settings of the app</returns>
        IGeneralSettings GetGeneralSettings();

        /// <summary>
        /// Deletes the database and all its tables.
        /// </summary>
        void DeleteDatabase();
    }
}
