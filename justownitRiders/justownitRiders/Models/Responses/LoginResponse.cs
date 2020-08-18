using System;
using System.Collections.Generic;
using System.Text;

namespace justownitRiders.Models.Responses
{
	public class LoginResponse
	{

		public int status { get; set; }
		public Data data { get; set; }

		public class Data
		{
			public string token { get; set; }
			public int id { get; set; }
			public string firstname { get; set; }
			public string lastname { get; set; }
			public string email { get; set; }
			public string phoneNumber { get; set; }
			public string locationId { get; set; }
			//public Role role { get; set; }
			public Location location { set; get; }

		}

		public class Location
		{
			public string name { set; get; }
		}

		//public class Role
		//{
		//	public string name { get; set; }
		//	public string[] claims { get; set; }
		//}


	}
}
