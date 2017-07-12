using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class BottomView : ContentView
	{
		public GridBtnLayuot btnSearch;
		public GridBtnLayuot btnCatalog;
		public GridBtnLayuot btnLogin;
		GridBtnLayuot btnDelivery;

		public BottomView ()
		{
			BackgroundColor = ApplicationStyle.ButtonColor;

			btnSearch = new GridBtnLayuot (PageName.Search, "Поиск", Device.OnPlatform("Footer/loupet_podval_.png", "podval_loupe_.png", "loupet_podval_.png"));
			btnCatalog = new GridBtnLayuot (PageName.Catalog, "Каталог", Device.OnPlatform("Footer/menut_podval_.png", "podval_menu_.png", "menut_podval_.png"));
			btnLogin = new GridBtnLayuot (PageName.Login, "Кабинет", Device.OnPlatform("Footer/cabinet_podval_.png", "podval_kabinet_.png", "cabinet_podval_.png"));
			btnDelivery = new GridBtnLayuot (PageName.Info, "Инфо", Device.OnPlatform("Footer/infot_podval_.png", "podval_info_.png", "podval_info_.png"));


			GridLength widthGrid = new GridLength(1, GridUnitType.Star);
			Grid grid = new Grid {
				ColumnSpacing = 0,
				RowDefinitions = {
					new RowDefinition { Height = Constants.HeightButtom-1 },
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = widthGrid },
					new ColumnDefinition { Width = widthGrid },
					new ColumnDefinition { Width = widthGrid },
				}
			};
			grid.Children.Add (btnSearch, 0, 0);
			grid.Children.Add (btnCatalog, 1, 0);
			grid.Children.Add (btnLogin, 2, 0);
			grid.Children.Add (btnDelivery, 3, 0);

			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped +=  ClickButton;
			btnSearch.GestureRecognizers.Add(tapGestureRecognizer);
			btnCatalog.GestureRecognizers.Add(tapGestureRecognizer);
			btnLogin.GestureRecognizers.Add(tapGestureRecognizer);
			btnDelivery.GestureRecognizers.Add(tapGestureRecognizer);
				
			BoxView border = new BoxView ();

			StackLayout mainLayout = new StackLayout { 
				Spacing = 0,
				HeightRequest = Constants.HeightButtom,
				Orientation = StackOrientation.Vertical,
				Children = {
					border,
					grid
				}
			};
			Content = mainLayout;
		}

		private void ClickButton(object s, EventArgs e)
		{
			GridBtnLayuot item = s as GridBtnLayuot;
			OnePage.redirectApp.RedirectToPage (item.PageName, false, false);
			if (item == btnCatalog)
				OnePage.redirectApp.catalogView.GotoMainCategory();
			
		}
	}
}

