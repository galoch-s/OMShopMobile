using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class SortItemCell : ViewCell
	{
		Image img;
		public SortItemCell ()
		{
			Label lblName = new Label () {
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = ApplicationStyle.TextColor
			};
			lblName.SetBinding (Label.TextProperty, "Name");

			img = new Image { 
				HorizontalOptions = LayoutOptions.EndAndExpand, 
				Source = Device.OnPlatform("Catalog/Galochka_vibor_.png", "green_galochka_.png", "green_galochka_.png"),
//				HeightRequest = 15,
//				WidthRequest = 15,
				IsVisible = false
			};

			StackLayout layout = new StackLayout {
				Padding = new Thickness(16, 0),
				Orientation = StackOrientation.Horizontal,
				Children = {
					lblName,
					img
				}					
			};
			View = layout;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			ParamForSort entityBind = BindingContext as ParamForSort;
			if (entityBind != null)
				img.IsVisible = entityBind.IsCheck;
		}
	}
}