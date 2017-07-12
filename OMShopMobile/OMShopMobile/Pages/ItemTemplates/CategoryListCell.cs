using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class CategoryListCell : ViewCell
	{
		public CategoryListCell ()
		{
			Label lblName = new Label () {
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = ApplicationStyle.TextColor,
			};
			lblName.SetBinding (Label.TextProperty, "Description.Name");

			Image img = new Image { 
				HorizontalOptions = LayoutOptions.EndAndExpand, 
				Source = "Catalog/arrow_catalog_.png"
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
	}
}