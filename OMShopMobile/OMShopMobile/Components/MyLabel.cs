using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class MyLabel : Label
	{
		public static readonly BindableProperty LineSpacingProperty =
			BindableProperty.Create<MyLabel, float>(p => p.LineSpacing, 1);
		public float LineSpacing
		{
			get	{ return (float)GetValue(LineSpacingProperty); }
			set	{ SetValue(LineSpacingProperty, value);	}
		}

		public static readonly BindableProperty IsUnderlineProperty =
			BindableProperty.Create<MyLabel, bool>(p => p.IsUnderline, false);
		public bool IsUnderline
		{
			get	{ return (bool)GetValue(IsUnderlineProperty); }
			set	{ SetValue(IsUnderlineProperty, value);	}
		}

		public static readonly BindableProperty IsStrikeThroughProperty =
			BindableProperty.Create<MyLabel, bool>(p => p.IsStrikeThrough, false);
		public bool IsStrikeThrough {
			get { return (bool)GetValue(IsStrikeThroughProperty); }
			set { SetValue(IsStrikeThroughProperty, value); }
		}
	}
}

