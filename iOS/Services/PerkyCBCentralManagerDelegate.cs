using System;
using CoreBluetooth;
using System.Timers;
using Foundation;
using System.Diagnostics;

namespace PerkyTemp.iOS.Services {
    public class PerkyCBCentralManagerDelegate : CBCentralManagerDelegate {
        public PerkyCBCentralManagerDelegate () {
        }

        override public void UpdatedState (CBCentralManager mgr) {
            if (mgr.State == CBCentralManagerState.PoweredOn) {
                Debug.WriteLine ("Scanning for peripherals");
                CBUUID[] cbuuids = null;
                // Initiate async calls of DiscoveredPeripheral
                mgr.ScanForPeripherals (cbuuids);
                //Timeout of 30 secs    
                var timer = new Timer (30 * 1000);
                timer.Elapsed += (sender, e) => mgr.StopScan ();

            } else {
                Console.WriteLine ("Bluetooth is not available!");
            }
        }

        public override void DiscoveredPeripheral (CBCentralManager mgr, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI) {
            if (peripheral.Name == "8735ED6D") {
                Debug.WriteLine ("Found \"8735ED6D\"... Stopping Scan for Devices");
                // Stop Scanning
                mgr.StopScan ();
                // Connect that Peripheral
                mgr.ConnectPeripheral (peripheral);
            }

           
            Debug.WriteLine ("Discovered {0}, data {1}, RSSI {2}", peripheral.Name, advertisementData, RSSI);
        }
    }
}
