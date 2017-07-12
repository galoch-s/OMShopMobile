using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMShopMobile
{
	public class Delivery
	{
		[JsonProperty("shipping_method_id")]
		public int Id { get; set; }

		[JsonProperty("name_method")]
		public string Name { get; set; }

		public static async Task<List<Delivery>> GetDeliveryList()
		{
			string url = WebRequestUtils.GetUrl (Constants.PathToListDelivery);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url);

			string json = contentAndHeads.Content[0];
			List<Delivery> deliveryList = new List<Delivery> ();
			deliveryList.AddRange(JsonConvert.DeserializeObject<List<Delivery>> (json));

			return deliveryList;
		}
	}
}