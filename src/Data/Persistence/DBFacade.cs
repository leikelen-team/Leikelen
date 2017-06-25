using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.Data.Persistence
{
    public class DBFacade
    {
        public IDBProvider Provider { get; private set; }

        private static DBFacade _instance;

        public static DBFacade Instance
        {
            get
            {
                if (_instance == null) _instance = new DBFacade();
                return _instance;
            }
        }

        private DBFacade()
        {
            Provider = new PgSQLProvider();
        }
    }
}
