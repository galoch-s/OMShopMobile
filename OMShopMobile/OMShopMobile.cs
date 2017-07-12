using System;
using Xamarin.Forms;
using OMShopMobile;
using OMShopMobile.Pages;

namespace OMShopMobile
{
	public class App : Application
	{
		public static int DisplayWidth;
		public static int DisplayHeight;

		public static int ScreenHeight;
		public static int ScreenWidth;
		public static double ScreenScale;

		public static double ScaleWidth;
		public static double ScaleHeight;

		public static double ScreenPanWidth;
		public static double ScreenPanHeight;

		public static string Version;	

		public static float Density;


		public App ()
		{
			Double diagonal = (float)Math.Sqrt(Math.Pow(ScreenWidth, 2) + Math.Pow(ScreenHeight, 2));
			ScaleWidth = DisplayWidth / Constants.BaseWidth;
			ScaleHeight = DisplayHeight / Constants.BaseHeight;
			ScreenScale = diagonal / Constants.BaseDiagonal;

			MainPage = new OnePage();

			//string fff = "http://api.odezhda-master.ru/api/freedata/products?expand=productsAttributesFullInfo,productsDescription&advancedFilter=[\n\t{\"tableAlias\": \"productsAlias\"},\n\t{ \n\t\t\"relation\":\"productsDescription\", \n\t\t\"relationAlias\":\"productsDescriptionAlias\", \n\t\t\"relationParameter\": \"\"\n\t},\n\t\t\t[\">\", \"productsAlias.products_quantity\", \"0\"]\n\t\t,\n\t{ \n\t\t\"relation\":\"productsToCategories\", \n\t\t\"relationAlias\":\"productsToCategoriesAlias\", \n\t\t\"relationParameter\": {\"productsToCategoriesAlias.categories_id\": [1632]}\n\t}\n\t\t]&advancedSort={\"products_date_added\": \"desc\"}&page=1&per-page=40;\r\n";
			//fff = System.Net.WebUtility.UrlDecode(fff);
			//			DateTime dt = DateTime.Now.AddDays (1);
			//			var logs = AppsLog.GetLog (DateTime.Now.AddDays (-1), DateTime.Now.AddDays (+1), "");

			//Product product = Product.GetProductsByID(1467329);
			//Utils.GetProductAvailabilitySchedule(product.SchedulesList);

		}
		protected override void OnPropertyChanged (string propertyName)
		{
			base.OnPropertyChanged (propertyName);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			if (OnePage.mainPage == null) {
				MainPage = new OnePage ();
			} else {
				OnePage.mainPage.ValidInternetConnection ();
			}
			// Handle when your app resumes
		}
	}
}

