using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.API.DataAccess
{
    /// <summary>
    /// Interface to access the submodal types
    /// </summary>
    public interface ISubModalAccess
    {
        /// <summary>
        /// Gets all submodal types of a given modal type.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <returns>List of submodal types</returns>
        List<Data.Model.SubModalType> GetAll(string modalName);

        /// <summary>
        /// Gets the modal type of the given name.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="name">The name of the submodal type.</param>
        /// <returns>The submodal type</returns>
        Data.Model.SubModalType Get(string modalName, string name);

        /// <summary>
        /// Adds a new submodal type.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="name">The name of the new submodal type.</param>
        /// <param name="description">The description (optional, may be null).</param>
        /// <param name="path">The path of file (only the fileName, optional, may be null).</param>
        /// <returns>The new Submodal type</returns>
        Data.Model.SubModalType Add(string modalName, string name, string description, string path);

        /// <summary>
        /// Adds a new submodal type if not exists, otherwise returns the 
        /// actual submodal type of the given modal and submodal type name.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="name">The name of the new submodal type.</param>
        /// <param name="description">The description (optional, may be null).</param>
        /// <param name="path">The path of file (only the fileName, optional, may be null).</param>
        /// <returns>The new Submodal type</returns>
        Data.Model.SubModalType AddIfNotExists(string modalName, string name, string description, string path);


        /// <summary>
        /// Deletes the specified submodal type.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="name">The name of the submodal type.</param>
        void Delete(string modalName, string name);

        /// <summary>
        /// Verify if exists the specified submodal type.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="subModalName">Name of the sub modal type.</param>
        /// <returns>True if exists, False if not</returns>
        bool Exists(string modalName, string subModalName);

        /// <summary>
        /// Updates the submodal type (according to its modal and submodal names).
        /// </summary>
        /// <param name="subModalType">Type of the sub modal type.</param>
        /// <returns>The updated submodal type</returns>
        Data.Model.SubModalType Update(Data.Model.SubModalType subModalType);
    }
}
