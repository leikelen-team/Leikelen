using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    public class SqliteProvider : EfAbstractProvider
    {
        public SqliteProvider()
        {

        }

        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite(options);
            Db = new DbDataContext(optionsBuilder.Options);
        }

        public void Save(Scene scene)
        {
            Db.Database.EnsureCreated();
            Db.Scenes.Add(scene);
            Db.SaveChanges();
        }
    }
}
