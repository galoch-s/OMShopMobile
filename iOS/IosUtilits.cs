using Xamarin.Forms;
using UIKit;
using OMShopMobile.iOS;
using Foundation;
using System.Drawing;

[assembly: Xamarin.Forms.Dependency(typeof(IosUtilits))]
namespace OMShopMobile.iOS
{
	public class IosUtilits: IApplicationState, IKeyboardInteractions
	{
		public static UITextAlignment ToUITextAlignment(TextAlignment alignment)
		{
			switch (alignment) {
			case TextAlignment.Start:
				return UITextAlignment.Left;
			case TextAlignment.Center:
				return UITextAlignment.Center;
			case TextAlignment.End:
				return UITextAlignment.Right;
			default:
				return UITextAlignment.Left;
			}
		}

		public float GetMemory()
		{
			return Foundation.NSProcessInfo.ProcessInfo.PhysicalMemory / (1024 * 1024);
		}

		public void HideKeyboard()
		{
			//NSObject keyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, (notification) => {
			//	UIApplication.EnsureUIThread();
			//});
			//if (keyboardHideObserver != null) {
			//	NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardHideObserver);
			//}
		}
	}
}

