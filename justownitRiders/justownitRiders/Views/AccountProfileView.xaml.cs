using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace justownitRiders.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountProfileView : ContentPage
	{
		public AccountProfileView()
		{
			InitializeComponent();
			//NavigationPage.SetHasNavigationBar(this, false);
		}

		private async void ToPersonal_Tapped(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AccountPersonalView());
		}

		private async void ToContact_Tapped(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AccountPersonalView());
		}

		private async void ToSecurity_Tapped(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AccountSecurityView());
		}

		private void ToLogout_Tapped(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				var exit = await this.DisplayAlert("Confirm Exit", "Do you really want to exit the application?", "Yes", "No").ConfigureAwait(false);

				if (exit)
					System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
			});
		}
	}
}