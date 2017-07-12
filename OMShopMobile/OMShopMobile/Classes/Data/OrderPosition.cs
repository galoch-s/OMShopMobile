using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OMShopMobile
{
	public class OrderPosition
	{
		[JsonProperty("products_name")]
		public string Name { get; set; }

		[JsonProperty("products_model")]
		public string Article { get; set; }

		[JsonProperty("size")]
		public string Size { get; set; }

		[JsonProperty("final_price")]
		public double Price { get; set; }

		[JsonProperty("image")]
		public string Img { get; set; }

		[JsonProperty("quantity")]
		public int Quantity { get; set; }

		public static async Task<ContentAndHeads<OrderPosition>> GetOrderPositionAsync(int orderID, int currentPage, int countItems)
		{
			string path = string.Format (Constants.PathToOrderPosition, orderID);
			string url = WebRequestUtils.GetUrl (path, currentPage, countItems);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url);

			if (contentAndHeads == null || contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				return null;

			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = new List<Product>();

			List<OrderPosition> orderPositionList = JsonConvert.DeserializeObject<List<OrderPosition>> (json);

			ContentAndHeads<OrderPosition> ContentList = new ContentAndHeads<OrderPosition> {
				countPage = contentAndHeads.countPage,
				currentPage = contentAndHeads.currentPage,
				ContentList = orderPositionList
			};
			return ContentList;
		}

		public static string GetUrl(int orderID)
		{
			string path = string.Format(Constants.PathToOrderPosition, orderID);
			return WebRequestUtils.GetUrl(path);
		}

		public static async Task<ContentAndHeads<OrderPosition>> GetOrderPositionAsync(string url, int currentPage, int countItems)
		{
			url = WebRequestUtils.GetUrlPage(url, currentPage, countItems);
			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url, isCancelable: true, isCatalog: true);

			ContentAndHeads<OrderPosition> ContentList = new ContentAndHeads<OrderPosition> {
				countPage = contentAndHeads.countPage,
				currentPage = contentAndHeads.currentPage,
			};
			if (contentAndHeads.exceptionStatus == System.Net.WebExceptionStatus.RequestCanceled)
				return ContentList;

			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = new List<Product>();
			List<OrderPosition> orderPositionList = JsonConvert.DeserializeObject<List<OrderPosition>>(json);
			ContentList.ContentList = orderPositionList;

			return ContentList;
		}
	}
}