using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Security.Permissions;

namespace OMShopMobile
{
	public class CatalogView : ContentView
	{
		private ListView listViewCategory;
		private ListView breadCrumbsListView;

		private ActivityIndicator allActivityIndicator;
		private Button btnSort;
		private ContentView viewBtnSort;

		private StackLayout layout;
		private ScrollView scrollView;
		private StackLayout mainLayout;
		private List<Category> breadCrumbs;

		ProductsListTemplate productsListTemplate;

		public ProductsSortView productSort;
		TableSizeView tableSizeView;

		private List<int> categoriesIDList;
		private int currentPage = 1;
		private ProductView productTemplate;
		public FilterParam filterParam { get; set; }
		FilterParam filterParamDefault;

		event EventHandler eventRefresh;

		#region Инициализация компонентов
		public CatalogView (bool isLoadAllCategories)
		{
			OnePage.mainPage.appRequest.AbortAll();

			filterParamDefault = new FilterParam { 
				paramForSort = ParamSort.ParamsList.SingleOrDefault (g => g.Id == ProductsIDSort.products_date_added)
			};
			VerticalOptions = LayoutOptions.FillAndExpand;
			OnePage.topView.layoutImgFilter.IsVisible = false;

			InitializationIndication ();
			Content = allActivityIndicator;

			int countColumn = 2;
			productsListTemplate = new ProductsListTemplate(countColumn, ClickProduct);

			InitializationProductSort ();
			mainLayout = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand };

			InitializationCategoryList (isLoadAllCategories);

			layout = new StackLayout {
				Spacing = 0,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children = {
					listViewCategory,
					productsListTemplate
				}
			};
			scrollView = new ScrollView { VerticalOptions = LayoutOptions.FillAndExpand };
			scrollView.Content = layout;

			productTemplate = new ProductView { IsVisible = false };
			productTemplate.EventTableSizeClick += OnTableSize;
			productTemplate.ImageClick.Tapped += ProductImageClick;

			mainLayout.Children.Add (productSort);
			mainLayout.Children.Add (scrollView);
			mainLayout.Children.Add (productTemplate);
		}

		public void GotoMainCategory()
		{
			productSort.IsVisible = false;
			productTemplate.IsVisible = false;
			scrollView.IsVisible = true;
			productsListTemplate.CancelTSProduct.Cancel();
			productsListTemplate.StopLoad();

			productsListTemplate.IsCompleteShowGrid = true;

			listViewCategory.SelectedItem = null;
			listViewCategory.ItemsSource = OnePage.categoryList;
			listViewCategory.HeightRequest = GetHeightListCategory();
		}

		public void CloseFilter()
		{
			if (productSort.IsVisible) {
				if (productsListTemplate.IsCompleteShowGrid) {
					scrollView.IsVisible = true;
					productSort.IsVisible = false;
					OnePage.topView.layoutImgFilter.IsVisible = true;
				} else {
					History history = OnePage.redirectApp.GetCurrentTransition();
					//OnePage.redirectApp.DeleteLastTransition();
					scrollView.IsVisible = true;
					productSort.IsVisible = false;
					productsListTemplate.IsCompleteShowGrid = true;
					productsListTemplate.CancelTSProduct = new CancellationTokenSource();
			       	GoToCategory(history.ContentCategory, history.BreadCrumbs, true, true, history.CurrentProductPage);
				}
			}
			else
				OnePage.redirectApp.BackToHistory ();
		}

		void InitializationIndication ()
		{
			allActivityIndicator = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				IsVisible = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
		}

		private void InitializationCategoryList (bool isLoadAllCategories)
		{
			breadCrumbs = new List<Category>();
			breadCrumbsListView = new ListView {
				ItemTemplate = new DataTemplate (typeof(CategoryListCell)),
				ItemsSource = breadCrumbs,
				BackgroundColor = Color.FromHex ("B0B0B0")
			};
			breadCrumbsListView.ItemTapped += CategoryClick;

			listViewCategory = new ListView {
				ItemTemplate = new DataTemplate (typeof(CategoryListCell)),
			};
			if (isLoadAllCategories) {
				listViewCategory.ItemsSource = OnePage.categoryList;
			}
			listViewCategory.ItemTapped += CategoryClick;
			listViewCategory.HeightRequest = GetHeightListCategory();
			Content = mainLayout;
		}

		void InitializationProductSort ()
		{
			btnSort = new Button {
				Text = "Сортировка",
				HorizontalOptions = LayoutOptions.Start,
			};
			viewBtnSort = new ContentView {
				Padding = new Thickness (1),
				IsVisible = false,
				Content = btnSort
			};
			btnSort.Clicked += ClickSorted;
			productSort = new ProductsSortView (filterParamDefault) {
				IsVisible = false
			};
			productSort.ClickFilterItem = ClickSortItem;
		}
		#endregion

