using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
//using Microsoft.Data.Sqlite;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.db
{
    public class BackupDataContext : DbContext
    {
        public static BackupDataContext db = null;
        public DbSet<Scene> Scene { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<PostureIntervalGroup> PostureIntervalGroup { get; set; }
        public DbSet<PostureType> PostureType { get; set; }
        public DbSet<Interval> Interval { get; set; }


        public BackupDataContext(DbContextOptions options)
            : base(options)
        { }

        public static BackupDataContext CreateConnection(string filePath)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Filename=" + filePath);
            db = new BackupDataContext(optionsBuilder.Options);
            return db;
        }

        public Scene LoadScene()
        {
            var groups = PostureIntervalGroup
                .Include(pig => pig.Intervals)
                .Include(pig => pig.PostureType)
                .Include(pig => pig.Person.Scene);
            return groups.ToList()[0].Person.Scene;
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Filename="+Properties.Resources.SQLiteAppDbPath);

        //}
    }
}
