/*
    Bluetooth manager for IOS
*/
using System;
using CoreBluetooth;
using CoreFoundation;
using PerkyTemp.Interfaces;
using Xamarin.Forms;
using System.Diagnostics;
using System.Timers;
using Foundation;
using PerkyTemp.Utilities;
using System.Text.RegularExpressions;
using PerkyTemp.Models;
using UIKit;

[assembly: Dependency (typeof (PerkyTemp.iOS.Services.BluetoothManager))]
namespace PerkyTemp.iOS.Services {
    public class BluetoothManager : IBluetoothManager {

        private CBCentralManager _centralManager;
        private CBPeripheral _activePeripheral;
        private bool _timerRunning;

        public BluetoothManager () {
            _centralManager = new CBCentralManager ();
            _centralManager.UpdatedState += OnUpdatedState;
            _centralManager.DiscoveredPeripheral += OnDiscoveredPeripheral;
        }

        public string Test () {
            return "Meow";
        }

        /// <summary>
        /// Method subscribed to a CBMCentralManager's Update state event
        /// </summary>
        /// <param name="s">S.</param>
        /// <param name="e">E.</param>
        public void OnUpdatedState (Object s, EventArgs e) {
            if (_centralManager.State == CBCentralManagerState.PoweredOn) {
                if (!_centralManager.IsScanning)
                    ScanForTemperatureSensor ();
            } else {
                var okAlertController = UIAlertController.Create ("Bluetooth is off", "Please activate bluetooth to receive updates from your vest's temperature sensor.", UIAlertControllerStyle.Alert);
                okAlertController.AddAction (UIAlertAction.Create ("Understood", UIAlertActionStyle.Default, null));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController (okAlertController, true, () => { });
            }
        }

        /// <summary>
        /// Method subscribed to a CBMCentralManager's OnDiscoveredPeripheral Event
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnDiscoveredPeripheral (Object sender, CBDiscoveredPeripheralEventArgs e) {
            var peripheral = e.Peripheral;

            // Get advertised data and convert to dict
            var serviceDataDict = (NSDictionary) e.AdvertisementData[CBAdvertisement.DataServiceDataKey];
            // Get keys becase keys are NSobjects and are weird to decipher, if no keys then null
            var serviceDataKeys = serviceDataDict?.Keys;


            // If the previous ID matches or the peripheral has the desired content...
            if (TemperatureSensor.Instance.UUID == peripheral.Identifier.ToString () ||
                (serviceDataKeys != null && 
                serviceDataKeys.Length == 2 && 
                serviceDataKeys[0].Description == Constants.HEALTH_THERMOMETER && 
                 serviceDataKeys[1].Description == Constants.BATTERY)) {

                // Extract data
                var temperature = serviceDataDict[serviceDataKeys[0]].Description;
                temperature = Regex.Replace (temperature, "[<>]", "");
                var battery = serviceDataDict[serviceDataKeys[1]].Description;
                battery = Regex.Replace (battery, "[<>]", "");

                // Stop the scan once found
                _centralManager.StopScan ();
                _SetActivePeripheral (peripheral, temperature);
            }
        }
       
        /// <summary>
        /// Called when a scanning phase timesout with no temperature sensor found
        /// </summary>
        private void OnScanTimeout () {
            Debug.WriteLine ("Stopping Scan for devices");
            _centralManager?.StopScan ();
            InitTimerNextScan ();
        }

        /// <summary>
        /// Scans for temperature sensor.
        /// </summary>
        public void ScanForTemperatureSensor () {
            if (_centralManager.IsScanning) {
                return;
            }

            Debug.WriteLine ("Scanning for peripherals");

            _centralManager.ScanForPeripherals (new CBUUID[0]);

            // Set a timer to auto-stop scanning after a specified timer
            var timer = new Timer (Constants.SCAN_TIME_DURATION * 1000);
            timer.Elapsed += (sender, ev) => {
                ((Timer)sender).Stop ();
                ((Timer)sender).Dispose ();
                OnScanTimeout ();
            };
            timer.Start ();
        }

        /// <summary>
        /// Sets the active peripheral.
        /// </summary>
        /// <param name="peripheral">Peripheral.</param>
        /// <param name="temperature">Temperature.</param>
        private void _SetActivePeripheral (CBPeripheral peripheral, string temperature) {
            Debug.WriteLine ("Setting active peripheral... " + peripheral.Identifier);
            _activePeripheral = peripheral;

            var tempSensor = TemperatureSensor.Instance;
            tempSensor.Temperature = Utilities.Utilities.StringHexToTemperature (temperature);
            tempSensor.UUID = peripheral.Identifier.ToString ();

            Debug.WriteLine ("Temperature: {0}", tempSensor.Temperature);

            InitTimerNextScan ();
        }

        /// <summary>
        /// Inits the timer for the next scan phase.
        /// </summary>
        private void InitTimerNextScan () {
            if (_timerRunning == true) return;

            Debug.WriteLine ("Starting timer for next scan...");

            var timer = new Timer (Constants.TIME_UNTIL_NEXT_SCAN * 1000);
            timer.Elapsed += (sender, e) => {
                ScanForTemperatureSensor ();
                ((Timer)sender).Stop ();
                ((Timer)sender).Dispose ();
                _timerRunning = false;
            };
            timer.Start ();
            _timerRunning = true;
        }
    }
}
