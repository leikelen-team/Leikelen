using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    public class PgSqlProvider : DbDataContext
    {
        public PgSqlProvider()
        {
            
        }

        public PgSqlProvider(DbContextOptions options)
            : base(options)
        { }

        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(options);
            Db = new PgSqlProvider(optionsBuilder.Options);
        }

        public override void CloseConnection()
        {
            Db.CloseConnection();
            Db = null;
        }
    }
}
