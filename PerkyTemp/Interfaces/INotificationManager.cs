using System;
namespace PerkyTemp.Interfaces {
    public interface INotificationManager {
        /// <summary>
        /// Schedules the notification.
        /// </summary>
        /// <param name="timeInterval">Time interval in seconds.</param>
        /// <param name="repeats">If set to <c>true</c> repeats.</param>
        /// <param name="title">Title.</param>
        /// <param name="body">Body.</param>
        string ScheduleNotification (double timeInterval, bool repeats, string title, string body);

        /// <summary>
        /// Removes the pending notification.
        /// </summary>
        /// <param name="requestID">Request identifier.</param>
        void RemovePendingNotification (string requestID);

        /// <summary>
        /// Removes all pending notifications.
        /// </summary>
        void RemoveAllPendingNotifications ();
    }
}
