using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OMShopMobile
{
	public class ParamForSort
	{
		public ProductsIDSort Id { get; set; }
		public ProductsSort FieldSort { get; set; }
		public string Name { get; set; }
		public bool IsDesc { get; set; }
		public bool IsCheck { get; set; }
	}

	public class ParamSort
	{
		public static List<ParamForSort> ParamsList = new List<ParamForSort> () {
			{ new ParamForSort { Id = ProductsIDSort.products_ordered, 		FieldSort = ProductsSort.products_ordered, 		Name = "Популярности", 		IsDesc = true, IsCheck = false } },
			{ new ParamForSort { Id = ProductsIDSort.products_name, 		FieldSort = ProductsSort.products_name, 		Name = "Наименование", 		IsDesc = false, IsCheck = false } },
			{ new ParamForSort { Id = ProductsIDSort.products_date_added, 	FieldSort = ProductsSort.products_date_added, 	Name = "Новинкам", 			IsDesc = true, IsCheck = false } },
			{ new ParamForSort { Id = ProductsIDSort.products_price_decs, 	FieldSort = ProductsSort.products_price, 		Name = "Убыванию цены", 	IsDesc = true, IsCheck = false } },
			{ new ParamForSort { Id = ProductsIDSort.products_price_asc, 	FieldSort = ProductsSort.products_price, 		Name = "Возрастанию цены", 	IsDesc = false, IsCheck = false } },
		};

		public static ParamForSort oldItem;
	}
}