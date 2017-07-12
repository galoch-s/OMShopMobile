using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace OMShopMobile
{
	public class SizeArticle
	{
		[JsonProperty("articles_id")]
		public int Id { get; set; }

		[JsonProperty("articles_name")]
		public string Name { get; set; }

		[JsonProperty("articles_description")]
		public string Description { get; set; }

		public static async Task<List<SizeArticle>> GetSizeArticleAsync()
		{	
			string url = WebRequestUtils.GetUrl (Constants.PathToSizeArticle);
			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url);
			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
				throw new Exception ();

			string json = contentAndHeads.Content[0];
			List<SizeArticle> sizeArticleList = new List<SizeArticle>();
			sizeArticleList.AddRange (JsonConvert.DeserializeObject<List<SizeArticle>> (json));

			return sizeArticleList;
		}
	}
}

