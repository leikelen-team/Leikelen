using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cl.uv.multimodalvisualizer.models;

namespace cl.uv.multimodalvisualizer.db
{
    public class PostureTypeContext : DbContext
    {
        public static PostureTypeContext db = new PostureTypeContext();
        public DbSet<PostureType> PostureType { get; set; }
        //public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename="+Properties.Resources.PostureTypeDbPath); //PostureTypeDbPath
        }
    }
}
