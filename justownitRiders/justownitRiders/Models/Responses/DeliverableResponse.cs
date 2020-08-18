using System;
using System.Collections.Generic;
using System.Text;

namespace justownitRiders.Models.Responses
{
	public class DeliverableResponse
	{
		//public int status { get; set; }
		//public RiderDeliveryRequestsResponse[] data { get; set; }

		//public class RiderDeliveryRequestsResponse
		//{
		//	public int id { get; set; }
		//	public int deviceId { get; set; }
		//	public int locationId { get; set; }
		//	public int bankId { get; set; }
		//	public int requesterId { get; set; }
		//	public string status { get; set; }
		//	public float amount { get; set; }
		//	public string dispatchType { get; set; }
		//	public string dispatchStatus { get; set; }
		//	public int riderId { get; set; }
		//}

		public int status { get; set; }
		public RiderDeliveryRequestsResponse[] data { get; set; }

		public class RiderDeliveryRequestsResponse
		{
			public int id { get; set; }
			public string status { get; set; }
			public string dispatchType { get; set; }
			public string dispatchStatus { get; set; }
			public float amount { get; set; }
			public string TDSalesId { get; set; }
			public string deviceId { get; set; }
			public string locationId { get; set; }
			public DateTime createdAt { get; set; }
			public Requester requester { get; set; }
			public Device device { get; set; }
		}

		public class Requester
		{
			public string firstname { get; set; }
			public string lastname { get; set; }
			public string email { get; set; }
			public string phoneNumber { get; set; }
		}

		public class Device
		{
			public string id { get; set; }
			public string name { get; set; }
			public string description { get; set; }
			public string image { get; set; }
			public int price { get; set; }
			public int quantity { get; set; }
			public Location location { get; set; }

		}

		public class Location
		{
			public string id { get; set; }
			public string name { get; set; }
		}


	}
}
