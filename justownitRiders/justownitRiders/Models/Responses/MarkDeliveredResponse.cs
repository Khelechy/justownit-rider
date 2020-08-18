using System;
using System.Collections.Generic;
using System.Text;

namespace justownitRiders.Models.Responses
{
	public class MarkDeliveredResponse
	{
		public int status { get; set; }
		public Data data { get; set; }

		public class Data
		{
			public int id { get; set; }
			public int requesterId { get; set; }
			public string deviceId { get; set; }
			public string locationId { get; set; }
			public int bankId { get; set; }
			public string TDSalesId { get; set; }
			public float amount { get; set; }
			public string status { get; set; }
			public string dispatchType { get; set; }
			public string dispatchStatus { get; set; }
			public int riderId { get; set; }
			public DateTime createdAt { get; set; }
			public DateTime updatedAt { get; set; }
		}

	}
}
