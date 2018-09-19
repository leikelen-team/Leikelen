using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.API.DataAccess
{
    /// <summary>
    /// Interface to access the modal types
    /// </summary>
    public interface IModalAccess
    {
        /// <summary>
        /// Gets all modal types in the database.
        /// </summary>
        /// <returns>A list of all modal types (without joins)</returns>
        List<Data.Model.ModalType> GetAll();

        /// <summary>
        /// Adds a new modal type.
        /// </summary>
        /// <param name="name">The name of the modal type (its the id, and must to be not null and unique).</param>
        /// <param name="description">The description (optional, may be null).</param>
        /// <returns>The new modal type</returns>
        Data.Model.ModalType Add(string name, string description);

        /// <summary>
        /// Adds a new modal type if not exists, otherwise, returns the actual modal type of the given name.
        /// </summary>
        /// <param name="name">The name of the modal type (its the id, and must to be not null and unique).</param>
        /// <param name="description">The description (optional, may be null).</param>
        /// <returns>The new modal type, if exists, the actual modal type with same name</returns>
        Data.Model.ModalType AddIfNotExists(string name, string description);

        /// <summary>
        /// Verify is exists a modal type of given name.
        /// </summary>
        /// <param name="name">The name of the modal type.</param>
        /// <returns>True if already exists, otherwise False</returns>
        bool Exists(string name);
    }
}