		public void ClickSorted (object sender, EventArgs e)
		{
			if (productSort.CurrentCategory == null) return;
				
			if (productsListTemplate.CancelTSProduct != null && !productsListTemplate.CancelTSProduct.IsCancellationRequested && !productsListTemplate.IsCompleteShowGrid) {
				OnePage.mainPage.appRequest.AbortAll();
				productsListTemplate.CancelTSProduct.Cancel();				
			}
			OnePage.redirectApp.AddTransition (PageName.Catalog, "Настройки выдачи", HistoryStep.FilterProduct);
			scrollView.IsVisible = false;
			productTemplate.IsVisible = false;
			productSort.Show ();
			productSort.IsVisible = true;
		}

		public void ClickSortItem (object sender, EventArgs e)
		{
			filterParam = sender as FilterParam;
			currentPage = 1;

			OnePage.redirectApp.DeleteLastTransition();
			History history = OnePage.redirectApp.GetCurrentTransition();
			scrollView.IsVisible = true;
			productSort.IsVisible = false;
			productsListTemplate.IsCompleteShowGrid = true;
			productsListTemplate.CancelTSProduct = new CancellationTokenSource();
			OnePage.mainPage.mainlayout.Children.Add(OnePage.bottomView);
			OnePage.bottomView.IsVisible = true;
			GoToCategory(history.ContentCategory, history.BreadCrumbs, true, false, currentPage);
		}

		public void StopLoad()
		{
			if (productsListTemplate.CancelTSProduct != null && !productsListTemplate.CancelTSProduct.IsCancellationRequested && !productsListTemplate.IsCompleteShowGrid)
				productsListTemplate.CancelTSProduct.Cancel();
		}

		private void CategoryClick(object sender, ItemTappedEventArgs e)
		{
			Category category = e.Item as Category;
			GoToCategory (category, null, false);
		}

		public void GoToCategory (Category category, List<Category> historyBreadCrumbs, bool isHistory, bool isClearFilter = true, int numberPage = 1)
		{
			Console.WriteLine("!!!!!!!!!!! GoToCategory = " + category.Description.Name);
			OnePage.topView.layoutImgFilter.IsVisible = false;
			if (productsListTemplate.CancelTSProduct != null && !productsListTemplate.CancelTSProduct.IsCancellationRequested && !productsListTemplate.IsCompleteShowGrid) {
				if (!OnePage.mainPage.appRequest.AbortAll()) {
					productsListTemplate.CancelTSProduct.Cancel();
				}
			} else {
				productsListTemplate.CancelTSProduct = new CancellationTokenSource();
			}
			productsListTemplate.IsCompleteShowGrid = false;
			currentPage = numberPage;

			if (!isClearFilter) {
				allActivityIndicator.IsVisible = true;
				Content = allActivityIndicator;
				OnePage.topView.layoutImgFilter.IsVisible = false;
				productTemplate.IsVisible = false;

			} else {
				if (OnePage.redirectApp.GetCurrentTransition().Page != PageName.Catalog) {
					allActivityIndicator.IsVisible = true;
					Content = allActivityIndicator;
				}
				productsListTemplate.StopLoad();
				filterParam = filterParamDefault;
				productSort.Sorted(productSort.sortList, filterParam.paramForSort);
				productSort.ClearFilter();
			}
			productSort.IsVisible = false;
			SetCategory (category, historyBreadCrumbs, isHistory);
		}

		async void SetCategory(Category category2, List<Category> historyBreadCrumbs, bool isHistory)
		{
			Category catCopy = category2;
			await Task.Delay(10);
			try {
				if (category2.Description == null)
					catCopy = await Category.GetegoryByIDAsync(category2.ID, true);
				
				category2 = catCopy;

				if (catCopy.Children == null || catCopy.Children.Count == 0) {
					Category categoryTree = null;
					categoryTree = Category.GetCategoryInTree2(OnePage.categoryList, catCopy);
					if (categoryTree != null && categoryTree.Children != null && categoryTree.Children.Count > 0) {
						catCopy.Children = categoryTree.Children;
					} else {
						bool isChildren = true;
						catCopy.Children = await Category.GetCategoriesByIDAsync(catCopy.ID, isChildren);
					}
				}

				productSort.CurrentCategory = catCopy;

				listViewCategory.SelectedItem = null;
				listViewCategory.ItemsSource = catCopy.Children;
				listViewCategory.HeightRequest = GetHeightListCategory();

				Content = mainLayout;
				if (!isHistory)
					OnePage.redirectApp.AddTransition(PageName.Catalog, catCopy.Description.Name, false, false, catCopy);
				productsListTemplate.ShowIndicator();
				GetProductsListAsync(catCopy);
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (sender, e) => {
					Button content = sender as Button;
					content.IsEnabled = false;
					SetCategory(catCopy, historyBreadCrumbs, isHistory);
				};
				Content = OnePage.mainPage.ShowMessageError(eventRefresh);
				return;
			}
		}

