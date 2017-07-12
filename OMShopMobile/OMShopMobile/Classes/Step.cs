using System;

namespace OMShopMobile
{
	public enum HistoryStep : short
	{
		// Login
		Default = 0,
		MyOrders,
		StatusOrders,
		InfoOrder,
		ProductOrder,
		OrdersList,

		// Product
		TableSizes,
		TableSizeDescription,
		FilterProduct,
	}
}

