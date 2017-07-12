using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class OrderCellTemplate : ViewCell
	{
		Image img;
		StackLayout layout;
		Label lblValue;

		public OrderCellTemplate ()
		{
			Label lblName = new Label () {
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = ApplicationStyle.TextColor,
			};
			lblName.SetBinding (Label.TextProperty, "Name");

			img = new Image { 
				HorizontalOptions = LayoutOptions.EndAndExpand, 
				Source = Device.OnPlatform("Catalog/arrow_catalog_.png", "arrow_right_.png", "arrow_right_.png")
			};

			lblValue = new Label {
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.EndAndExpand, 
				TextColor = ApplicationStyle.TextColor,
			};
			lblValue.SetBinding (Label.TextProperty, "Value");

			layout = new StackLayout {
				Padding = new Thickness(16, 0),
				Orientation = StackOrientation.Horizontal,
				Children = {
					lblName,
//					lblValue
				}					
			};
			View = layout;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			InfoOrder info = BindingContext as InfoOrder;

			if (info.Page != InfoBasketPage.Empty)
				layout.Children.Add (img);
			else
				layout.Children.Add (lblValue);
		}
	}
}

