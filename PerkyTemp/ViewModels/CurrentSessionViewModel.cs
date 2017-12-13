using System;
using System.ComponentModel;
using Xamarin.Forms;
using PerkyTemp.Interfaces;
using PerkyTemp.Models;

namespace PerkyTemp.ViewModels {
    /// <summary>
    /// A ViewModel for CurrentSessionPage.
    /// </summary>
    public class CurrentSessionViewModel : INotifyPropertyChanged {
        private CurrentSession currentSession = null;
        private IBluetoothManager _bluetoothManager;
        private INotificationManager _notificationManager;
        private string _currentSessionNotificationID;
        private bool _isFahrenheit;

        /// <summary>
        /// The current temperature, as should be shown in the UI. This will
        /// return "Scanning" if no temperature data has yet been found.
        /// </summary>
        public string CurrentTemp
        {
            get {
                string temp;
                if (_isFahrenheit)
                    temp = Utilities.Utilities.CelsiusToFahrenheit (TemperatureSensor.Instance.Temperature).ToString ("##.##");
                else
                    temp = TemperatureSensor.Instance.Temperature.ToString ();
                
                return TemperatureSensor.Instance.UUID != null ? temp : "Scanning...";
            }
        }

        /// <summary>
        /// The text to show on the Start/Stop Session button.
        /// </summary>
        public string ButtonText
        {
            get => currentSession == null ? "Start Session" : "Stop Session";
        }

        /// <summary>
        /// The text to show on the fahrenheit/celsius toggle button.
        /// </summary>
        public string ConvertText {
            get => TemperatureSensor.Instance.UUID != null ? (_isFahrenheit ? "°F" : "°C") : "";
        }

        /// <summary>
        /// The opposite of ConvertText.
        /// </summary>
        public string OppositeConvertText {
            get => _isFahrenheit ? "°C" : "°F";
        }

        /// <summary>
        /// The current status of the session, if one has been started. This
        /// will include the session duration (so far) and either the time
        /// remaining until the vest expires or a message that the vest has
        /// expired.
        /// </summary>
        public string Status
        {
            get
            {
                if (currentSession == null) return "No current session";

                string msg = "Session started " + Math.Round((DateTime.Now - currentSession.StartTime).TotalMinutes) + " minutes ago";
                double? timeRemaining = currentSession.GetTimeRemainingSec();
                if (timeRemaining != null)
                {
                    // If less than 30 secs, say "vest has expired"
                    if (timeRemaining < 30.0)
                    {
                        msg += "\n vest has EXPIRED";
                    }
                    else
                    {
                        msg += "\n vest will expire in " + Math.Round(TimeSpan.FromSeconds(timeRemaining.Value).TotalMinutes) + " minutes";
                    }
                }
                return msg;
             }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Handler for when the database is updated.
        /// </summary>
        public void OnDatabaseUpdated()
        {
            OnPropertyChanged(nameof(Status));
            RescheduleNotification();
        }

        public CurrentSessionViewModel()
        {
            _bluetoothManager = DependencyService.Get<IBluetoothManager> ();
            _notificationManager = DependencyService.Get<INotificationManager> ();
            TemperatureSensor.Instance.OnTemperatureUpdatedEvent += OnTemperatureChanged;
            PerkyTempDatabase.Database.AddDatabaseChangeListener(OnDatabaseUpdated);
        }

        /// <summary>
        /// Button handler to start or stop the current session.
        /// </summary>
        public void StartOrStopCurrentSession()
        {
            if (currentSession != null)
            {
                StopCurrentSession();
            }
            else
            {
                StartCurrentSession();
            }
            OnPropertyChanged(nameof(ButtonText));
            OnPropertyChanged(nameof(Status));
        }

        /// <summary>
        /// Start a new current session.
        /// </summary>
        private void StartCurrentSession()
        {
            currentSession = new CurrentSession();
            if (TemperatureSensor.Instance.UUID != null)
                currentSession.RecordTempReading(TemperatureSensor.Instance.Temperature);
            RescheduleNotification();
        }

        /// <summary>
        /// Stop the currently running current session.
        /// </summary>
        private void StopCurrentSession()
        {
            PastSession maybePastSession = currentSession.EndSession();
            if (maybePastSession == null)
            {
                _notificationManager.Alert("PerkyTemp", "Not recording current session because there is not enough temperature data");
            }
            else
            {
                PerkyTempDatabase.Database.SaveSession(maybePastSession);
            }

            currentSession = null;
            if (_currentSessionNotificationID != null)
            {
                _notificationManager.RemovePendingNotification(_currentSessionNotificationID);
                _currentSessionNotificationID = null;
            }
        }

        /// <summary>
        /// Toggles the temperature conversion.
        /// </summary>
        public void ToggleTemperatureConversion () {
            _isFahrenheit = !_isFahrenheit;
            OnTemperatureChanged ();
        }

        /// <summary>
        /// On the temperature changed, calls the necessary OnPropertyChange functions
        /// </summary>
        private void OnTemperatureChanged () {
            OnPropertyChanged (nameof (CurrentTemp));
            OnPropertyChanged (nameof (ConvertText));
            OnPropertyChanged (nameof (OppositeConvertText));
            currentSession?.RecordTempReading(TemperatureSensor.Instance.Temperature);
            OnPropertyChanged(nameof(Status));
            RescheduleNotification();
        }

        /// <summary>
        /// Schedule a new notification for when the vest expires. This will
        /// also cancel any existing notifications.
        /// </summary>
        private void RescheduleNotification()
        {
            if (_currentSessionNotificationID != null)
            {
                _notificationManager.RemovePendingNotification(_currentSessionNotificationID);
            }

            if (currentSession == null) return;

            double? vestTimeRemainingSec = currentSession.GetTimeRemainingSec();
            if (vestTimeRemainingSec != null)
            {
                // If less than 30 secs remaining, say "expired"
                if (vestTimeRemainingSec < 30.0)
                {
                    _currentSessionNotificationID = _notificationManager.ScheduleNotification(
                        0.01,  // "now"
                        false,
                        "Vest has expired",
                        "PerkyTemp: Vest has expired. Take shelter!");
                }
                else
                {
                    double notificationTimeSettingMins = PerkyTempDatabase.Database.GetSettings().NotificationTime;
                    double secsInFuture = vestTimeRemainingSec.Value - notificationTimeSettingMins * 60.0;
                    int minsRemaining = (int)Math.Round(vestTimeRemainingSec.Value / 60.0);
                    // If it's less than 30 secs in the future, just send it "now"
                    if (secsInFuture < 30.0) secsInFuture = 0.01;
                    _currentSessionNotificationID = _notificationManager.ScheduleNotification(
                        secsInFuture,
                        false,
                        minsRemaining + " minutes remaining",
                        "PerkyTemp: Vest will expire in " + minsRemaining + " minutes");
                }
            }
        }
    }
}
