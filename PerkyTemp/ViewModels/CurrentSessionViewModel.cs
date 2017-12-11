using System;
using System.ComponentModel;
using Xamarin.Forms;
using PerkyTemp.Interfaces;
using PerkyTemp.Models;

namespace PerkyTemp.ViewModels {
    public class CurrentSessionViewModel : INotifyPropertyChanged {
        private bool isSessionStarted = false;
        private DateTime whenSessionStarted;

        public float CurrentTemp
        {
            get => TemperatureSensor.Instance.Temperature;
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

        public string Log { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private IBluetoothManager _bluetoothManager;

        public CurrentSessionViewModel()
        {
            _bluetoothManager = DependencyService.Get<IBluetoothManager> ();
            Log = _bluetoothManager.Test ();
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
        }

        private void StopCurrentSession()
        {
            // TODO: Start temp and final temp
            PerkyTempDatabase.Database.SaveSession(PastSession.FromFields(whenSessionStarted, DateTime.Now, 5, 6));
            isSessionStarted = false;
        }

        private void OnTemperatureChanged () {
            OnPropertyChanged ("CurrentTemp");
        }
    }
}
