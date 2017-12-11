using System;
using System.Collections.Generic;
using PerkyTemp.Interfaces;
using Xamarin.Forms;

namespace PerkyTemp.Views
{
    public partial class CurrentSessionPage : ContentPage
    {
        public CurrentSessionPage()
        {
            InitializeComponent();
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            TheViewModel.StartOrStopCurrentSession();
        }
    }
}
