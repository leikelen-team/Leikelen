using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API.DataAccess;
using cl.uv.leikelen.src.Data.Persistence;
using cl.uv.leikelen.src.Data.Model;

namespace cl.uv.leikelen.src.Data.Access.External
{
    public class ModalAccess : IModalAccess
    {
        public List<ModalType> GetAll()
        {
            return DBFacade.Instance.Provider.LoadModals();
        }

        public void Add(string name, string description)
        {
            if (Exists(name))
            {
                throw new DBException("ModalType "+name+" already exists");
            }
            else
            {
                DBFacade.Instance.Provider.SaveModal(new ModalType()
                {
                    Name = name,
                    Description = description
                });
            }
        }

        public bool Exists(string name)
        {
            var modalTypes = DBFacade.Instance.Provider.LoadModals();
            return modalTypes.Exists(m => m.Name.Equals(name));
        }
    }
}
