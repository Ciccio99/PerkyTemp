using PerkyTemp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerkyTemp.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double NotificationTime
        {
            get => PerkyTempDatabase.Database.GetSettings().NotificationTime;
            set
            {
                SettingsModel currentSettings = PerkyTempDatabase.Database.GetSettings();
                currentSettings.NotificationTime = value;
                PerkyTempDatabase.Database.SaveSettings(currentSettings);
                OnPropertyChanged(nameof(NotificationTime));
            }
        }

        public float TemperatureThreshold
        {
            get => PerkyTempDatabase.Database.GetSettings().TemperatureThreshold;
            set
            {
                SettingsModel currentSettings = PerkyTempDatabase.Database.GetSettings();
                currentSettings.TemperatureThreshold = value;
                PerkyTempDatabase.Database.SaveSettings(currentSettings);
                OnPropertyChanged(nameof(TemperatureThreshold));
            }
        }
    }
}
