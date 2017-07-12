using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OMShopMobile
{
	public class SearchView : ContentView
	{
		int countSecond = 2;

		DateTime oldDate;
		ProductsListTemplate productsListTemplate;
		private ProductView productTemplate;
		ActivityIndicator indicator;
		string Text;
		event EventHandler eventRefresh;
		ScrollView scrollMain;

		TableSizeView tableSizeView;

		public SearchView ()
		{
			Padding = new Thickness (0, 12);
			VerticalOptions = LayoutOptions.FillAndExpand;
			OnePage.topView.SearchProduct += OnSearchProduct;

			int countColumn = 2;
			productsListTemplate = new ProductsListTemplate (countColumn, OnClickProduct);
			//productsListTemplate.ClickProduct += OnClickProduct;
			productTemplate = new ProductView ();
			productTemplate.EventTableSizeClick += OnTableSize;
			productTemplate.ImageClick.Tapped += ProductImageClick;

			indicator = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
			scrollMain = new ScrollView() { Content = productsListTemplate };
			Content = scrollMain;
			SearchProduct ();
			OnePage.topView.searchBar.Focus ();
		}

		public void SearchProduct()
		{
			oldDate = DateTime.Now;
			ShowProductAsync (Text);
		}

		void OnSearchProduct (object sender, TextChangedEventArgs e)
		{	
			Text = e.NewTextValue;
			SearchProduct ();
		}

		public void ShowResult()
		{
			Padding = new Thickness (0, 12);
			Content = scrollMain;
		}

		public void ClearText()
		{
			OnePage.topView.searchBar.Text = "";
			productsListTemplate.IsVisible = false;
			ShowProductAsync(Text);
		}

		private async void ShowProductAsync(string textSearch)
		{
			Content = scrollMain;
			// Усыпить поток
			await Task.Delay (countSecond * 1000).ConfigureAwait (true);
			DateTime newDate = DateTime.Now;
			TimeSpan differenceTime = newDate - oldDate;
			if (differenceTime.Seconds < countSecond)
				return;

			if (string.IsNullOrEmpty (textSearch)) {
				productsListTemplate.IsVisible = false;
				return;
			} else
				productsListTemplate.IsVisible = true;

			List<Product> productsList = null;

			Content = indicator;

			Regex rxNums = new Regex (@"^\d+$"); // любые цифры
			if (rxNums.IsMatch (textSearch)) {
				try {
					productsList = await Product.GetProductsByArticleAsync (textSearch);
				} catch (Exception) {
					eventRefresh = null;
					eventRefresh += (sender, e) => { 
						Button content = sender as Button;
						content.IsEnabled = false;
						ShowProductAsync (textSearch);
					};
					Content = OnePage.mainPage.ShowMessageError (eventRefresh);
					return;
				}
			}
			Content = scrollMain;

			if (productsList != null && productsList.Count > 0) {
				productsListTemplate.ShowProducts (productsList);
				return;
			}

			string advancedFilter = string.Format (AdvancedFiltersList.ProductToName, textSearch);
			string expandList = ExpandList.ProductsAttributesFullInfo + "," + ExpandList.ProductsDescription + "," + ExpandList.ProductsSchedule;
			string url = WebRequestUtils.GetUrl (Constants.UrlProducts, expandList, advancedFilter);
			productsListTemplate.CurrentPage = 1;
			productsListTemplate.ShowProducts (textSearch, url);
		}

		void OnClickProduct (object sender, EventArgs e)
		{
			TappedEventArgs eTapped = e as TappedEventArgs;
			ProductInCategoryTemplate productInCategory = (ProductInCategoryTemplate)eTapped.Parameter;
			if (productInCategory == null)
				return;
			int productID = productInCategory.Id;

			ShowProduct(productID);
		}

		public void ShowProduct(int productID)
		{ 
			Content = indicator;
			GoToProduct(productID);
		}

		async void GoToProduct(int productID)
		{ 
			Product product = await Product.GetProductsByIDAsync(productID);
			OnePage.redirectApp.AddTransition(PageName.Search, productID, product.productsDescription.Name);
			productTemplate.ShowProduct(product);

			Padding = new Thickness(0);
			Content = productTemplate;
		}

		void OnTableSize(object sender, EventArgs e)
		{
			GotoTableSize(false);
		}

		public void GotoTableSize(bool isHistory)
		{
			if (!isHistory)
				OnePage.redirectApp.AddTransition(PageName.Search, "Размерные таблицы", HistoryStep.TableSizes);

			if (tableSizeView == null) {
				tableSizeView = new TableSizeView();
			} else {
				tableSizeView.GotoMain();
			}
			Content = tableSizeView;
			//productTemplate.IsVisible = false;
			//scrollView.IsVisible = false;
			//tableSizeView.IsVisible = true;
		}

		private void ProductImageClick(object sender, EventArgs e)
		{
			Image img = sender as Image;
			TappedEventArgs eTapped = e as TappedEventArgs;
			Product product = eTapped.Parameter as Product;
			Uri uri = img.Source.GetValue(UriImageSource.UriProperty) as Uri;
			GoToImage(uri.ToString(), product.Article, false);
		}

		public void GoToImage(string imgSource, string article, bool isHistory)
		{
			//if (productsListTemplate.CancelTSProduct != null && !productsListTemplate.CancelTSProduct.IsCancellationRequested && !productsListTemplate.IsCompleteShowGrid) {
			//	if (!OnePage.mainPage.appRequest.AbortAll()) {
			//		productsListTemplate.CancelTSProduct.Cancel();
			//	}
			//}
			OnePage.redirectApp.RedirectToPage(PageName.Image, false, false);
			OnePage.redirectApp.imageView.Show(imgSource, article);
		}
	}
}