using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class ProductNewTemplate : ContentView
	{
		public int Id { get; set; }
		public string Name { get; set; }
		Image img;
		Image imgClock;
		Image imgSklad;
		Label lblName;
		Label lblPrice;

//		float imgHeight = App.Density * 89;
//		float imgWidth = App.Density * 59;

		int heightOtherBlocks;
		int heightCurrent;
		int widthCurrent;
		double scaleIco;

		public ProductNewTemplate (Product product) : this ()
		{
//			Padding = new Thickness (8, 0);
			Id = product.ProductsID;
			Name = product.productsDescription?.Name;
//			img.Source = Constants.PathToPreviewImage + imgSource + "&h=" + imgHeight + "&w=" + imgWidth;
			img.Source = Constants.PathToPreviewImage + product.Image + "&h=" + heightCurrent * App.Density + "&w=" + widthCurrent * App.Density;
			lblName.Text = product.productsDescription?.Name;
			lblPrice.Text = Math.Ceiling(product.Price).ToString ("F0") + " р.";
			imgClock.IsVisible = product.SchedulesList?.Count > 0;
			imgSklad.IsVisible = product.Express;
		}

		public ProductNewTemplate ()
		{
			int heightMedium = MainPageView.heightSearch +
			                   MainPageView.heightBanner +
			                   MainPageView.heightBottomBorder +
			                   MainPageView.heightCategoryPanel;
			heightOtherBlocks = Constants.HeightTopMain + heightMedium + Constants.HeightButtom;

			heightCurrent = (int)App.ScreenHeight - heightOtherBlocks;
			widthCurrent = (int)App.DisplayWidth / 3 - 10;
			widthCurrent = (int)(Math.Ceiling (widthCurrent / 10.0) * 10);

//			heightCurrent = (int)(heightCurrent * App.Density);
//			widthCurrent = (int)(widthCurrent * App.Density);

//			int heightPadding = Constants.TopPaddingNewProductLayout +
//						Constants.HeightTitleNew +
//			            Constants.TopPaddingNewProductList +
//			            Constants.HeightProductNewDescription +
//						Constants.HeightCaruselIndicator +
//						(int)(10 * App.ScaleHeight);

			int heightPadding = 
//				Constants.HeightTitleNew +
				Constants.TopPaddingNewProductList +
				Constants.HeightProductNewDescription +
				Constants.HeightCaruselIndicator +
				MainPageView.heightSpacingTitle;
//				(int)(10 * App.ScaleHeight);

//			this.HeightRequest = heightCurrent / 2;
			heightCurrent -= heightPadding;
			heightCurrent = (int)(Math.Ceiling (heightCurrent / 10.0) * 10) / 2;



			int heightImg = MainPageView.heightNewProduct - heightPadding;
			int heightIcoMedium = 18;
			int heightTempImg = 133;
			scaleIco = (double)heightImg / heightTempImg;
			int heightIco = (int)Utils.GetSize(heightIcoMedium * scaleIco);

			img = new Image {
				HeightRequest = heightImg,
//				WidthRequest = 59,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			imgClock = new Image {
				VerticalOptions = LayoutOptions.Start,
				Source = Device.OnPlatform("Catalog/clock_", "clock", "clock"),
				HeightRequest = heightIco,
				IsVisible = false
			};
			imgSklad = new Image {
				VerticalOptions = LayoutOptions.Start,
				Source = Device.OnPlatform("Catalog/sklad", "Sklad", "Sklad"),
				HeightRequest = heightIco,
				IsVisible = false
			};


			StackLayout icoLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.End,
				Padding = new Thickness(0, 0, Utils.GetSize(2 * scaleIco, 1), 0),
				//HeightRequest = heightImg,
				//WidthRequest = heightImg / 0.75,
				Spacing = 2,
				Children = {
					imgClock,
					imgSklad
				}
			};

			Grid gridImg = new Grid()
			{
				HeightRequest = heightImg,
				WidthRequest = (int)(heightImg / 0.75),
				//RowDefinitions = { new RowDefinition { Height = heightImg } },
				//ColumnDefinitions = { new ColumnDefinition { Width = (int)(heightImg / 0.75) } }
			};
			gridImg.Children.Add(img, 0, 0);
			gridImg.Children.Add(icoLayout, 0, 0);

			lblName = new Label {
				LineBreakMode = LineBreakMode.TailTruncation,
				TextColor = ApplicationStyle.TextColor,
				HorizontalOptions = LayoutOptions.Center,
				FontSize = Utils.GetSize(10)
			};

			lblPrice = new Label { 
				TextColor = ApplicationStyle.GreenColor,
				FontSize = Utils.GetSize(9),
				HorizontalOptions = LayoutOptions.Center,
				FontAttributes = FontAttributes.Bold,
			};

			StackLayout layoutDescription = new StackLayout {
				HeightRequest = Constants.HeightProductNewDescription  ,
				Spacing = 10,
				Children = {
					lblName,
					lblPrice
				}
			};

			StackLayout mainLayout = new StackLayout { 
//				VerticalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 3,
				WidthRequest = 80,
				Children = {
					gridImg,
					layoutDescription
				}
			};
			Content = mainLayout;
			HeightRequest = heightCurrent;
		}
//		protected override void OnBindingContextChanged ()
//		{
//			base.OnBindingContextChanged ();
//			Product product = BindingContext as Product;
//
//			img.Source = Constants.PathToPreviewImage + product.Image + "&h=" + imgHeight + "&w=" + imgWidth;
//			lblName.Text = product.productsDescription.Name;
//			lblPrice.Text = Math.Ceiling(product.Price).ToString ("F0") + " р.";
//		}
	}
}

