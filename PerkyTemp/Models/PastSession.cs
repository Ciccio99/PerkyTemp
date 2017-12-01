using System;
namespace PerkyTemp.Models
{
    public class PastSession
    {
        public byte[] MAC { get; private set; }
        public DateTime startDateTime { get; private set; }
        public DateTime finalDateTime { get; private set; }
        public double startTemp { get; private set; }
        public double finalTemp { get; private set; }


        public PastSession(byte[] MAC, DateTime startDateTime, DateTime finalDateTime, double startTemp, double finalTemp) {
            this.MAC = MAC;
            this.startDateTime = startDateTime;
            this.finalDateTime = finalDateTime;
            this.startTemp = startTemp;
            this.finalTemp = finalTemp;
        }

        public Int64 GetSessionDuration () {
            return (finalDateTime - startDateTime).Minutes;
        }
    }
}
