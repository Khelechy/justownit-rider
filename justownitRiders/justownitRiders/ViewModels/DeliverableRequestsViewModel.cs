using Acr.UserDialogs;
using justownitRiders.Managers;
using justownitRiders.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using static justownitRiders.Models.Responses.DeliverableResponse;

namespace justownitRiders.ViewModels
{
	public class DeliverableRequestsViewModel : BaseViewModel
	{

		private async Task<ObservableCollection<RiderDeliveryRequestsResponse>> GetDeliverableRequest()
		{

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
					await App.Current.MainPage.DisplayAlert("Delivery Error", "No Deliverable Request Found", "OK");
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
				await App.Current.MainPage.DisplayAlert("Delivery Error", "No Deliverable Request Found", "OK");
				UserDialogs.Instance.HideLoading();
			}
			return myRequests;

		}


		private async void AcceptDelivery(int request_id)
		{

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
				UserDialogs.Instance.HideLoading();
				await App.Current.MainPage.DisplayAlert("Successful", "Request Accepted", "OK");
			}
			else
			{
				UserDialogs.Instance.HideLoading();
				await App.Current.MainPage.DisplayAlert("Request Error", "Unable to accept request", "OK");
			}
		}

		private async void MarkDelivery(int request_id)
		{

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
