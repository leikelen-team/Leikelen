using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    public class MemoryProvider : DbDataContext
    {
        public MemoryProvider()
        {
            
        }

        public MemoryProvider(DbContextOptions options)
            : base(options)
        { }

        public override void CreateConnection(string options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase();
            Db = new MemoryProvider(optionsBuilder.Options);
        }

    }

    /*class DbContext
    {
        public List<Scene> Scenes { get; set; }
        public List<Person> Persons { get; set; }
        public List<PersonInScene> PersonInScenes { get; set; }
        public List<ModalType> ModalTypes { get; set; }
        public List<SubModalType> SubModalTypes { get; set; }
        public List<SubModalType_PersonInScene> SmtPis { get; set; }
        public List<RepresentType> RepresentTypes { get; set; }
        public List<EventData> EventDatas { get; set; }
        public List<IntervalData> IntervalDatas { get; set; }

        public DbContext()
        {
            Scenes = new List<Scene>();
            Persons = new List<Person>();
            PersonInScenes = new List<PersonInScene>();
            ModalTypes = new List<ModalType>();
            SubModalTypes = new List<SubModalType>();
            SmtPis = new List<SubModalType_PersonInScene>();
            RepresentTypes = new List<RepresentType>();
            EventDatas = new List<EventData>();
            IntervalDatas = new List<IntervalData>();
        }
    }*/
}
