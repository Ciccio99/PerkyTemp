using System;
using SQLite;

namespace PerkyTemp.Models
{
    public class PastSession
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; private set; }

        public double StartDateTimestamp { get; private set; }

        public double FinalDateTimestamp { get; private set; }

        public double StartTemp { get; private set; }

        public double FinalTemp { get; private set; }

        [Ignore]
        public string Date
        {
            get
            {
                return (new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(StartDateTimestamp)).ToString();
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

        public static PastSession FromFields(DateTime StartDateTime, DateTime FinalDateTime, double StartTemp, double FinalTemp) {
            PastSession session = new PastSession();
            session.ID = 0;
            session.StartDateTimestamp = (StartDateTime - new DateTime(1970, 1, 1)).TotalSeconds;
            session.FinalDateTimestamp = (FinalDateTime - new DateTime(1970, 1, 1)).TotalSeconds;
            session.StartTemp = StartTemp;
            session.FinalTemp = FinalTemp;
            return session;
        }

        public override string ToString()
        {
            return string.Format("[PastSession: ID={0}, StartDateTime={1}, FinalDateTime={2}, StartTemp={3}, FinalTemp={4}]",
                ID,
                new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(StartDateTimestamp),
                new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(FinalDateTimestamp),
                StartTemp,
                FinalTemp);
        }
    }
}
