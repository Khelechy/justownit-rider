using Acr.UserDialogs;
using justownitRiders.Managers;
using justownitRiders.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static justownitRiders.Models.Responses.DeliverableResponse;

namespace justownitRiders.ViewModels
{
	public class AccountHistoryViewModel : BaseViewModel
	{
		public AccountHistoryViewModel()
		{
			LoadDeliveries();
		}


		public async void LoadDeliveries()
		{
			Deliveries = await GetDeliverableRequest();
		}
		private ObservableCollection<RiderDeliveryRequestsResponse> deliveries;
		public ObservableCollection<RiderDeliveryRequestsResponse> Deliveries
		{
			get { return deliveries; }
			set
			{
				deliveries = value;
				OnPropertyChanged();
			}
		}

		private bool isNonFound = false;
		public bool IsNonFound
		{
			get { return isNonFound; }
			set
			{
				isNonFound = value;
				OnPropertyChanged();
			}
		}

		private bool isRefreshing;
		public bool IsRefreshing
		{
			get { return isRefreshing; }
			set
			{
				isRefreshing = value;
				OnPropertyChanged();
			}
		}



		public ICommand RefreshHistoryCommand => new Command(LoadDeliveries);

		private async Task<ObservableCollection<RiderDeliveryRequestsResponse>> GetDeliverableRequest()
		{
			IsRefreshing = true;
			ObservableCollection<RiderDeliveryRequestsResponse> myRequests = new ObservableCollection<RiderDeliveryRequestsResponse>();
			var manager = new ApiManager();
			UserDialogs.Instance.ShowLoading("Loading");
			var RiderRequestResponseTuple = await manager.GetRiderHistoryDeliverables();
			var RiderDeliveryRequestsResponse = RiderRequestResponseTuple.Item1;
			int statusCode = RiderRequestResponseTuple.Item3;
			if (statusCode == 200)
			{
				var rawRequest = JsonConvert.DeserializeObject<DeliverableResponse>(RiderDeliveryRequestsResponse);
				var RequestArray = rawRequest.data;
				if (RequestArray.Length == 0)
				{
					await App.Current.MainPage.DisplayAlert("Delivery Error", "You have made no delivery", "OK");
					IsNonFound = true;
					UserDialogs.Instance.HideLoading();

				}
				else
				{
					ObservableCollection<RiderDeliveryRequestsResponse> requestList = new ObservableCollection<RiderDeliveryRequestsResponse>(RequestArray);
					myRequests = requestList;
					UserDialogs.Instance.HideLoading();

				}
			}
			else
			{
				IsNonFound = true;
				await App.Current.MainPage.DisplayAlert("Delivery Error", "You have made no delivery", "OK");
				UserDialogs.Instance.HideLoading();
			}
			IsRefreshing = false;
			return myRequests;

		}
	}
}
