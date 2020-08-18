using Acr.UserDialogs;
using justownitRiders.Managers;
using justownitRiders.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace justownitRiders.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		public LoginViewModel()
		{
			BindEmail();

		}


		private string email;
		public string Email
		{
			get { return email; }
			set
			{
				email = value;
				OnPropertyChanged();
			}
		}

		private string password;
		public string Password
		{
			get { return password; }
			set
			{
				password = value;
				OnPropertyChanged();
			}
		}

		public async void BindEmail()
		{
			Email = await GetEmail();
		}



		public Command LoginCommand
		{
			get
			{
				return new Command(Login);
			}
		}

		private async void Login()
		{

			if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
			{
				await App.Current.MainPage.DisplayAlert("Login Error", "Empty Email or Password fields", "OK");
			}
			else
			{
				var manager = new ApiManager();
				UserDialogs.Instance.ShowLoading("Loading");
				var loginResponseTuple = await manager.Login(email, password);
				var loginResponse = loginResponseTuple.Item1;
				string error = loginResponseTuple.Item2;
				var loginRp = loginResponseTuple.Item3;
				if (!string.IsNullOrWhiteSpace(error))
				{
					UserDialogs.Instance.HideLoading();
					await App.Current.MainPage.DisplayAlert("Login Error", error, "OK");
					return;
				}
				if (loginResponse == null)
				{
					UserDialogs.Instance.HideLoading();
					await App.Current.MainPage.DisplayAlert("Login Error", error, "OK");
					return;
				}
				else if (loginResponse.status == 200)
				{
					if (loginRp.Contains("dispatch_rider"))
					{
						try
						{
							await SecureStorage.SetAsync("LoginEmail", email);
							manager.ExtractUserDetails();
						}
						catch (Exception ex)
						{
							//nothing
						}

						await App.Current.MainPage.Navigation.PushAsync(new ButtomNavigationBar(), true);
						UserDialogs.Instance.HideLoading();
					}
					else
					{
						UserDialogs.Instance.HideLoading();
						await App.Current.MainPage.DisplayAlert("Login Error", "You dont have access on this platform, register on this platform", "OK");
					}

					//try
					//{
					//	await SecureStorage.SetAsync("LoginEmail", email);
					//	manager.ExtractUserDetails();
					//}
					//catch (Exception ex)
					//{
					//	//nothing
					//}

					//await App.Current.MainPage.Navigation.PushAsync(new ButtomNavigationBar(), true);
					//UserDialogs.Instance.HideLoading();

				}
				else if (loginResponse.status == 401)
				{
					UserDialogs.Instance.HideLoading();
					await App.Current.MainPage.DisplayAlert("Login Error", error, "OK");
				}
				else
				{
					UserDialogs.Instance.HideLoading();
					await App.Current.MainPage.DisplayAlert("Login Error", error, "OK");
				}
			}


		}

		public async Task<string> GetEmail()
		{
			string loginEmail = string.Empty;
			loginEmail = await SecureStorage.GetAsync("LoginEmail");
			if (loginEmail == null)
			{
				loginEmail = "";
			}
			else
			{
				return loginEmail;
			}

			return loginEmail;
		}
	}
}
