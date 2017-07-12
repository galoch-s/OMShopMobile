using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace OMShopMobile
{
	public class TopView : StackLayout
	{	
		//public GridBtnLayuot btnBasket;
		Image imgArrow;
		Image imgArrowGreen;
		public StackLayout leftLayout;
		StackLayout centerLayout;
		private StackLayout rightLayout;
		Image imgLogo;
		public Label lblAction;
		public StackLayout layoutImgFilter;
		public Label lblPagePosition;
		Grid grid;
		Grid gridLogin;

		Label lblCountProduct;

		public MySearchBar searchBar;
		StackLayout layoutSearch;

		public event EventHandler ClickEvent;

		public event EventHandler<TextChangedEventArgs> SearchProduct;

		public TopView ()
		{
			int bodyHeight = Constants.HeightTopMain - 1;

			Padding = new Thickness (0, Device.OnPlatform (20, 0, 0), 0, 0);
			this.Spacing = 0;
			OnePage.redirectApp.EventRedirectPage += OnRedirectPage;

			imgArrow = new Image { 
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.Start,
				Source = Device.OnPlatform("Header/arrow_top_.png", "arrow_back_.png", "arrow_back_.png"),
			};
			imgArrowGreen = new Image { 
				VerticalOptions =LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.Start,
				Source = Device.OnPlatform("Header/arrow_green_.png", "arrow_back_green_.png", "arrow_back_green_.png"),
				IsVisible = false
			};

			StackLayout layoutArrow = new StackLayout {
				Padding = new Thickness(8),
				WidthRequest = Utils.GetSize(23),
				Children = {
					imgArrow,
					imgArrowGreen
				}
			};

			TapGestureRecognizer tapArrow = new TapGestureRecognizer ();
			tapArrow.Tapped += RedirectToBack;
			layoutArrow.GestureRecognizers.Add (tapArrow);
//			imgArrow.GestureRecognizers.Add (tapArrow);
//			imgArrowGreen.GestureRecognizers.Add (tapArrow);

			lblAction = new Label {
				TextColor = ApplicationStyle.GreenColor,
				Text = "Изменить",
				IsVisible = false,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
			TapGestureRecognizer tapAction = new TapGestureRecognizer ();
			tapAction.Tapped += OnActionClick;
			lblAction.GestureRecognizers.Add (tapAction);

			imgLogo = new Image { 
				Source = Device.OnPlatform("Header/logo_top_.png", "Logo_OM_gor.png", "Logo_OM_gor.png"),
//				HorizontalOptions = Device.OnPlatform(LayoutOptions.CenterAndExpand, LayoutOptions.Start, LayoutOptions.Start),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			imgLogo.SizeChanged += (sender, e) => 
			{
				Image img = sender as Image;
				img.HeightRequest = Utils.GetSize(24);
			};
			Image imgFilter = new Image { 
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = Device.OnPlatform("Header/filter_top_.png", "filter_topmenu_.png", "filter_topmenu_.png"),
			};
			layoutImgFilter = new StackLayout {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Padding = new Thickness (5, 8),
				WidthRequest = Utils.GetSize(23),
				Children = {
					imgFilter
				}
			};
			TapGestureRecognizer tapFilter = new TapGestureRecognizer ();
			tapFilter.Tapped += (sender, e) => { 
				OnePage.redirectApp.catalogView.ClickSorted(sender, e); 
			};
			layoutImgFilter.GestureRecognizers.Add (tapFilter);

//			Image imgPodelitsya = new Image { 
//				Source = "Header/podelitsya_top_.png", 
//				HorizontalOptions = LayoutOptions.End,
//			};
			Image imgCart = new Image { 
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = Device.OnPlatform("Header/cart_top_.png", "basket_topmenu_.png", "basket_topmenu_.png"),
//				HorizontalOptions = LayoutOptions.EndAndExpand,
			};

			Grid layoutImgCart = new Grid {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Padding = new Thickness (Utils.GetSize(5), Utils.GetSize(8)),
				WidthRequest = Utils.GetSize(23),
			};

			lblCountProduct = new Label {
//				BackgroundColor = Color.Green,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.White,
				FontSize = Utils.GetSize(8),
			};
			StackLayout layoutCountProduct = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(Utils.GetSize(8), Device.OnPlatform(Utils.GetSize(7), Utils.GetSize(5), Utils.GetSize(5)), Utils.GetSize(6), 0),
				Children = {
					lblCountProduct
				}
			};

			layoutImgCart.Children.Add(imgCart, 0, 0);
			layoutImgCart.Children.Add(layoutCountProduct, 0, 0);

			TapGestureRecognizer tapCart = new TapGestureRecognizer ();
			tapCart.Tapped += OnCartClick;
			layoutImgCart.GestureRecognizers.Add (tapCart);

			lblPagePosition = new Label { 
				LineBreakMode = LineBreakMode.TailTruncation,
				TextColor = ApplicationStyle.TextColor,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions =LayoutOptions.Center,
				FontSize = Utils.GetSize(17),
			};

			leftLayout = new StackLayout { 
				Spacing = 0,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Orientation = StackOrientation.Horizontal,
				Children = {
					layoutArrow,
				}
			};

			StackLayout leftLoginLayout = new StackLayout { 
				Spacing = 0,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Orientation = StackOrientation.Horizontal,
				Children = {
					//layoutArrow,
					lblAction
				}
			};

			centerLayout = new StackLayout { 
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					imgLogo,
					lblPagePosition,
				}
			};
			rightLayout = new StackLayout {
				Spacing = 0,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					layoutImgFilter,
//					imgPodelitsya,
					layoutImgCart
				}
			};

			grid = new Grid {
				HeightRequest = bodyHeight,
//				Padding = new Thickness(0, 0, 8 * App.ScaleWidth, 0),
				RowSpacing = 0,
				ColumnSpacing = 0,
				ColumnDefinitions = {
					new ColumnDefinition { Width = 70 * App.ScaleWidth },
					new ColumnDefinition { Width = 180 * App.ScaleWidth },
					new ColumnDefinition { Width = 70 * App.ScaleWidth },
				},
				RowDefinitions = {
					new RowDefinition { Height = bodyHeight }
				}
			};

			gridLogin = new Grid {
				HeightRequest = bodyHeight,
//				Padding = new Thickness(8, 0),
				RowSpacing = 0,
				ColumnSpacing = 0,
				ColumnDefinitions = {
					new ColumnDefinition { Width = 100 * App.ScaleWidth},
					new ColumnDefinition { Width = 120 * App.ScaleWidth },
					new ColumnDefinition { Width = 100 * App.ScaleWidth},
				},
				RowDefinitions = {
					new RowDefinition { Height = bodyHeight }
				},
//				IsVisible = false	
			};
			gridLogin.Children.Add (leftLoginLayout, 0, 0);
			gridLogin.Children.Add (centerLayout, 1, 0);
			gridLogin.Children.Add (rightLayout, 2, 0);


			intilizeSearchNotKitkat();
			//#if __ANDROID__
			//if (Android.OS.Build.VERSION.SdkInt == Android.OS.BuildVersionCodes.Kitkat) {
			//	searchBar = new Entry { 
			//		TextColor = ApplicationStyle.TextColor,
			//		BackgroundColor = ApplicationStyle.LineColor,
			//		HorizontalOptions = LayoutOptions.FillAndExpand,
			//		HeightRequest = 43,
			//		Placeholder = "Поиск по каталогу",
			//	};
			//	(searchBar as Entry).TextChanged += (sender, e) => {
			//		if (SearchProduct != null)
			//			SearchProduct(sender, e);
			//	};
			//}
			//else
			//	intilizeSearchNotKitkat();
			//#else
			//	intilizeSearchNotKitkat();
			//#endif


			Image imgSearchArrow = new Image { 
				Source = Device.OnPlatform("Header/arrow_top_.png", "arrow_back_.png", "arrow_back_.png"),
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			StackLayout layoutSearchArrow = new StackLayout {
				Padding = new Thickness(8),
				WidthRequest = Utils.GetSize(23),
				Children = {
					imgSearchArrow
				}
			};
			layoutSearchArrow.GestureRecognizers.Add (tapArrow);

			layoutSearch = new StackLayout {
				Spacing = 0,
				Padding = new Thickness(0, 0, 8, 0),
				Orientation = StackOrientation.Horizontal,
				HeightRequest = bodyHeight,
				Children = {
					layoutSearchArrow,
					searchBar
				}
			};

			grid.Children.Add (leftLayout, 0, 0);
			grid.Children.Add (centerLayout, 1, 0);
			grid.Children.Add (rightLayout, 2, 0);

			Orientation = StackOrientation.Vertical;
			BoxView rec = new BoxView { HeightRequest = 1, BackgroundColor = ApplicationStyle.GreenColor  };
			Children.Add(grid);
//			Children.Add(gridLogin);
			Children.Add(rec);
//			StackLayout layout = new StackLayout { 
//				BackgroundColor = Color.Red,
//				HeightRequest = 44,
//				Orientation = StackOrientation.Horizontal,
//				Children = 
//				{
//					leftLayout,
//					centerLayout,
//					rightLayout
//				}
//			};
		}

		void intilizeSearchNotKitkat()
		{
			searchBar = new MySearchBar {
				TextColor = ApplicationStyle.TextColor,
				PlaceholderColor = ApplicationStyle.LabelColor,
				BackgroundColor = ApplicationStyle.LineColor,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 43,
				WidthRequest = 100,
				Placeholder = "Поиск по каталогу",
			};
			searchBar.TextChanged += (sender, e) => {
				if (SearchProduct != null)
					SearchProduct(sender, e);
			};
		}

		void OnActionClick (object sender, EventArgs e)
		{
			if (ClickEvent != null)
				ClickEvent(sender, e);
		}

		void OnCartClick (object sender, EventArgs e)
		{
			OnePage.redirectApp.RedirectToPage (PageName.Basket, false, false);
		}

		void RedirectToBack (object sender, EventArgs e)
		{		
			OnePage.redirectApp.BackToHistory ();
		}

		public void RefreshCountProduct()
		{
			int countProduct = 0;
			if (User.Singleton != null) {
				countProduct = User.Singleton.BasketList.Count;
			}
			countProduct += BasketDB.GetItems ().Count;
			
			if (countProduct < 1)
				lblCountProduct.Text = "";
			else
				lblCountProduct.Text = countProduct.ToString();
		}

		public void HideLeftPanel()
		{ 
			rightLayout.IsVisible = false;
		}

		void OnRedirectPage (object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() => { 
			RefreshCountProduct ();
			History history = sender as History;

			if (history.Page == PageName.Basket || history.Page == PageName.Order || 
			    history.Step == HistoryStep.TableSizes || history.Step == HistoryStep.TableSizeDescription) {
				rightLayout.IsVisible = false;
			} else {
				rightLayout.IsVisible = true;
			}

			if (history.Page == PageName.Basket || history.Page == PageName.Image || 
			    	(history.Page == PageName.Login && User.Singleton == null) || 
			    	((history.Page == PageName.Catalog || history.Page == PageName.Search) && 
			        (history.ProductID != 0 || history.Step != HistoryStep.Default))) {
				OnePage.mainPage.mainlayout.Children.Remove(OnePage.bottomView);
				OnePage.bottomView.IsVisible = false;
			} else {
				OnePage.mainPage.mainlayout.Children.Add(OnePage.bottomView);
				OnePage.bottomView.IsVisible = true;
			}

			if (history.Page == PageName.Image)
				OnePage.topView.IsVisible = false;
			else {
				if (!OnePage.topView.IsVisible)
					OnePage.topView.IsVisible = true;
			}

			if (history.Page == PageName.Search && history.Step == HistoryStep.Default && history.ProductID == 0) {
				if (!this.Children.Contains (layoutSearch)) {
					this.Children.Remove (grid);
					this.Children.Remove (gridLogin);
					this.Children.Insert (0, layoutSearch);
					BackgroundColor = ApplicationStyle.LineColor;
				}
			}
			else {
//				if (history.IsNarrowTitle) {
//					if (!this.Children.Contains (gridLogin)) {
//						this.Children.Remove (grid);
//						this.Children.Remove (layoutSearch);
//						this.Children.Insert (0, gridLogin);
//					}
//					lblAction.IsVisible = true;
//					imgArrow.IsVisible = false;
//					imgArrowGreen.IsVisible = true;
//				} else {
					if (!this.Children.Contains (grid)) {
						this.Children.Remove (gridLogin);
						this.Children.Remove (layoutSearch);
						this.Children.Insert (0, grid);
					}
					lblAction.IsVisible = false;
					imgArrow.IsVisible = true;
					imgArrowGreen.IsVisible = false;
//				}
				BackgroundColor = Color.White;

				if (history.Page == PageName.Main) {
					lblPagePosition.IsVisible = false;
					imgLogo.IsVisible = true;
					leftLayout.IsVisible = false;
				} else {
					imgLogo.IsVisible = false;
					lblPagePosition.IsVisible = true;
					leftLayout.IsVisible = true;
					searchBar.Focus ();
					lblPagePosition.Text = history.CurrentPosition;
				}

				if (history.Page != PageName.Catalog || history.ContentCategory == null) 
					layoutImgFilter.IsVisible = false;
				//else
				//	layoutImgFilter.IsVisible = true;
			}
			searchBar.Focus ();
				});
		}
	}
}