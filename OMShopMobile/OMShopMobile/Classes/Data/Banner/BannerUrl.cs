using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace OMShopMobile
{
	public class BannerUrl
	{
		[JsonProperty("mask")]
		public string Mask { get; set; }

		[JsonProperty("link")]
		public string Link { get; set; }

		[JsonProperty("maskKeys")]
		public Dictionary<string, string> MaskKey { get; set; }
	}
}
