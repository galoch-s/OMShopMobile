using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OMShopMobile;
using OMShopMobile.Droid;
using Android.Graphics;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

[assembly: ExportRenderer (typeof (MyPicker), typeof (MyPickerRenderer))]
namespace OMShopMobile.Droid
{
	public class MyPickerRenderer : PickerRenderer
	{
		public void ChangeColor ()
		{
			this.Control.SetTextColor(Android.Graphics.Color.Aqua);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Picker> e)
		{
			base.OnElementChanged (e);
			if (e.NewElement != null) {
				MyPicker Content = e.NewElement as MyPicker;
				SetPlaceholderColor(Content);
				SetTextColor (Content);
				SetFontSize (Content);
				SetXAlign (Content);

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
			MyPicker Content = sender as MyPicker;

//			Control.BorderStyle = UITextBorderStyle.None;

			if (e.PropertyName == MyPicker.PlaceholderColorProperty.PropertyName) {
				SetPlaceholderColor (Content);
			}
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

		private void SetPlaceholderColor(MyPicker Content)
		{
			this.Control.SetHintTextColor (Content.PlaceholderColor.ToAndroid ());
		}

		private void SetTextColor(MyPicker Content)
		{
			this.Control.SetTextColor(Content.TextColor.ToAndroid ());
		}

		private void SetFontSize(MyPicker Content)
		{
			Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "MyriadProRegular.ttf");
			TextView label = (TextView)Control;
			label.Typeface = font;
			this.Control.TextSize = Content.FontSize;
		}

		private void SetXAlign(MyPicker Content)
		{
			this.Control.TextAlignment =  AndroidUtilits.ToUITextAlignment(Content.HorizontalTextAlignment);
		}
	}
}

