using System;

namespace OMShopMobile
{
	/// <summary>
	/// Class contain list advancedFilter for request
	/// </summary>
	public class AdvancedFiltersList
	{
		// Filter for Categories
		public const string CategoriesToParentID = @"[{{""parent_id"":{0}}}]";

		public const string CategoriesToID = @"[{{""categories_id"":{0}}}]";

		// Filter for Products
		public const string ProductToListCategoryID = @"
[
	{{""tableAlias"": ""productsAlias""}},
	{{
		""relation"": ""productsToCategories"",
		""relationAlias"": ""productsToCategoriesAlias"",
		""relationParameter"": {{""productsToCategoriesAlias.categories_id"": [{0}]}}
	}},
	["">"", ""productsAlias.products_quantity"", ""0""]
]";

		public const string ProductToListCategoryIDAndSortToProductDescrioption = @"
	{{""tableAlias"": ""productsAlias""}},
	{{ 
		""relation"":""productsDescription"", 
		""relationAlias"":""productsDescriptionAlias"", 
		""relationParameter"": """"
	}}";

		public const string ProductToListFilterProductsQuantity = @"
			["">"", ""productsAlias.products_quantity"", ""0""]
		";
		
		public const string ProductToListFilterCategories = @"
	{{ 
		""relation"":""productsToCategories"", 
		""relationAlias"":""productsToCategoriesAlias"", 
		""relationParameter"": {{""productsToCategoriesAlias.categories_id"": [{0}]}}
	}}
		";

		public const string ProductToListFilterSizes = @"
	{{
	  ""relation"": ""productsAttributes"",
	  ""relationAlias"": ""productsAttributesAlias"",
	  ""relationParameter"": [
		""and"",
		{{""productsAttributesAlias.options_values_id"": [{0}]}},
		["">"", ""productsAttributesAlias.quantity"", 0]
		]
	}}";
		public const string ProductToListFilterPriceBegin = @"["">="", ""productsAlias.products_price"", ""{0}""]";
		public const string ProductToListFilterPriceEnd = @"[""<="", ""productsAlias.products_price"", ""{0}""]";

//&advancedSort={{""productsDescriptionAlias.{1}"": ""{2}""}}";
	

		public const string ProductToID = @"
[
	{{""tableAlias"": ""productsAlias""}},
	{{""productsAlias.products_id"": [{0}]}}
]";
	
		public const string ProductToArticle = @"
[
	{{""tableAlias"": ""productsAlias""}},
	{{ 
		""relation"":""productsDescription"", 
		""relationAlias"":""productsDescriptionAlias"", 
		""relationParameter"": """"
	}},
	{{ ""productsAlias.products_model"": {0} }},
	["">"", ""productsAlias.products_quantity"", ""0""]
]";

		public const string ProductToName = @"
[
	{{""tableAlias"": ""productsAlias""}},
	{{ 
		""relation"":""productsDescription"", 
		""relationAlias"":""productsDescriptionAlias"", 
		""relationParameter"": [""like"", ""productsDescriptionAlias.products_name"", ""{0}""]
	}},
	["">"", ""productsAlias.products_quantity"", ""0""]
]";

		public const string ProductToDescription = @"
[
	{{""tableAlias"": ""productsAlias""}},
	{{ 
		""relation"":""productsDescription"", 
		""relationAlias"":""productsDescriptionAlias"", 
		""relationParameter"": [""like"", ""productsDescriptionAlias.products_description"", ""{0}""]
	}},
	["">"", ""productsAlias.products_quantity"", ""0""]
]";

		public const string ProductToNameAndArticle = @"
[
	{{""tableAlias"": ""productsAlias""}},
	{{
		""relation"":""productsDescription"", 
		""relationAlias"":""productsDescriptionAlias"", 
		""relationParameter"": """"
	}}, 
	[""or"", [""like"", ""productsDescriptionAlias.products_name"", ""{0}""],
		{{ ""productsAlias.products_model"": {0} }}
	],
	["">"", ""productsAlias.products_quantity"", ""0""]
]";

		public const string ProductToListSort = @"
		&advancedSort={{""{0}"": ""{1}""}}";

		public const string ProductInThePresence = @"
[
	{""tableAlias"": ""productsAlias""},
	{ 
		""relation"":""productsDescription"", 
		""relationAlias"":""productsDescriptionAlias"", 
		""relationParameter"": """"
	},
			["">"", ""productsAlias.products_quantity"", ""0""]]
		";
	}
}

