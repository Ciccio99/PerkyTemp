using System;
using CoreBluetooth;
using CoreFoundation;
using PerkyTemp.Interfaces;
using Xamarin.Forms;
using System.Diagnostics;
using System.Timers;
using Foundation;
using PerkyTemp.Utilities;
using System.Linq;
using System.Text.RegularExpressions;
using PerkyTemp.Models;

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

            // Get advertised data and convert to dict
            var serviceDataDict = (NSDictionary) e.AdvertisementData[CBAdvertisement.DataServiceDataKey];
            // Get keys becase keys are NSobjects and are weird to decipher, if no keys then null
            var serviceDataKeys = serviceDataDict?.Keys;

            if (serviceDataKeys != null && 
                serviceDataKeys.Length == 2 && 
                serviceDataKeys[0].Description == Constants.HEALTH_THERMOMETER && 
                serviceDataKeys[1].Description == Constants.BATTERY) {

                var temperature = serviceDataDict[serviceDataKeys[0]].Description;
                temperature = Regex.Replace (temperature, "[<>]", "");
                var battery = serviceDataDict[serviceDataKeys[1]].Description;
                battery = Regex.Replace (battery, "[<>]", "");

                Debug.WriteLine ("service data: {0}", temperature);
                Debug.WriteLine ("service data: {0}", battery);
                // Stop the scan once found
                _centralManager.StopScan ();
                _SetActivePeripheral (peripheral, temperature);
            }
        }

        private void OnFailedToConnectToPeripheral (Object e, CBPeripheralErrorEventArgs args) {
            Debug.WriteLine ("Failed to connect to {0} because {1} ", args.Peripheral.Identifier, args.Error.ToString ());
        }

        private void _SetActivePeripheral (CBPeripheral peripheral, string temperature) {
            Debug.WriteLine ("Setting active peripheral... " + peripheral.Identifier);
            _activePeripheral = peripheral;

            var tempSensor = TemperatureSensor.Instance;
            tempSensor.Temperature = Utilities.Utilities.StringHexToTemperature (temperature);
            tempSensor.UUID = peripheral.Identifier.ToString ();

            Debug.WriteLine ("Temperature: {0}", tempSensor.Temperature);
        }
    }
}
