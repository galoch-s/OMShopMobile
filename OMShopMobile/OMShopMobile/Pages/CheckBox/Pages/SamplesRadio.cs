using System;

using Xamarin.Forms;
using OMShopMobile.CustomControls;
using OMShopMobile.ViewModel;

namespace OMShopMobile
{
	public class SamplesRadio : ContentPage
	{
		public SamplesRadio ()
		{
			Padding = new Thickness (20);
			RadioGroupDemoViewModel rr = new RadioGroupDemoViewModel ();


			BindableRadioGroup dd = new BindableRadioGroup {
				ItemsSource= rr.MyList.Values,

//					SelectedIndex="{Binding SelectedIndex}" VerticalOptions="Start" />	
			};


			Content = new StackLayout { 
				Children = {
					dd
				}
			};
		}
	}
}


