//using System;
//using Newtonsoft.Json;
//using System.Threading.Tasks;
//using System.Net;
//using System.Collections.Generic;
//
//namespace OMShopMobile
//{
//	public class Order
//	{
//		[JsonProperty("orders_id")]
//		public int Id { get; set; }
//
//		[JsonProperty("orderBookkeepingID")]
//		public string Number { get; set; }
//
//		[JsonProperty("date_purchased")]
//		public DateTime Date { get; set; }
//
//		[JsonProperty("productsSum")]
//		public double Sum { get; set; }
//
//		public string NumberAccount { get; set; }
//
//		[JsonProperty("count")]
//		public int Count { get; set; }
//
////		[JsonProperty("orders_status")]
////		public StatusOrder Status { get; set; }
//
//		[JsonProperty("orderStatus")]
//		public string StatusString { get; set; }
//
//
//		[JsonProperty("customers_telephone")]
//		public string Phone { get; set; }
//		[JsonProperty("customers_country")]
//		public string Country { get; set; }
//		[JsonProperty("customers_state")]
//		public string Zone { get; set; }
//		[JsonProperty("customers_city")]
//		public string City { get; set; }
//		[JsonProperty("customers_street_address")]
//		public string Street { get; set; }
//
//
//		public static async Task<List<Order>> GetHistOrdersAsync()
//		{
//			string expandList = ExpandList.OrderCustomer + "," + ExpandList.OrderSum;
//			string url = WebRequestUtils.GetUrl (Constants.PathToOrders, expandList, null);
//
//			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "GET", null);
//			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
//				return null;
//
//			List<Order> Order = JsonConvert.DeserializeObject<List<Order>>(contentAndHeads.content[0]);
//			return Order;
//		}
//
//		public static async Task<ContentAndHeads> GetHistOrdersAsync(int currentPage, int countItems)
//		{
//			string expandList = ExpandList.OrderCustomer + "," + ExpandList.OrderSum;
//			string url = WebRequestUtils.GetUrl (Constants.PathToOrders, expandList, null, currentPage, countItems);
//
//			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "GET", null);
//			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
//				return null;
//
//			List<Order> Order = JsonConvert.DeserializeObject<List<Order>>(contentAndHeads.content[0]);
//			contentAndHeads.entityList = Order;
//
//			return contentAndHeads;
//		}
//
//		public static async Task<List<Order>> GetHistOrdersToIDAsync(int statusID)
//		{
//			string expandList = ExpandList.OrderCustomer + "," + ExpandList.OrderSum;
//			string url = WebRequestUtils.GetUrl (Constants.PathToOrders, expandList, null);
//			string data = "statuses= " + statusID;
//
//			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "GET", null);
//			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
//				return null;
//
//			List<Order> Order = JsonConvert.DeserializeObject<List<Order>>(contentAndHeads.content[0]);
//			return Order;
//		}
//	}
//}
//
