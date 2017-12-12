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
        public float StartTemp { get; private set; }
        public float LatestTemp { get; private set; }
        public Dictionary<DateTime, float> TempReadings { get; private set; }

        public CurrentSession(float _startTemp)
        {
            TempReadings = new Dictionary<DateTime, float>();

            StartTime = DateTime.Now;
            StartTemp = _startTemp;
            LatestTemp = _startTemp;

            TempReadings[StartTime] = StartTemp;
        }

        public void RecordTempReading(float temp)
        {
            TempReadings[DateTime.Now] = temp;
            LatestTemp = temp;
        }

        public PastSession EndSession()
        {
            return PastSession.FromFields(StartTime, DateTime.Now, StartTemp, LatestTemp);
        }

        /// <summary>
        /// Get the amount of remaining time for the vest, in seconds.
        /// </summary>
        /// <returns></returns>
        public double GetTimeRemainingSec()
        {
            float threshold = PerkyTempDatabase.Database.GetSettings().TemperatureThreshold;
            // TODO: Compute based on the TempReadings over time
            return 120.0;
        }
    }
}
