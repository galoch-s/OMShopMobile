using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using OMShopMobile;
using OMShopMobile.iOS;


[assembly: ExportRenderer (typeof (MyDatePicker), typeof (MyDatePickerRenderer))]
namespace OMShopMobile.iOS
{
	public class MyDatePickerRenderer : DatePickerRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
		{
			base.OnElementChanged (e);
			if (e.NewElement != null) {
				MyDatePicker Content = e.NewElement as MyDatePicker;

				SetTextColor (Content);
				SetFontSize (Content);
				SetXAlign (Content);
				SetTitle (Content);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			MyDatePicker Content = sender as MyDatePicker;

			Control.BorderStyle = UITextBorderStyle.None;

			if (e.PropertyName == MyDatePicker.TextColorProperty.PropertyName) {
				SetTextColor (Content);
			}
			if (e.PropertyName == MyDatePicker.FontSizeProperty.PropertyName) {
				SetFontSize (Content);
			}
			if (e.PropertyName == MyDatePicker.HorizontalTextAlignmentProperty.PropertyName) {
				SetXAlign (Content);
			}
			if (e.PropertyName == MyDatePicker.IsFocusedProperty.PropertyName) {
				this.Control.Text = Content.Date.ToString(Content.Format);
				this.Control.TextColor = Content.TextColor.ToUIColor();
			}
		}

		private void SetTextColor(MyDatePicker Content)
		{
			this.Control.TextColor = Content.TextColor.ToUIColor ();
		}

		private void SetFontSize(MyDatePicker Content)
		{
			Control.Font = UIFont.FromName("Myriad Pro", Content.FontSize);
		}

		private void SetXAlign(MyDatePicker Content)
		{
			this.Control.TextAlignment = IosUtilits.ToUITextAlignment (Content.HorizontalTextAlignment);
		}

		private void SetTitle(MyDatePicker Content)
		{
			if (!string.IsNullOrEmpty (Content.Title)) {
				this.Control.Text = Content.Title;
				this.Control.TextColor = Content.PlaceholderColor.ToUIColor ();
			}
		}
	}
}

