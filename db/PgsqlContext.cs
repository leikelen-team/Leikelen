using Microsoft.EntityFrameworkCore;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.db
{
    public class PgsqlContext : DbContext
    {
        public static PgsqlContext db = null;
        public DbSet<Scene> Scene { get; set; }
        public DbSet<Person> Person { get; set; }
        //public DbSet<PostureIntervalGroup> PostureIntervalGroup { get; set; }
        //public DbSet<PostureType> PostureType { get; set; }
        //public DbSet<Interval> Interval { get; set; }


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
            //var builder = new NpgsqlConnectionStringBuilder();
            //builder.Host = "host.com";
            //builder.Username = "postgres";
            //builder.Password = "pass!";
            //builder.Database = "name";
            //var conn = new NpgsqlConnection(builder);
            //optionsBuilder.UseNpgsql(conn);

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

        //public Scene LoadScene()
        //{
        //    var groups = PostureIntervalGroup
        //        .Include(pig => pig.Intervals)
        //        .Include(pig => pig.PostureType)
        //        .Include(pig => pig.Person.Scene);
        //    return groups.ToList()[0].Person.Scene;
        //}
    }
}