		private async void GetProductsListAsync(Category category)
		{
			productTemplate.IsVisible = false;
			categoriesIDList = new List<int> ();
			allActivityIndicator.IsVisible = false;
			OnePage.topView.layoutImgFilter.IsVisible = true;
			scrollView.IsVisible = true;

			if (!mainLayout.Children.Contains(allActivityIndicator))
				mainLayout.Children.Insert(0, allActivityIndicator);
			if (!viewBtnSort.IsVisible)
				viewBtnSort.IsVisible = true;

			//OnePage.topView.layoutImgFilter.IsVisible = false;
			// Посчитать разницу по времени
			DateTime oldDate = DateTime.Now;

			Category categoryFree = OnePage.categoryList.FirstOrDefault(g => g.ID == category.ID);
			if (categoryFree != null && !categoryFree.IsTreeFill) {
				try {
					await Category.GetTreeCategoriesAsync(category, categoriesIDList, productsListTemplate.CancelTSProduct);
					if (productsListTemplate.CancelTSProduct.IsCancellationRequested) {
						productsListTemplate.CancelTSProduct = new CancellationTokenSource ();
						return;
					}
				} catch (Exception) {
					if (eventRefresh == null)
						eventRefresh += (sender, e) => { 
						Button content = sender as Button;
						content.IsEnabled = false;
						GetProductsListAsync(category); };
					Content = OnePage.mainPage.ShowMessageError (eventRefresh);
					return;
				}
				Category curCategory = OnePage.categoryList.SingleOrDefault (g => g.ID == category.ID);
				if (curCategory != null) {
					curCategory.Children = category.Children;
					curCategory.IsTreeFill = true;
				}
			} else
				GetCategoryToID (category, categoriesIDList);

			Content = mainLayout;

			DateTime newDate = DateTime.Now;
			TimeSpan ts = newDate - oldDate;
			//Console.WriteLine ("Total minutes " + ts.TotalMilliseconds);

			categoriesIDList.Add (category.ID);
			int newSize = Constants.MaxCountCategoryInAPI;
			if (categoriesIDList.Count > newSize)
			{
				categoriesIDList.RemoveRange(newSize, categoriesIDList.Count - newSize);
			}

			string url = Product.GetUrlByCategoryIDAsync(categoriesIDList.ToArray(), filterParam);
			productsListTemplate.CurrentPage = currentPage;
			productsListTemplate.ShowProducts(url);
		}


		void GetCategoryToID(Category category, List<int> categoriesIDList)
		{
			if (category.Children == null) return;
			
			foreach(Category categoryItem in category.Children) {
				categoriesIDList.Add(categoryItem.ID);	
			} 

			foreach (Category cat in category.Children) {
				if (cat.Children != null && cat.Children.Count != 0) 
					GetCategoryToID (cat, categoriesIDList);
			}
		}

		void SetPagination(Pagination pagination, ContentAndHeads contentAndHeads)
		{
			pagination.CurrentPage = contentAndHeads.currentPage;
			pagination.CountPage = contentAndHeads.countPage;
			pagination.Show ();
			pagination.tapGestureRecognizer.Tapped += ClickPage;
		}

		private void ShowProducts(ContentAndHeads contentAndHeads)
		{
			List<Product> productsList;
			productsList = contentAndHeads.productsList;
			if (productsList != null && productsList.Count > 0) {
				productsListTemplate.ShowProducts(productsList);
			}
		}

		private void ClickProduct (object sender, EventArgs e)
		{
			TappedEventArgs eTapped = e as TappedEventArgs;
			ProductInCategoryTemplate product = (ProductInCategoryTemplate)eTapped.Parameter;

			if (productsListTemplate.CancelTSProduct != null && !productsListTemplate.CancelTSProduct.IsCancellationRequested && !productsListTemplate.IsCompleteShowGrid) {
				if (!OnePage.mainPage.appRequest.AbortAll()) {
					productsListTemplate.CancelTSProduct.Cancel();
				}
			}
			GoToProductAsync (product.Name, product.Id, false);
		}

