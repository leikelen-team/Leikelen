using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using cl.uv.leikelen.src.model;
using System.IO;

namespace cl.uv.leikelen.src.dbcontext
{
    public class BackupDataContext : DbContext
    {
        private static BackupDataContext db = null;
        public DbSet<Scene> Scene { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<PostureIntervalGroup> PostureIntervalGroup { get; set; }
        public DbSet<PostureType> PostureType { get; set; }
        public DbSet<Interval> Interval { get; set; }
        public DbSet<MicroPosture> MicroPosture { get; set; }
        public DbSet<Distance> Distance { get; set; }


        public BackupDataContext(DbContextOptions options)
            : base(options)
        { }

        private static BackupDataContext CreateConnection(string filePath)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Filename=" + filePath);
            db = new BackupDataContext(optionsBuilder.Options);
            return db;
        }

        public static Scene LoadScene(string filePath)
        {
            CreateConnection(filePath);
            var groups = db.PostureIntervalGroup
                .Include(pig => pig.Intervals)
                .Include(pig => pig.PostureType)
                .Include(pig => pig.Person.Scene);

            var microPosturesDb = db.MicroPosture
                .Include(m => m.PostureType)
                .OrderBy(m => m.SceneLocationTime);

            var distancesBd = db.Distance.OrderBy(d=> d.DistanceId);


            Scene newScene = (Scene) groups.ToList()[0].Person.Scene;
            List<MicroPosture> microPostures = microPosturesDb.ToList();
            List<Distance> distances = distancesBd.ToList();


            foreach (Person person in newScene.Persons)
            {
                if (!person.HasBeenTracked) continue;
                if (microPostures.Exists(mp => mp.PersonId == person.PersonId))
                {
                    person.MicroPostures = microPostures.FindAll(mp => mp.PersonId == person.PersonId);
                }
                if(distances.Exists(d => d.PersonId == person.PersonId))
                {
                    person.Distances = distances.FindAll(d => d.PersonId == person.PersonId);
                }
            }

                return newScene;
        }

        public static void SaveScene(string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            CreateConnection(filePath);
            db.Database.EnsureCreated();
            db.Scene.Add(model.Scene.Instance);
            db.SaveChanges();
        }
    }
}
