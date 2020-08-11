using justownitRiders.Models.Responses;
using justownitRiders.Utilities;
using justownitRiders.Utilities.Justownit.Utility;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace justownitRiders.Managers
{
	public class ApiManager
	{
		public async void ExtractUserDetails()
		{
			try
			{
				UserDetails.token = await SecureStorage.GetAsync("token");
				UserDetails.userId = await SecureStorage.GetAsync("userid");
				UserDetails.firstname = await SecureStorage.GetAsync("firstname");
				UserDetails.lastname = await SecureStorage.GetAsync("lastname");
				UserDetails.locationId = await SecureStorage.GetAsync("locationid");
				UserDetails.email = await SecureStorage.GetAsync("email");
				UserDetails.phoneNumber = await SecureStorage.GetAsync("phonenumber");
				UserDetails.location = await SecureStorage.GetAsync("location");
			}
			catch (Exception ex)
			{
				//nothing
			}
		}

		public async Task<Tuple<LoginResponse, string>> Login(string Email, string Password)
		{
			string error = "";
			try
			{
				//Compute authorization value
				var obj = new
				{
					email = Email,
					password = Password

				};

				var client = new RestClient(AppConfig.BaseURL);

				var request = new RestRequest("users/auth/login", Method.POST);

				var json = JsonConvert.SerializeObject(obj);
				request.AddParameter("application/json", json, ParameterType.RequestBody);

				//Call API
				var response = await client.ExecuteAsync<LoginResponse>(request);

				//Extract errors
				error = RestServiceManager.ExtractError(response.Content, response.StatusCode);
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					error = "Invalid Email/Password";
				}
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					//Extract and save token and Firstname
					try
					{
						await SecureStorage.SetAsync("token", response.Data.data.token);
						await SecureStorage.SetAsync("userid", response.Data.data.id.ToString());
						await SecureStorage.SetAsync("firstname", response.Data.data.firstname);
						await SecureStorage.SetAsync("lastname", response.Data.data.lastname);
						await SecureStorage.SetAsync("locationid", response.Data.data.locationId);
						await SecureStorage.SetAsync("email", response.Data.data.email);
						await SecureStorage.SetAsync("phonenumber", response.Data.data.phoneNumber);
						await SecureStorage.SetAsync("location", response.Data.data.location.name);


					}
					catch (Exception ex)
					{
						//nothing
					}


				}

				return new Tuple<LoginResponse, string>(response.Data, error);
			}
			catch (Exception ex)
			{
				error = "Login Failed!";
				return null;
			}
		}


		public async Task<Tuple<string, string, int>> GetRiderDeliverables()
		{
			string error = "";
			int statusCode = 0;
			try
			{
				var client = new RestClient(AppConfig.BaseURL);

				var request = new RestRequest("/requests/ready-for-delivery", Method.GET);
				request.AddHeader("Authorization", "Bearer " + UserDetails.token);

				//Call API
				var response = await client.ExecuteAsync(request);
				error = RestServiceManager.ExtractError(response.Content, response.StatusCode);
				statusCode = (int)response.StatusCode;

				return new Tuple<string, string, int>(response.Content, error, statusCode);
			}
			catch (Exception ex)
			{
				error = "Cant Load Data";
				return null;
			}
		}

		public async Task<Tuple<AcceptDeliveryResponse, string>> AcceptDelivery(int request_id)
		{
			string error = "";
			try
			{

				var client = new RestClient(AppConfig.BaseURL);

				var request = new RestRequest("request/" + request_id + "/accept", Method.PATCH);
				request.AddHeader("Authorization", "Bearer " + UserDetails.token);


				//Call API
				var response = await client.ExecuteAsync<AcceptDeliveryResponse>(request);

				//Extract errors
				error = RestServiceManager.ExtractError(response.Content, response.StatusCode);
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					error = "Can not accept request";
				}

				return new Tuple<AcceptDeliveryResponse, string>(response.Data, error);
			}
			catch (Exception ex)
			{
				error = "Can not accept request";
				return null;
			}
		}

		public async Task<Tuple<MarkDeliveredResponse, string>> MarkAsDelivered(int request_id)
		{
			string error = "";
			try
			{

				var client = new RestClient(AppConfig.BaseURL);

				var request = new RestRequest("request/" + request_id + "/mark-delivered", Method.PATCH);
				request.AddHeader("Authorization", "Bearer " + UserDetails.token);


				//Call API
				var response = await client.ExecuteAsync<MarkDeliveredResponse>(request);

				//Extract errors
				error = RestServiceManager.ExtractError(response.Content, response.StatusCode);
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					error = "Could Mark Delivery";
				}

				return new Tuple<MarkDeliveredResponse, string>(response.Data, error);
			}
			catch (Exception ex)
			{
				error = "Could Mark Delivery";
				return null;
			}
		}


		public async Task<Tuple<UpdateProfileResponse, string>> UpdateProfile(object requestObject)
		{
			string error = "";
			try
			{

				var client = new RestClient(AppConfig.BaseURL);

				var request = new RestRequest("users/user-profile", Method.PATCH);
				request.AddHeader("Authorization", "Bearer " + UserDetails.token);

				var json = JsonConvert.SerializeObject(requestObject);
				request.AddParameter("application/json", json, ParameterType.RequestBody);

				//Call API
				var response = await client.ExecuteAsync<UpdateProfileResponse>(request);

				//Extract errors
				error = RestServiceManager.ExtractError(response.Content, response.StatusCode);
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					error = "Can not Update Profile";
				}
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					//Extract and save token and Firstname
					try
					{
						await SecureStorage.SetAsync("token", response.Data.data.token);
						await SecureStorage.SetAsync("firstname", response.Data.data.firstname);
						await SecureStorage.SetAsync("lastname", response.Data.data.lastname);
						await SecureStorage.SetAsync("email", response.Data.data.email);
					}
					catch (Exception ex)
					{
						//nothing
					}


				}



				return new Tuple<UpdateProfileResponse, string>(response.Data, error);
			}
			catch (Exception ex)
			{
				error = "Can not Upadte Profile!";
				return null;
			}
		}


		public async Task<Tuple<ChangePasswordResponse, string>> ChangePassword(object requestObject)
		{
			string error = "";
			try
			{

				var client = new RestClient(AppConfig.BaseURL);

				var request = new RestRequest("users/auth/change-password", Method.PATCH);
				request.AddHeader("Authorization", "Bearer " + UserDetails.token);

				var json = JsonConvert.SerializeObject(requestObject);
				request.AddParameter("application/json", json, ParameterType.RequestBody);

				//Call API
				var response = await client.ExecuteAsync<ChangePasswordResponse>(request);

				//Extract errors
				error = RestServiceManager.ExtractError(response.Content, response.StatusCode);
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					error = "Cannot Change Password";
				}
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					//Extract and save token and Firstname
					try
					{
						await SecureStorage.SetAsync("token", response.Data.data.token);
					}
					catch (Exception ex)
					{
						//nothing
					}


				}

				return new Tuple<ChangePasswordResponse, string>(response.Data, error);
			}
			catch (Exception ex)
			{
				error = "Cannot Change Password";
				return null;
			}
		}
	}
}