		public void GoToProductAsync(Basket basket, Product product)
		{
			OnePage.redirectApp.AddTransition(PageName.Catalog, basket.ProductID, basket.ProductName);
			OnePage.topView.layoutImgFilter.IsVisible = false;
			productTemplate.ShowProduct (basket, product);
			productTemplate.IsVisible = true;

			productSort.IsVisible = false;
			scrollView.IsVisible = false;
		}

		public async Task GoToProductAsync(string productName, int productID, bool isHistory)
		{
			productsListTemplate.CancelTSProduct = new CancellationTokenSource();
			productsListTemplate.IsCompleteShowGrid = false;

			productTemplate.IsVisible = true;
			/// КОСТЫЛЬ что бы не сжималось окно просмотра товара
			OnePage.bottomView.IsVisible = false;
			OnePage.mainPage.mainlayout.Children.Remove(OnePage.bottomView);
			scrollView.IsVisible = false;

			if (!mainLayout.Children.Contains(allActivityIndicator))
				mainLayout.Children.Insert(0, allActivityIndicator);
			
			productTemplate.IsVisible = false;
			if (tableSizeView != null)
				tableSizeView.IsVisible = false;
			OnePage.topView.layoutImgFilter.IsVisible = false;
			allActivityIndicator.IsVisible = true;

			Content = mainLayout;

			if (!isHistory) OnePage.redirectApp.AddTransition (PageName.Catalog, productID, productName);
			Product product;
			try {
				if (productsListTemplate.CancelTSProduct.IsCancellationRequested) {
					productsListTemplate.CancelTSProduct = new CancellationTokenSource();
					return;
				}
				product = await Product.GetProductsByIDAsync (productID);
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (sender, e) => { 
					Button content = sender as Button;
					content.IsEnabled = false;
					GoToProductAsync(productName, productID, false);
				};
				Content = OnePage.mainPage.ShowMessageError (eventRefresh);
				return;
			}
			if (product == null) return;

			productTemplate.ShowProduct (product);
			//productTemplate.EventTableSizeClick += OnTableSize;
			//productTemplate.ImageClick.Tapped += ProductImageClick;
			productTemplate.IsVisible = true;
			productsListTemplate.IsCompleteShowGrid = true;

			allActivityIndicator.IsVisible = false;
		}

		void OnTableSize (object sender, EventArgs e)
		{
			GotoTableSize (false);
		}

		public void GotoTableSize(bool isHistory, HistoryStep step = HistoryStep.Default)
		{
			if (!isHistory) OnePage.redirectApp.AddTransition (PageName.Catalog, "Размерные таблицы", HistoryStep.TableSizes);

			if (tableSizeView == null) {
				tableSizeView = new TableSizeView ();
				mainLayout.Children.Add (tableSizeView);
			} else {
				tableSizeView.GotoMain();
			}
			productTemplate.IsVisible = false;
			scrollView.IsVisible = false;
			tableSizeView.IsVisible = true;
		}

		private void ProductImageClick (object sender, EventArgs e)
		{
			Image img = sender as Image;
			TappedEventArgs eTapped = e as TappedEventArgs;
			Product product = eTapped.Parameter as Product;
			Uri uri = img.Source.GetValue (UriImageSource.UriProperty) as Uri;
			GoToImage(uri.ToString(), product.Article, false);
		}

		public void GoToImage(string imgSource, string article, bool isHistory)
		{
			//if (productsListTemplate.CancelTSProduct != null && !productsListTemplate.CancelTSProduct.IsCancellationRequested && !productsListTemplate.IsCompleteShowGrid) {
			//	if (!OnePage.mainPage.appRequest.AbortAll()) {
			//		productsListTemplate.CancelTSProduct.Cancel();
			//	}
			//}
			OnePage.redirectApp.RedirectToPage (PageName.Image, false, false);
			OnePage.redirectApp.imageView.Show (imgSource, article);
		}

		private async void ClickPage(object s, EventArgs e)
		{
			Label lbl = s as Label;
			int numberPage = int.Parse (lbl.Text);
			OnePage.redirectApp.ClickProductPage(numberPage);
			GoToPage(numberPage);
		}

		public async void GoToPage(int numberPage)
		{
			ContentAndHeads contentAndHeads;
			try
			{
				contentAndHeads = await Product.GetProductsByCategoryIDAsync(categoriesIDList.ToArray(), numberPage, XPagination.CountProduct, filterParam);
			}
			catch (Exception)
			{
				eventRefresh = null;
				eventRefresh += (obj, evn) =>
				{
					Button content = obj as Button;
					content.IsEnabled = false;
					GoToPage(numberPage);
				};
				Content = OnePage.mainPage.ShowMessageError(eventRefresh);
				return;
			}
			ShowProducts(contentAndHeads);
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