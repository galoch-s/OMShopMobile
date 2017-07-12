using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class ImageView : ContentView
	{
		Image image;
		Label lblArticle;
		Button btnPlus;
		Button btnMinus;

		double imgScaleDefault = Math.Round((0.4 * App.ScaleHeight / 1.5), 2);

		public event EventHandler eventPinch;

		public ImageView (string imgSource = "")
		{
			VerticalOptions = LayoutOptions.FillAndExpand;
			HorizontalOptions = LayoutOptions.FillAndExpand;

			image = new Image { 
				WidthRequest = (1000 * App.ScaleHeight), HeightRequest = (1000 * App.ScaleHeight),
				Scale = imgScaleDefault,
				TranslationX = -(300 * App.ScaleHeight),
				TranslationY = -(200 * App.ScaleHeight),
			};
			lblArticle = new Label { Margin = new Thickness(8) };
			GestureLayout gestureLayout = new GestureLayout (eventPinch) {
				BackgroundColor = Color.Black,
				Content = image,
			};

			Image imgBack = new Image { 
				Source = Device.OnPlatform("Catalog/green_delete_.png", "cancelfoto.png", "cancelfoto.png"),
			};
			TapGestureRecognizer tapBack = new TapGestureRecognizer ();
			imgBack.GestureRecognizers.Add (tapBack);
			tapBack.Tapped += OnClickBack;

			btnPlus = new Button { 
				BackgroundColor = Color.White,
				Style = ApplicationStyle.ButtonCountStyle,
				Text = "+" 
			};
			btnPlus.Clicked += (sender, e) => { 
				if (image.Scale < 1)
				image.Scale += 0.2;  
			};
			btnMinus = new Button { 
				BackgroundColor = Color.White,
				Style = ApplicationStyle.ButtonCountStyle,
				Text = "-" };
			btnMinus.Clicked += (sender, e) => { 
				if (image.Scale > 0.4)
				image.Scale -= 0.2; 
			};
				
			Content = new Grid {
				Children = {
					new AbsoluteLayout {
						Children = { 
							gestureLayout
						}
					},
					new Frame { 
						VerticalOptions = LayoutOptions.CenterAndExpand,
						HorizontalOptions = LayoutOptions.EndAndExpand,
						Content = new StackLayout {
							Children = {
								btnPlus,
								btnMinus
							}
						}
					},
					new Frame {
						VerticalOptions = LayoutOptions.Start,
						HorizontalOptions = LayoutOptions.EndAndExpand,
						Content = new StackLayout {
							Children = {
								imgBack
							}
						}
					},
					lblArticle
				}
			};
		}

		void OnClickBack (object sender, EventArgs e)
		{
			OnePage.redirectApp.BackToHistory ();
		}

		public void Show(string imgSource, string article)
		{
			image.TranslationX = -(300 * App.ScaleHeight);
			image.TranslationY = -(200 * App.ScaleHeight);
			image.Scale = imgScaleDefault;

			image.Source = imgSource;
			lblArticle.Text = "Артикул " + article;
		}
	}
}