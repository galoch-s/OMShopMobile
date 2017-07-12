using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class MyEntry : Entry, IControlExtention
	{
		public static readonly BindableProperty BorderRadiusProperty =
			BindableProperty.Create<MyEntry, float>(p => p.BorderRadius, 0);
		public float BorderRadius
		{
			get { return (float)base.GetValue(BorderRadiusProperty);}
			set { base.SetValue(BorderRadiusProperty, value);}
		}

		public static readonly BindableProperty BorderWidthProperty =
			BindableProperty.Create<MyEntry, int>(p => p.BorderWidth, 0);
		public int BorderWidth
		{
			get { return (int)base.GetValue(BorderWidthProperty);}
			set { base.SetValue(BorderWidthProperty, value);}
		}

		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create<MyEntry, Color>(p => p.BorderColor, Color.Default);
		public Color BorderColor
		{
			get { return (Color)base.GetValue(BorderColorProperty);}
			set { base.SetValue(BorderColorProperty, value);}
		}

		public static readonly BindableProperty PaddingProperty =
			BindableProperty.Create<MyEntry, Thickness>(p => p.Padding, new Thickness());
		public Thickness Padding
		{
			get { return (Thickness)base.GetValue(PaddingProperty);}
			set { base.SetValue(PaddingProperty, value);}
		}
	}
}

