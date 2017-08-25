using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Data.Persistence
{
    public class DbFacade
    {
        public IDbProvider Provider { get; private set; }

        private static DbFacade _instance;

        public static DbFacade Instance
        {
            get
            {
                if (_instance == null) _instance = new DbFacade();
                return _instance;
            }
        }

        private DbFacade()
        {
            Provider = new DbDataContext();
            Provider.CreateConnection(GeneralSettings.Instance.DbConectionString.Value);
            
        }
    }
}
