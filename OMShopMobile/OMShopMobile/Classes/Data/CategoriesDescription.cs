using System;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class CategoriesDescription
	{
		[JsonProperty("categories_id")]
		public int ID { get; set;}

		[JsonProperty("categories_name")]
		public string Name { get; set;}

		[JsonProperty("categories_description")]
		public string Description { get; set;}

		public CategoriesDescription ()
		{
		}
	}
}

