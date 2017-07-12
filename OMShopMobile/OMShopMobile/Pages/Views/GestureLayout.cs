using System;
using Xamarin.Forms;
using OMShopMobile;

namespace OMShopMobile
{
	public class GestureLayout : ContentView
	{
//		double currentScale = 1;
//		double startScale = 1;
//		double xOffset = 0;
//		double yOffset = 0;

		double x = -300, y = -200;


		public GestureLayout (EventHandler eventPinch)
		{
			// Set PanGestureRecognizer.TouchPoints to control the 
			// number of touch points needed to pan
			var panGesture = new PanGestureRecognizer ();
			panGesture.PanUpdated += OnPanUpdated;
			GestureRecognizers.Add (panGesture);

//			eventPinch += OnPinch;
		}

//		void OnPinch (object sender, EventArgs e)
//		{
////			this.Scale
//
//			currentScale += (this.Scale - 1) * startScale;
//			currentScale = Math.Max (1, currentScale);
//
//			// The ScaleOrigin is in relative coordinates to the wrapped user interface element,
//			// so get the X pixel coordinate.
//			double renderedX = Content.X + xOffset;
//			double deltaX = renderedX / Width;
//			double deltaWidth = Width / (Content.Width * startScale);
//			double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;
//
//			// The ScaleOrigin is in relative coordinates to the wrapped user interface element,
//			// so get the Y pixel coordinate.
//			double renderedY = Content.Y + yOffset;
//			double deltaY = renderedY / Height;
//			double deltaHeight = Height / (Content.Height * startScale);
//			double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;
//
//			// Calculate the transformed element pixel coordinates.
//			double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
//			double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);
//
//			// Apply translation based on the change in origin.
//			Content.TranslationX = targetX.Clamp (-Content.Width * (currentScale - 1), 0);
//			Content.TranslationY = targetY.Clamp (-Content.Height * (currentScale - 1), 0);
//
//			// Apply scale factor
//			Content.Scale = currentScale;
//		}

		void OnPanUpdated (object sender, PanUpdatedEventArgs e)
		{
			double startX = -(Content.Width * this.Content.Scale - Content.Width) / 2;
			double startY = -(Content.Height * this.Content.Scale - Content.Height) / 2;



			double width = Content.Width * this.Content.Scale;
			double height = Content.Height * this.Content.Scale;

			switch (e.StatusType) {

			case GestureStatus.Running:
				// Translate and ensure we don't pan beyond the wrapped user interface element bounds.
//				double begX = Math.Min (Math.Abs(startX), Math.Abs(startX - (x + e.TotalX)));
//				double enX = -Math.Abs (width - App.ScreenPanWidth);
//
//				Content.TranslationX = Math.Max (begX, enX);
//
//
//				double begY = Math.Min (Math.Abs(startY), Math.Abs(startY - (y + e.TotalY)));
//				double enY = -Math.Abs (height - App.ScreenPanHeight);
//				Content.TranslationY = Math.Max (begY, enY);

//				if ((x + e.TotalX) < startX)
//					Content.TranslationX = startX;
//				else
//					Content.TranslationX = x + e.TotalX;
//				if ((x + e.TotalX) > Content.Width * this.Scale)
//					Content.TranslationX = Content.Width * this.Scale - Content.Width;
//
//
//
//				if ((y + e.TotalY) < startY)
//					Content.TranslationY = startY;
//				else
//					Content.TranslationY = y + e.TotalY;
//				if ((y + e.TotalY) > Content.Height * this.Scale)
//					Content.TranslationY = Content.Height * this.Scale - Content.Height;

				Content.TranslationX = Math.Max (Math.Min (0, x + e.TotalX), -Math.Abs (Content.Width - App.ScreenWidth));
				Content.TranslationY = Math.Max (Math.Min (0, y + e.TotalY), -Math.Abs (Content.Height - App.ScreenHeight));	
                break;
			
			case GestureStatus.Completed:
				// Store the translation applied during the pan
				x = Content.TranslationX;
				y = Content.TranslationY;
				break;
			}
		}
	}
}
