using System;
namespace PerkyTemp.Interfaces {
    /// <summary>
    /// Interface for the bluetooth manager. This should be instantiated via
    /// dependency injection to get the correct instance depending on platform
    /// (iOS or Android). When instantiated, it should update properties on the
    /// singleton TemperatureSensor whenever there is updated temperature info.
    /// </summary>
    public interface IBluetoothManager {
        string Test ();
    }
}
