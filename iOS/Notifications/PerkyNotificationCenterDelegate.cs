using System;
using UserNotifications;

namespace PerkyTemp.iOS.Notifications {
    public class PerkyNotificationCenterDelegate : UNUserNotificationCenterDelegate {
        public PerkyNotificationCenterDelegate () {
        }

        public override void WillPresentNotification (UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler) {
            Console.WriteLine ("Active Notification: {0}", notification);

            completionHandler (UNNotificationPresentationOptions.Alert);
        }
    }
}
