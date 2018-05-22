using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Persistence.Provider;
using System.Windows;

namespace cl.uv.leikelen.Data.Persistence
{
    /// <summary>
    /// Database facade of different engines
    /// </summary>
    public class DbFacade
    {
        /// <summary>
        /// Gets the data provider.
        /// </summary>
        /// <value>
        /// The data provider.
        /// </value>
        public IDbProvider Provider { get; private set; }

        /// <summary>
        /// Gets the database engine list.
        /// </summary>
        /// <value>
        /// The database engine list.
        /// </value>
        public Dictionary<string, DatabaseEngine> DbEngineList { get; private set; }

        private static DbFacade _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DbFacade Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new DbFacade();
                return _instance;
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public static void Reset()
        {
            _instance.Provider.CloseConnection();
            _instance = new DbFacade();
        }

        /// <summary>
        /// Test the connection to see if an error occur.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="dbname">The database name.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
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

    /// <summary>
    /// Class that represent a database engine
    /// </summary>
    public class DatabaseEngine
    {
        /// <summary>
        /// Gets the data provider.
        /// </summary>
        /// <value>
        /// The data provider.
        /// </value>
        public IDbProvider Provider { get; }

        /// <summary>
        /// Gets the default port.
        /// </summary>
        /// <value>
        /// The default port.
        /// </value>
        public int DefaultPort { get; }

        /// <summary>
        /// Gets the function to create connection string.
        /// </summary>
        /// <value>
        /// The function to create the connection string.
        /// </value>
        public Func<string, int, string, string, string, string> CreateConnectionString { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseEngine"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="defaultPort">The default port.</param>
        /// <param name="createConnectionString">The function to create the connection string.</param>
        public DatabaseEngine(IDbProvider provider, int defaultPort, 
            Func<string, int, string, string, string, string> createConnectionString)
        {
            Provider = provider;
            DefaultPort = defaultPort;
            CreateConnectionString = createConnectionString;
        }
    }
}
