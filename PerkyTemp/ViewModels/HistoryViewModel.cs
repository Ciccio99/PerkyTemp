using PerkyTemp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerkyTemp.ViewModels
{
    /// <summary>
    /// A ViewModel for HistoryPage.
    /// </summary>
    class HistoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The list of PastSessions that should be shown in the history list.
        /// </summary>
        public List<PastSession> Sessions { get => PerkyTempDatabase.Database.GetSessions(); }

        public HistoryViewModel()
        {
            PerkyTempDatabase.Database.AddDatabaseChangeListener(OnDatabaseUpdated);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// A handler for when the database has changed.
        /// </summary>
        public void OnDatabaseUpdated()
        {
            OnPropertyChanged(nameof(Sessions));
        }
    }
}
