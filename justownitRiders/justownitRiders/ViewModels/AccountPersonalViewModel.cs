using Acr.UserDialogs;
using justownitRiders.Managers;
using justownitRiders.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace justownitRiders.ViewModels
{
	public class AccountPersonalViewModel : BaseViewModel
	{

		public AccountPersonalViewModel()
		{
			LoadPersonalDetails();
		}
		public string firstName;
		public string FirstName
		{
			get { return firstName; }
			set
			{
				firstName = value;
				OnPropertyChanged();
			}
		}

		public string lastName;
		public string LastName
		{
			get { return lastName; }
			set
			{
				lastName = value;
				OnPropertyChanged();
			}
		}

		public string email;
		public string Email
		{
			get { return email; }
			set
			{
				email = value;
				OnPropertyChanged();
			}
		}

		public string phoneNumber;
		public string PhoneNumber
		{
			get { return phoneNumber; }
			set
			{
				phoneNumber = value;
				OnPropertyChanged();
			}
		}


		public void LoadPersonalDetails()
		{
			FirstName = UserDetails.firstname;
			LastName = UserDetails.lastname;
			PhoneNumber = UserDetails.phoneNumber;
			Email = UserDetails.email;
		}


		public ICommand UpdateCommand => new Command(UpdateProfile);

		public async void UpdateProfile()
		{

			if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(PhoneNumber))
			{
				await App.Current.MainPage.DisplayAlert("Update Error", "A field must be entered", "OK");
			}
			else
			{
				var manager = new ApiManager();
				var UpdateProfileDetailsObj = new
				{
					firstname = FirstName,
					lastname = LastName,
					phoneNumber = PhoneNumber
				};
				UserDialogs.Instance.ShowLoading("Loading");
				var requestResponseTuple = await manager.UpdateProfile(UpdateProfileDetailsObj);
				var requestResponse = requestResponseTuple.Item1;
				string error = requestResponseTuple.Item2;
				if (requestResponse == null)
				{

					UserDialogs.Instance.HideLoading();
					await App.Current.MainPage.DisplayAlert("Error", error, "OK");
					return;
				}
				else if (requestResponse.status == 200)
				{
					UserDialogs.Instance.HideLoading();
					await App.Current.MainPage.DisplayAlert("Success", "Profile Updated", "OK");
				}
			}
		}

	}
}
