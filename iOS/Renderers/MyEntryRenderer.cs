using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using OMShopMobile;
using OMShopMobile.iOS;


[assembly: ExportRenderer (typeof (MyEntry), typeof (MyEntryRenderer))]
namespace OMShopMobile.iOS
{
	public class MyEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);
			if (e.NewElement != null) {
				MyEntry Content = e.NewElement as MyEntry;
				SetCornerRadius (Content);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			MyEntry Content = sender as MyEntry;

			Control.BorderStyle = UITextBorderStyle.None;
			Control.Layer.BorderWidth = 1;
			Control.Layer.BorderColor = Content.BackgroundColor.ToCGColor ();

			if (e.PropertyName == MyEntry.BorderRadiusProperty.PropertyName) {
				SetCornerRadius (Content);
			}
		}

		private void SetCornerRadius(MyEntry Content)
		{
			this.Control.Layer.CornerRadius = Content.BorderRadius;
		}
	}
}

