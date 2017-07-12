using System;

namespace OMShopMobile
{
	/// <summary>
	/// Class contain list expand for request
	/// </summary>
	public static class ExpandList
	{
		// Param expand for Categories
		public const string CategoriesDescription = "categoriesDescription";
		public const string CategoriesChildren = "childrenCategoriesDesc";

		// Param expand for Products
		public const string ProductsDescription = "productsDescription";
		public const string ProductsAttributesFullInfo = "productsAttributesFullInfo";
		public const string ProductsSchedule = "schedule";
		public const string ProductsExpress = "express";

		// Param expand for Zone
		public const string ZoneCountry = "country";

		// Param expand for Basket
		public const string BasketProductName = "productName";
//		public const string BasketProductDescription = "productDescriptionText";
		public const string BasketSizeName = "sizeName";
//		public const string BasketProductQuantity = "productQuantity";
//		public const string BasketProductSizeQuantity = "productSizeQuantity";
//		public const string BasketProductActualQuantity = "productActualQuantity";


		// Param expand for Order
		public const string OrdersProductsWithAttributes = "ordersProductsWithAttributes";
		public const string OrderBookkeepingID = "orderBookkeepingID";
		public const string OrderShippingForCustomer = "shippingForCustomer";

		// Param expand for Order
		public const string OrderCustomer = "shipping";
		public const string OrderSum = "productsSum";
		public const string OrderPositionsCount = "positionsCount";
	}
}

