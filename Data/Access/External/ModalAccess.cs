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
    public class ModalAccess : IModalAccess
    {
        public static List<ModalType> TmpModals { get; private set; } = new List<ModalType>();

        public static void LoadTmpModals()
        {
            TmpModals = DbFacade.Instance.Provider.LoadModals();
        }

        public List<ModalType> GetAll()
        {
            return DbFacade.Instance.Provider.LoadModals();
        }

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

        public bool Exists(string name)
        {
            var modalType = DbFacade.Instance.Provider.LoadModal(name);
            return !ReferenceEquals(null, modalType);
        }

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
