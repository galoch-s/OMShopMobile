using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OMShopMobile;
using OMShopMobile.Droid;
using Android.Graphics;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;


[assembly: ExportRenderer (typeof (MyDatePicker), typeof (MyDatePickerRenderer))]
namespace OMShopMobile.Droid
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

				ShapeDrawable shape = new ShapeDrawable(new RectShape());
				shape.Paint.Color = Xamarin.Forms.Color.Transparent.ToAndroid ();
				shape.Paint.StrokeWidth = 5;
				shape.Paint.SetStyle(Paint.Style.Stroke);
				this.Control.SetBackground(shape);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			MyDatePicker Content = sender as MyDatePicker;

			//			this.Control.BorderStyle = UITextBorderStyle.None;

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
				this.Control.SetTextColor(Content.TextColor.ToAndroid());
			}
		}

		private void SetTextColor(MyDatePicker Content)
		{
			this.Control.SetTextColor(Content.TextColor.ToAndroid());
		}

		private void SetFontSize(MyDatePicker Content)
		{
			Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "MyriadProRegular.ttf");
			TextView label = (TextView)Control;
			label.Typeface = font;
			this.Control.TextSize = Content.FontSize;
		}

		private void SetXAlign(MyDatePicker Content)
		{
			this.Control.TextAlignment = AndroidUtilits.ToUITextAlignment (Content.HorizontalTextAlignment);
		}

		private void SetTitle(MyDatePicker Content)
		{
			if (!string.IsNullOrEmpty (Content.Title)) {
				this.Control.Text = Content.Title;
				this.Control.SetTextColor(Content.PlaceholderColor.ToAndroid ());
			}
		}
	}
}

