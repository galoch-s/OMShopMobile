using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using OMShopMobile;
using OMShopMobile.iOS;


[assembly: ExportRenderer (typeof (MyPicker), typeof (MyPickerRenderer))]
namespace OMShopMobile.iOS
{
	public class MyPickerRenderer : PickerRenderer
	{
		public void ChangeColor ()
		{
			this.Control.TextColor = UIColor.Black;
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Picker> e)
		{
			base.OnElementChanged (e);
			if (e.NewElement != null) {
				MyPicker Content = e.NewElement as MyPicker;
				SetTextColor (Content);
				SetFontSize (Content);
				SetXAlign (Content);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			MyPicker Content = sender as MyPicker;

			Control.BorderStyle = UITextBorderStyle.None;

			if (e.PropertyName == MyPicker.TextColorProperty.PropertyName) {
				SetTextColor (Content);
			}
			if (e.PropertyName == MyPicker.FontSizeProperty.PropertyName) {
				SetFontSize (Content);
			}
			if (e.PropertyName == MyPicker.HorizontalTextAlignmentProperty.PropertyName) {
				SetXAlign (Content);
			}
		}

		private void SetTextColor(MyPicker Content)
		{
			this.Control.TextColor = Content.TextColor.ToUIColor ();
		}

		private void SetFontSize(MyPicker Content)
		{
			Control.Font = UIFont.FromName("Myriad Pro", Content.FontSize);
		}

		private void SetXAlign(MyPicker Content)
		{
			this.Control.TextAlignment = IosUtilits.ToUITextAlignment(Content.HorizontalTextAlignment);
		}
	}
}

