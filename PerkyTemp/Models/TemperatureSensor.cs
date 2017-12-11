﻿/*
 * Singleton TemperatureSensor
 */
using System;
namespace PerkyTemp.Models {
    public class TemperatureSensor {

        public static TemperatureSensor Instance {
            get {
                if (instance == null)
                    instance = new TemperatureSensor ();
                return instance;
            }
        }

        public string UUID { get; set;}
        public float Temperature {set {
                Temperature = value;
                OnTemperatureUpdatedEvent?.Invoke ();
            } 
        }

       

        private static TemperatureSensor instance;

        private TemperatureSensor () {}

        public delegate void OnTemperatureUpdated();

        public event OnTemperatureUpdated OnTemperatureUpdatedEvent;



    }
}