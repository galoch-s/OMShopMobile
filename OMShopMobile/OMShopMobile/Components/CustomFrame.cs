using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class CustomFrame : Frame
	{
		public static readonly BindableProperty OutlineWidthProperty =
			BindableProperty.Create<CustomFrame, float>(p => p.OutlineWidth, 1);
		public float OutlineWidth {
			get { return (float)GetValue(OutlineWidthProperty); }
			set { SetValue(OutlineWidthProperty, value); }
		}
	}
}