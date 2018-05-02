using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Persistence;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.Properties;
using System.IO;

namespace cl.uv.leikelen.Data.Access.External
{
    /// <summary>
    /// Access the modal types
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.DataAccess.IModalAccess" />
    public class ModalAccess : IModalAccess
    {
        /// <summary>
        /// Gets the temporary modal types meanwhile is recording.
        /// </summary>
        /// <value>
        /// The temporary modal types meanwhile is recording.
        /// </value>
        public static List<ModalType> TmpModals { get; private set; } = new List<ModalType>();

        /// <summary>
        /// Loads the temporary modal types before recording.
        /// </summary>
        public static void LoadTmpModals()
        {
            TmpModals = DbFacade.Instance.Provider.LoadModals();
        }

        /// <summary>
        /// Gets all modal types in the database.
        /// </summary>
        /// <returns>A list of all modal types (without joins)</returns>
        public List<ModalType> GetAll()
        {
            return DbFacade.Instance.Provider.LoadModals();
        }

        /// <summary>
        /// Adds a new modal type.
        /// </summary>
        /// <param name="name">The name of the modal type (its the id, and must to be not null and unique).</param>
        /// <param name="description">The description (optional, may be null).</param>
        /// <returns>The new modal type</returns>
        public ModalType Add(string name, string description)
        {
            AddDirectory(name);
            if (Exists(name))
            {
                throw new DbException("ModalType "+name+ Error.AlreadyExists);
            }
            else
            {
                return DbFacade.Instance.Provider.SaveModal(new ModalType()
                {
                    ModalTypeId = name,
                    Description = description
                });
            }
        }

        /// <summary>
        /// Verify is exists a modal type of given name.
        /// </summary>
        /// <param name="name">The name of the modal type.</param>
        /// <returns>True if already exists, otherwise False</returns>
        public bool Exists(string name)
        {
            var modalType = DbFacade.Instance.Provider.LoadModal(name);
            return !ReferenceEquals(null, modalType);
        }

        /// <summary>
        /// Adds a new modal type if not exists, otherwise, returns the actual modal type of the given name.
        /// </summary>
        /// <param name="name">The name of the modal type (its the id, and must to be not null and unique).</param>
        /// <param name="description">The description (optional, may be null).</param>
        /// <returns>The new modal type, if exists, the actual modal type with same name</returns>
        public ModalType AddIfNotExists(string name, string description)
        {
            AddDirectory(name);
            if (Exists(name))
            {
                return DbFacade.Instance.Provider.LoadModal(name);
            }
            else
            {
                return Add(name, description);
            }
        }

        private void AddDirectory(string modalTypeName)
        {
            string modalDirectory = Path.Combine(Path.Combine(new SettingsAccess().GetDataDirectory(), "modal/"), modalTypeName);
            if (!Directory.Exists(modalDirectory))
                Directory.CreateDirectory(modalDirectory);
        }
    }
}
