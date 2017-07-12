using System;

using Xamarin.Forms;
using System.Collections.Generic;
using CustomLayouts.Controls;
using CustomLayouts;
using CustomLayouts.ViewModels;
using System.Linq;

namespace OMShopMobile
{
	public class MainPageView : ContentView
	{
		StackLayout newProductLayout;
		StackLayout newProductLayout2;

		public static int heightSearch = Utils.GetSize(43);
		public static readonly int heightBanner = Utils.GetSize(148, 1);// (int)(148 * App.ScaleHeight);
		public const int heightBottomBorder = 11;
		public static readonly int heightCategoryPanel = Utils.GetSize(71);
		public static readonly int heightSpacingTitle = Utils.GetSize(10);
		public static int heightNewProduct;

		public MainPageView ()
		{
			if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.iOS)
				heightSearch = 43;
			

			int countProductClothingAndShoes = 15;
			int countProductOther = 20;

			int countProductOnPage = 3;
			int heightMedium = MainPageView.heightSearch +
			                   MainPageView.heightBanner +
			                   MainPageView.heightBottomBorder +
			                   MainPageView.heightCategoryPanel;
			
			if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Android) {
				countProductClothingAndShoes = 20;
				countProductOnPage = 4;
				heightMedium += MainPageView.heightBottomBorder * 2;
			}

			int heightOtherBlocks = Constants.HeightTopMain + heightMedium + Constants.HeightButtom + Constants.HeightTitleNew;

