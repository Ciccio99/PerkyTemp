/*
 * Singleton TemperatureSensor
 */
using System;
namespace PerkyTemp.Models {
    public class TemperatureSensor {
        
        public static TemperatureSensor instance;
        public byte[] UUID { get; set;}

        public static TemperatureSensor Instance {
            get {
                if (instance == null)
                    instance = new TemperatureSensor ();
                return instance;
            }
        }

    }
}
