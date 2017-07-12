using System;
using Xamarin.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace OMShopMobile
{
	public class InfoOrder 
	{
		public InfoBasketPage Page { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public enum InfoBasketPage : short
	{
		Empty = 0,
		Delivery = 1,
		Address = 2,
		Product = 3
	}

	public class OrderResultView : ContentView
	{
		ListView infoListView;
		Order OrderResult;
		StackLayout layoutList;
		ActivityIndicator indicator;
		StackLayout layoutMain;
		Label lblInfo;
		StackLayout layoutLabel;
		ScrollView scrollProduct;
		ListView listProduct;
		StackLayout layoutProduct;

		event EventHandler eventRefresh;
		event EventHandler EventImageClick;
		List<InfoOrder> infoOrderList;

		int CurrentPage = 1;
		Pagination paginationBegin;
		Pagination paginationEnd;

		string Url;

		public OrderResultView (Order order, bool isProduct)
		{
			VerticalOptions = LayoutOptions.FillAndExpand;

			OrderResult = order;

			infoOrderList = new List<InfoOrder> () { 
				new InfoOrder { Page = InfoBasketPage.Delivery, Name = "Доставка до ТК ПЭК" },
				new InfoOrder { Page = InfoBasketPage.Address, Name = "Адрес доставки" },
				new InfoOrder { Page = InfoBasketPage.Empty, Name = "Телефон", Value = order.Phone },
				new InfoOrder { Page = InfoBasketPage.Empty, Name = "Сумма заказа", Value = order.Sum.ToString() + "р." },
			};
			if (!order.SumCoupon.Equals(0)) {
				infoOrderList.Add(new InfoOrder { Page = InfoBasketPage.Empty, Name = "Скидка по купону", Value = order.SumCoupon.ToString() + "р." });
			}
			infoOrderList.Add(new InfoOrder { Page = InfoBasketPage.Empty, Name = "Статус заказа", Value = order.StatusString });

			CultureInfo ci = new CultureInfo ("ru-ru");

			Label lblOrderName = new Label {
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.Center,
				FontAttributes = FontAttributes.Bold,
				TextColor = ApplicationStyle.TextColor,
				FontSize = Utils.GetSize(17),
				Text = "Заказ № " + order.Number + "\r от " + order.Date.ToString ("dd MMMMM  yyyy", ci),
			};

			StackLayout layoutName = new StackLayout { 
				Padding = new Thickness(26),
				Children = { lblOrderName }
			};

			infoListView = new ListView {
				ItemTemplate = new DataTemplate (typeof(OrderCellTemplate)),
				ItemsSource = infoOrderList,
				VerticalOptions = LayoutOptions.Start,
			};
			infoListView.ItemSelected += OnSelectInfo;

			Label lblTotal = new Label {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				FontAttributes = FontAttributes.Bold,
				TextColor = ApplicationStyle.TextColor,
				Text = string.Format("Итого: {0}р.", order.Sum - order.SumCoupon + order.DeliveryForCustomer?.Value ?? 0),
			};

			StackLayout layoutTotal = new StackLayout { 
				Padding = new Thickness (0, 14, 16, 34),
				Children = {
					lblTotal
				}
			};

			Label lblMessage = new Label { 
				HorizontalOptions = LayoutOptions.Center,
				TextColor = ApplicationStyle.GreenColor,
				FontAttributes = FontAttributes.Bold,
				FontSize = Utils.GetSize(17),
				Text = "Спасибо за заказ!"
			};

			layoutList = new StackLayout { 
				Spacing = 0,
				Children = {
					layoutName,
					new BoxView(),
					infoListView,
					layoutTotal,
					lblMessage
				}
			};

			if (isProduct) {
				infoOrderList.Add(new InfoOrder { Page = InfoBasketPage.Product, Name = "Состав заказа" });
				OnePage.topView.leftLayout.IsVisible = true;
				lblMessage.IsVisible = false;
			} else {
				OnePage.topView.leftLayout.IsVisible = false;
				lblMessage.IsVisible = true;
			}
			
			lblInfo = new Label {
				TextColor = ApplicationStyle.TextColor
			};
			layoutLabel = new StackLayout {
				Padding = new Thickness(8, 20),
				IsVisible = false,
				Children = {
					lblInfo,
				}
			};

			paginationBegin = new Pagination();
			paginationEnd = new Pagination();
			listProduct = new ListView {
				VerticalOptions = LayoutOptions.Start,
				ItemTemplate = new DataTemplate(() => new OrderTemplate(EventImageClick)),
				BackgroundColor = Color.White,
				RowHeight = Utils.GetSize(107),
			};
			listProduct.ItemSelected += (sender, e) => { listProduct.SelectedItem = false; };

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

			layoutMain = new StackLayout { 
				Children = {
					layoutLabel,
					layoutList	
				}
			};

			Content = layoutMain;
		}

		void OnSelectInfo (object sender, SelectedItemChangedEventArgs e)
		{
			indicator = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				IsVisible = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			ListView listView = sender as ListView;
			listView.SelectedItem = null;
			InfoOrder infoOrder = e.SelectedItem as InfoOrder;
			if (infoOrder != null && infoOrder.Page != InfoBasketPage.Empty) {
				switch (infoOrder.Page) {
				case InfoBasketPage.Delivery:
					lblInfo.Text = OrderResult.DeliveryForCustomer?.Title;
					OnePage.redirectApp.AddTransition (PageName.Order, "Доставка");
					break;
				case InfoBasketPage.Address:
					lblInfo.Text = OrderResult.Country + ", " +
						OrderResult.Zone + ", " +
						OrderResult.City + ", " +
						OrderResult.Street;

					OnePage.redirectApp.AddTransition (PageName.Order, "Адрес");
					break;
				case InfoBasketPage.Product:
					OnePage.redirectApp.AddTransition (PageName.Login, "Состав заказа", HistoryStep.OrdersList);
					Content = indicator;
					string url = OrderPosition.GetUrl(OrderResult.Id);
					ShowProducts(url);
					break;
				default:
					break;
				}
				layoutList.IsVisible = false;
				layoutLabel.IsVisible = true;
//				Content = layout;
			} else {
//				infoListView.SelectedItem = null;
			}
		}

		public void ShowResult()
		{
			Content = layoutMain;
		}

		public void ShowOrdersList()
		{
			Content = listProduct;
		}

		private void SetPagination(Pagination pagination, ContentAndHeads<OrderPosition> contentAndHeads)
		{
			pagination.CurrentPage = contentAndHeads.currentPage;
			pagination.CountPage = contentAndHeads.countPage;
			pagination.Show();
			pagination.tapGestureRecognizer.Tapped += ClickPage;
		}

		private void ClickPage(object s, EventArgs e)
		{
			Label lbl = s as Label;
			int numberPage = int.Parse(lbl.Text);
			CurrentPage = numberPage;
			Content = indicator;

			ShowProducts(Url);
		}

		async void ShowProducts(string url)
		{
			Url = url;
			ContentAndHeads<OrderPosition> contentList;
			try {
				contentList = await OrderPosition.GetOrderPositionAsync (url, CurrentPage, XPagination.CountProduct);
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (sender, e) => { 
					Button content = sender as Button;
					content.IsEnabled = false;
					ShowProducts(url);
				};
				Content = OnePage.mainPage.ShowMessageError (eventRefresh);
				return;
			}
			if (contentList == null || contentList.ContentList == null || contentList.ContentList.Count == 0) {
				Content = new Label { Margin = new Thickness(8), Text = "Количество товаров 0" };
				return;
			}

			if (contentList.countPage > 1) {
				SetPagination(paginationBegin, contentList);
				SetPagination(paginationEnd, contentList);
				paginationBegin.IsVisible = true;
				paginationEnd.IsVisible = true;
			} else {
				paginationBegin.IsVisible = false;
				paginationEnd.IsVisible = false;
			}

			listProduct.ItemsSource = OrderListToBasketList(contentList.ContentList);;
			EventImageClick += OnImageClick;
			listProduct.HeightRequest = listProduct.RowHeight * contentList.ContentList.Count;
			Content = scrollProduct;
		}

		List<Basket> OrderListToBasketList(List<OrderPosition> orderList)
		{
			List<Basket> basketList = new List<Basket>();  
			foreach (var order in orderList) {
				basketList.Add(new Basket {
					Article = order.Article,
					ProductName = order.Name,
					Price = order.Price,
					SizeName = order.Size,
					Quantity = order.Quantity,
					ProductImage = order.Img,
					IsHistoryProduct = true
				});
			}
			return basketList;
		}

		public void GoToMain()
		{
			OnePage.topView.leftLayout.IsVisible = false;
			layoutList.IsVisible = true;
			layoutLabel.IsVisible = false;
//			Content = mainLayout;
		}

		void OnImageClick (object sender, EventArgs e)
		{
			OnePage.redirectApp.AddTransition (PageName.Image, "Просмотр изображения", false);
			TappedEventArgs eTapped = e as TappedEventArgs;
			Content = new ImageView ((string)eTapped.Parameter);
		}
	}
}