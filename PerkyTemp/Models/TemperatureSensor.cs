using System;

namespace PerkyTemp.Models {
    /// <summary>
    /// Singleton model used to manage when a sensor has pushed new
    /// temperature data.
    /// </summary>
    public class TemperatureSensor {

        /// <summary>
        /// Get the singleton instance of the TemperatureSensor class.
        /// </summary>
        public static TemperatureSensor Instance {
            get {
                if (instance == null)
                    instance = new TemperatureSensor ();
                return instance;
            }
        }

        public string UUID { get; set;}

        /// <summary>
        /// Get the current temperature, as last reported via a bluetooth
        /// device.
        /// </summary>
        public float Temperature {
            get => _temperature;
            set {
                _temperature = value;
                OnTemperatureUpdatedEvent?.Invoke ();
            } 
        }

        private float _temperature = float.MinValue;

        private static TemperatureSensor instance;

        private TemperatureSensor () {}

        public delegate void OnTemperatureUpdated();
        /// <summary>
        /// Event that is invoked whenever a new temperature is set.
        /// </summary>
        public event OnTemperatureUpdated OnTemperatureUpdatedEvent;
    }
}
