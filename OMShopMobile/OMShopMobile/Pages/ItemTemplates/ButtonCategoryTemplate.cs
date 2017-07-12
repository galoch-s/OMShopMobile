using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class ButtonCategoryTemplate : ContentView
	{	
		public CatalogEnum CatalogID { get; set; }
		public string Name { get; set; }

		public ButtonCategoryTemplate (CatalogEnum catalogID,  string name, string imageSource)
		{
			BackgroundColor = Color.White;
			CatalogID = catalogID;
			Name = name;

			Image img = new Image { 
				Source = imageSource, 
				HeightRequest = Utils.GetSize(24),
			};
			Label lblName = new Label {
//				HorizontalOptions = LayoutOptions.Center,
				Text = name,
				FontSize = Utils.GetSize(12),
				HeightRequest = Utils.GetSize(26),
			};

			StackLayout layout = new StackLayout {
				VerticalOptions = LayoutOptions.Center,
				Spacing = 10,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Center,
				Children = {
					img,
					lblName
				}	
			};
			Content = layout;
		}
	}
}