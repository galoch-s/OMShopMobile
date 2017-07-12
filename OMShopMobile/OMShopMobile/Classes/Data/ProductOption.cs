using System;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class ProductOption
	{
		[JsonProperty("products_options_id")]
		public int ID { get; set;}

		[JsonProperty("products_options_name")]
		public string Name { get; set;}
	}
}

