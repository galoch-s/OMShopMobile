using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SQLite;
using System.IO;
using Xamarin.Forms;
using System.Linq;

namespace OMShopMobile
{
	public class User
	{
		[JsonProperty("customers_id")]
		public int Id { get; set; }
		[JsonProperty("customers_firstname")]
		public string Firstname { get; set;}
		[JsonProperty("customers_lastname")]
		public string Lastname { get; set;}
		[JsonProperty("customers_email_address")]
		public string Email { get; set;}
		[JsonProperty("customers_telephone")]
		public string Phone { get; set;}
		[JsonProperty("customers_password", NullValueHandling=NullValueHandling.Ignore)]
		public string Password { get; set;}
		[JsonProperty("defaultAddress")]
		public UserAddress Address { get; set;}

		public List<OrderStatus> OrderStatusList { get; set; }

		[JsonIgnore]
		public string HashKey { get; set;}
		[JsonIgnore]
		public bool IsAuth 
		{ 
			get 
			{
				if ((Id != 0 && Email != null && Password != null) || (Email != null && HashKey != null))
					return true;
				else
					return false;
			}
		}
		List<Basket> basketList;
		[JsonIgnore]
		public List<Basket> BasketList { 
			get 
			{  
				if (basketList == null)
					basketList = new List<Basket> ();
				return basketList;
			}
			set { basketList = value; }
		}

		public static User user;

		public static async Task AuthorizationUserToKeyAsync(string email, string hash)
		{
			Singleton = new User();
			Singleton.Email = email;
			Singleton.HashKey = hash;

			Singleton = await User.GetPersonalData();
			Singleton.HashKey = hash;
			await User.LoadBasket();
		}

		public static async Task AuthorizationUserAsync()
		{	
			Session session = Session.GetSession ();
			if (session != null && session.Email != null) {
				bool isLogin = await User.LoginAsync (session.Email, session.Password);
				if (!isLogin) {
					User.LoginExit ();
				}
			}
		}

		public static User Singleton
		{
			get	{ return user; }
			set { user = value; }
		}

		public static void LoginExit()
		{
			User user = new User ();
			user.Id = 1;
			Session.SaveUser (user);
			User.Singleton = null;


//			if (OnePage.topView != null)
//				OnePage.redirectApp.SetStatusBasket (OnePage.topView.btnBasket);
		}

		public static async Task<bool> LoginAsync(string email, string password)
		{
//			email = "8888888888@mail.ru";
//			password = "8888888888";
			Dictionary<string, string> data = new Dictionary<string, string> ();
			data.Add ("email", email);
			data.Add ("password", password);
			string postData = "";
			if (data != null) {
				foreach (string key in data.Keys) {
					postData += System.Net.WebUtility.UrlEncode (key) + "="
						+ System.Net.WebUtility.UrlEncode (data [key]) + "&";
				}
			}
			string url = WebRequestUtils.GetUrl (Constants.PathToLogin);
			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "POST" , postData);
			if (contentAndHeads == null || contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				return false;
			
			JObject results = JObject.Parse(contentAndHeads.Content[0]);
			if (user == null) user = new User ();
			user.Id = (int)results["id"];
			user.Email = email;
			user.Password = password;
			user.HashKey = (string)results["key"];

			User userAddress = await User.GetPersonalData();
			user.Address = userAddress.Address;
			await User.LoadBasket();
			return true;
		}

		public static async Task LoadBasket()
		{
			try {
				List<BasketDB> basketDBList = BasketDB.GetItems();
				List<Basket> basketProfileList = await Basket.GetProductInBasketAsync();
				int[] produtIDs = basketDBList.Select(g => g.ProductID).Distinct().ToArray();
				if (produtIDs.Length == 0) return;
				List<Product> productList = await Product.GetProductsByIDsListAsync(produtIDs);
				int[] deleteBasketDBList = await Basket.UpdateBasketAsync(basketDBList, basketProfileList, productList);
				BasketDB.DeleteItems(deleteBasketDBList);
			} catch { 
			}
 		}

		public static async Task<bool> Registration(string login, string password)
		{
			Dictionary<string, string> data = new Dictionary<string, string> ();
			data.Add ("customers_email_address", login);
			data.Add ("customers_password", password);


			string json = JsonConvert.SerializeObject (data);
			string postData = "data=" + json;

			string url = WebRequestUtils.GetUrl (Constants.PathToregistration);
			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "POST" , postData);

			if (contentAndHeads == null || contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				throw new Exception ();
//				return false;

			//await Task.Delay(1000).ConfigureAwait(true);
			return true;
		}

		public static async Task<ContentAndHeads> Registration(User user)
		{
			string json = JsonConvert.SerializeObject (user);
			string postData = "data=" + json;

			string url = WebRequestUtils.GetUrl (Constants.PathToregistration);
			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "POST" , postData);

			//if (contentAndHeads == null || contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
			//	throw new Exception ();

			//await Task.Delay(1000).ConfigureAwait(true);
			return contentAndHeads;
		}

		public static async Task<User> GetPersonalData()
		{
			string url = WebRequestUtils.GetUrl (Constants.PathToPersonalData);
			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "GET" , null);

			if (contentAndHeads == null || contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				throw new Exception ();
//				return null;
			
			string json = contentAndHeads.Content[0];
			User user = JsonConvert.DeserializeObject<User> (json);
			return user;
		}

		public static async Task<bool> SavePersonalData(User user)
		{
			string url = WebRequestUtils.GetUrl (Constants.PathToPersonalData);

			string json = JsonConvert.SerializeObject (user, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
			//json = json.Replace ("\"defaultAddress\":", "\"address\":");
			string postData = "data=" + json;

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "PUT", postData);

			if (contentAndHeads == null || contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				throw new Exception ();
//				return false;

			return true;
		}

		public static async Task<ContentAndHeads> ReseltPasswordAsync(string email)
		{
			string postData = System.Net.WebUtility.UrlEncode("eMail") + "=" + System.Net.WebUtility.UrlEncode(email) + "&";
			string url = WebRequestUtils.GetUrl (Constants.PathToPasswordReset);
			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "POST" , postData);

//			if (ContentAndHeads == null || ContentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
//				return null;
//				throw new Exception ();

			return contentAndHeads;
//			if (ContentAndHeads == null || ContentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
//				return false;
//			
//			return true;
		}
	}
}