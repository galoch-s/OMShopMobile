using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class CategoryPage : ContentPage
	{
		public CategoryPage ()
		{
			Label lbl1 = new Label();
			lbl1.SetBinding (Label.TextProperty, "CategoriesID");

			Label lbl2 = new Label();
			lbl2.SetBinding (Label.TextProperty, "CategoryDescription.Name");


			ContentView viewTop = new ContentView ();
			viewTop.BackgroundColor = Color.Red;
			viewTop.Content = lbl1;

			ContentView viewMain = new ContentView ();
			viewMain.BackgroundColor = Color.Aqua;
			viewMain.Content = lbl2;






			Content = new StackLayout {
				Children = {
					viewTop,
					viewMain
				}				
			};


//			Content = new StackLayout {
//				Children = {
//					lbl
//				}
//			};
		}
	}
}