			int screenHeight = App.ScreenHeight;
			if (Device.OS == TargetPlatform.iOS && App.ScreenHeight < 568) {
				screenHeight = 568;
			}
			if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Android) 
				heightNewProduct = screenHeight - heightOtherBlocks - 13 * 2;
			else
				heightNewProduct = screenHeight - heightOtherBlocks - 10;
				
			if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Android) {
				heightNewProduct /= 2;
			}

			VerticalOptions = LayoutOptions.FillAndExpand;
			Padding = new Thickness(0);

			var searchLayout = ShownPanelSearch();


			newProductLayout = new StackLayout ();
			ShowPanelProducNew(newProductLayout, "Новинки одежды и обуви", TypeNewsProduct.ClothingAndShoes, countProductClothingAndShoes, countProductOnPage);

			if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Android) {
				newProductLayout2 = new StackLayout();
				ShowPanelProducNew(newProductLayout2, "Новинки прочее", TypeNewsProduct.OtherProducts, countProductOther, countProductOnPage);
			}

			Grid gridCategory;
			BoxView bottomBorder = ShownCategoryPanel(out gridCategory);

			TapGestureRecognizer tapProduct = new TapGestureRecognizer();
			tapProduct.Tapped += ProductClick;


			StackLayout mainLayout = new StackLayout {
				Spacing = 0,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					searchLayout,
					new ContentView {
						HeightRequest = heightBanner,
						VerticalOptions = LayoutOptions.Start,
						Content = new CarouselView(CarouselLayout.IndicatorStyleEnum.Dots, new BannerViewModel(), new DataTemplate(typeof(BannerTemplate))) } ,
				}
			};
			if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Android) {
				mainLayout.Children.Add(new BoxView {
					VerticalOptions = LayoutOptions.End,
					HeightRequest = heightBottomBorder,
					BackgroundColor = ApplicationStyle.LineColor,
				});
				mainLayout.Children.Add(newProductLayout);
				mainLayout.Children.Add(new BoxView {
					VerticalOptions = LayoutOptions.End,
					HeightRequest = heightBottomBorder,
					BackgroundColor = ApplicationStyle.LineColor,
				});
				mainLayout.Children.Add(newProductLayout2);
			} else {
				mainLayout.Children.Add(newProductLayout);
			}
			mainLayout.Children.Add(bottomBorder);
			mainLayout.Children.Add(gridCategory);
			if (Device.OS == TargetPlatform.iOS && App.ScreenHeight < 568) {
				ScrollView scrollMain = new ScrollView {
					Content = mainLayout
				};
				Content = scrollMain;
			} else {
				Content = mainLayout;
			}
		}

		StackLayout ShownPanelSearch ()
		{
			return intilizeSearchNotKitkat();


			#if __ANDROID__
			if (Android.OS.Build.VERSION.SdkInt == Android.OS.BuildVersionCodes.Kitkat) {
				MyEntry entSearch = new MyEntry {
					Placeholder = "Поиск по каталогу",
					HeightRequest = heightSearch,
					BackgroundColor = Color.Transparent,
					HorizontalTextAlignment = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					PlaceholderColor = ApplicationStyle.LabelColor,


					BorderColor = ApplicationStyle.LineColor,
					BorderRadius = 7,
					BorderWidth = 10,
				};
				entSearch.Focused += OnSearchClick;

//				Image imgSearch = new Image {
//					Source = "Header/loup_poisk.png",
//					VerticalOptions = LayoutOptions.CenterAndExpand,
//					HorizontalOptions = LayoutOptions.Start
//				};
				StackLayout layoutText = new StackLayout { 
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal,
					BackgroundColor = ApplicationStyle.LineColor,
					Children = {
						entSearch,
					}
				};
				return layoutText;
			}
			else
				return intilizeSearchNotKitkat();
			#else
				return intilizeSearchNotKitkat();
			#endif
		}

		StackLayout intilizeSearchNotKitkat()
		{
			MySearchBar searchBar = new MySearchBar {
				BackgroundColor = ApplicationStyle.LineColor,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				PlaceholderColor = ApplicationStyle.LabelColor,
				Placeholder = "Поиск по каталогу",
				IsEnabled = false,
				//HorizontalTextAlignment = TextAlignment.Center,
			};


			StackLayout searchLayout = new StackLayout {
				Spacing = 0,
				BackgroundColor = ApplicationStyle.LineColor,
				Children =  {
					searchBar,
	//					layoutImg,
				}
			};
			if (Device.OS == TargetPlatform.iOS) {
				searchBar.HeightRequest = heightSearch;
				//searchBar.HeightRequest = heightSearch - (7 * 2);
				//searchLayout.Padding = new Thickness (0, 7);
			} else {
				searchBar.HeightRequest = heightSearch;
			}
			
			TapGestureRecognizer tapSearch = new TapGestureRecognizer ();
			tapSearch.Tapped += OnSearchClick;
			searchBar.GestureRecognizers.Add (tapSearch);
			searchLayout.GestureRecognizers.Add (tapSearch);
			searchBar.Focused += OnSearchClick;

			return searchLayout;
		}

		static void OnSearchClick (object sender, EventArgs e)
		{
			OnePage.redirectApp.RedirectToPage (PageName.Search, false, false);
		}

		BoxView ShownCategoryPanel (out Grid gridCategory)
		{
			BoxView bottomBorder = new BoxView {
				VerticalOptions = LayoutOptions.End,
				HeightRequest = heightBottomBorder,
				BackgroundColor = ApplicationStyle.LineColor,
			};
			gridCategory = new Grid {
				IsClippedToBounds = true,
				ColumnSpacing = 0,
				HeightRequest = heightCategoryPanel,
				BackgroundColor = ApplicationStyle.LineColor,
				ColumnDefinitions =  {
					new ColumnDefinition {	Width = new GridLength (25, GridUnitType.Star)},
					new ColumnDefinition {	Width = Utils.GetSize(0.5) },
					new ColumnDefinition {	Width = new GridLength (25, GridUnitType.Star)},
					new ColumnDefinition {	Width = Utils.GetSize(0.5) },
					new ColumnDefinition {	Width = new GridLength (25, GridUnitType.Star)},
					new ColumnDefinition {	Width = Utils.GetSize(0.5) },
					new ColumnDefinition {	Width = new GridLength (25, GridUnitType.Star)},
					new ColumnDefinition {	Width = Utils.GetSize(0.5)	},
				}
			};
			ButtonCategoryTemplate btnDress = new ButtonCategoryTemplate (CatalogEnum.Women, "Женская\r\n одежда", Device.OnPlatform("Catalog/dress_main_.png", "dress_menu_.png", "dress_menu_.png"));
			ButtonCategoryTemplate btnMen = new ButtonCategoryTemplate (CatalogEnum.Men, "Мужская\r\n одежда", Device.OnPlatform("Catalog/men_main_.png", "pidjak_menu_.png", "pidjak_menu_.png"));
			ButtonCategoryTemplate btnBag = new ButtonCategoryTemplate (CatalogEnum.Accessory, "Аксессуары", Device.OnPlatform("Catalog/bag_main_.png", "bag_menu_.png", "bag_menu_.png"));
			ButtonCategoryTemplate btnCatalog = new ButtonCategoryTemplate (CatalogEnum.All, "       Все\r\n категории", Device.OnPlatform("Catalog/catalog_main_.png", "menu_menu_.png", "menu_menu_.png"));

			gridCategory.Children.Add (btnDress, 0, 0);
			gridCategory.Children.Add (new BoxView { WidthRequest = Utils.GetSize(0.5) }, 1, 0);
			gridCategory.Children.Add (btnMen, 2, 0);
			gridCategory.Children.Add (new BoxView { WidthRequest = Utils.GetSize(0.5) }, 3, 0);
			gridCategory.Children.Add (btnBag, 4, 0);
			gridCategory.Children.Add (new BoxView { WidthRequest = Utils.GetSize(0.5) }, 5, 0);
			gridCategory.Children.Add (btnCatalog, 6, 0);
			gridCategory.Children.Add (new BoxView { WidthRequest = Utils.GetSize(0.5), BackgroundColor = Color.White }, 7, 0);

			TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += CategoryClick;

			btnDress.GestureRecognizers.Add (tapGestureRecognizer);
			btnMen.GestureRecognizers.Add (tapGestureRecognizer);
			btnBag.GestureRecognizers.Add (tapGestureRecognizer);
			btnCatalog.GestureRecognizers.Add (tapGestureRecognizer);
			return bottomBorder;
		}

		async void ShowPanelProducNew (StackLayout newProductLayout, string nameBlock, TypeNewsProduct type, int countProduct, int countProductOnPage)
		{
			List<Product> productNewList = null;
			newProductLayout.Padding = new Thickness (18, Constants.TopPaddingNewProductLayout, 18, 0);
			newProductLayout.Spacing = heightSpacingTitle;
			newProductLayout.HeightRequest = heightNewProduct;
			newProductLayout.VerticalOptions = LayoutOptions.FillAndExpand;

			TapGestureRecognizer tapProduct = new TapGestureRecognizer ();
			tapProduct.Tapped += ProductClick;
			Label lblProductNewClothes = new Label {
				Text = nameBlock,
				TextColor = ApplicationStyle.GreenColor,
				FontSize = Utils.GetSize(13),
				HeightRequest = Constants.HeightTitleNew,
			};
			newProductLayout.Children.Add(lblProductNewClothes);
			ActivityIndicator indicator = new ActivityIndicator {
				Color = Device.OnPlatform(Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
			newProductLayout.Children.Add(indicator);

			if (type == TypeNewsProduct.ClothingAndShoes) {
				if (OnePage.productNew1List == null) {
					//#if !DEBUG 
					productNewList = await Product.GetProductsNewsClothesListAsync(countProduct);
					//# endif
					OnePage.productNew1List = productNewList;
				}
				else
					productNewList = OnePage.productNew1List;
			} else {
				if (OnePage.productNew2List == null) {
					productNewList = await Product.GetProductsNewsOthesListAsync(countProduct);
					OnePage.productNew2List = productNewList;
				}
				else
					productNewList = OnePage.productNew2List;
			}	

			ContentView contentView;

			if (productNewList != null)
				//contentView = null;
				contentView = new CarouselView(CarouselLayout.IndicatorStyleEnum.Dots, new ProductNewViewModel(productNewList, tapProduct, countProductOnPage), new DataTemplate(typeof(ProductGridTemplate)));
			else
				contentView = new ContentView { Content = new Label { Text = "Не удалось загрузить новинки", VerticalOptions = LayoutOptions.CenterAndExpand } };
			indicator.IsVisible = false;
			newProductLayout.Children.Add(new ContentView {
				HeightRequest = heightNewProduct - heightSpacingTitle - Constants.HeightTitleNew,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = contentView
			});
		}

		void ProductClick (object sender, EventArgs e)
		{
			ProductNewTemplate productTemp = sender as ProductNewTemplate;
			OnePage.redirectApp.RedirectToPage (PageName.Catalog, true, false, false);
			OnePage.redirectApp.catalogView.GoToProductAsync (productTemp.Name, productTemp.Id, false);
		}

		void CategoryClick (object sender, EventArgs e)
		{
			ButtonCategoryTemplate item = sender as ButtonCategoryTemplate;
			if (item.CatalogID == CatalogEnum.All) {
				OnePage.redirectApp.RedirectToPage(PageName.Catalog, false, false);
				OnePage.redirectApp.catalogView.GotoMainCategory();
			} else {
				OnePage.redirectApp.RedirectToPage (PageName.Catalog, true, false, false);

				Category category = OnePage.categoryList.FirstOrDefault(g => g.ID == (int)item.CatalogID);
				if (category == null) { 
					category = new Category {
						ID = (int)item.CatalogID,
						Description = new CategoriesDescription { Name = item.Name.Replace("\r", "") }
					};
				}
				OnePage.redirectApp.catalogView.GoToCategory (category, null, false);
			}
		}
	}
}