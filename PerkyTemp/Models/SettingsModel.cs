using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerkyTemp.Models
{
    /// <summary>
    /// A model for the settings that PerkyTemp stores.
    /// Only one instance of this model should be used (this is handled by
    /// PerkyTempDatabase).
    /// </summary>
    /// <seealso cref="PerkyTempDatabase"/>
    public class SettingsModel
    {
        public const int DEFAULT_ID = 1;
        
        /// <summary>
        /// Key of the SettingsModel in the settings table in the database.
        /// This will always be DEFAULT_ID if being stored in SQLite.
        /// </summary>
        [PrimaryKey]
        public int ID { get; private set; }

        /// <summary>
        /// The amount of time (in minutes) that the user should be notified
        /// before their cooling expires.
        /// </summary>
        public double NotificationTime { get; set; }

        /// <summary>
        /// The temperature (in degrees C) at which the vest is no longer effective.
        /// </summary>
        public float TemperatureThreshold { get; set; }

        /// <summary>
        /// Construct a new SettingsModel with the defaults set.
        /// (This should only be used by PerkyTempDatabase.)
        /// </summary>
        public static SettingsModel NewSettingsModel()
        {
            SettingsModel settings = new SettingsModel();
            settings.ID = DEFAULT_ID;
            settings.NotificationTime = 15.0;
            settings.TemperatureThreshold = 20.0f;
            return settings;
        }
    }
}
