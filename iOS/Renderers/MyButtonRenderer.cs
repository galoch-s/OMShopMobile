using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using OMShopMobile;
using OMShopMobile.iOS;
using Foundation;
using CoreGraphics;

[assembly: ExportRenderer (typeof (MyButton), typeof (MyButtonRenderer))]
namespace OMShopMobile.iOS
{
	public class MyButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null) {
				MyButton entity = e.NewElement as MyButton;

				NSMutableAttributedString attrString = new NSMutableAttributedString(entity.Text);
				NSRange range = new NSRange(0, attrString.Length);

				if (entity.IsUnderline)
					attrString.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);
				attrString.AddAttribute(UIStringAttributeKey.ForegroundColor, entity.TextColor.ToUIColor(), range);

				this.Control.Font = UIFont.FromName("Myriad Pro", (nfloat)entity.FontSize);
				this.Control.SetAttributedTitle(attrString, Control.State);
			}
		}
	}
}