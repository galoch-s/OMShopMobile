using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Text;

namespace OMShopMobile
{
	public class Product
	{
		[JsonProperty("products_id")]
		public int ProductsID { get; set;}

		[JsonProperty("products_image")]
		public string Image { get; set;}

		[JsonProperty("products_quantity")]
		public int Quantity { get; set; }

		[JsonProperty("products_model")]
		public string Article { get; set;}

		[JsonProperty("products_price")]
		public double Price { get; set;}

		[JsonProperty("products_old_price")]
		public double PriceOld { get; set; }

		[JsonProperty("products_quantity_order_min")]
		public int ProductsQuantityOrderMin { get; set; }

		[JsonProperty("products_quantity_order_units")]
		public int ProductsQuantityOrderUnits { get; set; }

		[JsonProperty("productsDescription")]
		public ProductsDescription productsDescription { get; set;}

		[JsonProperty("productsAttributesFullInfo")]
		public List<ProductsAttributes> productsAttributes { get; set;}

		[JsonProperty("schedule")]
		public List<Schedule> SchedulesList { get; set; }

		[JsonProperty("express")]
		public bool Express { get; set; }


		//public static ContentAndHeads GetProductsByCategoryID(int[] catogoriesIDList, int currentPage, int countItems)
		//{
		//	string strCategoryID = string.Join (",", catogoriesIDList);
		//	string advancedFilter = string.Format(AdvancedFiltersList.ProductToListCategoryID, strCategoryID);
		//	string expandList = ExpandList.ProductsDescription;
		//	string url = WebRequestUtils.GetUrl (Constants.UrlProducts, expandList, advancedFilter, currentPage, countItems);

		//	ContentAndHeads contentAndHeads = WebRequestUtils.GetJsonAndHeads (url);

		//	string json = ContentAndHeads.Content[0];
		//	ContentAndHeads.productsList = new List<Product>();
		//	ContentAndHeads.productsList.AddRange (JsonConvert.DeserializeObject<List<Product>> (json));

		//	return ContentAndHeads;
		//}

		public static string GetUrlByCategoryIDAsync(int[] catogoriesIDList)
		{
			string strCategoryID = string.Join(",", catogoriesIDList);
			string advancedFilter = string.Format(AdvancedFiltersList.ProductToListCategoryID, strCategoryID);
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			return WebRequestUtils.GetUrl(Constants.UrlProducts, expandList, advancedFilter);
		}

		public static async Task<ContentAndHeads> GetProductsByCategoryIDAsync(int[] catogoriesIDList, int currentPage, int countItems)
		{
			string strCategoryID = string.Join (",", catogoriesIDList);
			string advancedFilter = string.Format(AdvancedFiltersList.ProductToListCategoryID, strCategoryID);
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			string url = WebRequestUtils.GetUrl (Constants.UrlProducts, expandList, advancedFilter, currentPage, countItems);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url);
			if (contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				return contentAndHeads;
			
			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = new List<Product>();
			contentAndHeads.productsList.AddRange (JsonConvert.DeserializeObject<List<Product>> (json));

			return contentAndHeads;
		}

		public static async Task<ContentAndHeads> GetProductsByCategoryIDAsync (int[] catogoriesIDList, int currentPage, int countItems, FilterParam paramForCheck)
		{
			if (paramForCheck == null)
			{
				return await Product.GetProductsByCategoryIDAsync(catogoriesIDList, currentPage, countItems);
			}
			string strCategoryID = string.Join (",", catogoriesIDList);
			string formatAdvancedFilter = string.Format(AdvancedFiltersList.ProductToListCategoryIDAndSortToProductDescrioption);
			formatAdvancedFilter += "," + AdvancedFiltersList.ProductToListFilterProductsQuantity;
			formatAdvancedFilter += "," + string.Format (AdvancedFiltersList.ProductToListFilterCategories, strCategoryID);

			if (paramForCheck.Sizes != null)
			{
				string strSizes = string.Join (",", paramForCheck.Sizes);
				formatAdvancedFilter += "," + string.Format (AdvancedFiltersList.ProductToListFilterSizes, strSizes);
			}
			if (paramForCheck.PriceBegin != 0)
				formatAdvancedFilter += "," + string.Format (AdvancedFiltersList.ProductToListFilterPriceBegin, paramForCheck.PriceBegin);
			if (paramForCheck.PriceEnd != 0)
				formatAdvancedFilter += "," + string.Format (AdvancedFiltersList.ProductToListFilterPriceEnd, paramForCheck.PriceEnd);

			formatAdvancedFilter = "[" + formatAdvancedFilter + "]&distinct=1";
				

			string advancedSort = null;
			if (paramForCheck.paramForSort != null) {
				string desc = paramForCheck.paramForSort.IsDesc ? "desc" : "asc";
				if (paramForCheck.paramForSort.FieldSort == ProductsSort.products_name)
					advancedSort = string.Format (AdvancedSort.ProductToListCategoryIDAndSortToProductDescrioption,
						paramForCheck.paramForSort.FieldSort, desc);
				else
					advancedSort = string.Format (AdvancedSort.ProductToListCategoryIDAndSort, paramForCheck.paramForSort.FieldSort, desc);
			}


			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			string url = WebRequestUtils.GetUrl (Constants.UrlProducts, expandList, formatAdvancedFilter, advancedSort);

			url = WebRequestUtils.GetUrlPage (url, currentPage, countItems);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, isCancelable: true);
			if (contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				throw new Exception();

			if (contentAndHeads.Content == null) return contentAndHeads;
			string json = contentAndHeads.Content[0];

			if (string.IsNullOrEmpty(json)) return contentAndHeads;

			contentAndHeads.productsList = new List<Product>();
			contentAndHeads.productsList.AddRange (JsonConvert.DeserializeObject<List<Product>> (json));

			return contentAndHeads;
		}

