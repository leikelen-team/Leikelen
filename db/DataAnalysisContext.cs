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
    public class DataAnalysisContext : DbContext
    {
        public static DataAnalysisContext db = null;
        public DbSet<Scene> Scene { get; set; }
        //public DbSet<Person> Person { get; set; }
        //public DbSet<PostureIntervalGroup> PostureIntervalGroup { get; set; }
        //public DbSet<PostureType> PostureType { get; set; }


        public DataAnalysisContext(DbContextOptions options)
            :base(options)
        { }

        public static DataAnalysisContext CreateConnection(string filePath)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Filename=" + filePath);
            db = new DataAnalysisContext(optionsBuilder.Options);
            return db;
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Filename="+Properties.Resources.SQLiteAppDbPath);
           
        //}
    }
}
