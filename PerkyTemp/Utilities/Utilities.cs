using System;
namespace PerkyTemp.Utilities {
    public static class Utilities {
        public static float CelsiusToFahrenheit (float temp) {
            return (temp * 9f / 5f) + 32f;
        }
    }
}
