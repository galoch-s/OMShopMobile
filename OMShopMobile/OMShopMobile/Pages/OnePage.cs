using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace OMShopMobile
{
	public class OnePage : ContentPage
	{
		public static TopView topView;
		public static OnePage mainPage;
		public static BottomView bottomView;
		public static RedirectApp redirectApp;
		public static List<Product> productNew1List;
		public static List<Product> productNew2List;
		public static List<Category> categoryList;
		public static List<Banner> bannerList;

		public static StackLayout bodyLayout;

		private ActivityIndicator indication;
		private MainPageView mainView;
		public StackLayout mainlayout;
		public AppRequest appRequest = new AppRequest();

		event EventHandler eventRefresh;

		public OnePage ()
		{
			float totalMemory = DependencyService.Get<IApplicationState>().GetMemory();
			//if (totalMemory < 1100 && totalMemory > 600)
			//	XPagination.CountProduct = 30;
			//else if (totalMemory < 600)
			//	XPagination.CountProduct = 40;

			if (totalMemory < 600)
				XPagination.CountProduct = 20;
			else
				XPagination.CountProduct = 30;
			
			Initialization ();
		}

		void Initialization()
		{
			if (!ValidInternetConnection ())
				return;
			
			mainPage = this;
			SetGlobalStyle ();

			indication = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			Content = indication;
			Show ();
		}

		private async void Show()
		{
			//string email = "kirillstarukhin@mail.ru";
			//string hash = "MDQ1NDAwYzkyMTAyYjZlNDhmZGU2M2E4M2Y4MDk3NzE6N2Y=";
			#if DEBUG 
				//await User.AuthorizationUserToKeyAsync(email, hash);
			#endif
			if (User.Singleton == null)
				await User.AuthorizationUserAsync ();

			await BasketDB.UpdateInfoBasketList();

			redirectApp = new RedirectApp ();
			topView = new TopView ();
			bottomView = new BottomView ();

			bodyLayout = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			OnePage.redirectApp.searchView = new SearchView ();
			mainlayout = new StackLayout { 
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0,
				Children = {
					topView,
					bodyLayout,
					bottomView
				}
			};
			bool isChildren = true;
			try {
				categoryList = await Category.GetCategoriesByIDAsync(0, isChildren);
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (obj, evn) => {
					Button content = obj as Button;
					content.IsEnabled = false;
					Show();
				};
				Content = OnePage.mainPage.ShowMessageError(eventRefresh);
				return;
			}
			Content = mainlayout;
			redirectApp.RedirectToPage (PageName.Main, false, false);
		}

		public bool ValidInternetConnection ()
		{
			DependencyService.Get<INetworkConnection> ().CheckNetworkConnection ();
			if (!DependencyService.Get<INetworkConnection> ().IsConnected) {
				BackgroundColor = Color.White;
				Button btnRefresh = new Button {
					HorizontalOptions = LayoutOptions.Center,
					BackgroundColor = Color.White,
					TextColor = ApplicationStyle.GreenColor,
					Text = "Обновить",
				};
				btnRefresh.Clicked += OnRefresh;
				Content = new StackLayout { 
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					Children = {
						new Label {
							TextColor = ApplicationStyle.TextColor,
							BackgroundColor = Color.White,
							Text = "Отсутствует соединение с интернетом",
							FontAttributes = FontAttributes.Bold,
							HorizontalTextAlignment = TextAlignment.Center,
							VerticalTextAlignment = TextAlignment.Center
						},
						btnRefresh
					}
				};
				return false;
			}
			return true;
		}

		public StackLayout ShowMessageError(EventHandler evClick = null)
		{
			BackgroundColor = Color.White;
			Button btnRefresh = new Button {
				HorizontalOptions = LayoutOptions.Center,
				BackgroundColor = Color.White,
				TextColor = ApplicationStyle.GreenColor,
				Text = "Обновить"
			};

			btnRefresh.Clicked += evClick;
			return new StackLayout { 
				BackgroundColor = Color.White,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new MyLabel {
						TextColor = ApplicationStyle.LabelColor,
						BackgroundColor = Color.White,
						//Text = "Не удалось загрузить данные",
						Text = "На сервере ведутся технические работы.\r\n Попробуйте повторить позже",
						FontAttributes = FontAttributes.Bold,
						HorizontalTextAlignment = TextAlignment.Center,
						VerticalTextAlignment = TextAlignment.Center,
						LineBreakMode = LineBreakMode.CharacterWrap,
						LineSpacing = 1.5f,
					},
					//btnRefresh
				}
			};
		}

		void OnRefresh (object sender, EventArgs e)
		{
			Initialization ();
		}

		private void SetGlobalStyle()
		{
			BackgroundColor = Color.White;

			ApplicationStyle.SetGlobalStyle ();
		}

		public void DisplayMessage(string message, string title = "Сообщение")
		{
			DisplayAlert (title, message, "ok");
		}

		public Task<bool> DisplayMessageQuestion(string title, string message, string buttonOk = "да", string buttonCancel = "нет")
		{
			Task<bool> isOk = DisplayAlert (title, message, buttonOk, buttonCancel);
			return isOk;
		}

		protected override bool OnBackButtonPressed ()
		{
			if (redirectApp != null)
				return redirectApp.BackToHistory ();
			else
				return base.OnBackButtonPressed ();
		}

//		protected override void OnAppearing ()
//		{
//			base.OnAppearing ();
//		}
//
//		protected override void OnBindingContextChanged ()
//		{
//			base.OnBindingContextChanged ();
//		}
//
//		protected override void OnChildMeasureInvalidated (object sender, EventArgs e)
//		{
//			base.OnChildMeasureInvalidated (sender, e);
//		}
//
//		protected override void OnDisappearing ()
//		{
//			base.OnDisappearing ();
//		}
//
//		protected override void OnParentSet ()
//		{
//			base.OnParentSet ();
//		}
//
//		protected override void OnSizeAllocated (double width, double height)
//		{
//			base.OnSizeAllocated (width, height);
//		}

//		protected override void OnSizeAllocated (double width, double height)
//		{	
//			base.OnSizeAllocated (width, height);
//
//			var ff = DeviceInfo.IsOrientationPortrait ();
//
//			if (DeviceInfo.IsOrientationPortrait () && width > height || !DeviceInfo.IsOrientationPortrait () && width < height) {
////				OnePage.redirectApp.catalogView.EditOrientation (width, height);
//				if (OnePage.topView != null)
//					OnePage.topView.ResizeGrid();
//			}
//		}
	}
}