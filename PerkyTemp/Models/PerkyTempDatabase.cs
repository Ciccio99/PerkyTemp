using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PerkyTemp.Models
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }

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

        private PerkyTempDatabase(string path)
        {
            conn = new SQLiteConnection(path);
            conn.CreateTable<PastSession>();

            // Insert some dummy data
            SaveSession(PastSession.FromFields(
                "00:11:22:33",
                new DateTime(2017, 11, 13, 12, 34, 00),
                new DateTime(2017, 11, 13, 12, 45, 00),
                58.7,
                72.1));
            SaveSession(PastSession.FromFields(
                "00:11:22:33",
                new DateTime(2017, 11, 16, 4, 00, 00),
                new DateTime(2017, 11, 16, 5, 45, 00),
                42.9,
                68.7));
        }

        public List<PastSession> GetSessions()
        {
            return conn.Table<PastSession>().ToList();
        }

        public int SaveSession(PastSession session)
        {
            if (session.ID != 0)
            {
                return conn.Update(session);
            }
            else
            {
                return conn.Insert(session);
            }
        }
    }
}
