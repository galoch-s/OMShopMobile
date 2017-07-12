using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace OMShopMobile
{
	public class SizeCategory
	{
		[JsonProperty("products_options_values_id")]
		public int Id { get; set; }

		[JsonProperty("products_options_values_name")]
		public string Name { get; set; }

		[JsonIgnore]
		public bool IsCheck { get; set; }

		public static async Task<List<SizeCategory>> GetSizeCategoryAsync(int categoryID)
		{	
			string url = string.Format (Constants.PathToSizeCategory, categoryID);
			url = WebRequestUtils.GetUrl (url, 1, 50);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url);
			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
				throw new Exception ();

			string json = contentAndHeads.Content[0];
			List<SizeCategory> sizeArticleList = new List<SizeCategory>();
			sizeArticleList.AddRange (JsonConvert.DeserializeObject<List<SizeCategory>> (json));

			return sizeArticleList;
		}
	}
}