using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.Data.Persistence.Provider
{
    /// <summary>
    /// Database context class
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class DbDataContext : DbContext
    {
        /// <summary>
        /// Gets or sets the scenes.
        /// </summary>
        /// <value>
        /// The scenes.
        /// </value>
        public DbSet<Scene> Scenes { get; set; }

        /// <summary>
        /// Gets or sets the persons.
        /// </summary>
        /// <value>
        /// The persons.
        /// </value>
        public DbSet<Person> Persons { get; set; }

        /// <summary>
        /// Gets or sets the person in scenes.
        /// </summary>
        /// <value>
        /// The person in scenes.
        /// </value>
        public DbSet<PersonInScene> PersonInScenes { get; set; }

        /// <summary>
        /// Gets or sets the modal types.
        /// </summary>
        /// <value>
        /// The modal types.
        /// </value>
        public DbSet<ModalType> ModalTypes { get; set; }

        /// <summary>
        /// Gets or sets the sub modal types.
        /// </summary>
        /// <value>
        /// The sub modal types.
        /// </value>
        public DbSet<SubModalType> SubModalTypes { get; set; }

        /// <summary>
        /// Gets or sets the SMT pis.
        /// </summary>
        /// <value>
        /// The SMT pis.
        /// </value>
        public DbSet<SubModalType_PersonInScene> SmtPis { get; set; }

        /// <summary>
        /// Gets or sets the represent types.
        /// </summary>
        /// <value>
        /// The represent types.
        /// </value>
        public DbSet<RepresentType> RepresentTypes { get; set; }

        /// <summary>
        /// Gets or sets the events data.
        /// </summary>
        /// <value>
        /// The events data.
        /// </value>
        public DbSet<EventData> EventDatas { get; set; }

        /// <summary>
        /// Gets or sets the intervals data.
        /// </summary>
        /// <value>
        /// The intervals data.
        /// </value>
        public DbSet<IntervalData> IntervalDatas { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDataContext"/> class.
        /// </summary>
        public DbDataContext()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDataContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public DbDataContext(DbContextOptions options)
            : base(options)
        { }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
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
    }
}
