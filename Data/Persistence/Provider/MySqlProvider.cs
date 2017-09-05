using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    public class MySqlProvider : EfAbstractProvider
    {
        public MySqlProvider()
        {
            
        }

        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySql(options);
            Db = new DbDataContext(optionsBuilder.Options);
            Db.Database.OpenConnection();
        }
    }
}
