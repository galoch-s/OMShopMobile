using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace OMShopMobile
{
	public class CategoryTreeID
	{
		public int Id { get; set; }
		public List<CategoryTreeID> CategoryTreeList { get; set; }
	}

	public class Category
	{
		private static Category allCategories;


		[JsonProperty("categories_id")]
		public int ID { get; set;}

		[JsonProperty("categories_name")]
		public string Name { 
			get { return Description?.Name; }
		}

		[JsonProperty("categories_image")]
		public string Image { get; set;}

		[JsonProperty("parent_id")]
		public int ParentId { get; set;}

		[JsonProperty("sort_order")]
		public int? SortOrder { get; set;}

		[JsonProperty("categories_status")]
		public int Status { get; set;}

		[JsonProperty("childrenCategoriesDesc")]
		public List<Category> Children { get; set;}

		[JsonProperty("categoriesDescription")]
		public CategoriesDescription Description { get; set; }

		[JsonIgnore]
		public bool IsTreeFill { get; set; }

		public override string ToString()
		{
			if (Description != null)
				return Description.Name;
			else
				return "Пустая катергория";
		}

		public static async Task<Category> GetegoryByIDAsync(int id, bool isChildren = false)
		{
			string advancedFilter = string.Format(AdvancedFiltersList.CategoriesToID, id);
			string expandList = ExpandList.CategoriesDescription;
			if (isChildren)
				expandList += "," + ExpandList.CategoriesChildren;
			string url = WebRequestUtils.GetUrl (Constants.UrlCategories, expandList, advancedFilter);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync (url);
			if (contentAndHeads == null || contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
				throw new Exception ();
			
			string json = contentAndHeads.Content[0];
			Category category = JsonConvert.DeserializeObject<List<Category>> (json)[0];
			return category;
		}

		//public static List<Category> GetCategoriesByID(int id)
		//{
		//	string advancedFilter = string.Format(AdvancedFiltersList.CategoriesToParentID, id);
		//	string expandList = ExpandList.CategoriesDescription;
		//	string url = WebRequestUtils.GetUrl (Constants.UrlCategories, expandList, advancedFilter);

		//	List<string> jsonsList = WebRequestUtils.GetJsonsAllPage (url);
		//	List<Category> categoriesList = new List<Category>();

		//	if (jsonsList != null)
		//		foreach (var item in jsonsList) {
		//			categoriesList.AddRange (JsonConvert.DeserializeObject<List<Category>> (item));
		//		}
		//	return categoriesList;
		//}

		public static async Task<List<Category>> GetCategoriesByIDAsync(int id, bool isChildren = false)
		{
			string advancedFilter = string.Format(AdvancedFiltersList.CategoriesToParentID, id);
			string expandList = ExpandList.CategoriesDescription;
			if (isChildren)
				expandList += "," + ExpandList.CategoriesChildren;
			string url = WebRequestUtils.GetUrl (Constants.UrlCategories, expandList, advancedFilter);

			ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonsAndHeadsAllPageAsync(url);
			List<string> jsonsList = contentAndHeads.Content;
			List<Category> categoriesList = new List<Category>();

			if (jsonsList != null)
				foreach (var item in jsonsList) {
					categoriesList.AddRange (JsonConvert.DeserializeObject<List<Category>> (item));
				}
			foreach (var cat in categoriesList) {
				cat.Children.RemoveAll(g => g.Status == 0);
			}
			return categoriesList;
		}

		public static async Task GetTreeCategoriesAsync(Category category, List<int> categoriesIDList, CancellationTokenSource _cancellationTokenSource)
		{
			if (!category.Children.Any(g => g.Children != null)) {
				string advancedFilter = string.Format(AdvancedFiltersList.CategoriesToParentID, category.ID);
				string expandList = ExpandList.CategoriesDescription + "," + ExpandList.CategoriesChildren;
				string url = WebRequestUtils.GetUrl(Constants.UrlCategories, expandList, advancedFilter);

				ContentAndHeads contentAndHeads = await WebRequestUtils.GetJsonsAndHeadsAllPageAsync(url);
				if (contentAndHeads == null)
					return;

				List<string> jsonsList = contentAndHeads.Content;
				category.Children = new List<Category>();

				foreach (string item in jsonsList) {
					List<Category> items = JsonConvert.DeserializeObject<List<Category>>(item);
					category.Children.AddRange(items);
				}
				foreach (var cat in category.Children) {
					cat.Children.RemoveAll(g => g.Status == 0);
				}
			}
			foreach (Category categoryItem in category.Children) {
				categoriesIDList.Add(categoryItem.ID);
			}
			foreach (Category cat in category.Children) {
				if (_cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested) {
					//_cancellationTokenSource = new CancellationTokenSource ();
					return;
				}
				if (cat.Children != null && cat.Children.Count != 0)
					await GetTreeCategoriesAsync(cat, categoriesIDList, _cancellationTokenSource);
			}
		}

		public static void GetCategoryInTree(List<Category> categoryList, Category category, ref Category findCategory)
		{
			if (categoryList != null)
				foreach (Category cat in categoryList) {
					if (cat.ID == category.ID) {
						findCategory = cat;
						return;
					} else
						GetCategoryInTree(cat.Children, category, ref findCategory);
				}
		}

		public static Category GetCategoryInTree2(List<Category> categoryList, Category category)
		{
			Category findCategory = null;
			if (categoryList == null) return null;

			foreach (Category cat in categoryList) {
				if (cat.ID == category.ID) {
					return cat;
				} else {
					findCategory = GetCategoryInTree2(cat.Children, category);
					if (findCategory != null)
						return findCategory;
				}

			}
			return findCategory;
		}


//		public static async Task GetTreeCategoriesAsync(int categoryID, CategoryTreeID category)
//		{
//			string advancedFilter = string.Format(AdvancedFiltersList.CategoriesToParentID, categoryID);
//			string expandList = ExpandList.CategoriesDescription + "," + ExpandList.CategoriesChildren;
//			string url = WebRequestUtils.GetUrl (Constants.UrlCategories, expandList, advancedFilter);
//
//			List<string> jsonsList = (await WebRequestUtils.GetJsonsAndHeadsAllPageAsync (url)).Content;
//			category.CategoryTreeList = new List<CategoryTreeID> ();
//
//			foreach (string item in jsonsList) {
//				List<Category> items = JsonConvert.DeserializeObject<List<Category>> (item);
//				category.CategoryTreeList.AddRange (items.Select(g=> new CategoryTreeID { Id = g.ID }).ToList<CategoryTreeID>());
//
//				foreach(Category categoryItem in items) {
//					category.CategoryTreeList.Add(categoryItem.ID);	
//				} 
//			}
//
//			foreach (Category cat in category.Children) {
//				if (cat.Children != null && cat.Children.Count != 0) 
//					await GetTreeCategoriesAsync (cat, categoriesIDList);
//			}
//		}
	}
}