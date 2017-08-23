using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    public class MySqlProvider : DbDataContext
    {
        public MySqlProvider()
        {
            
        }

        public MySqlProvider(DbContextOptions options)
        : base(options)
        { }

        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySql(options);
            Db = new MySqlProvider(optionsBuilder.Options);
        }

        public override void CloseConnection()
        {
            Db.CloseConnection();
            Db = null;
        }
    }
}
