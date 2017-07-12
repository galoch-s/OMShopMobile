using System;

using Xamarin.Forms;

namespace OMShopMobile
{
	public class TopViewTemplate : ViewCell
	{
		public TopViewTemplate ()
		{
			//Image img = new Image();
			//img.SetBinding (Image.SourceProperty, "ImageSource");

//			var label = new Label {
//				YAlign = TextAlignment.Center
//			};
//
//			label.SetBinding (Label.TextProperty, "Name");

			var label = new Label {
				YAlign = TextAlignment.Center
			};

			label.SetBinding (Label.TextProperty, "Text");





			var layout = new StackLayout {
				Padding = new Thickness (5),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Children = { label }
			};

			View = layout;
		}
	}
}


