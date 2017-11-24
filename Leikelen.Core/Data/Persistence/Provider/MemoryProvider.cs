using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    public class MemoryProvider : EfAbstractProvider
    {
        public MemoryProvider()
        {
            
        }

        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase();
            Db = new DbDataContext(optionsBuilder.Options);
        }

    }
}
