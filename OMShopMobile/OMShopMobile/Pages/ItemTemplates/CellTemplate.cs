using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class CellTemplate : ContentView
	{
		Image img;
		string imgLeft = Device.OnPlatform("Catalog/arrow_catalog_.png", "arrow_right_.png", "arrow_right_.png");
		string imgDown = Device.OnPlatform("Catalog/Galochka_filter_grey_.png", "arrow_down_.png", "arrow_down_.png");
		public CellTemplate (string name, int count = -1)
		{
			Label lblName = new Label () {
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = ApplicationStyle.TextColor,
				Text = name
			};

			Label lblCount = new Label () { 
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = ApplicationStyle.TextColor,
			};
			if (count != -1)
				lblCount.Text = count.ToString();

			img = new Image { 
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Source = imgLeft
			};
			StackLayout layoutEnd = new StackLayout {
				Spacing = 10,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Children = {
					lblCount,
					img
				}
			};

			StackLayout layout = new StackLayout {
				Padding = new Thickness(16, 0),
				Orientation = StackOrientation.Horizontal,
				Children = {
					lblName,
					layoutEnd
				}					
			};
			Content = layout;
		}

		public void SetDown()
		{
			img.Source = imgDown;
		}

		public void SetLeft()
		{
			img.Source = imgLeft;
		}
	}
}