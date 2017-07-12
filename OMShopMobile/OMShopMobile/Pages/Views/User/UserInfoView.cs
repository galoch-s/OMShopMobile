using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace OMShopMobile
{
//	public class StatusOrder
//	{
//		public int Id { get; set; }
//		public string Name { get; set; }
//		public string Icon { get; set; }
//		public int Count { get; set; }
//	}

	public class UserInfoView : ContentView
	{
		public event EventHandler EventExitUser;
		public event EventHandler EventPersonalData;

		ListView listViewStatus;
		ScrollView scrollBody;
		OrderResultView orderResultView;
		ActivityIndicator indicator;

		Label lblFio;
		Label lblMail;

		List<OrderStatus> OrderStatusList;
		Grid gridStatusOrder;
		StackLayout layoutMain;

		public OrderStatus Status { get; set; }
		Order order;


		public UserInfoView ()
		{
			OrderStatusList = User.Singleton.OrderStatusList;
			indicator = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				IsVisible = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
			Content = indicator;

			StackLayout layoutInfo = GetLayoutInfo ();
			TapGestureRecognizer tapInfo = new TapGestureRecognizer ();
			layoutInfo.GestureRecognizers.Add (tapInfo);
			tapInfo.Tapped += OnClickInfo;

			StackLayout lineOrderLayout = new StackLayout {
				BackgroundColor = ApplicationStyle.LineColor,
				HeightRequest = Utils.GetSize(22),
			};

			CellTemplate orderCell = new CellTemplate ("Мои заказы") {
				HeightRequest = Utils.GetSize(43)
			};
			TapGestureRecognizer tapOrdersList = new TapGestureRecognizer ();
			tapOrdersList.Tapped += OnSelectMyOrder;
			orderCell.GestureRecognizers.Add (tapOrdersList);

			StackLayout lineStatusOrderLayout = new StackLayout {
				BackgroundColor = ApplicationStyle.LineColor,
				HeightRequest = Utils.GetSize(22),
			};

			gridStatusOrder = new Grid {
				HeightRequest = Utils.GetSize(200),
				RowSpacing = 0,
				ColumnSpacing = 0,
				BackgroundColor = ApplicationStyle.LineColor,
				RowDefinitions =  {
					new RowDefinition { Height = new GridLength (33, GridUnitType.Star)	},
					new RowDefinition { Height = Utils.GetSize(0.6)	},
					new RowDefinition {	Height = new GridLength (33, GridUnitType.Star) },
					new RowDefinition { Height = Utils.GetSize(0.6) },
					new RowDefinition { Height = new GridLength (33, GridUnitType.Star) },
				},
				ColumnDefinitions =  {
					new ColumnDefinition { Width = new GridLength (50, GridUnitType.Star) },
					new ColumnDefinition { Width = Utils.GetSize(0.6) },
					new ColumnDefinition { Width = new GridLength (50, GridUnitType.Star) }
				}
			};


			listViewStatus = new ListView { 
				ItemTemplate = new DataTemplate(typeof(ListCellTemplate)),
				VerticalOptions = LayoutOptions.Start,
//				ItemsSource = OrderStatusList,
				IsVisible = false,
			};
			listViewStatus.ItemSelected += ListViewStatusClick;

			SetOrderStatus ();

			CellTemplate orderStatusCell = new CellTemplate ("Статус заказов") {
				HeightRequest = Utils.GetSize(43),
			};
			TapGestureRecognizer tapOrderStatusList = new TapGestureRecognizer ();
			tapOrderStatusList.Tapped += OnSelectMyOrderStatus;
			orderStatusCell.GestureRecognizers.Add (tapOrderStatusList);


			StackLayout layoutBody = new StackLayout {
				Spacing = 0,
				Children = {
					layoutInfo,
					lineOrderLayout,
					orderCell,
					lineStatusOrderLayout,
					gridStatusOrder,
					new BoxView (),
					orderStatusCell,
					new BoxView (),
					listViewStatus,
				}
			};

			scrollBody = new ScrollView { 
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = layoutBody,
			};

			Button btnExit = new Button { 
				BackgroundColor = Color.White,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = Utils.GetSize(42),
				TextColor = ApplicationStyle.GreenColor,
				FontSize = Utils.GetSize(17),
				Text = "Выйти" 
			};
			btnExit.Clicked += (sender, e) => {
				if (EventExitUser != null)
					EventExitUser(sender, e);
			};

			StackLayout layoutBottom = new StackLayout {
				Spacing = 0,
				HeightRequest = Utils.GetSize(43),
				VerticalOptions = LayoutOptions.End,
				Children = {
					new BoxView(),
					btnExit
				}
			};
			layoutMain = new StackLayout { 
				Spacing = 0,
				Children = {
					scrollBody,
					layoutBottom
				}
			};
//			Content = layoutMain;
//			SetLayoutInfo ();
		}

		public void SetLayoutInfo()
		{
			lblFio.Text = User.Singleton.Address.Firstname + " " + User.Singleton.Address.Lastname;
			lblMail.Text = User.Singleton.Email;
		}
			
		public void SetMain()
		{
			Content = indicator;
			gridStatusOrder.Children.Clear ();
			SetOrderStatus ();
		}
		StackLayout GetLayoutInfo ()
		{
			Image imgAvatar = new Image {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = Device.OnPlatform("Login/avatar_.png", "Kabinet_avatar.png", "Kabinet_avatar.png"),
				HeightRequest = Utils.GetSize(72)
			};
			lblFio = new Label {
				TextColor = ApplicationStyle.TextColor,
				FontSize = Utils.GetSize(17),
//				Text = User.Singleton.Address.Firstname + " " + User.Singleton.Address.Lastname,
			};
			lblMail = new Label {
//				Text = User.Singleton.Email
			};
			StackLayout layoutTextInfo = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children =  {
					lblFio,
					lblMail
				}
			};
			StackLayout layoutInfo = new StackLayout {
				HeightRequest = Utils.GetSize(106),
				Padding = new Thickness (Utils.GetSize(8), 0),
				Orientation = StackOrientation.Horizontal,
				Children =  {
					imgAvatar,
					layoutTextInfo
				}
			};
			return layoutInfo;
		}

		async void SetOrderStatus()
		{
			List<OrderStatus> OrderStatusList = await OrderStatus.GetOrderStatusListAsync ();
			SetLayoutStatus (OrderStatusList);
			listViewStatus.ItemsSource = OrderStatusList;
			Content = layoutMain;
		}

		void SetLayoutStatus (List<OrderStatus> orderStatus)
		{
			List<OrderStatus> arrImage = orderStatus.Where (g => g.Id == 1 || g.Id == 2 || g.Id == 3 || g.Id == 4 || g.Id == 5 || g.Id == 11).ToList<OrderStatus>();
			OrderStatus orderStatus1 = arrImage.Single (g => g.Id == 1);
			orderStatus1.Icon = Device.OnPlatform("Login/clock_lk_.png", "clock_lk.png", "clock_lk.png");
			orderStatus1.Index = 1;

			OrderStatus orderStatus2 = arrImage.Single (g => g.Id == 2);
			orderStatus2.Icon = Device.OnPlatform("Login/pay_lk_.png", "money.png", "money.png");
			orderStatus2.Index = 2;

			OrderStatus orderStatus3 = arrImage.Single(g => g.Id == 3);
			orderStatus3.Icon = Device.OnPlatform("Login/dostavka_lk_.png", "oplachen.png", "oplachen.png");
			orderStatus3.Index = 3;

			OrderStatus orderStatus4 = arrImage.Single (g => g.Id == 4);
			orderStatus4.Icon = Device.OnPlatform("Login/dostavka_lk_.png", "dostavka.png", "dostavka.png");
			orderStatus4.Index = 4;

			OrderStatus orderStatus5 = arrImage.Single(g => g.Id == 5);
			orderStatus5.Icon = Device.OnPlatform("Login/dostavka_lk_.png", "Dostavlen.png", "Dostavlen.png");
			orderStatus5.Index = 5;


			OrderStatus orderStatus11 = arrImage.Single(g => g.Id == 11);
			orderStatus11.Icon = Device.OnPlatform("Login/sborka_lk_.png", "sborka.png", "sborka.png");
			orderStatus11.Index = 11;

			arrImage = arrImage.OrderBy (g => g.Index).ToList<OrderStatus>();


//			Grid gridStatusOrder = new Grid {
//				HeightRequest = 170,
//				RowSpacing = 1,
//				ColumnSpacing = 1,
//				BackgroundColor = ApplicationStyle.LineColor,
//				RowDefinitions =  {
//					new RowDefinition { Height = new GridLength (50, GridUnitType.Star)	},
//					new RowDefinition {	Height = new GridLength (50, GridUnitType.Star) },
//				},
//				ColumnDefinitions =  {
//					new ColumnDefinition { Width = new GridLength (50, GridUnitType.Star) },
//					new ColumnDefinition { Width = new GridLength (50, GridUnitType.Star) }
//				}
//			};

			for (int i = 0; i < arrImage.Count; i++) {
				Image img = new Image { 
					Source = arrImage[i].Icon,
					HeightRequest = Utils.GetSize(36),
				};
				Label lblCount = new Label { 
					VerticalOptions = LayoutOptions.CenterAndExpand,
					TextColor = ApplicationStyle.GreenColor,
					Text = arrImage[i].Count.ToString(),
				};
				StackLayout layoutHorizontal = new StackLayout { 
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					Orientation = StackOrientation.Horizontal,
					Children = {
						img,
						lblCount
					}
				};
				Label lblName = new Label {
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					TextColor = ApplicationStyle.TextColor,
					Text = arrImage[i].Name,
				};
				StackLayout layoutVertical = new StackLayout { 
					//Padding = new Thickness(0, 14, 0, 0),
					Padding = new Thickness(0),
					BackgroundColor = Color.White,
					Children = {
						layoutHorizontal,
						lblName
					}
				};
				//int left = i % 2 < 1 ? (i % 2) : (i % 2 + 1);
				//int top = i / 2 < 1 ? (i / 2) : (i / 2 + 1);
				int left = i % 2 < 1 ? (i % 2) : (i % 2 + 1);
				int top = (i / 2) * 2;
				TapGestureRecognizer tapStatus = new TapGestureRecognizer ();
				tapStatus.Tapped += OnSelectBtnStatusOrder;
				tapStatus.CommandParameter = arrImage [i];
				layoutVertical.GestureRecognizers.Add (tapStatus);
 				gridStatusOrder.Children.Add (layoutVertical,   left,   top );
			}
			gridStatusOrder.Children.Add ( new BoxView (), 1, 0);
			gridStatusOrder.Children.Add ( new BoxView (), 1, 2);
			gridStatusOrder.Children.Add ( new BoxView (), 1, 4);

			gridStatusOrder.Children.Add ( new BoxView (), 0, 1);
			gridStatusOrder.Children.Add ( new BoxView (), 2, 1);
			gridStatusOrder.Children.Add ( new BoxView (), 0, 3);
			gridStatusOrder.Children.Add ( new BoxView (), 2, 3);
		}

		void OnClickInfo (object sender, EventArgs e)
		{
			if (EventPersonalData != null)
				EventPersonalData (this, null);
		}

		async void OnSelectMyOrderStatus (object sender, EventArgs e)
		{
			CellTemplate orderStatusCell = sender as CellTemplate;
//			CellTemplate Content = sender as CellTemplate;
//			Content.BackgroundColor = ApplicationStyle.LineColor;
//			if (Content.SelectedItem != null) {
			if (listViewStatus.IsVisible) {
				await scrollBody.ScrollToAsync (0, 0, true);
				listViewStatus.IsVisible = false;
				orderStatusCell.SetLeft ();

			} else {
				listViewStatus.IsVisible = true;
				await Task.Delay(10);
				await scrollBody.ScrollToAsync (listViewStatus, ScrollToPosition.End, true);
				orderStatusCell.SetDown ();
			}
//			}
//			Content.SelectedItem = null;
		}

		public void GotoOrdersList(bool isHistory)
		{
			if (!isHistory) OnePage.redirectApp.AddTransition (PageName.Login, "Мои заказы", HistoryStep.MyOrders);
			OrdersListView ordersListTemplate = new OrdersListView ();
			ordersListTemplate.eventResult += OnTapBodyLabel;
			Content = ordersListTemplate;
		}

		void OnTapBodyLabel (object sender, EventArgs e)
		{
			order = sender as Order;
			GoToOrderResultView (false);
		}

		public void GoToOrderResultView(bool isHistory)
		{
			if (!isHistory) OnePage.redirectApp.AddTransition (PageName.Login, "Информация о заказе", HistoryStep.InfoOrder);
			orderResultView = new OrderResultView (order, true);
			Content = orderResultView;
		}

		void OnSelectMyOrder (object sender, EventArgs e)
		{
			GotoOrdersList (false);
		}

		public void GotoStatusList(bool isHistory)
		{
			if (!isHistory) OnePage.redirectApp.AddTransition (PageName.Login, "Статус: " + Status.Name, HistoryStep.StatusOrders);
			OrdersListView ordersListTemplate = new OrdersListView (Status.Id);
			ordersListTemplate.eventResult += OnTapBodyLabel;
			Content = ordersListTemplate;
		}

		public void GoToOrdersList()
		{
			orderResultView.ShowOrdersList ();
			Content = orderResultView;
		}

		void OnSelectBtnStatusOrder (object sender, EventArgs e)
		{
			TappedEventArgs eTapped = e as TappedEventArgs;
			Status = (OrderStatus)eTapped.Parameter;
			GotoStatusList (false);
		}

		void ListViewStatusClick (object sender, SelectedItemChangedEventArgs e)
		{
			OrderStatus content = e.SelectedItem as OrderStatus;
			Status = content;
			GotoStatusList (false);
		}
	}
}