using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace OMShopMobile
{
	public partial class ProductInCategoryTemplateXaml : FastGridCell
	{
		float imgHeight;
		float imgWidth;

		protected override void InitializeCell()
		{
			InitializeComponent();
		}

		protected override void SetupCell(bool isRecycled)
		{
			imgHeight = (int)Utils.GetSize(App.Density * 140, 1);// 133;
			imgWidth = (int)Utils.GetSize(App.Density * 180, 1);// 166;

			Product product = BindingContext as Product;
			if (product != null) {
				ImageView.Source = Constants.PathToPreviewImage + product.Image + "&h=" + imgHeight + "&w=" + imgWidth;
			//	UserThumbnailView.ImageUrl = mediaItem.ImagePath ?? "";
			//	ImageView.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
			//	NameLabel.Text = mediaItem.Name;
			//	DescriptionLabel.Text = mediaItem.Description;
			}
		}
	}
}
