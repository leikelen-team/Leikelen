using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    /// <summary>
    /// A database provider to use in principal memory (not persistent)
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.Data.Persistence.Provider.EfAbstractProvider" />
    public class MemoryProvider : EfAbstractProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryProvider"/> class.
        /// </summary>
        public MemoryProvider()
        {
            
        }

        /// <summary>
        /// Creates the connection with the database.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase();
            Db = new DbDataContext(optionsBuilder.Options);
        }

    }
}
