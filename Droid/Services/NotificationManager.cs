using System;
using System.Diagnostics;
using PerkyTemp.Interfaces;
using Xamarin.Forms;
using Android.App;

[assembly: Dependency (typeof (PerkyTemp.Droid.Services.NotificationManager))]
namespace PerkyTemp.Droid.Services {
    /// <summary>
    /// Android implementation of NotificationManager.
    /// </summary>
    /// <see cref="INotificationManager"/>
    public class NotificationManager : INotificationManager {
        public NotificationManager () {
        }

        /// <summary>
        /// Schedules the notification.
        /// </summary>
        /// <returns>The notification.</returns>
        /// <param name="timeInterval">Time interval.</param>
        /// <param name="repeats">If set to <c>true</c> repeats.</param>
        /// <param name="title">Title.</param>
        /// <param name="body">Body.</param>
        public string ScheduleNotification(double timeInterval, bool repeats, string title, string body)
        {
            // TODO: Implement for Android
            Debug.WriteLine("Android: Schedule a notification, title: {0}, body: {1}, timeInterval: {2}", title, body, timeInterval);
            var requestID = Guid.NewGuid().ToString("D");
            return requestID;
        }

        /// <summary>
        /// Removes the pending notification.
        /// </summary>
        /// <param name="requestID">Request identifier.</param>
        public void RemovePendingNotification(string requestID)
        {
            // TODO: Implement for Android
            Debug.WriteLine("Android: Remove a pending notification: {0}", requestID);
        }

        /// <summary>
        /// Removes all pending notifications.
        /// </summary>
        public void RemoveAllPendingNotifications()
        {
            // TODO: Implement for Android
            Debug.WriteLine("Android: Remove all notifications!");
        }

        /// <summary>
        /// Show a simple alert box.
        /// </summary>
        public void Alert(string title, string message)
        {
            new AlertDialog.Builder(Forms.Context)
                .SetTitle(title)
                .SetMessage(message)
                .SetPositiveButton("Understood", (senderAlert, Args) => { })
                .Show();
        }
    }
}
