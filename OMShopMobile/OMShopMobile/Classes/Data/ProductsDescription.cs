using System;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class ProductsDescription
	{
		[JsonProperty("products_id")]
		public int ProductsID { get; set;}

		[JsonProperty("products_name")]
		public string Name { get; set;}

		[JsonProperty("products_description")]
		public string Description { get; set;}

		public ProductsDescription ()
		{
		}
	}
}

