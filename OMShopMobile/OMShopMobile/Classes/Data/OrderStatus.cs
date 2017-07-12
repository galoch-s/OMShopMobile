using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;

namespace OMShopMobile
{
	public class OrderStatus
	{
		[JsonProperty("orders_status_id")]
		public int Id { get; set; }

		[JsonProperty("orders_status_name")]
		public string Name { get; set; }

		[JsonIgnore]
		public int Count { get; set;}

		[JsonIgnore]
		public string Icon { get; set; }

		[JsonIgnore]
		public int Index { get; set; }

		public static async Task<List<OrderStatus>> GetOrderStatusListAsync()
		{
			string url = WebRequestUtils.GetUrl (Constants.PathToOrderStatus);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url);
			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
				return null;

			List<OrderStatus> orderStatusList = JsonConvert.DeserializeObject<List<OrderStatus>>(contentAndHeads.Content[0]);

			foreach (OrderStatus orderStatus in orderStatusList) {
				orderStatus.Count = await GetCountProductByStatusAsync (orderStatus.Id);
			}
			return orderStatusList;
		}

		public static async Task<int> GetCountProductByStatusAsync(int id)
		{
			string path = string.Format (Constants.PathToOrderCountStatus, id);
			string url = WebRequestUtils.GetUrl (path);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url);
			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
				return 0;
			
			int result;
			int.TryParse(contentAndHeads.Content [0], out result);

			return result;
		}
	}
}