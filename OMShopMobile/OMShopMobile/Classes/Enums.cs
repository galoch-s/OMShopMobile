using System;

using Xamarin.Forms;

namespace OMShopMobile
{
	public enum PageName : short
	{
		Main = 1,
		Catalog,
		Login,
		Basket,
		Order,
		Info,
		Search,
		Image,
	}

	public enum CatalogEnum : short
	{
		All = 0,
		Men = 1668,
		Women = 1632,
		Accessory = 932
	}

	public enum NumberInfoPage : short
	{
		Delivery = 1,
		Conacts = 2,
		Payment = 3,
		About = 4
	}

	public enum ProductsIDSort : short
	{
		default_value = 0,
		products_ordered,
		products_name,
		products_date_added,
		products_price_decs,
		products_price_asc,
	}

	public enum ProductsSort : short
	{
		default_value = 0,
		products_price,
		products_date_added,
		products_name,
		products_ordered
	}

	public enum TypeFieldExtension : short
	{
		Entry = 0,
		Picker = 1,
		DatePicker = 2
	}

	public enum ColunmHirtoryOrder : short
	{
		NumberOrder = -1,
		Date = 0,
		Sum = 1,
//		NumberAccount,
		Count,
		StatusString,
		Show,
	}

	public enum TypeNewsProduct : short
	{ 
		ClothingAndShoes,
		OtherProducts
	}
}