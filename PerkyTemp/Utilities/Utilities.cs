/*
    Utilities static class that maintains helper functions
*/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PerkyTemp.Utilities {
    public static class Utilities {
        /// <summary>
        /// Celsius to fahrenheit.
        /// </summary>
        /// <returns>The to fahrenheit.</returns>
        /// <param name="temp">Temperature in Celsius</param>
        public static float CelsiusToFahrenheit (float temp) {
            return (temp * 9f / 5f) + 32f;
        }

        /// <summary>
        /// Converts the given Bluetotth hexstring and converts it to a temperature float in celsius
        /// </summary>
        /// <returns>The hex to temperature.</returns>
        /// <param name="hexInput">Hex input.</param>
        public static float StringHexToTemperature (string hexInput) {
            // Clean input
            hexInput = Regex.Replace (hexInput, "[ ,|']", "[]");
            // Split into string array of bytes
            string[] hexSplitVals = SplitInParts (hexInput, 2);

            string joinedTempHex = String.Join ("", new string[] { hexSplitVals[1], hexSplitVals[0] });
            int temperatureValue = Convert.ToInt32 (joinedTempHex, 16);
            float tempf = (float) temperatureValue / 100f;

            return tempf;
        }

        public static string[] SplitInParts(String s, int partLength) {
            if (s == null)
                throw new ArgumentException ("s is null...");
            if (partLength <= 0)
                throw new ArgumentException ("Part length must be greater than 0.");

            List<string> stringSplits = new List<string> ();

            for (var i = 0; i < s.Length; i += partLength) {
                stringSplits.Add (s.Substring (i, Math.Min (partLength, s.Length - i)));
            }

            return stringSplits.ToArray ();
        }

        public static double DateTimeToUnixTimestamp(DateTime dt)
        {
            return (dt - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime UnixTimestampToDateTime(double ts)
        {
            return new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(ts);
        }
    }
}
