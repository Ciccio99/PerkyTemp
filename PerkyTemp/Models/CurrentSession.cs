﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PerkyTemp.Utilities.Utilities;

namespace PerkyTemp.Models
{
    /// <summary>
    /// Model representing the current session. Temperature readings over time
    /// should be sent to this model so it can accurately predict remaining
    /// cooling time.
    /// </summary>
    public class CurrentSession
    {
        /// <summary>
        /// The Unix timestamp that other Unix timestamps are relative to (this
        /// makes debugging the linear regression easier).
        /// </summary>
        private static readonly double BASE_TIME = DateTimeToUnixTimestamp(DateTime.Now);

        /// <summary>
        /// The time at which the session was started.
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// The first temperature recorded for the session.
        /// </summary>
        public float? FirstTemp { get; private set; }

        /// <summary>
        /// The most recent temperature recorded for the session.
        /// </summary>
        public float? LatestTemp { get; private set; }

        /// <summary>
        /// All the recorded temperature readings for this session.
        /// </summary>
        public Dictionary<DateTime, float> TempReadings { get; private set; }

        public CurrentSession()
        {
            TempReadings = new Dictionary<DateTime, float>();
            StartTime = DateTime.Now;
        }

        /// <summary>
        /// Record a new temperature reading for this session, associated with
        /// the current time.
        /// </summary>
        public void RecordTempReading(float temp)
        {
            TempReadings[DateTime.Now] = temp;
            LatestTemp = temp;
            if (FirstTemp == null) FirstTemp = temp;
        }

        /// <summary>
        /// Convert this CurrentSession into a PastSession.
        /// If we have not had at least 2 temperature readings, this will
        /// return null.
        /// </summary>
        public PastSession EndSession()
        {
            // If we don't have a temperature *range* (i.e. at least 2), no point in recording the session
            if (TempReadings.Count < 2) return null;
            return PastSession.FromFields(StartTime, DateTime.Now, FirstTemp.Value, LatestTemp.Value);
        }

        /// <summary>
        /// Get the amount of remaining time for the vest, in seconds, if
        /// available.
        /// </summary>
        public double? GetTimeRemainingSec()
        {
            // If we've only had one temperature reading, we can't predict yet
            if (TempReadings.Count < 2) return null;

            double thresholdTemp = PerkyTempDatabase.Database.GetSettings().TemperatureThreshold;

            // Perform linear regression on the current temp data
            LinearRegression(DateTimeDictToDoublePairs(TempReadings), out double m, out double b);

            // At what timestamp will we be at temperature "thresholdTemp"?
            double thresholdTime = (thresholdTemp - b) / m + BASE_TIME;

            // If that's more than 10 days in the future, assume that we have bad data
            if (thresholdTime > DateTimeToUnixTimestamp(DateTime.Now + TimeSpan.FromDays(10)))
            {
                return null;
            }

            // Make sure we don't return a negative time remaining
            return Math.Max(0.0, thresholdTime - DateTimeToUnixTimestamp(DateTime.Now));
        }

        /// <summary>
        /// Convert a dictionary mapping DateTimes to floats to an enumerable
        /// of doubles to doubles (where the doubles are seconds relative to
        /// BASE_TIME).
        /// </summary>
        private static IEnumerable<KeyValuePair<double, double>> DateTimeDictToDoublePairs(Dictionary<DateTime, float> d)
        {
            foreach (var keyval in d)
            {
                yield return new KeyValuePair<double, double>(DateTimeToUnixTimestamp(keyval.Key) - BASE_TIME, keyval.Value);
            }
        }

        /// <summary>
        /// Compute the linear regression y=mx+b using least-squares
        /// regression.
        /// </summary>
        public static void LinearRegression(IEnumerable<KeyValuePair<double, double>> points,
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

            m = sCo / ssX;
            b = Ymean - ((sCo / ssX) * Xmean);
        }
    }
}
