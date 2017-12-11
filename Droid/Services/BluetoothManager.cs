using System;
using PerkyTemp.Interfaces;
using Xamarin.Forms;
using System.Diagnostics;

[assembly: Dependency(typeof(PerkyTemp.Droid.Services.BluetoothManager))]
namespace PerkyTemp.Droid.Services
{
    public class BluetoothManager : IBluetoothManager
    {
        public string Test()
        {
            return "Meow from Android";
        }
    }
}
