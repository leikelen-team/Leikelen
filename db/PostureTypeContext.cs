using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cl.uv.multimodalvisualizer.models;
using System.IO;

namespace cl.uv.multimodalvisualizer.db
{
    public class PostureTypeContext : DbContext
    {
        public static PostureTypeContext db = null;
        public DbSet<PostureType> PostureType { get; set; }
        public DbSet<myGesture> myGesture { get; set; }
        public DbSet<PostureIntervalGroup> PostureIntervalGroup { get; set; }
        //public DbSet<Post> Posts { get; set; }


            /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename="+Properties.Resources.PostureTypeDbPath); //PostureTypeDbPath
        } */

        public PostureTypeContext(DbContextOptions options)
            : base(options)
        { }

        private static PostureTypeContext CreateConnection()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Filename=" + Properties.Resources.PostureTypeDbPath);
            db = new PostureTypeContext(optionsBuilder.Options);
            return db;
        }

        public static void SaveAppDB()
        {
            Boolean wasCreated = false;
            
            //if (File.Exists(Properties.Resources.PostureTypeDbPath)) File.Delete(Properties.Resources.PostureTypeDbPath);
            if (!File.Exists(Properties.Resources.PostureTypeDbPath)) wasCreated = true;


            CreateConnection();
            db.Database.EnsureCreated();

            if (wasCreated)
            {
                //db.PostureType.Add(new PostureType("Ninguna", null) { PostureTypeId = 0 });
                //db.SaveChanges();
                db.PostureType.Add(new PostureType("Seated", "Database\\Seated.gbd") { PostureTypeId = 20 });
                db.SaveChanges();
            }
                
            

        }
    }
}
