using System;
using CoreBluetooth;
using Foundation;

namespace PerkyTemp.iOS.Services {
    public class PerkyPeripheralDelegate : CBPeripheralDelegate {
        public override void DiscoveredService (CBPeripheral peripheral, NSError error) {
            System.Console.WriteLine ("Discovered a service");
            foreach (var service in peripheral.Services) {
                Console.WriteLine (service.ToString ());
                peripheral.DiscoverCharacteristics (service);
            }
        }

        public override void DiscoveredCharacteristic (CBPeripheral peripheral, CBService service, NSError error) {
            System.Console.WriteLine ("Discovered characteristics of " + peripheral);
            foreach (var c in service.Characteristics) {
                Console.WriteLine (c.ToString ());
                peripheral.ReadValue (c);
            }
        }

        public override void UpdatedValue (CBPeripheral peripheral, CBDescriptor descriptor, NSError error) {
            Console.WriteLine ("Value of characteristic " + descriptor.Characteristic + " is " + descriptor.Value);
        }

        public override void UpdatedCharacterteristicValue (CBPeripheral peripheral, CBCharacteristic characteristic, NSError error) {
            Console.WriteLine ("Value of characteristic " + characteristic.ToString () + " is " + characteristic.Value);
        }

    }
}


