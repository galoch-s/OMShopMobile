using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using OMShopMobile;
using OMShopMobile.iOS;


[assembly: ExportRenderer (typeof (MySearchBar), typeof (MySearchBarRenderer))]
namespace OMShopMobile.iOS
{
	public class MySearchBarRenderer : SearchBarRenderer
	{
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			MySearchBar Content = sender as MySearchBar;

			this.Control.BackgroundImage = new UIKit.UIImage();

			this.Control.Layer.BorderWidth = 0;
			this.Control.Layer.BorderColor = Color.Red.ToCGColor();
			this.Control.Layer.ShadowOpacity = 0;
		}
	}
}