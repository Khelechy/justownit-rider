using System;
using System.Collections.Generic;
using System.Text;

namespace justownitRiders.Models.Responses
{
	public class ChangePasswordResponse
	{

		public int status { get; set; }
		public Data data { get; set; }

		public class Data
		{
			public int id { get; set; }
			public string token { get; set; }
			public string firstname { get; set; }
			public string lastname { get; set; }
			public string email { get; set; }
			public bool isAdmin { get; set; }
			public string isActive { get; set; }
		}

	}
}
