using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class MyButton : Button
	{
		public static readonly BindableProperty IsUnderlineProperty =
			BindableProperty.Create<MyButton, bool>(p => p.IsUnderline, false);
		public bool IsUnderline
		{
			get	{ return (bool)GetValue(IsUnderlineProperty); }
			set	{ SetValue(IsUnderlineProperty, value);	}
		}

		public static readonly BindableProperty UseWithTextBoxProperty =
			BindableProperty.Create<MyButton, bool>(p => p.UseWithTextBox, false);
		public bool UseWithTextBox {
			get { return (bool)GetValue(UseWithTextBoxProperty); }
			set { SetValue(UseWithTextBoxProperty, value); }
		}
	}
}