using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class MyEditor : Editor
	{
		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create<MyEditor, string>(p => p.Placeholder, "");
		public string Placeholder
		{
			get { return (string)base.GetValue(PlaceholderProperty);}
			set { base.SetValue(PlaceholderProperty, value);}
		}

		public static readonly BindableProperty PlaceholderColorProperty =
			BindableProperty.Create<MyEditor, Color>(p => p.PlaceholderColor, Color.Gray);
		public Color PlaceholderColor
		{
			get { return (Color)base.GetValue(PlaceholderColorProperty);}
			set { base.SetValue(PlaceholderColorProperty, value);}
		}
	}
}

