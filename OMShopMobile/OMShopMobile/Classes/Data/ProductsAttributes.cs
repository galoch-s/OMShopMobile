using System;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class ProductsAttributes
	{
		[JsonProperty("products_attributes_id")]
		public int ID { get; set;}

		[JsonProperty("quantity")]
		public int Quantity { get; set; }

		[JsonProperty("options")]
		public ProductOption Option { get; set;}

		[JsonProperty("optionsValues")]
		public ProductOptionValue OptionValue { get; set;}
	}
}

