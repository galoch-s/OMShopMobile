using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace OMShopMobile
{
	public class Order
	{
		[JsonProperty("orders_id")]
		public int Id { get; set; }

		[JsonProperty("orderBookkeepingID")]
		public string Number { get; set; }

		[JsonProperty("orderingCommentKey")]
		public string Comments { get; set; }

		[JsonProperty("orderingShippingKey")]
		public int DeliveryID { get; set; }

		[JsonProperty("customers_telephone")]
		public string Phone { get; set; }
		[JsonProperty("customers_country")]
		public string Country { get; set; }
		[JsonProperty("customers_state")]
		public string Zone { get; set; }
		[JsonProperty("customers_city")]
		public string City { get; set; }
		[JsonProperty("customers_street_address")]
		public string Street { get; set; }

		[JsonProperty("productsSum")]
		public double Sum { get; set; }

		[JsonProperty("couponRedeemTrackSum")]
		public double SumCoupon { get; set; }

		public string NumberAccount { get; set; }

//		[JsonProperty("ordersProductsWithAttributes")]
		[JsonProperty("positions")]
		public List<Product> OrdersProducts { get; set; }

		[JsonProperty("date_purchased")]
		public DateTime Date { get; set; }

		[JsonProperty("positionsCount")]
		public int CountPosition { get; set; }

		[JsonProperty("orderStatus")]
		public string StatusString { get; set; }

		[JsonProperty("shipping")]
		public ShippingForCustomer DeliveryForCustomer { get; set; }


		public double Total { get; set; }

		static string paramSort = "date_purchased";
		static string desc = "desc";


		public static async Task<List<Order>> GetHistOrdersAsync()
		{
			string expandList = ExpandList.OrderCustomer + "," + ExpandList.OrderSum;
			string url = WebRequestUtils.GetUrl (Constants.PathToOrders, expandList, null);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "GET", null);
			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
				return null;

			List<Order> Order = JsonConvert.DeserializeObject<List<Order>>(contentAndHeads.Content[0]);
			return Order;
		}

		public static async Task<ContentAndHeads> GetHistOrdersAsync(int currentPage, int countItems)
		{
			return await GetHistOrdersToIDAsync(-1, currentPage, countItems);
		}

		public static async Task<ContentAndHeads> GetHistOrdersToIDAsync(int statusID, int currentPage, int countItems)
		{
			string expandList = ExpandList.OrderCustomer + "," + ExpandList.OrderSum + "," + ExpandList.OrderPositionsCount;
			string advancedSort = string.Format(AdvancedSort.ProductToListCategoryIDAndSort, paramSort, desc);
			string url = WebRequestUtils.GetUrl(Constants.PathToOrders, expandList, null, advancedSort, currentPage, countItems);

			if (statusID != -1) {
				string data = "&statuses= " + statusID;
				url += data;
			}

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "GET", null);
			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
				return null;

			List<Order> Order = JsonConvert.DeserializeObject<List<Order>>(contentAndHeads.Content[0]);
			contentAndHeads.ContentList = Order;

			return contentAndHeads;
		}

		public static async Task<Order> OrderFormBasket(int delivery, string comment, string cuoponCode)
		{
			string expandList = ExpandList.OrderBookkeepingID + "," + ExpandList.OrderShippingForCustomer + "," + ExpandList.OrdersProductsWithAttributes;
			string url = WebRequestUtils.GetUrl (Constants.PathToOrderFromBasket, expandList, null);

			string data;
			if (string.IsNullOrEmpty(cuoponCode))
				data = string.Format(@"data={{
		  			""orderingCommentKey"": ""{0}"",
		  			""orderingShippingKey"": {1},
					""referrerURL"": ""android_app"" }}", comment, delivery);
			else
				data = string.Format(@"data={{
		  			""orderingCommentKey"": ""{0}"",
		  			""orderingShippingKey"": {1},
					""couponCode"": ""{2}"",
					""referrerURL"": ""android_app"" }}", comment, delivery, cuoponCode);
			//select * from orders where customers_referer_url = "android_app"



			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "POST", data);
			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
				return null;

			Order order = JsonConvert.DeserializeObject<Order>(contentAndHeads.Content[0]);
			order.Total = order.OrdersProducts.Sum (g => g.Price);
			order.DeliveryForCustomer.Value = order.DeliveryForCustomer.Value ?? 0;
			return order;
		}
	}
}
	