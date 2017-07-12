using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class ProductGridTemplate : ContentView
	{
		public ProductGridTemplate ()
		{
		}
		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			Content = (View)BindingContext;
		}
	}
}

