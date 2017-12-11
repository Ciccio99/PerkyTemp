using System;
namespace PerkyTemp.Interfaces {
    public interface INotificationManager {
        string ScheduleNotification (double timeInterval, bool repeats, string title, string body);

        void RemovePendingNotification (string requestID);

        void RemoveAllPendingNotifications ();
    }
}
