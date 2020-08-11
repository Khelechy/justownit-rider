using System;
using System.Collections.Generic;
using System.Text;

namespace justownitRiders.Utilities
{
	using Newtonsoft.Json.Linq;
	using System;
	using System.Collections.Generic;
	using System.Text;

	namespace Justownit.Utility
	{
		public static class RestServiceManager
		{
			public static string ExtractError(string content, System.Net.HttpStatusCode statusCode)
			{
				string errorMessage = "";

				if (statusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					errorMessage = "You are unauthorized";
					return errorMessage;
				}

				if (statusCode == 0)
				{
					errorMessage = "Unable to reach server.";
					return errorMessage;
				}

				if (!string.IsNullOrWhiteSpace(content))
				{
					try
					{
						var data = JObject.Parse(content);
						if (data != null)
						{
							errorMessage = (string)data["message"];
							if (string.IsNullOrWhiteSpace(errorMessage))
							{
								errorMessage = (string)data["Message"];
								{
									if (!string.IsNullOrWhiteSpace(errorMessage))
									{
										var innerData = JObject.Parse(errorMessage);
										if (innerData != null)
										{
											errorMessage = (string)innerData["message"];
											if (string.IsNullOrWhiteSpace(errorMessage))
											{
												errorMessage = (string)innerData["Message"];
											}
										}
									}
								}
							}
							string modelState = (string)data["errors"];
							if (!string.IsNullOrWhiteSpace(modelState))
							{
								errorMessage = string.Join("|", errorMessage, modelState);
							}
							if (!string.IsNullOrWhiteSpace(errorMessage))
							{
								if (errorMessage.StartsWith("|"))
								{
									errorMessage = errorMessage.Substring(1);
								}
							}
						}
					}
					catch (Exception ex)
					{

					}
				}

				return errorMessage;

			}
		}
	}

}
