using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
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
        protected DbDataContext Db = null;

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
            Db.CloseConnection();
            Db.Dispose();
            Db = null;
        }

        public List<Scene> LoadScenes()
        {
            return Db.Scenes.ToList();
        }

        public Scene LoadScene(int sceneId)
        {
            return Db.Scenes.ToList().Find(s => s.SceneId == sceneId);
        }

        public Scene SaveScene(Scene instance)
        {
            var r = Db.Scenes.Add(instance).Entity;
            Db.SaveChanges();
            return r;
        }

        public Scene UpdateScene(Scene newScene)
        {
            var r = Db.Scenes.Update(newScene).Entity;
            Db.SaveChanges();
            return r;
        }

        public List<Person> LoadPersons()
        {
            return Db.Persons.ToList();
        }

        public Person SavePerson(Person person)
        {
            var r = Db.Persons.Add(person).Entity;
            Db.SaveChanges();
            return r;
        }

        public Person UpdatePerson(Person newPerson)
        {
            var r = Db.Persons.Update(newPerson).Entity;
            Db.Db.SaveChanges();
            return r;
        }

        public List<ModalType> LoadModals()
        {
            return Db.ModalTypes.ToList();
        }

        public ModalType LoadModal(string name)
        {
            return Db.ModalTypes.ToList().Find(m => m.ModalTypeId.Equals(name));
        }

        public ModalType SaveModal(ModalType modalType)
        {
            var r = Db.ModalTypes.Add(modalType).Entity;
            Db.SaveChanges();
            return r;
        }

        public List<SubModalType> LoadSubModals(string modalTypeName)
        {
            return Db.ModalTypes.Find(modalTypeName).SubModalTypes;
        }

        public SubModalType SaveSubModal(SubModalType submodalType)
        {
            var r = Db.SubModalTypes.Add(submodalType).Entity;
            Db.SaveChanges();
            return r;
        }

        public PersonInScene AddPersonToScene(Person person, Scene scene)
        {
            var r = Db.PersonInScenes.Add(new PersonInScene()
            {
                Person = person,
                Scene = scene
            }).Entity;
            Db.SaveChanges();
            return r;
        }

        public bool ExistsPersonInScene(Person person, Scene scene)
        {
            return Db.PersonInScenes.ToList().Exists(pis => pis.Person == person && pis.Scene == scene);
        }
    }
}
