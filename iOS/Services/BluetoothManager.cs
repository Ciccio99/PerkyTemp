using System;
using CoreBluetooth;
using CoreFoundation;
using PerkyTemp.Interfaces;
using Xamarin.Forms;
using System.Diagnostics;
using System.Timers;
using Foundation;
using PerkyTemp.Utilities;

// https://github.com/xamarin/mobile-samples/blob/master/BluetoothLEExplorer/BluetoothLEExplorer.iOS/BluetoothLEManager.cs

[assembly: Dependency (typeof (PerkyTemp.iOS.Services.BluetoothManager))]
namespace PerkyTemp.iOS.Services {
    public class BluetoothManager : IBluetoothManager {

        private PerkyCBCentralManagerDelegate _managerDel;
        private CBCentralManager _centralManager;
        private CBPeripheral _activePeripheral;

        public BluetoothManager () {
            _managerDel = new PerkyCBCentralManagerDelegate ();
            //_centralManager = new CBCentralManager (_managerDel, null);
            _centralManager = new CBCentralManager ();
            _centralManager.UpdatedState += OnUpdatedState;
            _centralManager.DiscoveredPeripheral += OnDiscoveredPeripheral;
            _centralManager.ConnectedPeripheral += OnPeripheralConnected;
            _centralManager.FailedToConnectPeripheral += OnFailedToConnectToPeripheral;

            Debug.WriteLine ("Bluetooth manager IOS constructed");
        }

        public string Test () {
            return "Meow";
        }

        // Event methods
        public void OnPeripheralConnected (Object sender, CBPeripheralEventArgs e) {
            _activePeripheral = e.Peripheral;
            Debug.WriteLine ("Connected to " + _activePeripheral.Name);

            if (_activePeripheral.Delegate == null) {
                _activePeripheral.Delegate = new PerkyPeripheralDelegate ();

                _activePeripheral.DiscoverServices ();
            }
        }

        public void OnUpdatedState (Object s, EventArgs e) {
            if (_centralManager.State == CBCentralManagerState.PoweredOn) {
                Debug.WriteLine ("Scanning for peripherals");
                ScanForTemperatureSensor ();
            } else {
                Console.WriteLine ("Bluetooth is not available!");
            }
        }

        public void ScanForTemperatureSensor () {
            CBUUID[] cbuuids = null;
            _centralManager.ScanForPeripherals (cbuuids);
            //Timeout of 70 secs    
            var timer = new Timer (70 * 1000);
            timer.Elapsed += (sender, ev) => { 
                ((Timer)sender).Stop ();
                ((Timer)sender).Dispose ();
                OnScanTimeout (); 
            };
            timer.Start ();

        }

        private void OnScanTimeout () {
            Debug.WriteLine ("Stopping Scan for devices");
            _centralManager?.StopScan ();
        }

        private void OnDiscoveredPeripheral (Object sender, CBDiscoveredPeripheralEventArgs e) {
            var peripheral = e.Peripheral;
            Debug.WriteLine ("Discovered {0}, data {1}, RSSI {2}", peripheral.Identifier, e.AdvertisementData, e.RSSI);
            var serviceData = e.AdvertisementData["kCBAdvDataServiceData"];
            var serviceDataDict = (NSDictionary)serviceData;
            if (peripheral.Name == "8735ED6D") {
                foreach (var data in serviceDataDict) {
                    Debug.WriteLine ("service data: {0}", data);
                }

            }
            //if (keys.Length == 2 && keys[0].ToString () == Constants.HEALTH_KEY && keys[1].ToString () == Constants.BATTERY_KEY) {
            //    Debug.WriteLine ("Found \"{0}\"... Stopping Scan for Devices", peripheral.Name);
            //    // Stop Scanning
            //    _centralManager.StopScan ();
            //    // Connect that Peripheral
            //    //_centralManager.ConnectPeripheral (peripheral);
            //    _SetActivePeripheral (peripheral);
            //}
        }

        private void OnFailedToConnectToPeripheral (Object e, CBPeripheralErrorEventArgs args) {
            Debug.WriteLine ("Failed to connect to {0} because {1} ", args.Peripheral.Identifier, args.Error.ToString ());
        }

        private void _SetActivePeripheral (CBPeripheral peripheral) {
            Debug.WriteLine ("Setting active peripheral... " + peripheral.Identifier);
            _activePeripheral = peripheral;
            _activePeripheral.Delegate = new PerkyPeripheralDelegate ();
            _activePeripheral.DiscoverServices ();
            Debug.WriteLine ("Peripheral state: " +_activePeripheral.State.ToString ());
            Debug.WriteLine ("Peripheral GUID: " + _activePeripheral.Identifier.GetBytes ().Length);
        }
    }
}
