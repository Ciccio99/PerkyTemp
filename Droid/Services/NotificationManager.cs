using System;
using System.Diagnostics;
using PerkyTemp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency (typeof (PerkyTemp.Droid.Services.NotificationManager))]
namespace PerkyTemp.Droid.Services {
    public class NotificationManager : INotificationManager {
        public NotificationManager () {
        }

        /// <summary>
        /// Removes all pending notifications.
        /// </summary>
        public void RemoveAllPendingNotifications () {
            Debug.WriteLine ("Remove Android Notifications!");
        }

        /// <summary>
        /// Removes the pending notification.
        /// </summary>
        /// <param name="requestID">Request identifier.</param>
        public void RemovePendingNotification (string requestID) {
            Debug.WriteLine ("Androind: Remove A pending notification: {0}", requestID);
        }

        /// <summary>
        /// Schedules the notification.
        /// </summary>
        /// <returns>The notification.</returns>
        /// <param name="timeInterval">Time interval.</param>
        /// <param name="repeats">If set to <c>true</c> repeats.</param>
        /// <param name="title">Title.</param>
        /// <param name="body">Body.</param>
        public string ScheduleNotification (double timeInterval, bool repeats, string title, string body) {
            Debug.WriteLine ("Android: Schedule a notification, title: {0}, body: {1}, timeInterval: {2}", title, body, timeInterval);
            var requestID = Guid.NewGuid ().ToString ("D");
            return requestID;
        }
    }
}
