using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class DeliveryTemplate : ImageCell
	{
		public DeliveryTemplate ()
		{
			
		}

		protected override void OnTapped ()
		{
			this.ImageSource = Device.OnPlatform("Basket/grey_kvadrat_galochka_.png", "green_checkbox_.png", "green_checkbox_.png");
			base.OnTapped ();
		}

		protected override void OnDisappearing ()
		{
			this.ImageSource = Device.OnPlatform("Basket/grey_kvadrat_.png", "grey_checkbox_.png", "grey_checkbox_.png");
			base.OnDisappearing ();
		}


	}
}

