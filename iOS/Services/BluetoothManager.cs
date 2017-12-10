using System;
using CoreBluetooth;
using CoreFoundation;
using PerkyTemp.Interfaces;
using Xamarin.Forms;
using System.Diagnostics;

[assembly: Dependency (typeof (PerkyTemp.iOS.Services.BluetoothManager))]
namespace PerkyTemp.iOS.Services {
    public class BluetoothManager : IBluetoothManager {

        private PerkyCBCentralManagerDelegate _managerDel;
        private CBCentralManager _centralManager;

        public BluetoothManager () {
            _managerDel = new PerkyCBCentralManagerDelegate ();
            _centralManager = new CBCentralManager (_managerDel, null);
            Debug.WriteLine ( "Bluetooth manager IOS constructed");
        }

        public string Test () {
            return "Meow";
        }
    }
}
