using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PerkyTemp.Utilities.Utilities;

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

            double threshold = PerkyTempDatabase.Database.GetSettings().TemperatureThreshold;

            // Perform linear regression on the current temp data
            LinearRegression(DateTimeDictToDoublePairs(TempReadings), out double m, out double b);

            // At what timestamp will we be at temperature "threshold"?
            double thresholdTime = (threshold - b) / m;
            return Math.Max(0.0, threshold - DateTimeToUnixTimestamp(DateTime.Now));
        }

        private static IEnumerable<KeyValuePair<double, double>> DateTimeDictToDoublePairs(Dictionary<DateTime, float> d)
        {
            foreach (var keyval in d)
            {
                yield return new KeyValuePair<double, double>(DateTimeToUnixTimestamp(keyval.Key), keyval.Value);
            }
        }

        /// <summary>
        /// Compute the linear regression y=mx+b using least-squares regression.
        /// </summary>
        private static void LinearRegression(IEnumerable<KeyValuePair<double, double>> points,
                                      out double m, out double b)
        {
            double Xsum = 0;
            double Ysum = 0;
            double XsqSum = 0;
            double YsqSum = 0;
            double sumCodeviates = 0;

            int count = 0;
            foreach (var pair in points)
            {
                count += 1;

                double x = pair.Key, y = pair.Value;
                sumCodeviates += x * y;
                Xsum += x;
                Ysum += y;
                XsqSum += x * x;
                YsqSum += y * y;
            }

            double ssX = XsqSum - ((Xsum * Xsum) / count);
            double ssY = YsqSum - ((Ysum * Ysum) / count);
            double sCo = sumCodeviates - ((Xsum * Ysum) / count);

            double Xmean = Xsum / count;
            double Ymean = Ysum / count;

            m = Ymean - ((sCo / ssX) * Xmean);
            b = sCo / ssX;
        }
    }
}
