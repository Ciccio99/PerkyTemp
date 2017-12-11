using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace PerkyTemp.Utilities {
    public static class Utilities {
        public static float CelsiusToFahrenheit (float temp) {
            return (temp * 9f / 5f) + 32f;
        }

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

        public static float ExtractTemperatureFromChars (char[] hexChars) {
            if (hexChars.Length < 2)
                throw new ArgumentException ("hexChars must contains at least 2 chars...");
            
            char val1 = hexChars[hexChars.Length - 1];
            char val2 = hexChars[hexChars.Length - 2];
            int num1 = Convert.ToInt32 (val1);
            int num2 = Convert.ToInt32 (val2);

            float temperature = num1 + val2 / 100f;

            return temperature;
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
    }
}
