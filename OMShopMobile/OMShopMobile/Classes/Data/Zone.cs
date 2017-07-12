using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMShopMobile
{
	public class Zone
	{
		[JsonProperty("zone_id")]
		public int Id { get; set; }

		[JsonProperty("zone_name")]
		public string Name { get; set; }

		[JsonProperty("country")]
		public Country Countries { get; set; }

		public static List<Zone> zoneList;
		public static async Task<List<Zone>> GetZoneList ()
		{
			if (zoneList == null) 
			{	
				zoneList = await Zone.GetZoneAndCountry ();
			}
			return zoneList;
		}

		public static async Task<List<Zone>> GetZoneAndCountry()
		{
			string expandList = ExpandList.ZoneCountry;
			string url = WebRequestUtils.GetUrl (Constants.PathToZone, expandList, null);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonsAndHeadsAllPageAsync (url);
			List<Zone> zoneList = new List<Zone> ();
			if (contentAndHeads.Content != null)
				foreach(string json in contentAndHeads.Content) {
					zoneList.AddRange (JsonConvert.DeserializeObject<List<Zone>> (json));
				}
			return zoneList;
		}
	}
}  