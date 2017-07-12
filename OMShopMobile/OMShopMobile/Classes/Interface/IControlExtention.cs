using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public interface IControlExtention
	{
		Color TextColor { get; set; }

		string Placeholder { get; set; }

		bool IsEnabled { get; set; }

		LayoutOptions VerticalOptions { get; set; }
	}
}

