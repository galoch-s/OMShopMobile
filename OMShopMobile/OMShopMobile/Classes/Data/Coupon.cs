using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class Coupon
	{
		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("valid")]
		public bool Valid { get; set; }

		[JsonProperty("orderSum")]
		public int OrderSum { get; set; }

		[JsonProperty("couponRedeemSum")]
		public double CouponRedeemSum { get; set; }

		[JsonProperty("orderSumTotal")]
		public int OrderSumTotal { get; set; }

		public static async Task<ContentAndHeads<Coupon>> GetProductsByIDAsync(string textCoupon)
		{
			string url = string.Format(Constants.PathToCoupon, textCoupon);
			url = WebRequestUtils.GetUrl(url);
			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url);

			ContentAndHeads<Coupon> result = new ContentAndHeads<Coupon>();
			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
				result.MessageError = contentAndHeads.serverException.Message;
			else {
				string json = contentAndHeads.Content[0];
				result.ContentList = new List<Coupon>();
				result.ContentList.Add(JsonConvert.DeserializeObject<Coupon>(json));
			}
			return result;
		}
	}
}
