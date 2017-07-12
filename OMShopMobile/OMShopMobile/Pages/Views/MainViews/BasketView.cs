using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.ComponentModel;

namespace OMShopMobile
{
	public class BasketView : ContentView
	{
		ActivityIndicator indicator;
		DeliveryView deliveryView;
		public Label lblTotal;
		public double total;

		int CurrentPage = 1;
		Pagination paginationBegin;
		Pagination paginationEnd;
		string Url;
		MyListView listProduct;
		StackLayout layoutProduct;
		StackLayout layoutProductZero;
		ScrollView scrollProduct;
		List<Basket> basketList;
		List<Basket> notValidBasketList;

		int countPage;
		int countProductPage = 10;

		event EventHandler eventRefresh;
		event EventHandler EventImageClick;

		const string login = "Войти";
		const string registration = "Зарегистрироваться";
//		public StackLayout mainLayout;
		public Grid mainGrid;

		public BasketView ()
		{
			Padding = new Thickness (0);
			VerticalOptions = LayoutOptions.FillAndExpand;

			listProduct = new MyListView {
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemTemplate = new DataTemplate (() => new OrderTemplate(EventImageClick)),
				BackgroundColor = Color.White,
				RowHeight = Utils.GetSize(107),
			};

			paginationBegin = new Pagination();
			paginationEnd = new Pagination();
			layoutProduct = new StackLayout {
				Padding = new Thickness(0, 8),
				Children = {
					paginationBegin,
					listProduct,
					paginationEnd
				}
			};
			scrollProduct = new ScrollView {
				Content = layoutProduct
			};

			Button btnGoToCatalog = new Button {
				BackgroundColor = Color.White,
				TextColor = ApplicationStyle.BlueColor,
				Text = "Перейти в каталог",
				FontSize = Utils.GetSize(14),
				HeightRequest = Utils.GetSize(20),
				FontAttributes = FontAttributes.Bold,
			};
			btnGoToCatalog.Clicked += OnGoToProduct;
			layoutProductZero = new StackLayout {
				Spacing = 0,
				Children = {
						new Label {
							Text = "В вашей корзине еще нет ни одного товара.",
							HorizontalOptions = LayoutOptions.Center,
							Margin = new Thickness(16)
						},
						btnGoToCatalog
					}
			};

			EventImageClick += OnImageClick;
			listProduct.ItemSelected += OnClickProduct;

			indicator = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			Content = indicator;
			ShowBasketAsync ();
		}

		void OnImageClick (object sender, EventArgs e)
		{
			OnePage.redirectApp.AddTransition (PageName.Image, "Просмотр изображения", false);
			TappedEventArgs eTapped = e as TappedEventArgs;
			Content = new ImageView ((string)eTapped.Parameter);
		}

		private void SetPagination(Pagination pagination)
		{
			pagination.CurrentPage = CurrentPage;
			pagination.CountPage = countPage;
			pagination.Show();
			pagination.tapGestureRecognizer.Tapped += ClickPage;
		}

		private async void ClickPage(object s, EventArgs e)
		{
			Label lbl = s as Label;
			int numberPage = int.Parse(lbl.Text);
			CurrentPage = numberPage;
			scrollProduct.Content = indicator;

			await Task.Delay(10);
			SetProductList();
		}

		void SetProductList()
		{
			if (countPage > 1) {
				SetPagination(paginationBegin);
				SetPagination(paginationEnd);
				paginationBegin.IsVisible = true;
				paginationEnd.IsVisible = true;
			} else {
				paginationBegin.IsVisible = false;
				paginationEnd.IsVisible = false;
			}
			listProduct.ItemsSource = basketList.Skip((CurrentPage-1) * countProductPage).Take(countProductPage).ToList();
			listProduct.VerticalOptions = LayoutOptions.Start;
			//Content = mainGrid;
			scrollProduct.Content = layoutProduct;
		}

