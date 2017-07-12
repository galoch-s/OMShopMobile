using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OMShopMobile;
using OMShopMobile.Droid;
using Android.Graphics;
using Android.Widget;


[assembly: ExportRenderer (typeof (Xamarin.Forms.Button), typeof (MyButtonRenderer))]
[assembly: ExportRenderer (typeof (MyButton), typeof (MyButtonRenderer))]
namespace OMShopMobile.Droid
{
	public class MyButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Button> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null) {
				MyButton Content = e.NewElement as MyButton;

				Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "MyriadProRegular.ttf");
				TextView label = (TextView)Control;
				label.Typeface = font;

				this.Control.TransformationMethod = null;
				this.Control.SetPadding (0, 0, 0, 0);

				if (e.NewElement is MyButton) {
					if (Content.IsUnderline)
						this.Control.PaintFlags = this.Control.PaintFlags | PaintFlags.UnderlineText;

					if (Content.UseWithTextBox)
						this.Control.SetBackgroundResource(Resource.Drawable.btn1);
				}
			}
		}
	}
}