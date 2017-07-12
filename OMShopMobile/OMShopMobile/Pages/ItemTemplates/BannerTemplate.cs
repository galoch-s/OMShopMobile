using System;
using Xamarin.Forms;
using OMShopMobile;
using CustomLayouts.ViewModels;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomLayouts
{
	public class BannerTemplate : ContentView
	{
		Banner entity;
		Image img;

		public BannerTemplate()
		{
			img = new Image {
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};


			img.SetBinding(Image.SourceProperty, "Image.Path");

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					img
				}
			};

			TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += CategoryClick;

			tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, "CategoryID");

			this.GestureRecognizers.Add(tapGestureRecognizer);
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			entity = BindingContext as Banner;
			img.Source = BannerConstant.Url + "/" + entity.Image.Path;
		}

		async void CategoryClick(object sender, EventArgs e)
		{
			int catId;
			string strCat = null;
			if (entity.Url.MaskKey.ContainsKey("cat_id"))
				strCat = entity.Url.MaskKey["cat_id"];
			if (strCat != null && int.TryParse(strCat, out catId)) {
				OnePage.redirectApp.RedirectToPage(PageName.Catalog, true, false, false);

				Category catCopy = new Category { ID = catId };
				catCopy = Category.GetCategoryInTree2(OnePage.categoryList, catCopy);
				if (catCopy == null)
					catCopy = await Category.GetegoryByIDAsync(catId);
				if (catCopy != null)
					OnePage.redirectApp.catalogView.GoToCategory(catCopy, null, false);
			}
			//catCopy catCopy = new catCopy {
			//	ID = (int)eTapped.Parameter,
			//	Description = new CategoriesDescription { Name = item.Title }
			//};
		}
	}
}