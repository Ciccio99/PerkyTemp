using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerkyTemp.Models
{
    class CurrentSession
    {
        public DateTime StartTime { get; private set; }
        public float? FirstTemp { get; private set; }
        public float? LatestTemp { get; private set; }
        public Dictionary<DateTime, float> TempReadings { get; private set; }

        public CurrentSession()
        {
            TempReadings = new Dictionary<DateTime, float>();
            StartTime = DateTime.Now;
        }

        public void RecordTempReading(float temp)
        {
            TempReadings[DateTime.Now] = temp;
            LatestTemp = temp;
            if (FirstTemp == null) FirstTemp = temp;
        }

        /// <summary>
        /// Convert this CurrentSession into a PastSession.
        /// If we have not had at least 2 temperature readings, this will return null.
        /// </summary>
        public PastSession EndSession()
        {
            // If we don't have a temperature *range* (i.e. at least 2), no point in recording the session
            if (TempReadings.Count < 2) return null;
            return PastSession.FromFields(StartTime, DateTime.Now, FirstTemp.Value, LatestTemp.Value);
        }

        /// <summary>
        /// Get the amount of remaining time for the vest, in seconds, if available
        /// </summary>
        /// <returns></returns>
        public double? GetTimeRemainingSec()
        {
            // If we've only had one temperature reading, we can't predict yet
            if (TempReadings.Count < 2) return null;

            float threshold = PerkyTempDatabase.Database.GetSettings().TemperatureThreshold;
            // TODO: Compute based on the TempReadings over time
            return 120.0;
        }
    }
}
