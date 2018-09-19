using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Persistence;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.Properties;

namespace cl.uv.leikelen.Data.Access.External
{
    /// <summary>
    /// Class to access the submodal types
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.DataAccess.ISubModalAccess" />
    public class SubModalAccess : ISubModalAccess
    {
        /// <summary>
        /// Gets all submodal types of a given modal type.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <returns>List of submodal types</returns>
        public List<Data.Model.SubModalType> GetAll(string modalName)
        {
            return DbFacade.Instance.Provider.LoadSubModals(modalName);
        }

        /// <summary>
        /// Adds a new submodal type.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="name">The name of the new submodal type.</param>
        /// <param name="description">The description (optional, may be null).</param>
        /// <param name="file">The path of file (only the fileName, optional, may be null).</param>
        /// <returns>The new Submodal type</returns>
        public Data.Model.SubModalType Add(string modalName, string name, string description, string file)
        {
            var modal = DbFacade.Instance.Provider.LoadModal(modalName);
            if (ReferenceEquals(null, modal))
            {
                throw new DbException(Error.ModalTypeNotExists + modalName);
            }
            else
            {
                if(Exists(modalName, name))
                {
                    throw new DbException("SubModalType: "+name+Error.inModalType +modalName+Error.AlreadyExists);
                }
                else
                {
                    return DbFacade.Instance.Provider.SaveSubModal(new SubModalType()
                    {
                        SubModalTypeId = name,
                        Description = description,
                        File = file,
                        ModalType = modal
                    });
                }
            }
        }

        /// <summary>
        /// Verify if exists the specified submodal type.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="subModalName">Name of the sub modal type.</param>
        /// <returns>True if exists, False if not</returns>
        public bool Exists(string modalName, string subModalName)
        {
            var modal = DbFacade.Instance.Provider.LoadModal(modalName);
            if (ReferenceEquals(null, modal))
            {
                throw new DbException(Error.ModalTypeNotExists  + modalName);
            }
            else
            {
                var submodals = DbFacade.Instance.Provider.LoadSubModals(modal.ModalTypeId);
                return submodals.Exists(sm => sm.SubModalTypeId.Equals(subModalName));
            }
            
        }

        /// <summary>
        /// Gets the modal type of the given name.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="name">The name of the submodal type.</param>
        /// <returns>The submodal type</returns>
        public Data.Model.SubModalType Get(string modalName, string name)
        {
            var submodalsInModal = DbFacade.Instance.Provider.LoadSubModals(modalName);
            foreach(var submodal in submodalsInModal)
            {
                if (submodal.SubModalTypeId.Equals(name))
                    return submodal;
            }
            return null;
        }

        /// <summary>
        /// Deletes the specified submodal type.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="name">The name of the submodal type.</param>
        public void Delete(string modalName, string name)
        {
            var submodal = Get(modalName, name);
            DbFacade.Instance.Provider.DeleteSubModal(submodal);
        }

        /// <summary>
        /// Updates the submodal type (according to its modal and submodal names).
        /// </summary>
        /// <param name="subModalType">Type of the sub modal type.</param>
        /// <returns>The updated submodal type</returns>
        public Data.Model.SubModalType Update(Data.Model.SubModalType subModalType)
        {
            return DbFacade.Instance.Provider.UpdateSubModalType(subModalType);
        }

        /// <summary>
        /// Adds a new submodal type if not exists, otherwise returns the 
        /// actual submodal type of the given modal and submodal type name.
        /// </summary>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="name">The name of the new submodal type.</param>
        /// <param name="description">The description (optional, may be null).</param>
        /// <param name="path">The path of file (only the fileName, optional, may be null).</param>
        /// <returns>The new Submodal type</returns>
        public Data.Model.SubModalType AddIfNotExists(string modalName, string name, string description, string path)
        {
            var modal = DbFacade.Instance.Provider.LoadModal(modalName);
            if (ReferenceEquals(null, modal))
            {
                new ModalAccess().Add(modalName, null);
            }
            if(Exists(modalName, name))
            {
                return Get(modalName, name);
            }
            else
            {
                return Add(modalName, name, description, path);
            }
        }
    }
}
