using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    public class PgSqlProvider : EfAbstractProvider
    {
        public PgSqlProvider()
        {
            
        }

        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(options);
            Db = new DbDataContext(optionsBuilder.Options);
            Db.Database.OpenConnection();
        }
    }
}
