using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class GridUtils
	{
		public GridUtils ()
		{
		}

		public static StackLayout GetTitleLabel(string text, TextAlignment HorizontalTextAlignment = TextAlignment.Center)
		{	
			Label label = new Label { 
				HorizontalTextAlignment = HorizontalTextAlignment,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				TextColor = Color.White,
				HeightRequest = Utils.GetSize(25),
				Text = text, 	
			};

			StackLayout layout = new StackLayout { 
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = ApplicationStyle.GreenColor,
				Children = {
					label,
				}
			};
			if (HorizontalTextAlignment != TextAlignment.Center)
				layout.Padding = new Thickness (8, 0);
			return layout;
		}

		public static StackLayout GetBodyLabel(string text) 
		{
			return GetBodyLabel (text, ApplicationStyle.TextColor);
		}

		public static StackLayout GetBodyLabel(string text, TextAlignment HorizontalTextAlignment) 
		{
			return GetBodyLabel (text, ApplicationStyle.TextColor, false, HorizontalTextAlignment);
		}

		public static StackLayout GetBodyLabel(string text, Color textColor, bool isIcon = false, TextAlignment HorizontalTextAlignment = TextAlignment.Center)
		{
			Image img = new Image { 
				Source = "Catalog/arrow_catalog_.png"
			};

			Label label = new Label {
				HorizontalTextAlignment = HorizontalTextAlignment,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				TextColor = textColor,
				HeightRequest = Utils.GetSize(25),
				Text = text,
			};

			StackLayout layout = new StackLayout { 
				Spacing = 0,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = ApplicationStyle.SpacingColor,
				Padding = new Thickness(8, 0),
				Children = {
					label,

				}
			};
			if (isIcon) {
				layout.Padding = new Thickness (18, 0, 12, 0);
				layout.Children.Add (img);
			}
			return layout;
		}
	}
}

