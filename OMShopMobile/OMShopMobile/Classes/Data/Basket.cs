using System;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace OMShopMobile
{
	public class Basket
	{
		[JsonProperty("customers_basket_id")]
		public int Id { get; set; }
		//[JsonProperty("products_id")]
		//public string ProductID { get; set; }
		[JsonProperty("customers_basket_quantity")]
		public int Quantity { get; set; }

		[JsonProperty("final_price")]
		public double Price { get; set; }

		[JsonProperty("productOldPrice")]
		public double PriceOld { get; set; }

		[JsonProperty("basket_date_added")]
		public DateTime DateAdded { get; set; }

		[JsonProperty("comment")]
		public string Comment { get; set; }

		[JsonProperty("true_product_id")]
		public int ProductID { get; set; }

		[JsonProperty("true_size_value_id")]
		public int? SizeValueId { get; set; }

    	[JsonProperty("productModel")]
		public string Article { get; set; }

		[JsonProperty("productImage")]
		public string ProductImage { get; set; }

		[JsonProperty("productName")]
		public string ProductName { get; set; }

		[JsonProperty("productDescriptionText")]
		public string ProductDescription { get; set; }

		[JsonProperty("sizeName")]
		public string SizeName { get; set; }

		[JsonProperty("productActualQuantity")]
		public int ProductActualQuantity { get; set; }

		[JsonProperty("productExpress")]
		public bool ProductExpress { get; set; }

		[JsonProperty("productSchedule")]
		public List<Schedule> SchedulesList { get; set; }

		[JsonProperty("productQuantityOrderMin")]
		public int ProductsQuantityOrderMin { get; set; }

		[JsonProperty("productQuantityOrderUnits")]
		public int ProductsQuantityOrderUnits { get; set; }

		[JsonProperty("productAvailable")]
		public bool ProductAvailable { get; set; }

		[JsonProperty("productSizeAvailable")]
		public bool ProductSizeAvailable { get; set; }

		[JsonIgnore]
		public bool IsSchedule { get; set; }
		[JsonIgnore]
		public bool IsLocalBasket { get; set; }
		[JsonIgnore]
		public bool IsHistoryProduct { get; set; }

		//		[JsonProperty("productQuantity")]
		//		public int ProductQuantity { get; set; }
		//
		//		[JsonProperty("productSizeQuantity")]
		//		public int ProductSizeQuantity { get; set; }
		//
		//		[JsonProperty("productActualQuantity")]
		//		public int ProductActualQuantity { get; set; }

		//[JsonProperty("attributes")]
		//public Dictionary<string, int> Attributes { get; set; }

		//[JsonIgnore]
		//public Product ProductItem { get; set; }
		////[JsonIgnore]
		////public int ProductIDInt { get; set; }
		//[JsonIgnore]
		//public int ProductSizeID { get; set; }
		//[JsonIgnore]
		//public int ProductCountInBasket { get; set; }

		//Сливает данные из локальной корзины в глобальную (корзину привязанную к пользователю)
		public static async Task<int[]> UpdateBasketAsync(List<BasketDB> basketDBList, List<Basket> basketProfileList, List<Product> productList)
		{
			List<int> deleteBasketDBList = new List<int>();
			Basket basketProfile;
			int maxCountProduct;
			foreach (BasketDB basktDB in basketDBList) {
				Basket basket = new Basket {
					ProductID = basktDB.ProductID,
					Quantity = basktDB.Quantity,
					SizeValueId = basktDB.SizeID
				};

				Product product = productList.SingleOrDefault(g => g.ProductsID == basktDB.ProductID);
				if (product == null)
					continue;

				if (product.productsAttributes.Count == 0)
					maxCountProduct = product.Quantity;
				else
					maxCountProduct = product.productsAttributes.SingleOrDefault(g => g.OptionValue.ID == basktDB.SizeID).Quantity;

				basketProfile = basketProfileList.FirstOrDefault(g => g.ProductID == basktDB.ProductID);
				if (basketProfile != null) {
					if (basket.Quantity + basketProfile.Quantity > maxCountProduct)
						basket.Quantity = maxCountProduct - basketProfile.Quantity;
				} else {
					if (basket.Quantity > maxCountProduct)
						basket.Quantity = maxCountProduct;
				}
				await PushToBasketAsync(basket);
				deleteBasketDBList.Add(basktDB.Id);
			}
			return deleteBasketDBList.ToArray();
		}

		public static async Task<ContentAndHeads> PushToBasketAsync(Basket basket)
		{
			string url = WebRequestUtils.GetUrl (Constants.PathToBasketAdd);
			string formatData;
			string postData;
			if (basket.SizeValueId == null || basket.SizeValueId == 0) {
				formatData = @"data={{
		  			""products_id"": ""{0}"",
					""customers_basket_quantity"": ""{1}"",
					""comment"": ""{2}"" }}";
				postData = string.Format(formatData, basket.ProductID, basket.Quantity, null);
			} else {
				formatData = @"data={{
		  			""products_id"": ""{0}"",
		  			""attributes"": {{ ""1"": {1} }},
					""customers_basket_quantity"": ""{2}"",
					""comment"": ""{3}"" }}";
				postData = string.Format(formatData, basket.ProductID, basket.SizeValueId, basket.Quantity, null);
			}
			//postData = "data=" + postData;

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "POST" , postData);
			if (contentAndHeads.requestStatus != HttpStatusCode.OK)
				throw new Exception ();
			
			if (User.Singleton != null) {
				Basket basketUser = User.Singleton.BasketList.SingleOrDefault (g => g.Id == basket.Id);
				if (basketUser != null)
					basketUser.Quantity += basket.Quantity;
				else
					User.Singleton.BasketList.Add (basket);
				//				OnePage.redirectApp.SetStatusBasket (PageName.Basket);
			}
			return contentAndHeads;
		}

		public static async Task<List<Basket>> GetProductInBasketAsync(bool isDeleteNoExist = false)
		{
			string expandList = ExpandList.BasketProductName + "," + ExpandList.BasketSizeName;
			string url = WebRequestUtils.GetUrl (Constants.PathToBasket, expandList, "", 1, 500);
			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url);
			List<Basket> basketList = new List<Basket> ();
			if (contentAndHeads == null)
				return basketList;
			
			basketList = JsonConvert.DeserializeObject<List<Basket>>(contentAndHeads.Content[0]);
			//**************			//basketList = await AddProductInfoAsync (basketList, isDeleteNoExist);

			if (User.Singleton != null) {
				User.Singleton.BasketList.Clear();
				User.Singleton.BasketList.AddRange(basketList);
				if (OnePage.topView != null)
					OnePage.redirectApp.SetStatusBasket (PageName.Basket);
			}

			return basketList;
		}

