using System;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class ShippingForCustomer
	{
		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("value")]
		public double? Value { get; set; }
	}
}

