using System;
using System.Collections.Generic;
using System.Text;

namespace justownitRiders.Utilities
{
	public class UserDetails
	{
		public static string token { get; set; }
		public static string userId { get; set; }
		public static string firstname { get; set; }
		public static string lastname { get; set; }
		public static string email { get; set; }
		public static string phoneNumber { get; set; }
		public static string locationId { get; set; }
		public static string bankId { get; set; }

		public static string location { get; set; }
		public static DateTime createdAt { get; set; }
		public static DateTime updatedAt { get; set; }
	}
}
