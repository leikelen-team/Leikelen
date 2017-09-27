using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Persistence.Provider;
using System.Windows;

namespace cl.uv.leikelen.Data.Persistence
{
    public class DbFacade
    {
        public IDbProvider Provider { get; private set; }
        public Dictionary<string, DatabaseEngine> DbEngineList { get; private set; }

        private static DbFacade _instance;

        public static DbFacade Instance
        {
            get
            {
                if (_instance == null) _instance = new DbFacade();
                return _instance;
            }
        }

        public static void Reset()
        {
            _instance.Provider.CloseConnection();
            _instance = new DbFacade();
        }

        public string TestError(string provider, string host, int port, string dbname, string user, string password)
        {
            try
            {
                var tmpProvider = DbEngineList[provider].Provider;
                tmpProvider.CreateConnection(DbEngineList[provider].CreateConnectionString.Invoke(
                    host,
                    port != -1 ? port : DbEngineList[provider].DefaultPort,
                    dbname,
                    user,
                    password));
                tmpProvider.CloseConnection();
                return null;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        private DbFacade()
        {
            DbEngineList = new Dictionary<string, DatabaseEngine>();
            FillDbEngines();

            //assign the selected engine in settings, and create the connection
            try
            {
                string providerName = GeneralSettings.Instance.Database.Value;
                Provider = DbEngineList[providerName].Provider;
                Provider.CreateConnection(DbEngineList[providerName].CreateConnectionString.Invoke(
                    GeneralSettings.Instance.DbHost.Value,
                    GeneralSettings.Instance.DbPort.Value != -1 ? GeneralSettings.Instance.DbPort.Value : DbEngineList[providerName].DefaultPort,
                    GeneralSettings.Instance.DbName.Value,
                    GeneralSettings.Instance.DbUser.Value,
                    GeneralSettings.Instance.DbPassword.Value));
                    
            }
            catch(Exception ex)
            {
                MessageBoxResult result = MessageBox.Show(Properties.Error.BdNotConnect+"\n"+ex.Message, 
                    Properties.Error.BdNotConnectTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Provider = DbEngineList["Memory"].Provider;
                Provider.CreateConnection("MemoryDb");
            }
        }

        /// <summary>
        /// Create engine providers and add its to DbEngineList attribute.
        /// </summary>
        private void FillDbEngines()
        {
            //create engine providers and add its to DbEngineList attribute
            var memoryEngine = new DatabaseEngine(new MemoryProvider(), 0, 
                (string host, int port, string name, string user, string password) =>
                {
                    return "MemoryDb";
                });
            DbEngineList.Add("Memory", memoryEngine);
            var postgreEngine = new DatabaseEngine(new PgSqlProvider(), 5432,
                (string host, int port, string name, string user, string password) =>
                {
                    return $"Host={host};Port={port};Database={name};Username={user};Password={password}";
                });
            DbEngineList.Add("PostgreSQL", postgreEngine);
            var mySqlEngine = new DatabaseEngine(new MySqlProvider(), 3306,
                (string host, int port, string name, string user, string password) =>
                {
                    return $"Server={host};Port={port};Database={name};uid={user};pwd={password}";
                });
            DbEngineList.Add("MySQL", mySqlEngine);
        }
    }

    public class DatabaseEngine
    {
        public IDbProvider Provider { get; }
        public int DefaultPort { get; }
        public Func<string, int, string, string, string, string> CreateConnectionString { get; }

        public DatabaseEngine(IDbProvider provider, int defaultPort, 
            Func<string, int, string, string, string, string> createConnectionString)
        {
            Provider = provider;
            DefaultPort = defaultPort;
            CreateConnectionString = createConnectionString;
        }
    }
}
