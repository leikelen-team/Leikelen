using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Persistence;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Access.External
{
    public class ModalAccess : IModalAccess
    {
        public List<ModalType> GetAll()
        {
            return DbFacade.Instance.Provider.LoadModals();
        }

        public void Add(string name, string description)
        {
            if (Exists(name))
            {
                throw new DbException("ModalType "+name+" already exists");
            }
            else
            {
                DbFacade.Instance.Provider.SaveModal(new ModalType()
                {
                    Name = name,
                    Description = description
                });
            }
        }

        public bool Exists(string name)
        {
            var modalTypes = DbFacade.Instance.Provider.LoadModals();
            return modalTypes.Exists(m => m.Name.Equals(name));
        }
    }
}
