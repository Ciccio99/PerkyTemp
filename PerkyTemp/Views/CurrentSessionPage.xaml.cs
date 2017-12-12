using System;
using System.Collections.Generic;
using PerkyTemp.Interfaces;
using Xamarin.Forms;

namespace PerkyTemp.Views {
    public partial class CurrentSessionPage : ContentPage {
        public CurrentSessionPage () {
            InitializeComponent ();
            this.BackgroundColor = Color.FromHex ("#36AFC5");
#if __IOS__
            Padding = new Thickness (0, 25, 0, 0);
#endif

            // Adds button interactions to different xaml objects
            var convertTemperatureTap = new TapGestureRecognizer ();
            convertTemperatureTap.Tapped += (sender, e) => {
                TheViewModel.ToggleTemperatureConversion ();
            };
            TemperatureConversionLabel.GestureRecognizers.Add (convertTemperatureTap);
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            TheViewModel.StartOrStopCurrentSession();
        }

        private void Convert_Btn_Clicked (object sender, EventArgs e) {
            TheViewModel.ToggleTemperatureConversion ();
        }
    }
}
