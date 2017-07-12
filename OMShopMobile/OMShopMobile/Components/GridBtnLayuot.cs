using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace OMShopMobile
{
	public class GridBtnLayuot : ContentView
	{
		public PageName PageName { get; set; }
		public Image Img;

		public GridBtnLayuot (PageName pageName, string text, string imageSource, int height=30, int width=30)
		{
			PageName = pageName;
			StackLayout layout = new StackLayout {
				Spacing = 1,
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.Center
			};
			Img = new Image { 
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Source = imageSource, 
//				HeightRequest = (int)(height * App.ScaleHeight),
//				WidthRequest = (int)(width * App.ScaleHeight),
				HeightRequest = (int)(height * App.ScaleHeight / 1.5),
				WidthRequest = (int)(width * App.ScaleHeight / 1.5),
			};

//			StackLayout l = new StackLayout {
//				WidthRequest = 21,
//				HeightRequest = 21,
//				Children = {
//					Img
//				},
//			};


			layout.Children.Add (Img);

			if (!String.IsNullOrEmpty (text)) {
				Label lblName = new Label {
					Text = text,
					HorizontalOptions = LayoutOptions.Center,
					TextColor = Color.Black,
					Style = ApplicationStyle.TopLabelStyle
				};
				layout.Children.Add (
					lblName
				);
			}
			Content = layout;
		}

		public int GetIndexContent(IList<View> children)
		{
			foreach (var item in children) {
				
			}
			return 1;
		}
	}
}