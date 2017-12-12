using System;
using PerkyTemp.Interfaces;
using Xamarin.Forms;
using System.Diagnostics;

[assembly: Dependency(typeof(PerkyTemp.Droid.Services.BluetoothManager))]
namespace PerkyTemp.Droid.Services
{
    /// <summary>
    /// TODO: Implement this for Android (see the interface for a description
    /// of what it should be doing).
    /// </summary>
    public class BluetoothManager : IBluetoothManager
    {
        public string Test()
        {
            return "Meow from Android";
        }
    }
}