		public static string GetUrlByCategoryIDAsync(int[] catogoriesIDList, FilterParam paramForCheck)
		{
			if (paramForCheck == null) {
				return Product.GetUrlByCategoryIDAsync(catogoriesIDList);
			}
			string strCategoryID = string.Join(",", catogoriesIDList);
			string formatAdvancedFilter = string.Format(AdvancedFiltersList.ProductToListCategoryIDAndSortToProductDescrioption);
			formatAdvancedFilter += "," + AdvancedFiltersList.ProductToListFilterProductsQuantity;
			formatAdvancedFilter += "," + string.Format(AdvancedFiltersList.ProductToListFilterCategories, strCategoryID);

			if (paramForCheck.Sizes != null && paramForCheck.Sizes.Length > 0) {
				string strSizes = string.Join(",", paramForCheck.Sizes);
				formatAdvancedFilter += "," + string.Format(AdvancedFiltersList.ProductToListFilterSizes, strSizes);
			}
			if (paramForCheck.PriceBegin != 0)
				formatAdvancedFilter += "," + string.Format(AdvancedFiltersList.ProductToListFilterPriceBegin, paramForCheck.PriceBegin);
			if (paramForCheck.PriceEnd != 0)
				formatAdvancedFilter += "," + string.Format(AdvancedFiltersList.ProductToListFilterPriceEnd, paramForCheck.PriceEnd);

			formatAdvancedFilter = "[" + formatAdvancedFilter + "]&distinct=1";

			string advancedSort = null;
			if (paramForCheck.paramForSort != null) {
				string desc = paramForCheck.paramForSort.IsDesc ? "desc" : "asc";
				if (paramForCheck.paramForSort.FieldSort == ProductsSort.products_name)
					advancedSort = string.Format(AdvancedSort.ProductToListCategoryIDAndSortToProductDescrioption,
						paramForCheck.paramForSort.FieldSort, desc);
				else
					advancedSort = string.Format(AdvancedSort.ProductToListCategoryIDAndSort, paramForCheck.paramForSort.FieldSort, desc);
			}

			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			return WebRequestUtils.GetUrl(Constants.UrlProducts, expandList, formatAdvancedFilter, advancedSort);
		}



		//public static Product GetProductsByID(int productID)
		//{
		//	string advancedFilter = string.Format(AdvancedFiltersList.ProductToID, productID);
		//	string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule;
		//	string url = WebRequestUtils.GetUrl (Constants.UrlProducts, expandList, advancedFilter);

		//	ContentAndHeads contentAndHeads = WebRequestUtils.GetJsonAndHeads (url);
		//	if (ContentAndHeads == null)
		//		return null;
		//	string json = ContentAndHeads.Content[0];
		//	ContentAndHeads.productsList = new List<Product>();

		//	List<Product> product = JsonConvert.DeserializeObject<List<Product>> (json);
		//	return product[0];
		//}



		public static async Task<ContentAndHeads> GetProductsByIDToBasketAsync(int productID)
		{
			string advancedFilter = string.Format(AdvancedFiltersList.ProductToID, productID);
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			string url = WebRequestUtils.GetUrl(Constants.UrlProducts, expandList, advancedFilter);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url, isCancelable: true);
			if (contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK || contentAndHeads.Content == null || contentAndHeads.Content.Count == 0)
				return contentAndHeads;

			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = JsonConvert.DeserializeObject<List<Product>>(json);
			return contentAndHeads;
		}


