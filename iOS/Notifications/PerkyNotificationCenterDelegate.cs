/*
    Notification delegage that informs iOS how to handle our applications notifications
*/
using System;
using UserNotifications;

namespace PerkyTemp.iOS.Notifications {
    public class PerkyNotificationCenterDelegate : UNUserNotificationCenterDelegate {
        public PerkyNotificationCenterDelegate () {
        }

        /// <summary>
        /// Overriden function establishes how a notitication should be displayed
        /// </summary>
        /// <param name="center">Center.</param>
        /// <param name="notification">Notification.</param>
        /// <param name="completionHandler">Completion handler.</param>
        public override void WillPresentNotification (UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler) {
            completionHandler (UNNotificationPresentationOptions.Alert);
        }
    }
}
