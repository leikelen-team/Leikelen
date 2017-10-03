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
    public class SubModalAccess : ISubModalAccess
    {
        public List<SubModalType> GetAll(string modalName)
        {
            return DbFacade.Instance.Provider.LoadSubModals(modalName);
        }

        public SubModalType Add(string modalName, string name, string description, string file)
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

        public SubModalType Get(string modalName, string name)
        {
            var submodalsInModal = DbFacade.Instance.Provider.LoadSubModals(modalName);
            foreach(var submodal in submodalsInModal)
            {
                if (submodal.SubModalTypeId.Equals(name))
                    return submodal;
            }
            return null;
        }

        public void Delete(string modalName, string name)
        {
            var submodal = Get(modalName, name);
            DbFacade.Instance.Provider.DeleteSubModal(submodal);
        }

        public SubModalType Update(SubModalType subModalType)
        {
            return DbFacade.Instance.Provider.UpdateSubModalType(subModalType);
        }
    }
}
