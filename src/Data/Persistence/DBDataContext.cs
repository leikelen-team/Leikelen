using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.src.Data.Persistence
{
    public class DBDataContext :DbContext
    {
        
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonInScene> PersonInScenes { get; set; }
        public DbSet<ModalType> ModalTypes { get; set; }
        public DbSet<SubModalType> SubModalTypes { get; set; }
        public DbSet<SubModalType_PersonInScene> SMT_PIS { get; set; }
        public DbSet<RepresentType> RepresentTypes { get; set; }
        public DbSet<EventData> EventDatas { get; set; }
        public DbSet<IntervalData> IntervalDatas { get; set; }
        public static DBDataContext db = null;

        public static bool IsConnected()
        {
            return db != null;
        }

        public DBDataContext(DbContextOptions options)
            : base(options)
        { }

    }
}
