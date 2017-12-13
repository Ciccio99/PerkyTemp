using System;
using Foundation;
using UIKit;
using UserNotifications;
using PerkyTemp.iOS.Notifications;

namespace PerkyTemp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            // Notifications check
            UNUserNotificationCenter.Current.GetNotificationSettings ((settings) => {
                var alertsAllowed = (settings.AlertSetting == UNNotificationSetting.Enabled);
                if (!alertsAllowed) {
                    UNUserNotificationCenter.Current.RequestAuthorization (UNAuthorizationOptions.Alert, (approved, err) => {
                    });
                }
            });

            return base.FinishedLaunching(app, options);
        }
    }
}
