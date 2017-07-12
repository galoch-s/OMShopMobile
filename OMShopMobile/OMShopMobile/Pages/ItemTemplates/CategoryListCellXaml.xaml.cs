using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OMShopMobile
{
	public partial class CategoryListCellXaml : ViewCell
	{
		public CategoryListCellXaml()
		{
			InitializeComponent();

			lblName.TextColor = ApplicationStyle.TextColor;
		}
	}
}