		public static async Task<Product> GetProductsByIDAsync(int productID)
		{
			string advancedFilter = string.Format(AdvancedFiltersList.ProductToID, productID);
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			string url = WebRequestUtils.GetUrl(Constants.UrlProducts, expandList, advancedFilter);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url, isCancelable:true);
			if (contentAndHeads.Content == null || contentAndHeads.Content.Count == 0)
				return null;
			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = new List<Product>();

			List<Product> product = JsonConvert.DeserializeObject<List<Product>> (json);
			if (product == null || product.Count == 0)
				return null;
			return product[0];
		}

		public static async Task<List<Product>> GetProductsByIDsListAsync(int[] productIDsList)
		{
			string advancedFilter = string.Format(AdvancedFiltersList.ProductToID, string.Join(", ", productIDsList));
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			string url = WebRequestUtils.GetUrl(Constants.UrlProducts, expandList, advancedFilter);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url);

			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = new List<Product>();

			List<Product> product = JsonConvert.DeserializeObject<List<Product>>(json);
			return product;
		}

		public static async Task<List<Product>> GetProductsByNameAsync(string productName, int currentPage, int countItems)
		{
			string advancedFilter = string.Format(AdvancedFiltersList.ProductToName, productName);
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			string url = WebRequestUtils.GetUrl(Constants.UrlProducts, expandList, advancedFilter, currentPage, countItems);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url);

			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = new List<Product>();

			List<Product> product = JsonConvert.DeserializeObject<List<Product>>(json);
			return product;
		}

		public static async Task<List<Product>> GetProductsByArticleAsync(string productArticle)
		{
			string advancedFilter = string.Format(AdvancedFiltersList.ProductToArticle, productArticle);
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			string url = WebRequestUtils.GetUrl(Constants.UrlProducts, expandList, advancedFilter);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url);
			if (contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				throw new Exception();

			string json = contentAndHeads.Content[0];
			List<Product> productsList = new List<Product>();
			productsList.AddRange(JsonConvert.DeserializeObject<List<Product>>(json));

			return productsList;
		}

		public static async Task<ContentAndHeads> GetProductsAsync(string url, int currentPage, int countItems)
		{
			url = WebRequestUtils.GetUrlPage(url, currentPage, countItems);
			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url, isCancelable: true, isCatalog: true);

			if (contentAndHeads.exceptionStatus == System.Net.WebExceptionStatus.RequestCanceled)
				return contentAndHeads;
			
			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = new List<Product>();
			contentAndHeads.productsList.AddRange(JsonConvert.DeserializeObject<List<Product>>(json));

			return contentAndHeads;
		}

		public static async Task<List<Product>> GetProductsNoveltyListAsync(int count)
		{
			string advancedFilter = string.Format(AdvancedFiltersList.ProductToListSort, "products_date_added", "desc");
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			string url = WebRequestUtils.GetUrl(Constants.UrlProducts, expandList, advancedFilter, 10, count);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url);

			if (contentAndHeads == null || contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				return null;

			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = new List<Product>();

			List<Product> product = JsonConvert.DeserializeObject<List<Product>>(json);
			return product;
		}

		public static async Task<List<Product>> GetProductsNewsClothesListAsync(int count)
		{
			//string advancedFilter = AdvancedFiltersList.ProductInThePresence;
			//advancedFilter += string.Format(AdvancedFiltersList.ProductToListSort, "products_date_added", "desc");
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			string url = WebRequestUtils.GetUrl(Constants.PathToProductsNewsClothes + "?limit=" + count, expandList, null);
			//string url = WebRequestUtils.GetUrl(Constants.PathToProductsNewsClothes + "?limit=" + count);


			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url, isCancelable: true);

			if (contentAndHeads == null || contentAndHeads.Content == null || contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				return null;

			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = new List<Product>();

			List<Product> product = JsonConvert.DeserializeObject<List<Product>>(json);
			return product;
		}

		public static async Task<List<Product>> GetProductsNewsOthesListAsync(int count)
		{
			string advancedFilter = AdvancedFiltersList.ProductInThePresence;
			advancedFilter += string.Format(AdvancedFiltersList.ProductToListSort, "products_date_added", "desc");
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule + "," + ExpandList.ProductsExpress;
			string url = WebRequestUtils.GetUrl(Constants.PathToProductsNewsOthes, expandList, advancedFilter, 1, count);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url);

			if (contentAndHeads == null || contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				return null;

			string json = contentAndHeads.Content[0];
			contentAndHeads.productsList = new List<Product>();

			List<Product> product = JsonConvert.DeserializeObject<List<Product>>(json);
			return product;
		}
	}
}