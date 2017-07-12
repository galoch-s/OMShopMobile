using System;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class Country
	{
		[JsonProperty("countries_id")]
		public int Id { get; set; }

		[JsonProperty("countries_name")]
		public string Name { get; set; }

		public override bool Equals (object obj)
		{
			return obj.ToString () == this.ToString ();
		}

		public override int GetHashCode ()
		{
			return this.ToString ().GetHashCode ();
		}

		public override string ToString ()
		{
			return string.Format ("[Country: Id={0}, Name={1}]", Id, Name);
		}
	}
}

