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

        public SettingsModel MockedSettingsForTesting { get; set; }

        public delegate void DatabaseChange();
        public event DatabaseChange DatabaseChangeListeners;

        private PerkyTempDatabase()
        {
        }

        public void AddDatabaseChangeListener(DatabaseChange listener)
        {
            DatabaseChangeListeners += listener;
        }

        private void InitDatabase()
        {
            if (_inited) return;

            conn = new SQLiteConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath("PerkyTempSQLite.db3"));
            conn.CreateTable<PastSession>();
            conn.CreateTable<SettingsModel>();
            _inited = true;
        }

        public List<PastSession> GetSessions()
        {
            InitDatabase();

            return conn.Table<PastSession>().Reverse().ToList();
        }

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

        public void SaveSettings(SettingsModel settings)
        {
            InitDatabase();

            conn.InsertOrReplace(settings);
            DatabaseChangeListeners?.Invoke();
        }
    }
}
