using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    /// <summary>
    /// A database provider for MySql databases
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.Data.Persistence.Provider.EfAbstractProvider" />
    public class MySqlProvider : EfAbstractProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Persistence.Provider.MySqlProvider"/> class.
        /// </summary>
        public MySqlProvider()
        {
            
        }

        /// <summary>
        /// Creates the connection with the database.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySql(options);
            Db = new DbDataContext(optionsBuilder.Options);
            Db.Database.EnsureCreated();

            Db.Database.OpenConnection();
        }
    }
}
