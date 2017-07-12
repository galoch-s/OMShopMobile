using System;

using Xamarin.Forms;

namespace OMShopMobile
{
	public class MyButtonProperties 
	{
		public MyButtonProperties (string text, string imageSource)
		{
			Text = text;
			ImageSource = imageSource;
		}

		public string Text { get; set;}

		public string ImageSource { get; set;}
	}
}


