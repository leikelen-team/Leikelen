using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    /// <summary>
    /// A database provider for PostgreSql databases
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.Data.Persistence.Provider.EfAbstractProvider" />
    public class PgSqlProvider : EfAbstractProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Persistence.Provider.PgSqlProvider"/> class.
        /// </summary>
        public PgSqlProvider()
        {
            
        }

        /// <summary>
        /// Creates the connection with the database.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(options);//+ ";Timeout=1000;CommandTimeout=1000");
            Db = new DbDataContext(optionsBuilder.Options);

            Db.Database.EnsureCreated();

            Db.Database.OpenConnection();
        }
    }
}
