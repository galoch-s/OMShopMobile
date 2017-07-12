using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OMShopMobile;
using OMShopMobile.Droid;
using Android.Graphics;
using Android.Widget;


[assembly: ExportRenderer (typeof (Label), typeof (MyLabelRenderer))]
[assembly: ExportRenderer (typeof (MyLabel), typeof (MyLabelRenderer))]
namespace OMShopMobile.Droid
{
	public class MyLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);
			if (e.NewElement != null) {
				Label Content = e.NewElement as Label;


				Typeface font;
				switch (Content.FontAttributes) {
				case FontAttributes.Bold:
					font = Typeface.CreateFromAsset (Forms.Context.Assets, "MyriadProBold.ttf");
					break;
				case FontAttributes.Italic:
					font = Typeface.CreateFromAsset (Forms.Context.Assets, "MyriadProItalic.ttf");
					break;
				default:
					font = Typeface.CreateFromAsset (Forms.Context.Assets, "MyriadProRegular.ttf");
					break;
				}
				TextView label = (TextView)Control;
				label.Typeface = font;

				MyLabel myLabel = e.NewElement as MyLabel;
				if (myLabel != null) {
					if (myLabel.IsStrikeThrough) { 
						this.Control.PaintFlags = this.Control.PaintFlags | PaintFlags.StrikeThruText;
					}
					if (myLabel.IsUnderline) {
						this.Control.PaintFlags = this.Control.PaintFlags | PaintFlags.UnderlineText;
					}
					this.Control.SetLineSpacing (1f, myLabel.LineSpacing);
				}
			}
		}
	}
}

