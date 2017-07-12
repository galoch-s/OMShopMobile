using System;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class UserAddress
	{
		[JsonProperty("entry_gender")]
		public string Gender { get; set;}

		[JsonProperty("entry_firstname")]
		public string Firstname { get; set;}

		[JsonProperty("entry_lastname")]
		public string Lastname { get; set;}


		[JsonProperty("pasport_seria")]
		public string PasportSeria { get; set;}

		[JsonProperty("pasport_nomer")]
		public string PasportNomer { get; set;}

		[JsonProperty("pasport_kem_vidan")]
		public string PasportKemVidan { get; set;}

		[JsonProperty("pasport_kogda_vidan")]
		public string PasportKogdaVidan { get; set;}


		[JsonProperty("entry_street_address")]
		public string Street { get; set; }

		[JsonProperty("entry_postcode")]
		public string PostCode { get; set; }

		[JsonProperty("entry_city")]
		public string City { get; set; }

		[JsonProperty("entry_country_id")]
		public int UserCountry { get; set; }

		[JsonProperty("entry_zone_id")]
		public int UserZone { get; set; }
	}
}