		public async void ShowBasketAsync()
		{
			basketList = new List<Basket>();
			if (User.Singleton != null) {
				try {
					if (BasketDB.GetItems().Count > 0)
						await User.LoadBasket();
					basketList = await Basket.GetProductInBasketAsync(true);
				} catch (Exception) {
					eventRefresh = null;
					eventRefresh += (sender, e) => {
						Button content = sender as Button;
						content.IsEnabled = false;
						ShowBasketAsync();
					};
					Content = OnePage.mainPage.ShowMessageError(eventRefresh);
					return;
				}	

				try {/// Нужно для определения минимальной суммы заказа. Для этого узнать были ли сделаны до этого заказы
					User.Singleton.OrderStatusList = await OrderStatus.GetOrderStatusListAsync();
				} catch (Exception) {
					if (eventRefresh == null)
						eventRefresh += (sender, e) => {
							Button content = sender as Button;
							content.IsEnabled = false;
							ShowBasketAsync();
						};
					Content = OnePage.mainPage.ShowMessageError(eventRefresh);
					return;
				}
			}
			List<Basket> basketFromDB = BasketDB.GetItemsToBasketList();
			if (basketFromDB != null) {
				int[] idsProduct = basketFromDB.Select(g => g.ProductID).ToArray();
				List<Product> productsList;
				try {
					productsList = await Product.GetProductsByIDsListAsync(idsProduct);
				} catch (Exception) {
					if (eventRefresh == null)
						eventRefresh += (sender, e) => {
							Button content = sender as Button;
							content.IsEnabled = false;
							ShowBasketAsync();
						};
					Content = OnePage.mainPage.ShowMessageError(eventRefresh);
					return;
				}
				foreach (Basket basetBD in basketFromDB) {
					Product product = productsList.FirstOrDefault(g => g.ProductsID == basetBD.ProductID);
					if (product != null) {
						if (product.productsAttributes?.Count > 0) {
							ProductsAttributes productsAttribute = product.productsAttributes.FirstOrDefault(g => g.OptionValue.ID == basetBD.SizeValueId);
							if (productsAttribute != null)
								basetBD.ProductActualQuantity = productsAttribute.Quantity;
						}
						else
							basetBD.ProductActualQuantity = product.Quantity;
						basetBD.ProductsQuantityOrderMin = product.ProductsQuantityOrderMin;
						basetBD.ProductsQuantityOrderUnits = product.ProductsQuantityOrderUnits;
						basetBD.ProductAvailable = true;
					}
					else
						basetBD.ProductAvailable = false;
				}
				basketList.AddRange(basketFromDB);
			}

			if (basketList == null || basketList.Count == 0) {
				Content = layoutProductZero;
				return;
			}

			countPage = (int)Math.Ceiling((double)basketList.Count / countProductPage);
			SetProductList();
			total = 0;
			notValidBasketList = new List<Basket>();
			foreach (var item in basketList) {
				if (!item.ProductAvailable) { 
					notValidBasketList.Add(item);
					continue; 
				}

				if (item.Quantity > item.ProductActualQuantity) {
					if (!item.IsLocalBasket)
						notValidBasketList.Add(item);
					total += item.ProductActualQuantity * item.Price;
				} else
					total += item.Quantity * item .Price;
			}
			//total = basketList.Sum (g => Math.Ceiling(g.Price) * g.Quantity);

			lblTotal = new Label {  
				FontSize = 24,
				TextColor = ApplicationStyle.GreenColor,
				VerticalOptions = LayoutOptions.CenterAndExpand, 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold,
			};
			Button btnOrder = new Button {
				Text = "Оформить", 
				FontSize = Utils.GetSize(18),
				TextColor = Color.White,
				BackgroundColor = ApplicationStyle.RedColor,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BorderRadius = 0,
			};
			btnOrder.Clicked += ClickCheckOut;

			Grid gridBottom = new Grid {
				HeightRequest = Utils.GetSize(49),
				VerticalOptions = LayoutOptions.FillAndExpand,
				ColumnDefinitions = 
				{
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
				},
				BackgroundColor = Color.White,
			};

			MyPicker pickerAuthorization = new MyPicker { 
				TextColor = Color.Transparent, 
				BackgroundColor = Color.Transparent,
				Items = {
					login,
					registration,
					"Отмена"
				}
			};
			pickerAuthorization.Unfocused += OnLoginClick;

			Grid gridPicker = new Grid ();
			gridPicker.Children.Add (btnOrder, 0, 0);
			if (User.Singleton == null)
				gridPicker.Children.Add (pickerAuthorization, 0, 0);

			gridBottom.Children.Add (lblTotal, 0, 0);
			gridBottom.Children.Add (gridPicker, 1, 0);

			StackLayout layoutBottom = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0,
				Children = {
					new BoxView (),
					gridBottom
				}
			};