//		public static async Task OrderFormBasket()
//		{
//			string url = WebRequestUtils.GetUrl (Constants.PathToOrderFromBasket);
//			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, "POST");
//			if (ContentAndHeads.requestStatus == HttpStatusCode.OK) {
//				OnePage.mainPage.DisplayMessage ("Ваш заказ оформен");
//			}
//		}
		/*
		public static async Task<List<Basket>> AddProductInfoAsync(List<Basket> basketList, bool isDeleteNoExist = false)
		{
			List<int> productsID = new List<int> ();
			foreach (var item in basketList) {
				string[] ids = item.ProductID.Split (new char [] { '{', '}' });
				int id = 0;
				int.TryParse (ids [0], out id);
				item.ProductIDInt = id;
				item.ProductID = ids [0];
				int sizeId = 0;
				if (ids.Length > 2) {
					int.TryParse (ids [2], out sizeId);
					item.ProductSizeID = sizeId;
				}
				productsID.Add (item.ProductIDInt);

				if (item.Attributes == null) {
					item.Attributes = new Dictionary<string, int> ();
					if (ids.Length == 1)
						continue;
					item.Attributes.Add (ids [1], int.Parse (ids [2]));
				}
			}
			await DeleteExtraProductToBasket(basketList,isDeleteNoExist);

//			List<Product> productsList = await Product.GetProductsByIDsListAsync (productsID.ToArray<int> ());
//
//			foreach (Basket basket in basketList) {
//				Product product = productsList.SingleOrDefault (g => g.ProductsID == basket.ProductIDInt);
//				if (product == null)
//					continue;
//				if (string.IsNullOrEmpty(basket.SizeName)) {
//					basket.ProductItem = product;
//					if (basket.Quantity > product.Quantity && isDeleteNoExist) {
//						string message = string.Format ("Товара «{0}» можно заказать не более {1}", basket.ProductName, product.Quantity);
//						OnePage.mainPage.DisplayMessage (message, "Предупреждение");
//						int count = product.Quantity - basket.Quantity;
//						basket.Quantity = product.Quantity;
//
//						AddCountProduct (basket.ProductIDInt, null, count);
//					}
//				}
//				else {
//					if (product.productsAttributes != null && product.productsAttributes.Count > 0) {
//						ProductsAttributes productsAttributes = product.productsAttributes.SingleOrDefault (g => g.OptionValue.ID == basket.ProductSizeID);
//						if (productsAttributes.Quantity > 0) {
//							basket.ProductItem = product;
//							if (basket.Quantity > productsAttributes.Quantity  && isDeleteNoExist) {
//								string message = string.Format ("Товара «{0}, размера: {1}» можно заказать не более {2}", basket.ProductName, basket.SizeName, productsAttributes.Quantity);
//								OnePage.mainPage.DisplayMessage (message, "Предупреждение");
//								int count = productsAttributes.Quantity - basket.Quantity;
//								basket.Quantity = productsAttributes.Quantity;
//
//								AddCountProduct (basket.ProductIDInt, basket.Attributes, count);
//							}
//						}
//					}
//				}
//			}
//
//			List<Basket> basketNoExist = basketList.Where (g => g.ProductItem == null).ToList<Basket> ();
//			foreach (var item in basketNoExist) {
//				basketList.Remove (item);
//				if (!string.IsNullOrEmpty (item.ProductName)) {
//					if (isDeleteNoExist) {
//						string message;
//						if (!string.IsNullOrEmpty (item.SizeName))
//							message = string.Format ("Товара «{0}, размера: {1}» нету в наличие, он будет удален из корзины", item.ProductName, item.SizeName);
//						else
//							message = string.Format ("Товара «{0}» нету в наличие, он будет удален из корзины", item.ProductName);
//						
//						OnePage.mainPage.DisplayMessage (message, "Предупреждение");
//						if (User.Singleton != null)
//							DeletePositionAsync (item.Id);
//						else
//							BasketDB.DeleteItem (item.Id);
//					}
//				} else {
//					if (User.Singleton != null)
//						DeletePositionAsync (item.Id);
//					else
//						BasketDB.DeleteItem (item.Id);
//				}
//			}
			return basketList;
		}
		*/

		public static async Task<bool> DeleteExtraProductToBasket(List<Basket> basketList, bool isDeleteNoExist)
		{
			bool isDelete = false;
			/*
			List<int> productsID = basketList.Select (g => g.ProductID).ToList<int>();
			List<Product> productsList = await Product.GetProductsByIDsListAsync (productsID.ToArray<int> ());
			foreach (Basket basket in basketList) {
				Product product = productsList.SingleOrDefault(g => g.ProductsID == basket.ProductID);
				basket.ProductItem = product;
				if (product == null)
					continue;
				if (string.IsNullOrEmpty(basket.SizeName)) {
					//					basket.ProductItem = product;
					if (basket.Quantity > product.Quantity && isDeleteNoExist) {
						string message = string.Format("Товара «{0}» можно заказать не более {1}", basket.ProductName, product.Quantity);
						OnePage.mainPage.DisplayMessage(message, "Предупреждение");
						int count = product.Quantity - basket.Quantity;
						basket.Quantity = product.Quantity;


						AddCountProduct(basket.ProductID, null, count);
						isDelete = true;
					}
				} else {
					if (product.productsAttributes != null && product.productsAttributes.Count > 0) {
						ProductsAttributes productsAttributes = product.productsAttributes.SingleOrDefault(g => g.OptionValue.ID == basket.ProductSizeID);
						if (productsAttributes.Quantity > 0) {
							//							basket.ProductItem = product;
							if (basket.Quantity > productsAttributes.Quantity && isDeleteNoExist) {
								string message = string.Format("Товара «{0}, размера: {1}» можно заказать не более {2}", basket.ProductName, basket.SizeName, productsAttributes.Quantity);
								OnePage.mainPage.DisplayMessage(message, "Предупреждение");
								int count = productsAttributes.Quantity - basket.Quantity;
								basket.Quantity = productsAttributes.Quantity;


								AddCountProduct(basket.ProductID, basket.Attributes, count);
								isDelete = true;
							}
						} else
							basket.ProductItem = null;
					}
				}
			}
			*/
			/*
			List<Basket> basketNoExist = basketList.Where (g => g.ProductItem == null).ToList<Basket> ();
			foreach (var item in basketNoExist) {
				basketList.Remove (item);

				if (!string.IsNullOrEmpty (item.ProductName)) {
					
					if (isDeleteNoExist) {
						string message;
						if (!string.IsNullOrEmpty (item.SizeName))
							message = string.Format ("Товара «{0}, размера: {1}» нету в наличие, он будет удален из корзины", item.ProductName, item.SizeName);
						else
							message = string.Format ("Товара «{0}» нету в наличие, он будет удален из корзины", item.ProductName);

						OnePage.mainPage.DisplayMessage (message, "Предупреждение");
						if (User.Singleton != null)
							DeletePositionAsync (item.Id);
						else
							BasketDB.DeleteItem (item.Id);
					}

				} else {
					if (User.Singleton != null)
						DeletePositionAsync (item.Id);
					else
						BasketDB.DeleteItem (item.Id);
				}
				isDelete = true;
			}
			*/
			return isDelete;
		}

		public class ExcludeContentKeyContractResolver : DefaultContractResolver
		{
			protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
			{
				IList<JsonProperty> properties = base.CreateProperties(type,memberSerialization);
				return properties.Where(p => p.PropertyName != "customers_basket_id" && p.PropertyName != "final_price").ToList();
			}
		}

		public static async void DeletePositionAsync(int id)
		{	
			string url = WebRequestUtils.GetUrl(string.Format(Constants.PathToDeleteFromBasket, id));
			await WebRequestUtils.GetJsonAndHeadsAsync (url, "DELETE");
		}

//		public static async void AddCountProduct(int productID, Dictionary<string, int> attributes, int count)
//		{
//			if (User.Singleton != null) {
//				Basket basketToServer = new Basket {
//					ProductID = productID,
//					//Attributes = attributes,
//					Quantity = count,
//				};
//				ContentAndHeads contentAndHeads = await Basket.PushToBasketAsync (basketToServer);
//			} else {
//				BasketDB basketDB = new BasketDB { 
//					ProductID = productID,
////					Attributes = JsonConvert.SerializeObject(attributes),
//					Quantity = count
//				};
//				//if (attributes != null)
//				//	basketDB.Attributes = JsonConvert.SerializeObject (attributes);
//				BasketDB.AddCount (basketDB);
//			}
//		}
	}
}