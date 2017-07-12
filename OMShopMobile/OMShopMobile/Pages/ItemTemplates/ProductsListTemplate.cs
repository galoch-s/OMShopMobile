using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace OMShopMobile
{
	public class ProductsListTemplate : ContentView
	{
		Grid gridVerticalProductsList;
		Pagination paginationBegin;
		Pagination paginationEnd;

		ActivityIndicator indicator;
		StackLayout layoutMain;

		StackLayout layoutProductGrid;


		public int CurrentPage { get; set; }
		private string TextSearch;

		public CancellationTokenSource CancelTSProduct { get; set; }
		public bool IsCompleteShowGrid { get; set; }

		string Url;

		public event EventHandler ClickProduct;

		event EventHandler eventRefresh;

		public ProductsListTemplate (int countColumn, EventHandler clickProduct)
		{
			ClickProduct = clickProduct;
			
			IsCompleteShowGrid = true;
			CancelTSProduct = new CancellationTokenSource();

			gridVerticalProductsList = new Grid {
				VerticalOptions = LayoutOptions.StartAndExpand,
				RowSpacing = 10,
				ColumnSpacing = 10,
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength (50, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength (50, GridUnitType.Star) },
				},
			};
			for (int i = 0; i < 10; i++)
				gridVerticalProductsList.RowDefinitions.Add(new RowDefinition { Height = 210 });


			paginationBegin = new Pagination ();
			paginationEnd = new Pagination ();
			indicator = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				IsVisible = false,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			layoutProductGrid = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(8, 8),
				Children = {
					paginationBegin,
					gridVerticalProductsList,
					paginationEnd
				},
				IsVisible = false
			};

			layoutMain = new StackLayout { 
				//VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					indicator,
					layoutProductGrid
				}
			};
			VerticalOptions = LayoutOptions.FillAndExpand;
			Content = layoutMain;

			for (int i = 0; i < XPagination.CountProduct; i++) {
				ProductInCategoryTemplate productItem = new ProductInCategoryTemplate();
				productItem.EventClick += ClickProduct;
				gridVerticalProductsList.Children.Add(productItem, i % countColumn, i / countColumn);
			}
		}

		public void StopLoad()
		{ 
			indicator.IsVisible = false;
			layoutProductGrid.IsVisible = false;
		}

		public void ShowIndicator()
		{
			indicator.IsVisible = true;
			CurrentPage = 1;
		}

		private void SetPagination(Pagination pagination, ContentAndHeads contentAndHeads)
		{
			pagination.CurrentPage = contentAndHeads.currentPage;
			pagination.CountPage = contentAndHeads.countPage;

			pagination.Clear();
			pagination.IsVisible = true;
			pagination.Show ();
			pagination.tapGestureRecognizer.Tapped += ClickPage;
		}

		public void ShowProducts(List<Product> productsList)
		{
			Content = layoutMain;

			foreach (var item in gridVerticalProductsList.Children) {
				item.IsVisible = false;
			}

			ShowGrid (gridVerticalProductsList, productsList, true, 2);

			indicator.IsVisible = false;
			layoutProductGrid.IsVisible = true;

			paginationBegin.IsVisible = false;
			paginationEnd.IsVisible = false;
		}

		public async void ShowProducts(string textSearch, string url)
		{
			Content = layoutMain;

			foreach (var item in gridVerticalProductsList.Children) {
				item.IsVisible = false;
			}

			indicator.IsVisible = true;
			layoutProductGrid.IsVisible = false;

			await Task.Delay(100);

			Url = url;
			TextSearch = textSearch;
			//ContentAndHeads contentAndHeads;
			try {
				contentAndHeads1 = await Product.GetProductsAsync(url, CurrentPage, XPagination.CountProduct);
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (sender, e) => {
					Button content = sender as Button;
					content.IsEnabled = false;
					ShowProducts(textSearch, url);
				};
				Content = OnePage.mainPage.ShowMessageError(eventRefresh);
				return;
			}

			List<Product> productsList = contentAndHeads1.productsList;
			if (!string.IsNullOrEmpty(textSearch) && productsList == null || productsList.Count == 0) {
				Content = new StackLayout {
					Children = {
						new Label {
							HorizontalOptions = LayoutOptions.Center,
							Text = "По запросу '" + textSearch + "' ничего не найдено",
						}
					}
				};
			}

			//if (contentAndHeads1.countPage > 1) {
			//	SetPagination(paginationBegin, contentAndHeads);
			//	SetPagination(paginationEnd, contentAndHeads);
			//	//paginationBegin.IsVisible = true;
			//	//paginationEnd.IsVisible = true;
			//} else {
			//	paginationBegin.IsVisible = false;
			//	paginationEnd.IsVisible = false;
			//}
			productsList = contentAndHeads1.productsList;

			//indicator.IsVisible = false;
			//layoutIndicator.IsVisible = false;
			//layoutProductGrid.IsVisible = true;

			ShowGrid(gridVerticalProductsList, productsList, true, 2);
		}


		#if __ANDROID__

		ContentAndHeads contentAndHeads1;

		public async void ShowProducts(string url)
		{
			Content = layoutMain;
			indicator.IsVisible = true;
			layoutProductGrid.IsVisible = false;

			foreach (var item in gridVerticalProductsList.Children) {
				item.IsVisible = false;
			}


			Url = url;
			try
			{
				contentAndHeads1 = await Product.GetProductsAsync(url, CurrentPage, XPagination.CountProduct);
			}
			catch (Exception ex)
			{
				eventRefresh = null;
				eventRefresh += (sender, e) =>
				{
					Button content = sender as Button;
					content.IsEnabled = false;
					ShowProducts(url);
				};
				Content = OnePage.mainPage.ShowMessageError(eventRefresh);
				return;
			}
			if (contentAndHeads1.exceptionStatus == System.Net.WebExceptionStatus.RequestCanceled) {
				CancelTSProduct = new CancellationTokenSource();
				return;
			}
			List<Product> productsList = contentAndHeads1.productsList;
			if (productsList == null || productsList.Count == 0)
			{
				Content = new Label
				{
					HorizontalOptions = LayoutOptions.Center,
					Text = "Количество товаро - 0"
				};
			}


			productsList = contentAndHeads1.productsList;
			ShowGrid(gridVerticalProductsList, productsList, true, 2);
		}

		public void ShowGrid(Grid grid, List<Product> productsList, bool IsOrientationPortrait, int countColumn)
		{
			bool isGridChildrenZero = grid.Children.Count == 0;

			bool isInExistence;
			if (productsList == null || productsList.Count == 0) {
				IsCompleteShowGrid = true;
				return;
			}
			int i = 0;

			Task.Run(() => {
				foreach (Product product in productsList) {
					if (CancelTSProduct.IsCancellationRequested) {
						CancelTSProduct = new CancellationTokenSource();
						IsCompleteShowGrid = true;
						return;
					}
					if (product.productsAttributes.Count > 0) {
						isInExistence = false;
						for (int j = 0; j < product.productsAttributes.Count; j++) {
							if (product.productsAttributes[j].Quantity > 0)
								isInExistence = true;
						}
					} else
						isInExistence = true;

					if (!isInExistence || product.productsDescription == null)
						continue;

					ProductInCategoryTemplate productItem = null;
					if (i <= grid.Children.Count - 1) {
						productItem = grid.Children[i] as ProductInCategoryTemplate;//.Take();
						Device.BeginInvokeOnMainThread(() => { 
							productItem.IsVisible = true;
							productItem.ShowProduct(product, isInExistence); 
						});
						if (isGridChildrenZero)
							AddProduct(productItem, i, countColumn, grid);
					} else {
						//productItem = new ProductInCategoryTemplate();
						//productItem.EventClick += ClickProduct;
						////OnePage.mainPage.productItemsList.Add(productItem);


						//productItem.ShowProduct(product, isInExistence);
						//AddProduct(productItem, i, countColumn, grid);
					}
					i++;
				}
				IsCompleteShowGrid = true;

				Device.BeginInvokeOnMainThread(() => {
					grid.HeightRequest = Math.Ceiling((double)productsList.Count / 2) * (210 + 10); 
					//if (CancelTSProduct.IsCancellationRequested) {
					//	CancelTSProduct = new CancellationTokenSource();

					//	indicator.IsVisible = false;
					//	layoutProductGrid.IsVisible = false;
					//}

					if (contentAndHeads1 != null && contentAndHeads1.countPage > 1) {
						SetPagination(paginationBegin, contentAndHeads1);
						SetPagination(paginationEnd, contentAndHeads1);
					} else {
						paginationBegin.IsVisible = false;
						paginationEnd.IsVisible = false;
					}
					indicator.IsVisible = false;  
					layoutProductGrid.IsVisible = true;
				});
			});
		}
		#else

		ContentAndHeads contentAndHeads1;

		public async void ShowProducts(string url)
		{
			Content = layoutMain;
			indicator.IsVisible = true;
			layoutProductGrid.IsVisible = false;


			Url = url;

			try {
				contentAndHeads1 = await Product.GetProductsAsync(url, CurrentPage, XPagination.CountProduct);
			} catch (Exception ex) {
				eventRefresh = null;
				eventRefresh += (sender, e) => {
					Button content = sender as Button;
					content.IsEnabled = false;
					ShowProducts(url);
				};
				Content = OnePage.mainPage.ShowMessageError(eventRefresh);
				return;
			}
			if (contentAndHeads1.exceptionStatus == System.Net.WebExceptionStatus.RequestCanceled)
				return;
			List<Product> productsList = contentAndHeads1.productsList;
			if (productsList == null || productsList.Count == 0) {
				Content = new Label {
					HorizontalOptions = LayoutOptions.Center,
					Text = "Количество товаро - 0"
				};
			}


			if (contentAndHeads1.countPage > 1) {
				SetPagination(paginationBegin, contentAndHeads1);
				SetPagination(paginationEnd, contentAndHeads1);
				paginationBegin.IsVisible = true;
				paginationEnd.IsVisible = true;
			} else {
				paginationBegin.IsVisible = false;
				paginationEnd.IsVisible = false;
			}


			 
			productsList = contentAndHeads1.productsList;
			ShowGrid(gridVerticalProductsList, productsList, true, 2);
		}

		ContentAndHeads contentAndHeads;

		public void ShowGrid(Grid grid, List<Product> productsList, bool IsOrientationPortrait, int countColumn)
		{
			bool isInExistence;
			if (productsList == null || productsList.Count == 0) {
				IsCompleteShowGrid = true;
				return;
			}
			int i = 0;


			indicator.IsVisible = false;
			layoutProductGrid.IsVisible = true;


			Task.Run(() => {
				foreach (Product product in productsList) {
					if (CancelTSProduct.IsCancellationRequested) {
						CancelTSProduct = new CancellationTokenSource();
						return;
					}
					if (product.productsAttributes.Count > 0) {
						isInExistence = false;
						for (int j = 0; j < product.productsAttributes.Count; j++) {
							if (product.productsAttributes[j].Quantity > 0)
								isInExistence = true;
						}
					} else
						isInExistence = true;

					if (!isInExistence || product.productsDescription == null)
						continue;

					ProductInCategoryTemplate productItem = null;
					if (i <= grid.Children.Count - 1) {
						productItem = grid.Children[i] as ProductInCategoryTemplate;
						productItem.ShowProduct(product, isInExistence);
					} else {
						productItem = new ProductInCategoryTemplate();
						productItem.EventClick += ClickProduct;
						grid.Children.Add(productItem);


						productItem.ShowProduct(product, isInExistence);
						AddProduct(productItem, i, countColumn, grid);
					}
					i++;
				}
				IsCompleteShowGrid = true;
			});
		}
		#endif



		void AddProduct(ProductInCategoryTemplate productItem, int i, int countColumn, Grid grid)
		{
			Device.BeginInvokeOnMainThread((Action)(() => {
				grid.Children.Add(productItem, i % countColumn, i / countColumn);
			}));
		}

		ProductInCategoryTemplate CreateProduct(Product product, bool isInExistence)
		{	
			ProductInCategoryTemplate productItem = new ProductInCategoryTemplate();
			productItem.ShowProduct(product, isInExistence);
			productItem.EventClick += ClickProduct;
			return productItem;


//#if __ANDROID__
//				var activityManager = Android.App.Application.Context.GetSystemService(Android.App.Activity.ActivityService) as Android.App.ActivityManager;
//				Android.App.ActivityManager.MemoryInfo memoryInfo = new Android.App.ActivityManager.MemoryInfo();
//				activityManager.GetMemoryInfo(memoryInfo);
//				double TotalUsed = memoryInfo.AvailMem / (1024 * 1024);
//				double TotalRam = memoryInfo.TotalMem / (1024 * 1024);
//				bool lowMemory = memoryInfo.LowMemory;
//				Console.WriteLine("LowMemory = " + lowMemory + " TotalUsed = " + TotalUsed.ToString("f2") + "/" + TotalRam.ToString("f2"));
//				//Java.Lang.Runtime.GetRuntime().MaxMemory
//#endif
//#if __IOS__
//				double totalRam = Foundation.NSProcessInfo.ProcessInfo.PhysicalMemory / (1024 * 1024);
//				Console.WriteLine("Memory = " + totalRam.ToString("f2"));
//#endif
				//return productItem;

		}

		private void ClickPage(object s, EventArgs e)
		{
			Label lbl = s as Label;
			int numberPage = int.Parse (lbl.Text);

			//if (CancelTSProduct != null && !CancelTSProduct.IsCancellationRequested && !IsCompleteShowGrid) {
			//	if (!OnePage.mainPage.appRequest.AbortAll()) {
			//		CancelTSProduct.Cancel();
			//	}
			//}
			IsCompleteShowGrid = false;
			CurrentPage = numberPage;
			OnePage.redirectApp.ClickProductPage(numberPage);
			ShowProducts (Url);
		}
	}
}