using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cl.uv.multimodalvisualizer.models;
using System.IO;
//using Microsoft.Data.Sqlite;

namespace cl.uv.multimodalvisualizer.db
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
        //public static void CloseConnection()
        //{
        //    db.Database.CloseConnection();
        //    db.Dispose();
        //    db = null;
        //}

        public static Scene LoadScene(string filePath)
        {
            CreateConnection(filePath);
            var groups = db.PostureIntervalGroup
                .Include(pig => pig.Intervals)
                .Include(pig => pig.PostureType)
                .Include(pig => pig.Person.Scene);

            return groups.ToList()[0].Person.Scene;
        }

        public static List<MicroPosture> Load_MicroPostures(string filePath)
        {
            CreateConnection(filePath);
            var query = db.MicroPosture
                .Include(m => m.PostureType)
                .OrderBy(m => m.SceneLocationTime);
            return query.ToList();
        }

        public static void SaveScene(string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            CreateConnection(filePath);
            db.Database.EnsureCreated();
            db.Scene.Add(models.Scene.Instance);
            db.SaveChanges();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Filename="+Properties.Resources.SQLiteAppDbPath);

        //}
    }
}
