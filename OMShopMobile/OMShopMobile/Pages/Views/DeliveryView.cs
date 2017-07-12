using System;
using Xamarin.Forms;
using System.Collections.Generic;
using OMShopMobile.Pages;
using OMShopMobile.CustomControls;
using System.Linq;
using System.Threading.Tasks;

namespace OMShopMobile
{
	public class DataRadio
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string SourceImage { get; set; }
	}

	public class DeliveryView : ContentView
	{
		double Total { get; set; }
		bool isDataTrue = false;
		ScrollView scrollBody;
		StackLayout layoutBody;
		Button btnOrder;
		BindableRadioGroup radioList;
		ActivityIndicator indicator;
		StackLayout layoutMain;
		Grid mainGrid;
		OrderResultView orderResultView;
		MyEditor editorComment;
		MyEntry entCoupon;
		Label lblMessageCoupon;
		StackLayout layoutCouponInfo;
		Label lblCouponMeassage;
		ActivityIndicator indicatorCoupon;
		Label lblTotal;

		string selectDelivery;

		List<Delivery> deliveryList;

		event EventHandler eventRefresh;
		List<Basket> _basketList;

		double OldThisHeight = 0;

		public DeliveryView (List<Basket> basketList, double total)
		{
			Total = total;
			_basketList = basketList;
			VerticalOptions = LayoutOptions.FillAndExpand;

			Label lblSort = new Label {
				Text = "Способ доставки:",
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			StackLayout titleSortLayout = new StackLayout {
				BackgroundColor = ApplicationStyle.LineColor,
				HeightRequest = Utils.GetSize(22),
				Padding = new Thickness(8, 0),
				Children = {
					lblSort
				}	
			};

			radioList = new BindableRadioGroup {
				Spacing = 1,
				BackgroundColor = Color.White,
				TextColor = ApplicationStyle.TextColor,
				Padding = new Thickness(8, 0, 0, 0)
			};
			radioList.CheckedChanged += (sender, e) => {
				var radio = sender as CustomRadioButton;
				if (radio == null || radio.Id == -1) return;
				selectDelivery = radio.Text;
			};

			StackLayout bottomLayout = new StackLayout {
				BackgroundColor = ApplicationStyle.LineColor,
				HeightRequest = Utils.GetSize(22),
				Padding = new Thickness(8, 0),
				Children = {
				}	
			};

			Switch elSwitch = new Switch {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.EndAndExpand
			};
			elSwitch.Toggled += OnClickDataTrue;

			StackLayout layoutSwitch = new StackLayout {
				Padding = new Thickness(8, 0),
				Orientation = StackOrientation.Horizontal,
				HeightRequest = Utils.GetSize(43),
				Children = {
					new Label { 
						Text = "Данные указаны верно", 
						VerticalOptions = LayoutOptions.CenterAndExpand,
						TextColor = ApplicationStyle.TextColor,
					},
					elSwitch
				}
			};

			editorComment = new MyEditor {
				//VerticalOptions = LayoutOptions.FillAndExpand,
				TextColor = ApplicationStyle.TextColor,
				Placeholder = "Оставить комментарий к заказу",
				PlaceholderColor = ApplicationStyle.LabelColor,
				FontSize = Utils.GetSize(14),
				HeightRequest = 100
			};
			editorComment.Focused += OnTextFocused;
			editorComment.Unfocused += OnTextUnFocused;

			entCoupon = new MyEntry { 
				PlaceholderColor = ApplicationStyle.LabelColor,
				Placeholder = "Введите код на скидку", 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(0),
				Padding = new Thickness(0),
			};

			MyButton btnCoupon = new MyButton {
				BackgroundColor = ApplicationStyle.LineColor,
				TextColor = ApplicationStyle.LabelColor,
				UseWithTextBox = true,
				FontSize = 14,
				Text = "Применить", 
				WidthRequest = 114,
			};
			btnCoupon.Clicked += CouponClick;;

			StackLayout layoutBtnCoupon = new StackLayout {
				BackgroundColor = ApplicationStyle.LineColor,
				WidthRequest = 114,
				Margin = new Thickness(1)
			};

			StackLayout layoutCoupon = new StackLayout {
				HeightRequest = 30,
				Orientation = StackOrientation.Horizontal,
				Children = {
					entCoupon,
					btnCoupon
				}
			};
			CustomFrame frameCoupon = new CustomFrame {
				OutlineColor = ApplicationStyle.LineColor,
				OutlineWidth = 2,
				HeightRequest = 31,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HasShadow = false,
				Content = layoutCoupon,
				Padding = new Thickness(0),
				Margin = new Thickness(8, 18)
			};


			Label lblTitleMessageOrder = new Label {
				VerticalOptions = LayoutOptions.Center,
				TextColor = ApplicationStyle.TextColor,
				Text = "Сумма заказа"
			};
			Label lblMessageOrder = new Label {
				TextColor = ApplicationStyle.TextColor,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Text = Total + " р."
			};
			StackLayout layoutSumOrder = new StackLayout {
				VerticalOptions = LayoutOptions.Center,
				Orientation = StackOrientation.Horizontal,
				HeightRequest = Constants.HeightRowListView,
				Padding = new Thickness(8, 0),
				Children = {
					lblTitleMessageOrder,
					lblMessageOrder
				}
			};

			Label lblTitleMessageCoupon = new Label {
				VerticalOptions = LayoutOptions.Center,
				TextColor = ApplicationStyle.TextColor,
				Text = "Скидка по купону"
			};
			lblMessageCoupon = new Label {
				TextColor = ApplicationStyle.TextColor,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.Center,
			};
			StackLayout layoutSumCoupon = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HeightRequest = Constants.HeightRowListView,
				Padding = new Thickness(8, 0),
				Children = {
					lblTitleMessageCoupon,
					lblMessageCoupon
				}
			};

			lblCouponMeassage = new Label {
				TextColor = ApplicationStyle.RedColor,
				IsVisible = false
			};
			layoutCouponInfo = new StackLayout {
				Spacing = 0,
				IsVisible = false,
				Children = {
					new BoxView (),
					layoutSumOrder,
					new BoxView (),
					layoutSumCoupon,
					new BoxView (),
				}
			};
			indicatorCoupon = new ActivityIndicator { 
				Color = Device.OnPlatform(Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				IsVisible = false,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			StackLayout layoutCouponMain = new StackLayout { 
				Padding = new Thickness(8, 0),
				Children = { 
					lblCouponMeassage,
					layoutCouponInfo,
					indicatorCoupon
				}
			};

			layoutBody = new StackLayout { 
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0,
				Children = {
					titleSortLayout,
					radioList,
					bottomLayout,
					editorComment,
					new StackLayout {
						BackgroundColor = ApplicationStyle.LineColor,
						HeightRequest = Utils.GetSize(22),
						Padding = new Thickness (8, 0),
						Children = {
						}	
					},
					frameCoupon,
					layoutCouponMain,
					layoutSwitch,
				}	
			};
			double heightBot = Utils.GetSize(49);
			Grid gridBottom = new Grid {
				Padding = new Thickness(0),
				HeightRequest = heightBot,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = {
					new RowDefinition { Height = heightBot, }
				},
				RowSpacing = 0,
				ColumnDefinitions = 
				{
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
				},
				BackgroundColor = Color.White,
			};

			lblTotal = new Label { 
				FontSize = 24,
				TextColor = ApplicationStyle.GreenColor,
				VerticalOptions = LayoutOptions.CenterAndExpand, 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold,
				Text = Total + "р."
			};
			btnOrder = new Button {
				Text = "Оформить", 
				FontSize = Utils.GetSize(18),
				TextColor = Color.White,
				BackgroundColor = ApplicationStyle.LabelColor,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = heightBot,
				BorderRadius = 0,
			};
			btnOrder.Clicked += ClickCheckOut;

			gridBottom.Children.Add (lblTotal, 0, 0);
			gridBottom.Children.Add (btnOrder, 1, 0);

			StackLayout layoutBottom = new StackLayout {
				Spacing = 0,
				Children = {
					new BoxView { VerticalOptions = LayoutOptions.EndAndExpand },
					gridBottom
				}
			};

			scrollBody = new ScrollView { 
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = layoutBody
			};
			mainGrid = new Grid {
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = 
				{
					new RowDefinition { Height = new GridLength(100, GridUnitType.Star) },
					new RowDefinition { Height = Utils.GetSize(50), },
				},
				BackgroundColor = Color.Green
			};
			mainGrid.Children.Add (scrollBody, 0, 0);
			mainGrid.Children.Add (layoutBottom, 0, 1);

			layoutMain = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0,
				Children = {
					scrollBody,
					layoutBottom,
				}
			};
			indicator = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				IsVisible = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			Content = indicator;
			SetDeliveryAsync ();
		}

		void CouponClick(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(entCoupon.Text)) {
				OnePage.mainPage.DisplayMessage("Введите купон", "Предупреждение");
				return;
			}
			layoutCouponInfo.IsVisible = false;
			lblCouponMeassage.IsVisible = false;
			indicatorCoupon.IsVisible = true;
			ShowCoupone();
		}
		async void ShowCoupone()
		{ 
			ContentAndHeads<Coupon> coupon = await Coupon.GetProductsByIDAsync(entCoupon.Text);
			indicatorCoupon.IsVisible = false;
			if (coupon.ContentList != null) {
				layoutCouponInfo.IsVisible = true;
				lblMessageCoupon.Text = coupon.ContentList[0].CouponRedeemSum.ToString() + " р.";
				lblTotal.Text = coupon.ContentList[0].OrderSumTotal.ToString();
				await Task.Delay(10);
				await scrollBody.ScrollToAsync(layoutBody, ScrollToPosition.End, true);
			} else {
				lblTotal.Text = Total + " p.";
				lblCouponMeassage.IsVisible = true;
				lblCouponMeassage.Text = coupon.MessageError;
				//OnePage.mainPage.DisplayMessage(coupon.MessageError, "Предупреждение");
			}
		}

		async void SetDeliveryAsync()
		{
			try {
				deliveryList = await Delivery.GetDeliveryList ();
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (sender, e) => { 
					Button content = sender as Button;
					content.IsEnabled = false;
					SetDeliveryAsync();
				};
				Content = OnePage.mainPage.ShowMessageError (eventRefresh);
				return;
			}
			string[] strDelivery = deliveryList.Select (g => g.Name).ToArray();
			radioList.ItemsSource = strDelivery;
//			Content = mainGrid;
			Content = layoutMain;
		}

		void OnTextFocused (object sender, EventArgs e)
		{
			this.VerticalOptions = LayoutOptions.Start;
			if (OldThisHeight == 0)
				OldThisHeight = this.Height;
			this.HeightRequest = this.Height - this.Height * 0.43;
		}

		void OnTextUnFocused (object sender, EventArgs e)
		{
			this.VerticalOptions = LayoutOptions.FillAndExpand;
			this.HeightRequest = OldThisHeight;
		}

		void OnClickDataTrue (object sender, ToggledEventArgs e)
		{
			if (e.Value) {
				isDataTrue = true;
				btnOrder.BackgroundColor = ApplicationStyle.RedColor;
			} else {
				isDataTrue = false;
				btnOrder.BackgroundColor = ApplicationStyle.LabelColor;
			}
		}

		async void ClickCheckOut (object sender, EventArgs e)
		{
			if (!isDataTrue) {
				OnePage.mainPage.DisplayMessage ("Согласитесь с \"Данные указаны верно\"");
				return;
			}
			if (string.IsNullOrEmpty (selectDelivery)) {
				OnePage.mainPage.DisplayMessage ("Выберете способ доставки");
				return;
			}
			//bool isDel = await Basket.DeleteExtraProductToBasket (_basketList, true);
			//if (isDel) {
			//	OnePage.redirectApp.RedirectToPage (PageName.Basket, true, true);
			//	return;
			//}
				
			int deliveryId = deliveryList.SingleOrDefault (g => g.Name == selectDelivery).Id;
			Content = indicator;
			Order order;
			try {
				order = await Order.OrderFormBasket (deliveryId, editorComment.Text, entCoupon.Text);
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (obj, evn) => { 
					Button content = sender as Button;
					content.IsEnabled = false;
					ClickCheckOut (sender, e);
				};
				Content = OnePage.mainPage.ShowMessageError (eventRefresh);
				return;
			}
			if (order != null) {
				User.Singleton.BasketList.Clear();
				OnePage.redirectApp.CrearHistory ();
				OnePage.redirectApp.AddTransition (PageName.Order, "Оформление заказа");
				orderResultView = new OrderResultView (order, false);
				Content = orderResultView;
			} else
				ErrorOrder (sender, e);
		}

		void ErrorOrder(object sender, EventArgs e)
		{
			eventRefresh = null;
			eventRefresh += (obj, evn) => { 
				Button content = sender as Button;
				content.IsEnabled = false;
				ClickCheckOut (sender, e);
			};
			Content = OnePage.mainPage.ShowMessageError (eventRefresh);
			return;
		}

		public void GoToResultView()
		{
			orderResultView.GoToMain ();
			Content = orderResultView;
		}
	}
}