			mainGrid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 0,
				RowDefinitions = 
				{
					new RowDefinition { Height = new GridLength(100, GridUnitType.Star) },
					new RowDefinition { Height = Utils.GetSize(50) },
				}
			};
			mainGrid.Children.Add (scrollProduct, 0, 0);
			mainGrid.Children.Add(layoutBottom, 0, 1);
			Content = mainGrid;

			if (total > 0)
				lblTotal.Text = total + "р.";
		}

		void OnGoToProduct(object sender, EventArgs e)
		{ 
			OnePage.redirectApp.RedirectToPage(PageName.Catalog, false, false);
		}

		void OnLoginClick (object sender, FocusEventArgs e)
		{
			Picker picker = sender as Picker;
			if (picker.SelectedIndex == -1)
				return;

			string doUser = picker.Items [picker.SelectedIndex];
			switch (doUser) {
			case login:
				Task.Run (() => {
					Task.Delay(20);
					Device.BeginInvokeOnMainThread (() => {
						OnePage.redirectApp.RedirectToPage (PageName.Login, false, true);
					});
				});
				break;
			case registration:
				Task.Run (() => {
					Task.Delay (20);
					Device.BeginInvokeOnMainThread (() => {
						OnePage.redirectApp.RedirectToPage (PageName.Login, true, false);
						OnePage.redirectApp.loginView.GoToPersonalData (true);
					});
				});
				break;
			default:
				break;
			}
		}

		async void OnClickProduct (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) return;
			Basket basket = e.SelectedItem as Basket;
			if (!basket.ProductAvailable) {
				ListView entity = sender as ListView;
				entity.SelectedItem = null;
				return;
			}
			//await Task.Delay(500);
			Product product = await Product.GetProductsByIDAsync(basket.ProductID);
			OnePage.redirectApp.RedirectToPage (PageName.Catalog, true, true);
			OnePage.redirectApp.catalogView.GoToProductAsync (basket, product);
		}

		public async void ClickCheckOut (object sender, EventArgs e)
		{
			int countIsNotTime = 0;
			foreach (Basket basket in basketList) {
				if (!Schedule.IsTimeOrder(basket.SchedulesList))
					countIsNotTime++;
			}
			if (countIsNotTime > 0) {
				if (countIsNotTime == basketList.Count) {
					OnePage.mainPage.DisplayMessage("Все выбранные вами товары недоступны для заказа в данное время. Эти товары останутся в корзине и Вы всегда сможете дооформить их позже.");
					return;
				} else
					OnePage.mainPage.DisplayMessage("В корзине имеются товары которые нельзя заказать в данное время. Их можно будет заказать позже. После оформления заказа они останутся в корзине.");
			}

			await EditNotValidProduct();

			//bool isDel = await Basket.DeleteExtraProductToBasket (basketList, true);
			//if (isDel) {
			//	OnePage.redirectApp.RedirectToPage (PageName.Basket, true, true);
			//	return;
			//}
				
			if ((User.Singleton.OrderStatusList == null || !User.Singleton.OrderStatusList.Any (g => g.Count > 0)) && total < Constants.MinFirstSumBasket) {
				OnePage.mainPage.DisplayMessage ("Минимальная сумма покупки составляет " + Constants.MinFirstSumBasket + " руб.");
				return;
			}
			if (total < Constants.MinSumBasket) {
				OnePage.mainPage.DisplayMessage ("Минимальная сумма покупки составляет " + Constants.MinSumBasket + " руб.");
				return;
			}
			deliveryView = new DeliveryView (basketList, total);
			OnePage.redirectApp.AddTransition (PageName.Basket, "Оформление заказа");
			Content = deliveryView;
		}

		async Task EditNotValidProduct()
		{
			foreach (var item in notValidBasketList) {
				if ((item.Quantity > item.ProductActualQuantity || !item.ProductAvailable) && !item.IsLocalBasket) {
					if (!item.ProductAvailable)
						item.Quantity = -item.Quantity;
					else
						item.Quantity = item.ProductActualQuantity - item.Quantity;
					await Basket.PushToBasketAsync(item);
				}
			}
		}

		public void GoToOrder()
		{
			deliveryView.GoToResultView ();
			Content = deliveryView;
		}
	}
}