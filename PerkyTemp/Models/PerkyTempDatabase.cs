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
    class PerkyTempDatabase
    {
        private static PerkyTempDatabase instance;

        public static PerkyTempDatabase Database
        {
            get
            {
                if (instance == null)
                {
                    instance = new PerkyTempDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("PerkyTempSQLite.db3"));
                }
                return instance;
            }
        }
        
        private SQLiteConnection conn;

        public delegate void DatabaseChange();
        public event DatabaseChange DatabaseChangeListeners;

        private PerkyTempDatabase(string path)
        {
            conn = new SQLiteConnection(path);
            conn.CreateTable<PastSession>();
            conn.CreateTable<SettingsModel>();
        }

        public void AddDatabaseChangeListener(DatabaseChange listener)
        {
            DatabaseChangeListeners += listener;
        }

        public List<PastSession> GetSessions()
        {
            return conn.Table<PastSession>().Reverse().ToList();
        }

        public int SaveSession(PastSession session)
        {
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
            if (conn.Find<SettingsModel>(SettingsModel.DEFAULT_ID) == null)
            {
                conn.Insert(SettingsModel.NewSettingsModel());
            }
            return conn.Get<SettingsModel>(SettingsModel.DEFAULT_ID);
        }

        public void SaveSettings(SettingsModel settings)
        {
            conn.InsertOrReplace(settings);
            DatabaseChangeListeners?.Invoke();
        }
    }
}
