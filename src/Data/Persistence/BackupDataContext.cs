using cl.uv.leikelen.src.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace cl.uv.leikelen.src.Data.Persistence
{
    public class BackupDataContext : DbContext
    {
        public static BackupDataContext db = null;
        public DbSet<Scene> Scene { get; set; }

        public static bool isConnected()
        {
            return db != null;
        }

        public BackupDataContext(DbContextOptions options)
            : base(options)
        { }
    }
}
