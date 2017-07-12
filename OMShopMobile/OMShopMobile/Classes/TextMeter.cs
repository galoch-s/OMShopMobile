//
//  TextMeter.cs
//  Created by Alexey Kinev on 11 Feb 2015.
//
//    Licensed under The MIT License (MIT)
//    http://opensource.org/licenses/MIT
//
//    Copyright (c) 2015 Alexey Kinev <alexey.rudy@gmail.com>
//
//  Usage example:
//
//    var label = new Label {
//        Text = "La-la-la-laaaaaaaaaaaaaaaaa! And even more La-la-la-laaaaa!",
//        FontSize = 16.0,
//    };
//    label.FontFamily = Device.OnPlatform (
//	      iOS: "HelveticaNeue"
//	      Android: "Roboto"
//    );
//    var labelHeight = TextMeter.MeasureTextSize(
//        label.Text, 200, label.FontSize, label.FontFamily).Height;
//

using System;
using Xamarin.Forms;

#if __IOS__

using System.Drawing;
using Foundation;
using UIKit;

namespace OMShopMobile
{
	public static class TextMeterImplementation
	{
		public static Xamarin.Forms.Size MeasureTextSize(string text, double fontSize, string fontName = null)
		{
			var nsText = new NSString(text);
//			var boundSize = new SizeF((float)width, float.MaxValue);
			var boundSize = new SizeF(float.MaxValue, float.MaxValue);
			var options = NSStringDrawingOptions.UsesFontLeading |
			NSStringDrawingOptions.UsesLineFragmentOrigin;

			if (fontName == null)
			{
				fontName = "HelveticaNeue";
			}
			fontName = "Myriad Pro";

			var attributes = new UIStringAttributes {
				Font = UIFont.FromName(fontName, (float)fontSize)
			};

			var sizeF = nsText.GetBoundingRect(boundSize, options, attributes, null).Size;

			return new Xamarin.Forms.Size((double)sizeF.Width, (double)sizeF.Height);
		}
	}
}

#endif

#if __ANDROID__

using Android.Widget;
using Android.Util;
using Android.Views;
using Android.Graphics;

namespace OMShopMobile
{
	public static class TextMeterImplementation
	{
		private static Typeface textTypeface;

		public static Xamarin.Forms.Size MeasureTextSize(string text, double fontSize, string fontName = null)
		{
			var textView = new TextView(global::Android.App.Application.Context);
			textView.Typeface = GetTypeface(fontName);
			textView.SetText(text, TextView.BufferType.Normal);
			textView.SetTextSize(ComplexUnitType.Px, (float)fontSize);

			int widthMeasureSpec = Android.Views.View.MeasureSpec.MakeMeasureSpec(
				0, MeasureSpecMode.Unspecified);
			int heightMeasureSpec = Android.Views.View.MeasureSpec.MakeMeasureSpec(
				0, MeasureSpecMode.Unspecified);

			textView.Measure(widthMeasureSpec, heightMeasureSpec);

			return new Xamarin.Forms.Size((double)textView.MeasuredWidth,
				(double)textView.MeasuredHeight);
		}

		private static Typeface GetTypeface(string fontName)
		{
			if (fontName == null)
			{
				return Typeface.Default;
			}

			if (textTypeface == null)
			{
				textTypeface = Typeface.Create(fontName, TypefaceStyle.Normal);
			}

			return textTypeface;
		}
	}
}

#endif

namespace OMShopMobile
{
	public static class TextMeter
	{
		public static Xamarin.Forms.Size MeasureTextSize(string text, double fontSize, string fontName = null)
		{
			return TextMeterImplementation.MeasureTextSize(text, fontSize, fontName);
		}
	}
}