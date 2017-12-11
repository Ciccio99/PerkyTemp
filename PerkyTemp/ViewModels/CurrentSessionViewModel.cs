using System;
using System.ComponentModel;
using Xamarin.Forms;
using PerkyTemp.Interfaces;
using PerkyTemp.Models;

namespace PerkyTemp.ViewModels {
    public class CurrentSessionViewModel : INotifyPropertyChanged {
        private bool isSessionStarted = false;
        private DateTime whenSessionStarted;
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
            get => isSessionStarted ? "Stop Session" : "Start Session";
        }

        public string ConvertText {
            get => TemperatureSensor.Instance.UUID != null ? (_isFahrenheit ? "°F" : "°C") : "";
        }

        public string OppositeConvertText {
            get => _isFahrenheit ? "°C" : "°F";
        }

        public string Status
        {
            get => isSessionStarted
                ? "Session started " + Math.Round((DateTime.Now - whenSessionStarted).TotalMinutes, 2) + " minutes ago"
                : "No current session";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public CurrentSessionViewModel()
        {
            _bluetoothManager = DependencyService.Get<IBluetoothManager> ();
            _notificationManager = DependencyService.Get<INotificationManager> ();
            TemperatureSensor.Instance.OnTemperatureUpdatedEvent += OnTemperatureChanged;
        }

        public void StartOrStopCurrentSession()
        {
            if (isSessionStarted)
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
            whenSessionStarted = DateTime.Now;
            isSessionStarted = true;
            _currentSessionNotificationID = _notificationManager.ScheduleNotification (10, false, "Session Started", "This message informs you that the current session has started.");
        }

        private void StopCurrentSession()
        {
            // TODO: Start temp and final temp
            PerkyTempDatabase.Database.SaveSession(PastSession.FromFields(whenSessionStarted, DateTime.Now, 5, 6));
            isSessionStarted = false;
            if (_currentSessionNotificationID != null)
                _notificationManager.RemovePendingNotification (_currentSessionNotificationID);
        }

        public void ToggleTemperatureConversion () {
            _isFahrenheit = !_isFahrenheit;
            OnTemperatureChanged ();
        }

        private void OnTemperatureChanged () {
            OnPropertyChanged (nameof (CurrentTemp));
            OnPropertyChanged (nameof (ConvertText) );
        }


    }
}
