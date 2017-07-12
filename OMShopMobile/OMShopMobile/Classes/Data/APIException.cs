using System;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class APIException
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("code")]
		public int Code { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}
}

