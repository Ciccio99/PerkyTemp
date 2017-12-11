using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerkyTemp.Models
{
    class SettingsModel
    {
        public const int DEFAULT_ID = 1;
        
        [PrimaryKey]
        public int ID { get; private set; }

        /// <summary>
        /// The amount of time (in minutes) that the user should be notified
        /// before their cooling expires.
        /// </summary>
        public double NotificationTime { get; set; }

        public static SettingsModel NewSettingsModel()
        {
            SettingsModel settings = new SettingsModel();
            settings.ID = DEFAULT_ID;
            return settings;
        }
    }
}
