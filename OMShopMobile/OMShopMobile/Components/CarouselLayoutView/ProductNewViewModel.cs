using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using OMShopMobile;

namespace CustomLayouts.ViewModels
{
	public class ProductNewData : BaseViewModel, ITabProvider
	{
		public string Title { get; set; }
		public Color Background { get; set; }
		public string ImageSource { get; set; }
	}

	public class ProductNewViewModel : BaseViewModel
	{
		public ProductNewViewModel(List<Product> productList, TapGestureRecognizer tapProduct, int countProduct)
		{
//			int countProduct = 4;
//			List<Product> productList = OnePage.productNewList;

			Pages = new List<Grid> ();

			if (productList == null)
				return;
			
			for (int j = 0; j < productList.Count/countProduct; j++) {
				Grid gridProduct = new Grid {
//					Padding = new Thickness (0, 10, 0, 0),
					RowSpacing = 10,
					ColumnSpacing = 10,
//					HeightRequest = 201
				};
				gridProduct.LayoutChanged += (sender, e) => {
					
				};

				int ind = j * countProduct;
				for (int i = 0; i < countProduct; i++) {
					ProductNewTemplate product = new ProductNewTemplate (
						productList[ind+i]
					);
					product.GestureRecognizers.Add (tapProduct);
					gridProduct.Children.Add (product, i, 0);
				}
				Pages.Add (gridProduct);
			}
			CurrentPage = Pages.First();


		}

		List<Grid> _pages;
		public List<Grid> Pages {
			get {
				return _pages;
			}
			set {
				SetObservableProperty (ref _pages, value);
				CurrentPage = Pages.FirstOrDefault ();
			}
		}

		Grid _currentPage;
		public Grid CurrentPage {
			get {
				return _currentPage;
			}
			set {
				SetObservableProperty (ref _currentPage, value);
			}
		}
	}
}