using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class ListCellTemplate : ViewCell
	{
		Image img;
		string imgLeft = Device.OnPlatform("Catalog/arrow_catalog_.png", "arrow_right_.png", "arrow_right_.png");
		string imgDown = Device.OnPlatform("Catalog/Galochka_filter_grey_.png", "arrow_down_.png", "arrow_down_.png");
		public ListCellTemplate ()
		{
			Label lblName = new Label () {
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = ApplicationStyle.TextColor,
			};
			lblName.SetBinding (Label.TextProperty, "Name");

			Label lblCount = new Label () { 
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = ApplicationStyle.TextColor,
			};
			lblCount.SetBinding (Label.TextProperty, "Count");

			img = new Image { 
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
			View = layout;
		}

		//protected override void OnTapped ()
		//{
		//	base.OnTapped ();
		//	FileImageSource source = img.Source as FileImageSource;
		//	if (source != null && source.File != imgDown)
		//		img.Source = imgDown;
		//	else
		//		img.Source = imgLeft;
		//}
	}
}

