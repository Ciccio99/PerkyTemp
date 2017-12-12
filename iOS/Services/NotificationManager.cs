using System;
using PerkyTemp.Interfaces;
using UserNotifications;
using PerkyTemp.iOS.Notifications;
using Xamarin.Forms;
using UIKit;

[assembly: Dependency (typeof (PerkyTemp.iOS.Services.NotificationManager))]
namespace PerkyTemp.iOS.Services {
    public class NotificationManager : INotificationManager {

        public NotificationManager () {
            // Attached delegate for how scheduled notifications should be handled
            UNUserNotificationCenter.Current.Delegate = new PerkyNotificationCenterDelegate ();
        }

        /// <summary>
        /// Schedules the notification.
        /// </summary>
        /// <returns> GUID for the notification. Can be used to cancel it in the future. Will return NULL if inapropriate values are passed in</returns>
        /// <param name="timeInterval">Time interval in seconds. MUST BE GREATER THAN 0.</param>
        /// <param name="repeats">If set to <c>true</c> repeats.</param>
        /// <param name="title">Title.</param>
        /// <param name="body">Body.</param>
        public string ScheduleNotification (double timeInterval, bool repeats, string title, string body) {
            if (timeInterval <= 0) {
                return null;
            }

            var content = new UNMutableNotificationContent ();
            content.Title = title;
            content.Body = body;

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger (timeInterval, repeats);
            var requestID = Guid.NewGuid ().ToString ("D");
            var request = UNNotificationRequest.FromIdentifier (requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest (request, (err) => {
                if (err != null) {
                    Console.WriteLine (err.Description);
                }
            });

            return requestID;
        }

        /// <summary>
        /// Removes the pending notification.
        /// </summary>
        /// <param name="requestID">Request identifier.</param>
        public void RemovePendingNotification (string requestID) {
            if (requestID == null)
                throw new ArgumentException ("Notification RequestID cannot be null...");
            
            var requests = new string[] { requestID };
            UNUserNotificationCenter.Current.RemovePendingNotificationRequests (requests);
        }

        /// <summary>
        /// Removes all pending notifications.
        /// </summary>
        public void RemoveAllPendingNotifications () {
            UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests ();
        }

        /// <summary>
        /// Show a simple alert box.
        /// </summary>
        public void Alert(string title, string message)
        {
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("Understood", UIAlertActionStyle.Default, null));
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(okAlertController, true, () => { });
        }
    }
}
