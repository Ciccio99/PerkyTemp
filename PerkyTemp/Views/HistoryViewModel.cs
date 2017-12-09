using PerkyTemp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerkyTemp.Views
{
    class HistoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public List<PastSession> Sessions { get => PerkyTempDatabase.Database.GetSessions(); }
        public List<PastSession> Sessions
        {
            get => new List<PastSession>() {
                PastSession.FromFields("00", new DateTime(1997, 11, 13, 1, 2, 3), new DateTime(1997, 11, 13, 4, 5, 6), 6, 7)
            };
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
