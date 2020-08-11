using Acr.UserDialogs;
using justownitRiders.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace justownitRiders.ViewModels
{
	public class AccountSecurityViewModel : BaseViewModel
	{

		public AccountSecurityViewModel()
		{
		}

		private string oldPassword;
		public string OldPassword
		{
			get { return oldPassword; }
			set
			{
				oldPassword = value;
				OnPropertyChanged();
			}
		}

		private string newPassword;
		public string NewPassword
		{
			get { return newPassword; }
			set
			{
				newPassword = value;
				OnPropertyChanged();
			}
		}

		public Command ChangePasswordCommand
		{
			get
			{
				return new Command(ChangePassword);
			}
		}

		private async void ChangePassword()
		{

			if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword))
			{
				await App.Current.MainPage.DisplayAlert("Error", "Old or New Password field is empty", "OK");
			}
			else
			{
				var manager = new ApiManager();
				var ChangePasswordObj = new
				{
					oldPassword = OldPassword,
					newPassword = NewPassword
				};
				UserDialogs.Instance.ShowLoading("Loading");
				var requestResponseTuple = await manager.ChangePassword(ChangePasswordObj);
				var requestResponse = requestResponseTuple.Item1;
				string error = requestResponseTuple.Item2;
				if (!string.IsNullOrWhiteSpace(error))
				{

					UserDialogs.Instance.HideLoading();
					await App.Current.MainPage.DisplayAlert("Error", error, "OK");
					return;
				}
				if (requestResponse == null)
				{

					UserDialogs.Instance.HideLoading();
					await App.Current.MainPage.DisplayAlert("Error", error, "OK");
					return;
				}
				else if (requestResponse.status == 200)
				{
					manager.ExtractUserDetails();
					await App.Current.MainPage.DisplayAlert("Success", "Password changed successfully", "OK");
					UserDialogs.Instance.HideLoading();
				}
				else if (requestResponse.status == 401)
				{

					UserDialogs.Instance.HideLoading();
					await App.Current.MainPage.DisplayAlert("Error", error, "OK");
				}
				else
				{

					UserDialogs.Instance.HideLoading();
					await App.Current.MainPage.DisplayAlert("Error", error, "OK");
				}
			}
		}
	}
}
