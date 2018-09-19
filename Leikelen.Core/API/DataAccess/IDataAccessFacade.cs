using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Interfaces to access the data.
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
        API.DataAccess.IEventAccess GetEventAccess();

        /// <summary>
        /// Gets the interval access.
        /// </summary>
        /// <returns>The object to access the interval represent types</returns>
        API.DataAccess.IIntervalAccess GetIntervalAccess();

        /// <summary>
        /// Gets the modal access.
        /// </summary>
        /// <returns>The object to access the modal types</returns>
        API.DataAccess.IModalAccess GetModalAccess();

        /// <summary>
        /// Gets the person access.
        /// </summary>
        /// <returns>The object to access the persons</returns>
        API.DataAccess.IPersonAccess GetPersonAccess();

        /// <summary>
        /// Gets the scene access.
        /// </summary>
        /// <returns>The object to access the scenes</returns>
        API.DataAccess.ISceneAccess GetSceneAccess();

        /// <summary>
        /// Gets the sub modal access.
        /// </summary>
        /// <returns>The object to access the sub modal types</returns>
        API.DataAccess.ISubModalAccess GetSubModalAccess();

        /// <summary>
        /// Gets the timeless access.
        /// </summary>
        /// <returns>The object to access the array represent types</returns>
        API.DataAccess.ITimelessAccess GetTimelessAccess();

        /// <summary>
        /// Gets the scene in use access.
        /// </summary>
        /// <returns>The object to access the actual scene in use</returns>
        API.DataAccess.ISceneInUseAccess GetSceneInUseAccess();

        /// <summary>
        /// Gets the general settings.
        /// </summary>
        /// <returns>The object to get the general settings of the app</returns>
        API.DataAccess.IGeneralSettings GetGeneralSettings();

        //TODO: sacar de aqui ._.
        /// <summary>
        /// Deletes the database and all its tables.
        /// </summary>
        void DeleteDatabase();
    }
}
