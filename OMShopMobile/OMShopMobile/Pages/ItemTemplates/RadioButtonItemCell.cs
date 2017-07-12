using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class RadioButtonItemCell : ViewCell
	{
		Image imgCheck;
		Image imgUnCheck;
		public RadioButtonItemCell ()
		{
			Label lblName = new Label () {
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = ApplicationStyle.TextColor
			};
			lblName.SetBinding (Label.TextProperty, "Name");

			imgCheck = new Image { 
				Source = Device.OnPlatform("Basket/grey_kvadrat_galochka_.png", "green_checkbox_.png", "green_checkbox_.png"),
				HeightRequest = 24,
				WidthRequest = 24,
				IsVisible = false
			};
			imgUnCheck = new Image { 
				Source = Device.OnPlatform("Basket/grey_kvadrat_.png", "grey_checkbox_.png", "grey_checkbox_.png"),
				HeightRequest = 24,
				WidthRequest = 24,
				IsVisible = true
			};
			StackLayout layoutImg = new StackLayout {
				VerticalOptions = LayoutOptions.Center,
				HeightRequest = 24,
				WidthRequest = 24,
				Children = {
					imgCheck,
					imgUnCheck
				}
			};

			StackLayout layout = new StackLayout {
				Padding = new Thickness(16, 0),
				Orientation = StackOrientation.Horizontal,
				Children = {
					layoutImg,
					lblName,
				}
			};
			View = layout;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			SizeCategory entityBind = BindingContext as SizeCategory;
			if (entityBind != null) {
				imgCheck.IsVisible = entityBind.IsCheck;
				imgUnCheck.IsVisible = !entityBind.IsCheck;
			}
		}
	}
}

