using Microsoft.EntityFrameworkCore;
using cl.uv.multimodalvisualizer.src.model;
using System;

namespace cl.uv.multimodalvisualizer.src.dbcontext
{
    public class PgsqlContext : DbContext
    {
        public static PgsqlContext db = null;
        public DbSet<Scene> Scene { get; set; }
        public DbSet<Person> Person { get; set; }

        public PgsqlContext(DbContextOptions options)
            : base(options)
        { }

        public static bool IsConnected
        {
            get
            {
                return db != null;
            }
        }

        public static PgsqlContext CreateConnection()
        {
            string server = "127.0.0.1";
            string port = "5432";
            string database = "MultimodalVisualizer";
            string userId = "mv_user";
            string password = "asd123";

            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql("Server="+server+ ";Port=" + port + ";Database=" + database + ";User Id=" + userId + ";Password=" + password + "; ");
            db = new PgsqlContext(optionsBuilder.Options);
            return db;
        }
    }
}
