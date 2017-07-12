using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class Banner
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("url")]
		public BannerUrl Url { get; set; }

		[JsonProperty("image")]
		public BannerImage Image { get; set; }

		public static string _sizeCoefficient;

		public static string SizeCoefficient {
			get {
				if (_sizeCoefficient == null) _sizeCoefficient = GetSizeCoefficient();
				return _sizeCoefficient;
			}
		}

		static string GetSizeCoefficient()
		{
			switch (string.Format("{0:0.##}", App.Density)) {
				case "0.75":
					return "ldpi";
				case "1":
					return "mdpi";
				case "1.5":
					return "hdpi";
				case "2":
					return "xhdpi";
				case "3":
					return "xxhdpi";
				case "4":
					return "xxxhdpi";
				default: return "hdpi";
			}
		}

		public static async Task<List<Banner>> GetProductsByIDAsync()
		{
			string url = BannerConstant.Url + string.Format(BannerConstant.BanersToCoeffCList, BannerConstant.BannerGroup, SizeCoefficient);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url, isCancelable: true, isBanner: true);
			if (contentAndHeads.Content == null || contentAndHeads.Content.Count == 0)
				return null;
			string json = contentAndHeads.Content[0];

 			List<Banner> entity = JsonConvert.DeserializeObject<List<Banner>>(json);
			if (entity == null || entity.Count == 0)
				return null;
			return entity;
		}
	}
}