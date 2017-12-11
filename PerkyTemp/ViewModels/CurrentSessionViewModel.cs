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

        public string CurrentTemp
        {
            get => TemperatureSensor.Instance.UUID != null ? 
                                    TemperatureSensor.Instance.Temperature.ToString () :
                                    "Scanning...";
        }

        public string ButtonText
        {
            get => isSessionStarted ? "Stop Session" : "Start Session";
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

        private void OnTemperatureChanged () {
            OnPropertyChanged (nameof (CurrentTemp));
        }
    }
}
