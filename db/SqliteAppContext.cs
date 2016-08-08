using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.db
{
    public class SqliteAppContext : DbContext
    {
        public static SqliteAppContext db = new SqliteAppContext();
        public DbSet<PostureType> PostureType { get; set; }
        //public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename="+Properties.Resources.SQLiteAppDbPath);
        }
    }
}
