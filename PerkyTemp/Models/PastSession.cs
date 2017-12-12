using System;
using SQLite;
using static PerkyTemp.Utilities.Utilities;

namespace PerkyTemp.Models
{
    /// <summary>
    /// A model representing a past session.
    /// This is stored in the database using PerkyTempDatabase.
    /// </summary>
    /// <seealso cref="PerkyTempDatabase"/>
    public class PastSession
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; private set; }

        public double StartDateTimestamp { get; private set; }

        public double FinalDateTimestamp { get; private set; }

        public float StartTemp { get; private set; }

        public float FinalTemp { get; private set; }

        [Ignore]
        public string Date
        {
            get
            {
                return UnixTimestampToDateTime(StartDateTimestamp).ToString();
            }
        }

        [Ignore]
        public string DurationString
        {
            get
            {
                return Math.Round(TimeSpan.FromSeconds(FinalDateTimestamp - StartDateTimestamp).TotalMinutes, 2) + " minutes";
            }
        }

        public PastSession() { }

        public static PastSession FromFields(DateTime StartDateTime, DateTime FinalDateTime, float StartTemp, float FinalTemp) {
            PastSession session = new PastSession();
            session.ID = 0;
            session.StartDateTimestamp = DateTimeToUnixTimestamp(StartDateTime);
            session.FinalDateTimestamp = DateTimeToUnixTimestamp(FinalDateTime);
            session.StartTemp = StartTemp;
            session.FinalTemp = FinalTemp;
            return session;
        }

        public override string ToString()
        {
            return string.Format("[PastSession: ID={0}, StartDateTime={1}, FinalDateTime={2}, StartTemp={3}, FinalTemp={4}]",
                ID,
                UnixTimestampToDateTime(StartDateTimestamp),
                UnixTimestampToDateTime(FinalDateTimestamp),
                StartTemp,
                FinalTemp);
        }
    }
}
