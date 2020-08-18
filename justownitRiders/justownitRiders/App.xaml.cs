using justownitRiders.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace justownitRiders
{
	public partial class App : Application
	{

		public static NavigationPage Navigation = null;

		const int smallWightResolution = 768;
		const int smallHeightResolution = 1280;
		public App()
		{
			InitializeComponent();
			LoadStyles();

			Navigation = new NavigationPage(new LoginView());
			Application.Current.MainPage = Navigation;
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		void LoadStyles()
		{
			if (IsASmallDevice())
			{
				dictionary.MergedDictionaries.Add(SmallDevicesStyle.SharedInstance);
			}
			else
			{
				dictionary.MergedDictionaries.Add(GeneralDevicesStyle.SharedInstance);
			}
		}

		public static bool IsASmallDevice()
		{
			// Get Metrics
			var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

			// Width (in pixels)
			var width = mainDisplayInfo.Width;

			// Height (in pixels)
			var height = mainDisplayInfo.Height;
			return (width <= smallWightResolution && height <= smallHeightResolution);
		}
	}
}
