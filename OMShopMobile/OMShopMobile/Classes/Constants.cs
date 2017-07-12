using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public static class Constants
	{
		public const string HostWork = @"http://api.odezhda-master.ru";
		public const string HostTestOld = @"http://svk.rezerv.odezhda-master.ru";
		public const string HostTest = @"http://testapi2.odezhda-master.ru";

//		public const string Hos tTest = "http://192.168.0.43";
		//public const string Host = @"http://api.odezhda-master.ru";

		//@"http://svk.rezerv.odezhda-master.ru/api/freedata/categories?expand=categoriesDescription,childrenCategoriesDesc&advancedFilter=[{""parent_id"":0}]";
		//@"http://svk.rezerv.odezhda-master.ru/api/freedata/categories?expand=categoriesDescription,&advancedFilter=[{""parent_id"":0}]";
//		public const string UrlCategories = @"/api/freedata/categories?expand=categoriesDescription";
//		public const string UrlProducts = @"/api/freedata/products?expand=productsDescription,productsAttributesFullInfo&
//[
//  {{""tableAlias"": ""productsAlias""}},
//  {{
//    ""relation"": ""productsToCategories"",
//    ""relationAlias"": ""productsToCategoriesAlias"",
//    ""relationParameter"": {{""productsToCategoriesAlias.categories_id"": [{0}]}}
//  }}
		//]&weDebugging=1
//";

		//		http://svk.rezerv.odezhda-master.ru/api/freedata/products?expand=productsDescription,productsAttributesFullInfo&advancedFilter=
//		[{"tableAlias": "productsAlias"},{ "relation":"productsDescription", "relationAlias":"productsDescriptionAlias", "relationParameter":"" },
//		[">", "productsAlias.products_quantity", "0"]]&advancedSort={"productsDescriptionAlias.products_name": "asc"}

		public const string UrlCategories = @"/api/freedata/categories";
		public const string UrlProducts = @"/api/freedata/products";
		public const string PathToProductsNewsClothes = @"/api/freedata/novelties-clothing-and-shoes";
		public const string PathToProductsNewsOthes = @"/api/freedata/novelties-other-products";


		//public const string PathToPreviewImage = @"/api/image/preview?url=";
		public static string PathToPreviewImage = HostWork + @"/api/image?url=";
		public static string PathToImage = HostWork + @"/images/";

		public const string PathToLogin = @"/api/customers/login";
		public const string PathToregistration = @"/api/customers/registration";
		public const string PathToPersonalData = @"/api/customers/personal-data";
		public const string PathToZone = @"/api/zones/default";

		public const string PathToBasketAdd = @"/api/customers/personal-data/basket-add-to";
		public const string PathToBasket = @"/api/customers/personal-data/basket";
		public const string PathToOrderFromBasket = @"/api/customers/personal-data/order-from-basket";
		public const string PathToDeleteFromBasket = @"/api/customers/personal-data/basket-delete-from/{0}";

		public const string PathToListDelivery = @"/api/customers/personal-data/shipping-methods";
		public const string PathToPasswordReset = @"/api/customers/personal-data/password-reset";
		public const string PathToOrders = @"/api/customers/personal-data/orders";
		public const string PathToOrderPosition = @"/api/customers/personal-data/order/{0}/positions";
		public const string PathToOrderStatus = @"/api/customers/personal-data/orders-statuses";
		public const string PathToOrderCountStatus = @"/api/customers/personal-data/orders-status/{0}/orders-count";
		public const string PathToSizeArticle = @"/api/freedata/sizes-articles";
		public const string PathToSizeCategory = @"/api/freedata/category/{0}/sizes";

		public const string PathToCoupon = @"/api/customers/personal-data/validate-coupon/{0}";


		public const string PathToLog = @"http://mobile.odezhda-master.ru/api/applog";

		public static readonly int HeightRowListView = Utils.GetSize(43);

		public const int HeightiPhone5 = 568;
		public const int MinSumBasket = 1500;
		public const int MinFirstSumBasket = 5000;

		public const int PerPageCategory = 50;
		public const int MaxCountCategoryInAPI = 300;
		public const int RequestTimeOut = 5000;

		public const double ImageWidthToHeight = 0.75;
		public const int ContentPaddingWidth = 5;
		public const int ContentPaddingHeight = 0;

		public const double BaseDiagonal = 652;
		public const double BaseWidth = 320;
		public const double BaseHeight = 568;

		public const string RegexEmail = @"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$";
		public const string RegexPhone = @"^[0-9()\-+ ]+$";
		public const string RegexStringRU = @"^[а-яА-ЯёЁ ]+$";
		public const string RegexNumber = @"^[0-9]+$";


		// Height Blocks
		public static readonly int HeightButtom = Utils.GetSize(49);
		public static readonly int HeightTopMain = Utils.GetSize(44);

		public static readonly int HeightCaruselIndicator = 28;// (int)(28 * App.ScaleHeight) -30;
		public static readonly int TopPaddingNewProductLayout = Utils.GetSize(7, 1);// (int)(7 * App.ScaleHeight);
		public static readonly int TopPaddingNewProductList = Utils.GetSize(10, 1); //(int)(10 * App.ScaleHeight);
		public static readonly int HeightProductNewDescription = Utils.GetSize(34, 1);// (int)(34 * App.ScaleHeight);
		public static readonly int HeightTitleNew = Utils.GetSize(16); //(int)(16 * App.ScaleHeight);

		public static string ErrorRegistration = "для «Customers Email Address» уже занято";
		//Authorization: Basic MTIzQHlhLnlhOk9EQXlNMk0zWkdRME9UY3pPVFl3Tmpsak1UUXpOV05rTlRFMk5UVXdNMlE2WlRBPQ==
		//Authorization: Basic "email":"base64(key)"

		public const string HeaderAppKey = "appToken";
		public const string HeaderAppValue = "WHDyw5tOdxzL13NjLo0ITa310vQcvLhN";
	}

	public static class XPagination
	{
		public const string TotalCount = "X-Pagination-Total-Count";
		public const string PageCount = "X-Pagination-Page-Count";
		public const string CurrentPage = "X-Pagination-Current-Page";
		public const string PerPage = "X-Pagination-Per-Page";

		public static int CountProduct = 40;
	}

	public static class SortDirection
	{
		public const string Min = " (min)";
		public const string Max = " (max)";
	}

	public static class BannerConstant
	{
		public const int BannerGroup = 2;

		public const string Url = "http://mobile.odezhda-master.ru";

		public const string BanersToCoeffCList = "/api/banners/{0}/images/{1}";
		public const string BanersList = "/api/banners/{0}/images/";
	}
}