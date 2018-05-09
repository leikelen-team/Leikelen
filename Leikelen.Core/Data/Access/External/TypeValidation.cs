using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.Data.Persistence;

namespace cl.uv.leikelen.Data.Access.External
{
    /// <summary>
    /// Class for validate the data before inserting
    /// </summary>
    public static class TypeValidation
    {
        /// <summary>
        /// Gets the SMT pis.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="modalName">Name of the modal.</param>
        /// <param name="subModalName">Name of the sub modal.</param>
        /// <returns></returns>
        /// <exception cref="DbException">
        /// SubModalType: " + subModalName + Properties.Error.inModalType + modalName + Properties.Error.AlreadyExists
        /// </exception>
        public static SubModalType_PersonInScene GetSmtPis(Person person, string modalName, string subModalName)
        {
            try
            {
                if (ModalAccess.TmpModals == null)
                    ModalAccess.LoadTmpModals();
                //get pis and smtPis
                var personInScene = Internal.SceneInUse.Instance.Scene.PersonsInScene.Find(pis => pis.Person.Equals(person));
                var subModalPersonInScene = personInScene.SubModalType_PersonInScenes.Find(smtPis => smtPis.SubModalType.SubModalTypeId.Equals(subModalName) && smtPis.SubModalType.ModalType.ModalTypeId.Equals(modalName));
                //if smtPis doesn't exists, then create it
                if (ReferenceEquals(null, subModalPersonInScene))
                {
                    var modalType = ModalAccess.TmpModals.Find(mt => mt.ModalTypeId.Equals(modalName));
                    if (ReferenceEquals(null, modalType))
                        throw new DbException(Properties.Error.ModalTypeNotExists + modalName);
                    var submodalType = modalType.SubModalTypes.Find(smt => smt.SubModalTypeId.Equals(subModalName));
                    if (ReferenceEquals(null, submodalType))
                        throw new DbException("SubModalType: " + subModalName + Properties.Error.inModalType + modalName + Properties.Error.AlreadyExists);

                    subModalPersonInScene = new SubModalType_PersonInScene()
                    {
                        PersonInScene = personInScene,
                        SubModalType = submodalType
                    };
                    personInScene.SubModalType_PersonInScenes.Add(subModalPersonInScene);
                }
                return subModalPersonInScene;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}
