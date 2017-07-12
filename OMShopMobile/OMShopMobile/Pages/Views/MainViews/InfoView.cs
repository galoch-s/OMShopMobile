using System;
using Xamarin.Forms;
using System.Collections.Generic;
//using Android.Graphics.Drawables;

namespace OMShopMobile
{
	public class InfoView : ContentView
	{
		public InfoView ()
		{
			List<InfoPage> infoPageList = new List<InfoPage> () {
				new InfoPage { Number = NumberInfoPage.About, Name = "О компании" },
				new InfoPage { Number = NumberInfoPage.Conacts, Name = "Контакты" },
				new InfoPage { Number = NumberInfoPage.Delivery, Name = "Доставка" },
				new InfoPage { Number = NumberInfoPage.Payment, Name = "Оплата" },
			};

			VerticalOptions = LayoutOptions.FillAndExpand;

			ListView listSection = new ListView {
				ItemTemplate = new DataTemplate(typeof(ListCellTemplate)),
				VerticalOptions = LayoutOptions.Start,
				ItemsSource = infoPageList
			};
			listSection.ItemSelected += OnSelectPage;

			StackLayout mainLayout = new StackLayout { 
				Children = {
					listSection 
				}
			};
			Content = mainLayout;
		}

		void OnSelectPage (object sender, SelectedItemChangedEventArgs e)
		{
			InfoPage infoPage = e.SelectedItem as InfoPage;

			History hist = OnePage.redirectApp.GetCurrentTransition ();
			OnePage.redirectApp.AddTransition (hist.Page, infoPage.Name);

			switch (infoPage.Number) {
			case NumberInfoPage.About:
				Content = new AboutInfoView ();
				break;
			case NumberInfoPage.Conacts:
				Content = new ConactsInfoView();
				break;
			case NumberInfoPage.Delivery:
				Content = new DeliveryInfoView ();
				break;
			case NumberInfoPage.Payment:
				Content = new PaymentInfoView ();
				break;
			default:
				break;
			}
		}
	}
}

