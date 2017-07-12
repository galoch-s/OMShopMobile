using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class MySearchBar : SearchBar
	{
		public static readonly BindableProperty BorderWidthProperty =
			BindableProperty.Create<MySearchBar, float>(p => p.BorderWidth, 1f);
		public float BorderWidth
		{
			get { return (float)base.GetValue(BorderWidthProperty);}
			set { base.SetValue(BorderWidthProperty, value);}
		}
	}
}

