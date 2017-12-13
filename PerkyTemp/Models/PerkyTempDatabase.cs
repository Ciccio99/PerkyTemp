using PerkyTemp.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PerkyTemp.Models
{
    /// <summary>
    /// Singleton class to connect to the SQLite database and store/retrieve
    /// data.
    /// </summary>
    /// <seealso cref="PastSession"/>
    /// <seealso cref="SettingsModel"/>
    public class PerkyTempDatabase
    {
        private static PerkyTempDatabase instance;

        /// <summary>
        /// Get the singleton instance of PerkyTempDatabase.
        /// </summary>
        public static PerkyTempDatabase Database
        {
            get
            {
                if (instance == null)
                {
                    instance = new PerkyTempDatabase();
                }
                return instance;
            }
        }
        
        private SQLiteConnection conn;
        private bool _inited = false;

        /// <summary>
        /// A mock SettingsModel to use instead of the real one. Usefu lfor
        /// unit tests.
        /// </summary>
        public SettingsModel MockedSettingsForTesting { get; set; }
        
        public delegate void DatabaseChange();
        /// <summary>
        /// An event that will be invoked whenever there is a change to the
        /// database contents.
        /// </summary>
        public event DatabaseChange DatabaseChangeListeners;

        /// <summary>
        /// Private constructor (to enforce use of the singleton instance).
        /// </summary>
        private PerkyTempDatabase() { }

        /// <summary>
        /// Add a new listener to the DatabaseChangeListeners event.
        /// </summary>
        /// <param name="listener"></param>
        public void AddDatabaseChangeListener(DatabaseChange listener)
        {
            DatabaseChangeListeners += listener;
        }

        /// <summary>
        /// Initialize the database if using it for the first time.
        /// </summary>
        private void InitDatabase()
        {
            if (_inited) return;

            conn = new SQLiteConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath("PerkyTempSQLite.db3"));
            conn.CreateTable<PastSession>();
            conn.CreateTable<SettingsModel>();
            _inited = true;
        }

        /// <summary>
        /// Get all the PastSessions stored in the database.
        /// </summary>
        public List<PastSession> GetSessions()
        {
            InitDatabase();

            return conn.Table<PastSession>().Reverse().ToList();
        }

        /// <summary>
        /// Save a new PastSession or update an existing PastSession in the
        /// database.
        /// </summary>
        public int SaveSession(PastSession session)
        {
            InitDatabase();

            int retval;
            if (session.ID != 0)
            {
                retval = conn.Update(session);
            }
            else
            {
                retval = conn.Insert(session);
            }
            DatabaseChangeListeners?.Invoke();
            return retval;
        }

        /// <summary>
        /// Get the latest SettingsModel from the database (or using the mock
        /// specified with MockedSettingsForTesting).
        /// </summary>
        public SettingsModel GetSettings()
        {
            if (MockedSettingsForTesting != null)
            {
                return MockedSettingsForTesting;
            }

            InitDatabase();

            if (conn.Find<SettingsModel>(SettingsModel.DEFAULT_ID) == null)
            {
                conn.Insert(SettingsModel.NewSettingsModel());
            }
            return conn.Get<SettingsModel>(SettingsModel.DEFAULT_ID);
        }

        /// <summary>
        /// Store an updated SettingsModel in the database.
        /// </summary>
        public void SaveSettings(SettingsModel settings)
        {
            InitDatabase();

            conn.InsertOrReplace(settings);
            DatabaseChangeListeners?.Invoke();
        }
    }
}
