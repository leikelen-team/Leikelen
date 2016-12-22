using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using cl.uv.leikelen.src.model;

namespace cl.uv.leikelen.src.dbcontext
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
