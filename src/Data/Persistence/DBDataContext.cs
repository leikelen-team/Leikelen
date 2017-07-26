using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence
{
    public class DbDataContext : DbContext
    {
        
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonInScene> PersonInScenes { get; set; }
        public DbSet<ModalType> ModalTypes { get; set; }
        public DbSet<SubModalType> SubModalTypes { get; set; }
        public DbSet<SubModalType_PersonInScene> SmtPis { get; set; }
        public DbSet<RepresentType> RepresentTypes { get; set; }
        public DbSet<EventData> EventDatas { get; set; }
        public DbSet<IntervalData> IntervalDatas { get; set; }
        public static DbDataContext Db = null;

        public static bool IsConnected()
        {
            return Db != null;
        }

        public DbDataContext(DbContextOptions options)
            : base(options)
        { }

    }
}
