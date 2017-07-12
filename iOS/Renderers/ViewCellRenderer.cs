using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using OMShopMobile.iOS;

[assembly: ExportRenderer (typeof (ViewCell), typeof (CustomViewCellRenderer))]
namespace OMShopMobile.iOS
{
	public class CustomViewCellRenderer : ViewCellRenderer
	{
		public override UITableViewCell GetCell (Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell (item, reusableCell, tv);
			
			cell.SelectedBackgroundView = new UIView {
				BackgroundColor = ApplicationStyle.GreenColor.ToUIColor(),
			};
			return cell;
		}
	}
}