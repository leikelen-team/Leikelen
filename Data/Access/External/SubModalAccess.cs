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
            var modal = DbFacade.Instance.Provider.LoadModal(modalName);
            if (modal == null)
            {
                throw new DbException(Error.ModalTypeNotExists + modalName);
            }
            else
            {
                return modal.SubModalTypes;
            }
        }

        public void Add(string modalName, string name, string description, string file)
        {
            var modal = DbFacade.Instance.Provider.LoadModal(modalName);
            if (modal == null)
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
                    DbFacade.Instance.Provider.SaveSubModal(modalName, new SubModalType()
                    {
                        SubModalTypeId = name,
                        Description = description,
                        File = file
                    });
                }
            }
        }

        public bool Exists(string modalName, string subModalName)
        {
            var modal = DbFacade.Instance.Provider.LoadModal(modalName);
            if (modal == null)
            {
                throw new DbException(Error.ModalTypeNotExists  + modalName);
            }
            else
            {
                var submodals = DbFacade.Instance.Provider.LoadSubModals(modal);
                return submodals.Exists(sm => sm.SubModalTypeId == subModalName);
            }
            
        }
    }
}
