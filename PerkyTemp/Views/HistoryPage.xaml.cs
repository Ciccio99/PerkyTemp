using PerkyTemp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PerkyTemp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
        //internal HistoryViewModel ViewModel { get; private set; }

        public HistoryPage ()
		{
            //ViewModel = HistoryViewModel.GetInstance();
			InitializeComponent ();
		}
    }
}