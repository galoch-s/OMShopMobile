using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class MyPicker : Picker, IControlExtention, IPickerExtention
	{
		public static readonly BindableProperty HorizontalTextAlignmentProperty =
			BindableProperty.Create<MyPicker, TextAlignment>(p => p.HorizontalTextAlignment, TextAlignment.Center);
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
			BindableProperty.Create<MyPicker, Color>(p => p.TextColor, Color.Default);
		public Color TextColor
		{
			get { return (Color)base.GetValue(TextColorProperty);}
			set { base.SetValue(TextColorProperty, value);}
		}

		public static readonly BindableProperty PlaceholderColorProperty =
			BindableProperty.Create<MyPicker, Color>(p => p.PlaceholderColor, Color.Default);
		public Color PlaceholderColor
		{
			get { return (Color)base.GetValue(PlaceholderColorProperty);}
			set { base.SetValue(PlaceholderColorProperty, value);}
		}

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create<MyPicker, float>(p => p.FontSize, 14f);
		public float FontSize
		{
			get { return (float)base.GetValue(FontSizeProperty); }
			set { base.SetValue(FontSizeProperty, value); }
		}

		public string Placeholder {
			get { return this.Title; }
			set { this.Title = value; }
		}
	}
}

