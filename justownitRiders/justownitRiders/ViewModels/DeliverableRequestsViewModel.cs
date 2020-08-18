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
	public class DeliverableRequestsViewModel : BaseViewModel
	{
		public DeliverableRequestsViewModel()
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

		private bool canDeliver = true;
		public bool CanDeliver
		{
			get { return canDeliver; }
			set
			{
				canDeliver = value;
				OnPropertyChanged();
			}
		}

		private bool canMark = false;
		public bool CanMark
		{
			get { return canMark; }
			set
			{
				canMark = value;
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

		public ICommand RefreshDeliveriesCommand => new Command(LoadDeliveries);

		public ICommand AcceptDeliveryCommand => new Command(AcceptDelivery);
		public ICommand MarkDeliveryCommand => new Command(MarkDelivery);


		private async Task<ObservableCollection<RiderDeliveryRequestsResponse>> GetDeliverableRequest()
		{
			IsRefreshing = true;
			ObservableCollection<RiderDeliveryRequestsResponse> myRequests = new ObservableCollection<RiderDeliveryRequestsResponse>();
			var manager = new ApiManager();
			UserDialogs.Instance.ShowLoading("Loading");
			var RiderRequestResponseTuple = await manager.GetRiderDeliverables();
			var RiderDeliveryRequestsResponse = RiderRequestResponseTuple.Item1;
			int statusCode = RiderRequestResponseTuple.Item3;
			if (statusCode == 200)
			{
				var rawRequest = JsonConvert.DeserializeObject<DeliverableResponse>(RiderDeliveryRequestsResponse);
				var RequestArray = rawRequest.data;
				if (RequestArray.Length == 0)
				{
					await App.Current.MainPage.DisplayAlert("Delivery Error", "There are no deliveries to be made", "OK");
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
				await App.Current.MainPage.DisplayAlert("Delivery Error", "There are no deliveries to be made", "OK");
				UserDialogs.Instance.HideLoading();
			}
			IsRefreshing = false;
			return myRequests;

		}


		public async void AcceptDelivery(object delivery)
		{
			var selectedDeliverable = (RiderDeliveryRequestsResponse)delivery;
			int request_id = selectedDeliverable.id;
			var manager = new ApiManager();
			UserDialogs.Instance.ShowLoading("Loading");
			var request = await manager.AcceptDelivery(request_id);
			var acceptResponse = request.Item1;
			var error = request.Item2;
			if (!string.IsNullOrWhiteSpace(error))
			{
				UserDialogs.Instance.HideLoading();
				await App.Current.MainPage.DisplayAlert("Request Error", error, "OK");
				return;
			}
			if (acceptResponse == null)
			{
				UserDialogs.Instance.HideLoading();
				await App.Current.MainPage.DisplayAlert("Request Error", error, "OK");
				return;
			}
			else if (acceptResponse.status == 200)
			{
				CanDeliver = false;
				CanMark = true;
				UserDialogs.Instance.HideLoading();
				await App.Current.MainPage.DisplayAlert("Successful", "Request Accepted", "OK");
			}
			else
			{
				UserDialogs.Instance.HideLoading();
				await App.Current.MainPage.DisplayAlert("Request Error", "Unable to accept request", "OK");
			}
		}

		private async void MarkDelivery(object delivery)
		{
			var selectedDeliverable = (RiderDeliveryRequestsResponse)delivery;
			int request_id = selectedDeliverable.id;
			var manager = new ApiManager();
			UserDialogs.Instance.ShowLoading("Loading");
			var request = await manager.MarkAsDelivered(request_id);
			var markResponse = request.Item1;
			var error = request.Item2;
			if (!string.IsNullOrWhiteSpace(error))
			{
				UserDialogs.Instance.HideLoading();
				await App.Current.MainPage.DisplayAlert("Mark Error", error, "OK");
				return;
			}
			if (markResponse == null)
			{
				UserDialogs.Instance.HideLoading();
				await App.Current.MainPage.DisplayAlert("Mark Error", error, "OK");
				return;
			}
			else if (markResponse.status == 200)
			{
				UserDialogs.Instance.HideLoading();
				await App.Current.MainPage.DisplayAlert("Successful", "Marked As Delivered", "OK");
			}
			else
			{
				UserDialogs.Instance.HideLoading();
				await App.Current.MainPage.DisplayAlert("Mark Error", "Unable to mark request", "OK");
			}
		}
	}
}
