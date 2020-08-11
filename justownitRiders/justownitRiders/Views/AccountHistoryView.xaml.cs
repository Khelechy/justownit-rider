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
	public partial class AccountHistoryView : ContentPage
	{
		public AccountHistoryView()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
	}
}