using System;
using Xamarin.Forms;
using System.Collections.Generic;
using FFImageLoading.Forms;

namespace OMShopMobile
{
	public class ProductInCategoryTemplate : ContentView
	{
		public event EventHandler EventClick;
		public int Id { get; set; }
		public string Name { get; set; }

		float imgHeight;
		float imgWidth;

		CachedImage cachedImage;
		Image imgClock;
		Image imgSklad;

		Label lblName;
		MyLabel lblPriceOld;
		Label lblPrice;
		Label lblSize;

		public ProductInCategoryTemplate()
		{
			double paddingLayput = Utils.GetSize(0.5);
			Padding = new Thickness(paddingLayput, paddingLayput, paddingLayput, 0);

			BackgroundColor = ApplicationStyle.LineColor;
			imgHeight = (int)Utils.GetSize(App.Density * 140, 1);// 133;
			imgWidth = (int)Utils.GetSize(App.Density * 180, 1);// 166;

			cachedImage = new CachedImage() {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = Utils.GetSize(133, 1),
				CacheDuration = TimeSpan.FromDays(30),
				Margin = new Thickness(0, 5 * App.ScaleWidth, 0, 0),
				LoadingPlaceholder = "Zaglushka_",
				RetryCount = 2
			};

			imgClock = new Image {
				VerticalOptions = LayoutOptions.Start,
				Source = Device.OnPlatform("Catalog/clock_", "clock", "clock"),
				IsVisible = false
			};
			imgSklad = new Image {
				VerticalOptions = LayoutOptions.Start,
				Source = Device.OnPlatform("Catalog/sklad_", "Sklad", "Sklad"),
				IsVisible = false
			};
			StackLayout icoLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.Start,
				Padding = new Thickness(0, 0, Utils.GetSize(12, 1), 0),
				Spacing = 2,
				Children = {
					imgClock,
					imgSklad
				}
			};

			Grid gridImg = new Grid();// { HeightRequest = 136 };
			gridImg.Children.Add(cachedImage, 0, 0);
			gridImg.Children.Add(icoLayout, 0, 0);

			lblName = new Label {
				LineBreakMode = LineBreakMode.TailTruncation,
				TextColor = ApplicationStyle.TextColor,
				FontSize = Utils.GetSize(14),
				HorizontalOptions = LayoutOptions.Center,
			};
			lblPriceOld = new MyLabel {
				TextColor = ApplicationStyle.LabelColor,
				FontSize = Utils.GetSize(11),
				HorizontalOptions = LayoutOptions.Center,
				Margin = new Thickness(0, 8, 0, 5),
				IsVisible = false,
				IsStrikeThrough = true
			};
			lblPrice = new Label {
				TextColor = ApplicationStyle.GreenColor,
				FontSize = Utils.GetSize(12),
				HorizontalOptions = LayoutOptions.Center,
				Margin = new Thickness(0, 8, 0, 5),
			};
			StackLayout layoutPrice = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center,
				Children = {
					lblPriceOld,
					lblPrice
				}
			};
			lblSize = new Label {
				LineBreakMode = LineBreakMode.TailTruncation,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Utils.GetSize(10),
			};

			BoxView bottomLine = new BoxView {
				VerticalOptions = LayoutOptions.EndAndExpand,
				BackgroundColor = ApplicationStyle.GreenColor,
				HeightRequest = 1.5,
			};

			StackLayout layoutDescription = new StackLayout {
				VerticalOptions = LayoutOptions.End,
				Spacing = 0,
				Padding = new Thickness(8, 0),
				Children = {
					lblName,
					layoutPrice,
					lblSize,
				}
			};

			StackLayout layout = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 5,
				BackgroundColor = Color.White,
				Orientation = StackOrientation.Vertical,
				Children = {
					gridImg,
					layoutDescription,
					bottomLine
				}
			};
			TapGestureRecognizer tap = new TapGestureRecognizer();
			layout.GestureRecognizers.Add(tap);
			tap.CommandParameter = this;
			tap.Tapped += (sender, e) => {
				if (EventClick != null)
					EventClick(sender, e);
			};

			Content = layout;
		}

		public void ShowProduct(Product product, bool isInExistence)
		{
			if (product == null) return;

			Id = product.ProductsID;
			Name = product.productsDescription?.Name;

			if (string.IsNullOrEmpty(product.Image))
				cachedImage.Source = "Zaglushka_";
			else
				cachedImage.Source = Constants.PathToPreviewImage + product.Image + "&h=" + imgHeight + "&w=" + imgWidth;
			
			imgClock.IsVisible = product.SchedulesList?.Count > 0;
			imgSklad.IsVisible = product.Express;

			lblName.Text = Name;

			if (product.PriceOld != 0 && product.PriceOld > product.Price) {
				lblPriceOld.Text = Math.Ceiling(product.PriceOld) + "р.";
				lblPriceOld.IsVisible = true;
			} else
				lblPriceOld.IsVisible = false;


			lblPrice.Text = Math.Ceiling(product.Price) + "р.";
			SetSizes(product.productsAttributes);

			IsVisible = true;
		}

		void SetSizes(List<ProductsAttributes> productsAttributes)
		{
			lblSize.Text = "";
			for (int i = 0; i < productsAttributes.Count; i++) 
			{
				if (productsAttributes [i].Quantity > 0)
					lblSize.Text += productsAttributes [i].OptionValue.Value + ", ";
			}
			if (productsAttributes.Count > 0) {
				if (string.IsNullOrEmpty (lblSize.Text))
					lblSize.Text = "нет размеров";
				else
					lblSize.Text = lblSize.Text.TrimEnd (new char[]{ ' ', ',' });
			}
		}
	}
}