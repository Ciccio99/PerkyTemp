using System;
using System.ComponentModel;
using Xamarin.Forms;
using PerkyTemp.Interfaces;

namespace PerkyTemp.ViewModels {
    public class CurrentSessionViewModel : INotifyPropertyChanged{

        public string Log { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private IBluetoothManager _bluetoothManager;

        public CurrentSessionViewModel () {
            _bluetoothManager = DependencyService.Get<IBluetoothManager> ();

            Log = _bluetoothManager.Test ();
        }

    }
}
