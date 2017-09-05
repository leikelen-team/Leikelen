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
    public static class TypeValidation
    {
        public static SubModalType_PersonInScene GetSmtPis(int personId, string modalName, string subModalName)
        {
            //get pis and smtPis
            var personInScene = Internal.SceneInUse.Instance.Scene.PersonsInScene.Find(pis => pis.PersonId == personId);
            var subModalPersonInScene = personInScene.SubModalType_PersonInScenes.Find(smtPis => smtPis.SubModalType.SubModalTypeId.Equals(subModalName) && smtPis.SubModalType.ModalType.ModalTypeId.Equals(modalName));

            //if smtPis doesn't exists, then create it
            if (subModalPersonInScene is null)
            {
                var modalType = DbFacade.Instance.Provider.LoadModal(modalName);
                if (Object.ReferenceEquals(null, modalType))
                    throw new DbException(Properties.Error.ModalTypeNotExists + modalName);
                var submodalType = modalType.SubModalTypes.Find(smt => smt.SubModalTypeId.Equals(subModalName));
                if (submodalType == null)
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
    }
}
