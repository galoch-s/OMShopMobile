using System;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class BannerImage
	{
		[JsonProperty("path")]
		public string Path { get; set; }

		[JsonProperty("width")]
		public string Width { get; set; }

		[JsonProperty("height")]
		public string Height { get; set; }
	}
}
