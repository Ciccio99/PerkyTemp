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
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
            this.BackgroundColor = Color.FromHex ("#36AFC5");
#if __IOS__
            Padding = new Thickness (0, 25, 0, 0);
#endif
		}
	}
}