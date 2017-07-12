using System;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class ProductOptionValue
	{
		[JsonProperty("products_options_values_id")]
		public int ID { get; set;}

		[JsonProperty("products_options_values_name")]
		public string Value { get; set;}

	}
}

