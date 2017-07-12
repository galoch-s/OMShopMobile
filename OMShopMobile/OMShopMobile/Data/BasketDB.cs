using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMShopMobile
{
	public class BasketDB
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public int ProductID { get; set; }
		public string Article { get; set; }
		public string ProductName { get; set; }
		public string SizeName { get; set; }
		public string Image { get; set; }
		public int SizeID { get; set; }
		public double Price { get; set; }
		public double PriceOld { get; set; }
		public int Quantity { get; set; }
		public bool ProductExpress { get; set; }
		public bool IsSchedule { get; set; }

		public static void AddCount(BasketDB entity)
		{
			List<BasketDB> basketDBList = GetItems();
			BasketDB basketDB = basketDBList.SingleOrDefault(g => g.ProductID == entity.ProductID && g.SizeID == entity.SizeID);
			if (basketDB != null) {
				basketDB.Quantity += entity.Quantity;
				Update(basketDB);
			} else
				Add(entity);
		}

		public static int Add(BasketDB Content)
		{
			lock (SqlConnect.locker) {
				if (BasketDB.GetItemByID(Content.Id) != null) {
					SqlConnect.database.Update(Content);
					return Content.Id;
				} else {
					return SqlConnect.database.Insert(Content);
				}
			}
		}

		public static int Update(BasketDB Content)
		{
			return SqlConnect.database.Update(Content);
		}

		public static void Update(BasketDB entity, int productID, int SizeID)
		{
			BasketDB basket = SqlConnect.database.Table<BasketDB>().FirstOrDefault(g => g.ProductID == productID && g.SizeID == SizeID);
			if (basket == null) return;

			basket.SizeID = entity.SizeID;
			basket.SizeName = entity.SizeName;
			basket.Quantity = entity.Quantity;
			Update(basket);
		}

		public static BasketDB GetItem(int productID)
		{
			lock (SqlConnect.locker) {
				return SqlConnect.database.Table<BasketDB>().FirstOrDefault(x => x.ProductID == productID);
			}
		}

		public static BasketDB GetItem(int productID, int sizeID)
		{
			lock (SqlConnect.locker) {
				return SqlConnect.database.Table<BasketDB>().FirstOrDefault(x => x.ProductID == productID && x.SizeID == sizeID);
			}
		}

		public static BasketDB GetItemByID(int id)
		{
			lock (SqlConnect.locker) {
				return SqlConnect.database.Table<BasketDB>().FirstOrDefault(x => x.Id == id);
			}
		}

		public static List<BasketDB> GetItems()
		{
			lock (SqlConnect.locker) {
				return (from i in SqlConnect.database.Table<BasketDB>() select i).ToList();
			}
		}

		public static List<BasketDB> GetItems(int currentPage, int countPage)
		{
			lock (SqlConnect.locker) {
				return (from i in SqlConnect.database.Table<BasketDB>() select i).Skip((currentPage - 1) * countPage).Take(countPage).ToList();
			}
		}

		public static int DeleteItem(int id)
		{
			lock (SqlConnect.locker) {
				return SqlConnect.database.Delete<BasketDB>(id);
			}
		}

		public static void DeleteItems(int[] idsList)
		{
			lock (SqlConnect.locker) {
				foreach (int id in idsList) {
					DeleteItem(id);
				}
			}
		}

		public static void Clear()
		{
			lock (SqlConnect.locker) {
				SqlConnect.database.DeleteAll<BasketDB>();
			}
		}

		public static List<Basket> GetItemsToBasketList()
		{
			List<Basket> basketList = new List<Basket>();
			List<BasketDB> basketDBList = BasketDB.GetItems();
			foreach (BasketDB entity in basketDBList) {
				Basket basket = new Basket {
					ProductID = entity.ProductID,
					Article = entity.Article,
					ProductName = entity.ProductName,
					SizeName = entity.SizeName,
					ProductImage = entity.Image,
					SizeValueId = entity.SizeID,
					Price = entity.Price,
					Quantity = entity.Quantity,
					ProductExpress = entity.ProductExpress,
					IsSchedule = entity.IsSchedule,
					IsLocalBasket = true
				};
				basketList.Add(basket);
			}
			return basketList;
		}

		public static async Task UpdateInfoBasketList()
		{
			List<BasketDB> basketDBList = GetItems();
			if (basketDBList == null) return;

			int[] idsProduct = basketDBList.Select(g => g.ProductID).ToArray();
			if (idsProduct.Length == 0) return;
			List<Product> productList = null;
			try {
				productList = await Product.GetProductsByIDsListAsync(idsProduct);
			} catch { }

			if (productList == null) return;

			Product product;
			foreach (BasketDB basketDB in basketDBList) {
				product = productList.SingleOrDefault(g => g.ProductsID == basketDB.ProductID);
				if (product != null) {
					basketDB.ProductID = product.ProductsID;
					basketDB.Article = product.Article;
					basketDB.ProductName = product.productsDescription?.Name;
					basketDB.Image = product.Image;
					basketDB.Price = product.Price;
					basketDB.ProductExpress = product.Express;
					basketDB.IsSchedule = product.SchedulesList?.Count > 0;

					//if (product.productsAttributes?.Count > 0) {
					//	ProductsAttributes productsAttribute = product.productsAttributes.FirstOrDefault(g => g.OptionValue.ID == basketDB.SizeID);
					//	if (productsAttribute != null)
					//		basketDB.SizeName = productsAttribute.OptionValue.Value;
					//}
					Update(basketDB);
				}
			}
		}
	}
}