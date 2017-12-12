using System;
using System.ComponentModel;
using Xamarin.Forms;
using PerkyTemp.Interfaces;
using PerkyTemp.Models;

namespace PerkyTemp.ViewModels {
    public class CurrentSessionViewModel : INotifyPropertyChanged {
        private CurrentSession currentSession = null;
        private IBluetoothManager _bluetoothManager;
        private INotificationManager _notificationManager;
        private string _currentSessionNotificationID;
        private bool _isFahrenheit;

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

        public string ButtonText
        {
            get => currentSession == null ? "Start Session" : "Stop Session";
        }

        public string ConvertText {
            get => TemperatureSensor.Instance.UUID != null ? (_isFahrenheit ? "°F" : "°C") : "";
        }

        public string OppositeConvertText {
            get => _isFahrenheit ? "°C" : "°F";
        }

        public string Status
        {
            get => currentSession == null
                ? "No current session"
                : "Session started " + Math.Round((DateTime.Now - currentSession.StartTime).TotalMinutes, 2) + " minutes ago";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnDatabaseUpdated()
        {
            RescheduleNotification();
        }

        public CurrentSessionViewModel()
        {
            _bluetoothManager = DependencyService.Get<IBluetoothManager> ();
            _notificationManager = DependencyService.Get<INotificationManager> ();
            TemperatureSensor.Instance.OnTemperatureUpdatedEvent += OnTemperatureChanged;
            PerkyTempDatabase.Database.AddDatabaseChangeListener(OnDatabaseUpdated);
        }

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

        private void StartCurrentSession()
        {
            // TODO: What to do if we don't have a temperature reading??
            currentSession = new CurrentSession(TemperatureSensor.Instance.Temperature);
            RescheduleNotification();
        }

        private void StopCurrentSession()
        {
            PerkyTempDatabase.Database.SaveSession(currentSession.EndSession());
            currentSession = null;
            if (_currentSessionNotificationID != null)
            {
                _notificationManager.RemovePendingNotification(_currentSessionNotificationID);
                _currentSessionNotificationID = null;
            }
        }

        public void ToggleTemperatureConversion () {
            _isFahrenheit = !_isFahrenheit;
            OnTemperatureChanged ();
        }

        private void OnTemperatureChanged () {
            OnPropertyChanged (nameof (CurrentTemp));
            OnPropertyChanged (nameof (ConvertText) );
            currentSession?.RecordTempReading(TemperatureSensor.Instance.Temperature);
            RescheduleNotification();
        }

        private void RescheduleNotification()
        {
            if (_currentSessionNotificationID != null)
            {
                _notificationManager.RemovePendingNotification(_currentSessionNotificationID);
            }

            if (currentSession == null) return;

            double notificationTimeSettingMins = PerkyTempDatabase.Database.GetSettings().NotificationTime;
            double notificationTime = currentSession.GetTimeRemainingSec();
            _currentSessionNotificationID = _notificationManager.ScheduleNotification(
                notificationTime - notificationTimeSettingMins * 60.0,
                false,
                notificationTimeSettingMins + " min remaining",
                "PerkyTemp: Vest will expire in " + notificationTimeSettingMins + " minutes");
        }


    }
}
