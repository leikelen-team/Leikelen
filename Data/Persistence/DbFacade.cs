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
        public List<DatabaseEngine> DbEngineList { get; private set; }

        private static DbFacade _instance;

        public static DbFacade Instance
        {
            get
            {
                if (_instance == null) _instance = new DbFacade();
                return _instance;
            }
        }

        private DbFacade()
        {
            DbEngineList = new List<DatabaseEngine>();
            FillDbEngines();

            //assign the selected engine in settings, and create the connection
            try
            {
                foreach (var engine in DbEngineList)
                {
                    if (engine.Name.Equals(GeneralSettings.Instance.Database.Value))
                    {
                        Provider = engine.Provider;
                        Provider.CreateConnection(engine.CreateConnectionString.Invoke(
                            GeneralSettings.Instance.DbHost.Value,
                            GeneralSettings.Instance.DbPort.Value != 0 ? GeneralSettings.Instance.DbPort.Value : engine.DefaultPort,
                            GeneralSettings.Instance.DbName.Value,
                            GeneralSettings.Instance.DbUser.Value,
                            GeneralSettings.Instance.DbPassword.Value));
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBoxResult result = MessageBox.Show(Properties.Error.BdNotConnect+"\n"+ex.Message, Properties.Error.BdNotConnectTitle, MessageBoxButton.OK,
                MessageBoxImage.Exclamation);
                Provider = DbEngineList[0].Provider;
                Provider.CreateConnection(null);
            }
        }

        /// <summary>
        /// Create engine providers and add its to DbEngineList attribute.
        /// </summary>
        private void FillDbEngines()
        {
            //create engine providers and add its to DbEngineList attribute
            var memoryEngine = new DatabaseEngine("Memory", new MemoryProvider(), 0, 
                (string host, int port, string name, string user, string password) =>
                {
                    return null;
                });
            DbEngineList.Add(memoryEngine);
            var postgreEngine = new DatabaseEngine("PostgreSQL", new PgSqlProvider(), 5432,
                (string host, int port, string name, string user, string password) =>
                {
                    return $"Host={host};Port={port};Database={name};Username={user};Password={password}";
                });
            DbEngineList.Add(postgreEngine);
            var mySqlEngine = new DatabaseEngine("MySQL", new MySqlProvider(), 3306,
                (string host, int port, string name, string user, string password) =>
                {
                    return $"Server={host};Port={port};Database={name};uid={user};pwd={password}";
                });
            DbEngineList.Add(mySqlEngine);
        }
    }

    public class DatabaseEngine
    {
        public string Name { get; }
        public IDbProvider Provider { get; }
        public int DefaultPort { get; }
        public Func<string, int, string, string, string, string> CreateConnectionString { get; }

        public DatabaseEngine(string name, IDbProvider provider, int defaultPort, 
            Func<string, int, string, string, string, string> createConnectionString)
        {
            Name = name;
            Provider = provider;
            DefaultPort = defaultPort;
            CreateConnectionString = createConnectionString;
        }
    }
}
