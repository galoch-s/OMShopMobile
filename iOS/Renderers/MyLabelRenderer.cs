using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using OMShopMobile;
using OMShopMobile.iOS;
using Foundation;
using System.Drawing;
using CoreGraphics;


[assembly: ExportRenderer (typeof (MyLabel), typeof (MyLabelRenderer))]
namespace OMShopMobile.iOS
{
	public class MyLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null) {
				MyLabel entity = e.NewElement as MyLabel;


				SetTextAttribute(entity);




				//NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle()
				//{
				//	LineSpacing = (nfloat)entity.LineSpacing,
				//	Alignment = this.Control.TextAlignment,
				//};

				//NSMutableAttributedString attrString = new NSMutableAttributedString(entity.Text);
				//NSString style = UIStringAttributeKey.ParagraphStyle;
				//NSRange range = new NSRange(0, attrString.Length);

				//attrString.AddAttribute(style, paragraphStyle, range);

				//this.Control.Font = UIFont.FromName("Myriad Pro", (nfloat)Content.FontSize);


				//// Ширина текста
				//CGSize size = new NSString (entity.Text).StringSize (this.Control.Font, UIScreen.MainScreen.Bounds.Width, UILineBreakMode.TailTruncation);


				//this.Control.AttributedText = attrString;
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == MyLabel.TextColorProperty.PropertyName || e.PropertyName == MyLabel.TextProperty.PropertyName) {
				SetTextAttribute((MyLabel)sender);
			}
		}

		void SetTextAttribute(MyLabel entity)
		{
			if (entity.Text == null)
				return;
			
			NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle() {
				LineSpacing = (nfloat)entity.LineSpacing,
				Alignment = this.Control.TextAlignment,
			};

			NSMutableAttributedString attrString = new NSMutableAttributedString(entity.Text);
			NSString style = UIStringAttributeKey.ParagraphStyle;
			NSRange range = new NSRange(0, attrString.Length);

			attrString.AddAttribute(style, paragraphStyle, range);
			attrString.AddAttribute(UIStringAttributeKey.ForegroundColor, entity.TextColor.ToUIColor(), range);
			if (entity.IsStrikeThrough) {
				attrString.AddAttribute(UIStringAttributeKey.StrikethroughStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);
			}

			this.Control.Font = UIFont.FromName("Myriad Pro", (nfloat)entity.FontSize);
			// Ширина текста
			CGSize size = new NSString(entity.Text).StringSize(this.Control.Font, UIScreen.MainScreen.Bounds.Width, UILineBreakMode.TailTruncation);

			this.Control.AttributedText = attrString;
		}
	}
}

