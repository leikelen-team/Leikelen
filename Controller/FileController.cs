using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Persistence.Provider;
using cl.uv.leikelen.Data.Access.Internal;

namespace cl.uv.leikelen.Controller
{
    public class FileController
    {
        public void Import(string fileName)
        {

        }

        public void Export(bool isOnlyBd, string fileName)
        {
            var sqliteProvider = new SqliteProvider();
            if (isOnlyBd)
            {
                sqliteProvider.CreateConnection("Filename=" + fileName);
                sqliteProvider.Save(SceneInUse.Instance.Scene);
                sqliteProvider.CloseConnection();
            }
            else
            {
                //TODO: guardar sqlite en tmp, y luego comprimir y mandar a fileName
            }
            
        }
    }
}
