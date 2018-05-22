using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    /// <summary>
    /// A database provider for using Sqlite databases
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.Data.Persistence.Provider.EfAbstractProvider" />
    public class SqliteProvider : EfAbstractProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteProvider"/> class.
        /// </summary>
        public SqliteProvider()
        {

        }

        /// <summary>
        /// Creates the connection with the database.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite(options);
            Db = new DbDataContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Saves the specified scene.
        /// </summary>
        /// <param name="scene">The scene.</param>
        public void Save(Scene scene)
        {
            Db.Database.EnsureCreated();
            Db.Scenes.Add(scene);
            Db.SaveChanges();
        }
    }
}
