using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Persistence.Interface;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace cl.uv.leikelen.src.Data.Persistence.Sqlite
{
    public class SqliteDataContext : IBackupDataContext
    {
        private bool _isConnected;

        public bool isConnected()
        {
            return _isConnected;
        }

        public SqliteDataContext()
        {
            _isConnected = false;
        }

        /// <summary>
        /// Sqlite Persistence Class Constructor with connection create
        /// </summary>
        /// <param name="optionString">String with option parameters to create connection</param>
        public SqliteDataContext(string optionString)
        {
            _isConnected = false;
            CreateConnection(optionString);
        }

        public DbContext CreateConnection(string optionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            //"Filename=" + filePath
            optionsBuilder.UseSqlite(optionString);
            BackupDataContext.db = new BackupDataContext(optionsBuilder.Options);
            _isConnected = true;
            return BackupDataContext.db;
        }

        public void CloseConnection()
        {
            BackupDataContext.db.Database.CloseConnection();
            BackupDataContext.db.Dispose();
            BackupDataContext.db = null;
            _isConnected = false;
        }

        public Scene LoadScene()
        {
            //TODO: Hacer LoadScene para Sqlite
            return new Scene();
        }
        public void SaveScene(Scene instance)
        {
            if (File.Exists(Properties.Paths.CurrentDataFile)) File.Delete(Properties.Paths.CurrentDataFile);
            BackupDataContext.db.Database.EnsureCreated();
            BackupDataContext.db.Scene.Add(instance);
            BackupDataContext.db.SaveChanges();
        }
    }
}
