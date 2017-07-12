using System;
using System.Collections.Generic;
using System.Linq;
using OMShopMobile;
using Xamarin.Forms;

namespace CustomLayouts.ViewModels
{
	//public class BannerData : BaseViewModel, ITabProvider
	//{
	//	public string Title { get; set; }
	//	public int CategoryID { get; set; }
	//	public string ImageSource { get; set; }
	//}

	public class BannerViewModel : BaseViewModel
	{
		public BannerViewModel()
		{
			Show();
			//Pages = new List<BannerData>() {
			//	new BannerData { Title = "Джемперы, пуловеры и свитеры", CategoryID = 1845, ImageSource = Device.OnPlatform("Banner/OM_07092016_1.png", "OM_07092016_1.png", "OM_07092016_1.png") },
			//	new BannerData { Title = "Полусапожки", CategoryID = 1987, ImageSource = Device.OnPlatform("Banner/OM_07092016_2.png", "OM_07092016_2.png", "OM_07092016_2.png") },
			//	new BannerData { Title = "Блузы", CategoryID = 1729, ImageSource = Device.OnPlatform("Banner/OM_07092016_3.png", "OM_07092016_3.png", "OM_07092016_3.png") },
			//	new BannerData { Title = "Женские сумки, рюкзаки", CategoryID = 2047, ImageSource = Device.OnPlatform("Banner/OM_07092016_4.png", "OM_07092016_4.png", "OM_07092016_4.png") },
			//	new BannerData { Title = "Демисезонные куртки", CategoryID = 1748, ImageSource = Device.OnPlatform("Banner/OM_07092016_5.png", "OM_07092016_5.png", "OM_07092016_5.png") },
			//	new BannerData { Title = "Корм для животных", CategoryID = 2764, ImageSource = Device.OnPlatform("Banner/OM_07092016_6.png", "OM_07092016_6.png", "OM_07092016_6.png") }
			//};
			//CurrentPage = Pages.First();
		}

		async void Show()
		{
			if (OnePage.bannerList == null) {
				OnePage.bannerList = await Banner.GetProductsByIDAsync();
			}
			Pages = OnePage.bannerList;
			CurrentPage = Pages.First();
		}

		IEnumerable<Banner> _pages;
		public IEnumerable<Banner> Pages {
			get {
				return _pages;
			}
			set {
				SetObservableProperty (ref _pages, value);
				CurrentPage = Pages.FirstOrDefault ();
			}
		}

		Banner _currentPage;
		public Banner CurrentPage {
			get {
				return _currentPage;
			}
			set {
				SetObservableProperty (ref _currentPage, value);
			}
		}
	}
}