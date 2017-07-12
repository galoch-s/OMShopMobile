using System;
using System.ComponentModel;
//using MonoTouch.UIKit;
using Xamarin.Forms;

using Xamarin.Forms.Platform.iOS;
using OMShopMobile.CustomControls;
using OMShopMobile.iOS.Controls;
using UIKit;

[assembly: ExportRenderer(typeof(CustomRadioButton), typeof(RadioButtonRenderer))]

namespace OMShopMobile.iOS.Controls
{
	/// <summary>
	/// The Radio button renderer for iOS.
	/// </summary>
	public class RadioButtonRenderer : ViewRenderer<CustomRadioButton, RadioButtonView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<CustomRadioButton> e)
		{
			base.OnElementChanged(e);

//			BackgroundColor = Element.BackgroundColor.ToUIColor();

			if (Control == null)
			{
//				var checkBox = new RadioButtonView(Bounds);
				var checkBox = new RadioButtonView();
				checkBox.TouchUpInside += (s, args) => Element.Checked = Control.Checked;

				SetNativeControl(checkBox);
			}

			if (e.NewElement != null) {
				Control.LineBreakMode = UILineBreakMode.CharacterWrap;
				Control.VerticalAlignment = UIControlContentVerticalAlignment.Top;
				Control.Text = e.NewElement.Text;
				Control.Checked = e.NewElement.Checked;
				Control.SetTitleColor (e.NewElement.TextColor.ToUIColor (), UIControlState.Normal);
				Control.SetTitleColor (e.NewElement.TextColor.ToUIColor (), UIControlState.Selected);
			}
		}

		private void ResizeText()
		{
			var text = this.Element.Text;


			var bounds = this.Control.Bounds;

			var width = this.Control.TitleLabel.Bounds.Width;

			var height = text.StringHeight(this.Control.Font, (float)width);

			var minHeight = string.Empty.StringHeight(this.Control.Font, (float)width);

			var requiredLines = Math.Round(height / minHeight, MidpointRounding.AwayFromZero);

			var supportedLines = Math.Round(bounds.Height / minHeight, MidpointRounding.ToEven);


			bounds.Height += 43;
			this.Control.Bounds = bounds;
			this.Element.HeightRequest = 43;
			this.Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			this.Control.Font = UIFont.FromName("Myriad Pro", 14);
			this.Element.TextColor = ApplicationStyle.TextColor;
			this.Element.BackgroundColor = Color.White;

//			if (supportedLines != requiredLines)
//			{
//				bounds.Height += (float)(minHeight * (requiredLines - supportedLines));
//				this.Control.Bounds = bounds;
//				this.Element.HeightRequest = bounds.Height;
//			}
		}

		public override void Draw (CoreGraphics.CGRect rect)
		{
			base.Draw (rect);
			this.ResizeText();
		}

//		public override void Draw(System.Drawing.RectangleF rect)
//		{
//			base.Draw(rect);
//			this.ResizeText();
//		}



		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			switch (e.PropertyName)
			{
			case "Checked":
				Control.Checked = Element.Checked;
				break;
			case "Text":
				Control.Text = Element.Text;
				break;
			case "TextColor":
				Control.SetTitleColor(Element.TextColor.ToUIColor(), UIControlState.Normal);
				Control.SetTitleColor(Element.TextColor.ToUIColor(), UIControlState.Selected);
				break;
			case "Element":
				break;
			default:
				System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
				return;
			}
		}
	}
}