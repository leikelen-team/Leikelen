using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using cl.uv.multimodalvisualizer.src.model;

namespace cl.uv.multimodalvisualizer.src.dbcontext
{
    public class PostureTypeContext : DbContext
    {
        public static PostureTypeContext db = new PostureTypeContext();
        public DbSet<PostureType> PostureType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename="+Properties.Resources.PostureTypeDbPath); //PostureTypeDbPath
        }
    }
}
