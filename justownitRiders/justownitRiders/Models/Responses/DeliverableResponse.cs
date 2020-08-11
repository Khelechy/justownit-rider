using System;
using System.Collections.Generic;
using System.Text;

namespace justownitRiders.Models.Responses
{
	public class DeliverableResponse
	{
		public int status { get; set; }
		public RiderDeliveryRequestsResponse[] data { get; set; }

		public class RiderDeliveryRequestsResponse
		{
			public int id { get; set; }
			public int deviceId { get; set; }
			public int locationId { get; set; }
			public int bankId { get; set; }
			public int requesterId { get; set; }
			public string status { get; set; }
			public float amount { get; set; }
			public string dispatchType { get; set; }
			public string dispatchStatus { get; set; }
			public int riderId { get; set; }
		}

	}
}
