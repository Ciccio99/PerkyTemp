﻿namespace PerkyTemp.Utilities {
    /// <summary>
    /// Class that maintains constants that are used throughout the app.
    /// </summary>
    public static class Constants {

        // BLE consts
        public const string HEALTH_THERMOMETER = "Health Thermometer";
        public const string BATTERY = "Battery";
        public const int GATT_HEALTH_THERMOMETER = 0x1809;
        public const int GATT_BATTERY = 0x180f;

        // Timer Consts
        public const int SCAN_TIME_DURATION = 30;
        public const int TIME_UNTIL_NEXT_SCAN = 5;

    }
}
