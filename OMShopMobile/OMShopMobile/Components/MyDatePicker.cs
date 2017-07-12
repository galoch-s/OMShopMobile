using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class MyDatePicker : DatePicker, IControlExtention
	{
		public static readonly BindableProperty HorizontalTextAlignmentProperty =
			BindableProperty.Create<MyDatePicker, TextAlignment>(p => p.HorizontalTextAlignment, TextAlignment.Center);
		/// <summary>
		/// Aling horizontal
		/// </summary>
		/// <value>The X align.</value>
		public TextAlignment HorizontalTextAlignment
		{
			get { return (TextAlignment)base.GetValue(HorizontalTextAlignmentProperty); }
			set { base.SetValue(HorizontalTextAlignmentProperty, value); }
		}

		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create<MyDatePicker, Color>(p => p.TextColor, Color.Default);
		public Color TextColor
		{
			get { return (Color)base.GetValue(TextColorProperty);}
			set { base.SetValue(TextColorProperty, value);}
		}

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create<MyDatePicker, float>(p => p.FontSize, 14f);
		public float FontSize
		{
			get { return (float)base.GetValue(FontSizeProperty); }
			set { base.SetValue(FontSizeProperty, value); }
		}

		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create<MyDatePicker, string>(p => p.Title, "");
		public string Title
		{
			get { return (string)base.GetValue(TitleProperty); }
			set { base.SetValue(TitleProperty, value); }
		}

		public static readonly BindableProperty PlaceholderColorProperty =
			BindableProperty.Create<MyDatePicker, Color>(p => p.PlaceholderColor, Color.Black);
		public Color PlaceholderColor
		{
			get { return (Color)base.GetValue(PlaceholderColorProperty); }
			set { base.SetValue(PlaceholderColorProperty, value); }
		}

		public string Placeholder {
			get { return this.Title; }
			set { this.Title = value; }
		}
	}
}

