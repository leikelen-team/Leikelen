using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    /// <summary>
    /// Interface to access the timeless data
    /// </summary>
    public interface ITimelessAccess
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="modalName">Name of the modal.</param>
        /// <param name="subModalName">Name of the sub modal.</param>
        /// <returns>The list of timeless</returns>
        List<API.DataAccess.Timeless> GetAll(Data.Model.Person person, string modalName, string subModalName);

        /// <summary>
        /// Adds the timeless data.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="modalName">Name of the modal.</param>
        /// <param name="subModalName">Name of the sub modal.</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        void Add(Data.Model.Person person, string modalName, string subModalName, int index, double value);

        /// <summary>
        /// Adds the timeless data.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="modalName">Name of the modal.</param>
        /// <param name="subModalName">Name of the sub modal.</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <param name="subtitle">The subtitle.</param>
        void Add(Data.Model.Person person, string modalName, string subModalName, int index, double value, string subtitle);

        /// <summary>
        /// Adds the timeless data.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="modalName">Name of the modal.</param>
        /// <param name="subModalName">Name of the sub modal.</param>
        /// <param name="index">The index.</param>
        /// <param name="subtitle">The subtitle.</param>
        void Add(Data.Model.Person person, string modalName, string subModalName, int index, string subtitle);

        /// <summary>
        /// Adds the timeless data.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="modalName">Name of the modal.</param>
        /// <param name="subModalName">Name of the sub modal.</param>
        /// <param name="index">The index.</param>
        void Add(Data.Model.Person person, string modalName, string subModalName, int index);
    }
}
