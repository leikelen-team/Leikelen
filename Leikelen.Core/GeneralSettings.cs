using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;
using cl.uv.leikelen.API.DataAccess;

namespace cl.uv.leikelen
{
    /// <summary>
    /// Class with the general settings
    /// </summary>
    /// <seealso cref="Config.Net.SettingsContainer" />
    public class GeneralSettings : SettingsContainer
    {
        /// <summary>
        /// The language of the application
        /// </summary>
        public readonly Option<string> Language = new Option<string>("Language", "auto");
        /// <summary>
        /// The data directory with scene, modal and person files
        /// </summary>
        public readonly Option<string> DataDirectory = new Option<string>("DataDirectory", "data/");
        /// <summary>
        /// The temporary directory used to save the zip (.leikelen) file
        /// </summary>
        public readonly Option<string> TmpDirectory = new Option<string>("SceneInUseDirectory", "tmp/");
        /// <summary>
        /// The temporary scene directory used to extract or copy before zip the leikelen file
        /// </summary>
        public readonly Option<string> TmpSceneDirectory = new Option<string>("SceneInUseDirectory", "tmp/current_scene/");
        /// <summary>
        /// The default extension of exportable/importable files
        /// </summary>
        public readonly Option<string> Extension = new Option<string>("Extension", ".leikelen");
        /// <summary>
        /// The extension filter used in file dialogs
        /// </summary>
        public readonly Option<string> ExtensionFilter = new Option<string>("ExtensionFilter", "Leikelen scene file (*.leikelen)|*.leikelen");

        /// <summary>
        /// The default milliseconds threshold to create the intervals from a list of events
        /// </summary>
        public readonly Option<int> DefaultMillisecondsThreshold = new Option<int>("DefaultMilliseconds", 2000);

        /// <summary>
        /// The database engine
        /// </summary>
        public readonly Option<string> Database = new Option<string>("Database", "PostgreSQL");
        /// <summary>
        /// The database hostname
        /// </summary>
        public readonly Option<string> DbHost = new Option<string>("DbHost", "localhost");
        /// <summary>
        /// The database port
        /// </summary>
        public readonly Option<int> DbPort = new Option<int>("DbPort", -1);
        /// <summary>
        /// The database name
        /// </summary>
        public readonly Option<string> DbName = new Option<string>("DbName", "leikelen");
        /// <summary>
        /// The database user
        /// </summary>
        public readonly Option<string> DbUser = new Option<string>("DbUser", "postgres");
        /// <summary>
        /// The database user's password
        /// </summary>
        public readonly Option<string> DbPassword = new Option<string>("DbPassword", "erick1992");

        /// <summary>
        /// The intervals graph (in the intervals tab) minimum height
        /// </summary>
        public readonly Option<int> IntervalsGraphMinHeight = new Option<int>("IntervalsGraphMinHeight", 60);
        /// <summary>
        /// The events graph (in the events tab) minimum height
        /// </summary>
        public readonly Option<int> EventsGraphMinHeight = new Option<int>("EventsGraphMinHeight", 200);

        /// <summary>
        /// The index of the scene in the file to import. By default its the first scene (index = 0)
        /// </summary>
        public readonly Option<int> IndexOfSceneInFile = new Option<int>("IndexOfSceneInFile", 0);

        /// <summary>
        /// The static private instance to make the singleton
        /// </summary>
        private static GeneralSettings _instance;

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The singleton instance.
        /// </value>
        public static GeneralSettings Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new GeneralSettings();
                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralSettings"/> class.
        /// </summary>
        public GeneralSettings()
        {

        }

        /// <summary>
        /// Called when [configure]. Configures the settings file location (at config/general.json ).
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        protected override void OnConfigure(IConfigConfiguration configuration)
        {
            configuration.UseJsonFile(@"config/general.json");
        }
    }
}
