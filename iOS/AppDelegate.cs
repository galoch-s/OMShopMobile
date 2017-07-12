using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace OMShopMobile.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			FFImageLoading.Forms.Touch.CachedImageRenderer.Init();

			global::Xamarin.Forms.Forms.Init ();

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif

			App.DisplayWidth = (int)UIScreen.MainScreen.Bounds.Width;
			App.DisplayHeight = (int)UIScreen.MainScreen.Bounds.Height;

			App.Density = (float)UIScreen.MainScreen.Scale;

			App.ScreenWidth = App.DisplayWidth;
			App.ScreenHeight = App.DisplayHeight;
//
//			App.ScreenPanWidth = (width - 0.5f) / density;
//			App.ScreenPanHeight = (height - 0.5f) / density;

			App.Version = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();


			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

