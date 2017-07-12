using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OMShopMobile
{
	public partial class CatalogViewXaml : ContentView
	{
		private int currentPage = 1;
		public int CurrentPage { get; set; }

		event EventHandler eventRefresh;
		ActivityIndicator allActivityIndicator;
		public FilterParam filterParam { get; set; }

		public CatalogViewXaml()
		{
			InitializeComponent();

			allActivityIndicator = new ActivityIndicator {
				Color = Device.OnPlatform(Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				IsVisible = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			Category category = new Category {
				ID = (int)CatalogEnum.Men,
				Description = new CategoriesDescription { Name = "Мужская одежда" }
			};

			//GoToCategory(category, null, false);
		}

		//protected override void LayoutChildren(double x, double y, double width, double height)
		//{
		//	base.LayoutChildren(x, y, width, height);
		//	gridProduct.Layout(new Rectangle(0, 0, width, height));
		//}

		public void GoToCategory(Category category, List<Category> historyBreadCrumbs, bool isHistory, bool isClearFilter = true, int numberPage = 1)
		{
			currentPage = numberPage;

			if (!isClearFilter) {
				allActivityIndicator.IsVisible = true;
				OnePage.topView.layoutImgFilter.IsVisible = false;
				//productTemplate.IsVisible = false;
			} else {
				if (OnePage.redirectApp.GetCurrentTransition().Page != PageName.Catalog) {
					allActivityIndicator.IsVisible = true;
					//Content = allActivityIndicator;
				}
				//productSort.CurrentCategory = category;
				//filterParam = filterParamDefault;
				//productSort.Sorted(productSort.sortList, filterParam.paramForSort);
			}
			//productSort.IsVisible = false;
			SetCategory(category, historyBreadCrumbs, isHistory);
		}

		async void SetCategory(Category category, List<Category> historyBreadCrumbs, bool isHistory)
		{
			await Task.Delay(10);
			try {
				if (category.Children == null || category.Children.Count == 0) {
					Category categoryTree = null;
					categoryTree = Category.GetCategoryInTree2(OnePage.categoryList, category);
					if (categoryTree != null && categoryTree.Children != null && categoryTree.Children.Count > 0) {
						category.Children = categoryTree.Children;
					} else {
						bool isChildren = true;
						category.Children = await Category.GetCategoriesByIDAsync(category.ID, isChildren);
					}
				}
				listViewCategory.ItemsSource = category.Children;
				listViewCategory.HeightRequest = GetHeightListCategory();

				//Content = mainLayout;
				//productsListTemplate.ShowIndicator();
				GetProductsListAsync(category);
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (sender, e) => {
					Button content = sender as Button;
					content.IsEnabled = false;
					SetCategory(category, historyBreadCrumbs, isHistory);
				};
				Content = OnePage.mainPage.ShowMessageError(eventRefresh);
				return;
			}
			if (!isHistory)
				OnePage.redirectApp.AddTransition(PageName.Catalog, category.Description.Name, false, false, category);
		}

		private async void GetProductsListAsync(Category category)
		{
			List<int> categoriesIDList = new List<int>();

			Category categoryFree = OnePage.categoryList.FirstOrDefault(g => g.ID == category.ID);
			if (categoryFree != null && !categoryFree.IsTreeFill) {
				await Category.GetTreeCategoriesAsync(category, categoriesIDList, null);
				
				Category curCategory = OnePage.categoryList.SingleOrDefault(g => g.ID == category.ID);
				if (curCategory != null) {
					curCategory.Children = category.Children;
					curCategory.IsTreeFill = true;
				}
			} else
				GetCategoryToID(category, categoriesIDList);





			categoriesIDList.Add(category.ID);
			int newSize = Constants.MaxCountCategoryInAPI;
			if (categoriesIDList.Count > newSize) {
				categoriesIDList.RemoveRange(newSize, categoriesIDList.Count - newSize);
			}

			string url = Product.GetUrlByCategoryIDAsync(categoriesIDList.ToArray(), filterParam);
			ContentAndHeads contentAndHeads = await Product.GetProductsAsync(url, CurrentPage, XPagination.CountProduct);

			List<Product> productsList = contentAndHeads.productsList;
			gridProduct.ItemsSource = productsList;
		}

		void GetCategoryToID(Category category, List<int> categoriesIDList)
		{
			if (category.Children == null) return;

			foreach (Category categoryItem in category.Children) {
				categoriesIDList.Add(categoryItem.ID);
			}

			foreach (Category cat in category.Children) {
				if (cat.Children != null && cat.Children.Count != 0)
					GetCategoryToID(cat, categoriesIDList);
			}
		}

		void OnItemSelected(object sender, XLabs.GridEventArgs<object> e)
		{
			//var item = e.Value as MediaItem;
			//DisplayAlert("you selected an item", item.Name, "Ok");
		}

		private int GetHeightListCategory()
		{
			if (listViewCategory.ItemsSource == null)
				return 0;
			int countCategory = (listViewCategory.ItemsSource as List<Category>).Count;
			if (countCategory == 0)
				return 0;
			else
				return countCategory * Constants.HeightRowListView + (int)(countCategory * 0.5);
		}
	}
}
