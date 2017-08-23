using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence
{
    public class DbDataContext : DbContext, IDbProvider
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
        public DbDataContext Db = null;

        public bool IsConnected()
        {
            return Db != null;
        }

        public DbDataContext()
        {
            
        }

        public DbDataContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Scene>()
                .HasKey(sc => sc.SceneId);
            modelBuilder.Entity<Person>()
                .HasKey(p => p.PersonId);
            modelBuilder.Entity<PersonInScene>()
                .HasKey(pis => new {pis.SceneId, pis.PersonId});
            modelBuilder.Entity<ModalType>()
                .HasKey(mt => mt.ModalTypeId);
            modelBuilder.Entity<SubModalType>()
                .HasKey(smt => new {Name = smt.SubModalTypeId, ModalTypeName = smt.ModalTypeId });
            modelBuilder.Entity<SubModalType_PersonInScene>()
                .HasKey(smtPis => smtPis.SubModalType_PersonInSceneId);
            modelBuilder.Entity<RepresentType>()
                .HasKey(rt => rt.RepresentTypeId);
            modelBuilder.Entity<EventData>()
                .HasKey(ed => ed.EventDataId);
            modelBuilder.Entity<IntervalData>()
                .HasKey(id => id.IntervalDataId);


            modelBuilder.Entity<Scene>()
                .HasMany(sc => sc.PersonsInScene)
                .WithOne(pis => pis.Scene);
            modelBuilder.Entity<Person>()
                .HasMany(p => p.PersonInScenes)
                .WithOne(pis => pis.Person);

            modelBuilder.Entity<PersonInScene>()
                .HasOne(pis => pis.Scene)
                .WithMany(sc => sc.PersonsInScene);
            modelBuilder.Entity<PersonInScene>()
                .HasOne(pis => pis.Person)
                .WithMany(p => p.PersonInScenes);
            modelBuilder.Entity<PersonInScene>()
                .HasMany(pis => pis.SubModalType_PersonInScenes)
                .WithOne(smtPis => smtPis.PersonInScene);

            modelBuilder.Entity<ModalType>()
                .HasMany(mt => mt.SubModalTypes)
                .WithOne(smt => smt.ModalType);
            modelBuilder.Entity<SubModalType>()
                .HasOne(smt => smt.ModalType)
                .WithMany(mt => mt.SubModalTypes);
            modelBuilder.Entity<SubModalType>()
                .HasMany(smt => smt.SubModalType_PersonInScenes)
                .WithOne(smtPis => smtPis.SubModalType);

            modelBuilder.Entity<SubModalType_PersonInScene>()
                .HasOne(smtPis => smtPis.SubModalType)
                .WithMany(smt => smt.SubModalType_PersonInScenes);
            modelBuilder.Entity<SubModalType_PersonInScene>()
                .HasOne(smtPis => smtPis.PersonInScene)
                .WithMany(pis => pis.SubModalType_PersonInScenes);
            modelBuilder.Entity<SubModalType_PersonInScene>()
                .HasMany(smtPis => smtPis.RepresentTypes)
                .WithOne(rt => rt.SubModalType_PersonInScene);

            modelBuilder.Entity<RepresentType>()
                .HasOne(rt => rt.SubModalType_PersonInScene)
                .WithMany(smtPis => smtPis.RepresentTypes);
            modelBuilder.Entity<RepresentType>()
                .HasOne(rt => rt.EventData)
                .WithOne(ed => ed.RepresentType);
            modelBuilder.Entity<RepresentType>()
                .HasOne(rt => rt.IntervalData)
                .WithOne(id => id.RepresentType);
        }

        public virtual void CreateConnection(string options)
        {
        }

        public virtual void CloseConnection()
        {
        }

        public List<Scene> LoadScenes()
        {
            return Db.Scenes.ToList();
        }

        public Scene LoadScene(int sceneId, bool timeless, bool intervals, bool events)
        {
            return Db.Scenes.ToList().Find(s => s.SceneId == sceneId);
        }

        public void SaveScene(Scene instance)
        {
            Db.Scenes.Add(instance);
        }

        public void UpdateScene(int sceneId, Scene newScene)
        {
            Db.Scenes.Update(newScene);
        }

        public List<Person> LoadPersons()
        {
            return Db.Persons.ToList();
        }

        public void SavePerson(Person person)
        {
            Db.Persons.Add(person);
        }

        public void UpdatePerson(int personId, Person newPerson)
        {
            Db.Persons.Update(newPerson);
        }

        public List<ModalType> LoadModals()
        {
            return Db.ModalTypes.ToList();
        }

        public ModalType LoadModal(string name)
        {
            return Db.ModalTypes.ToList().Find(m => m.ModalTypeId == name);
        }

        public void SaveModal(ModalType modalType)
        {
            Db.ModalTypes.Add(modalType);
        }

        public List<SubModalType> LoadSubModals(ModalType modalType)
        {
            return Db.ModalTypes.ToList().Find(m => m.ModalTypeId == modalType.ModalTypeId).SubModalTypes;
        }

        public void SaveSubModal(string modalTypeName, SubModalType submodalType)
        {
            Db.ModalTypes.ToList().Find(m => m.ModalTypeId == modalTypeName).SubModalTypes.Add(submodalType);
        }

    }
}
