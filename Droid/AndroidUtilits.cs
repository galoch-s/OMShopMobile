using System;
using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using OMShopMobile.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidUtilits))]
namespace OMShopMobile.Droid
{
	public class AndroidUtilits: IApplicationState, IKeyboardInteractions
	{
		public static Android.Views.TextAlignment ToUITextAlignment(Xamarin.Forms.TextAlignment alignment)
		{
			switch (alignment) {
			case Xamarin.Forms.TextAlignment.Start:
				return Android.Views.TextAlignment.TextStart;
			case Xamarin.Forms.TextAlignment.Center:
				return Android.Views.TextAlignment.Center;
			case Xamarin.Forms.TextAlignment.End:
				return Android.Views.TextAlignment.TextEnd;
			default:
				return Android.Views.TextAlignment.TextStart;
			}
		}

		public float GetMemory()
		{ 
			var activityManager = Android.App.Application.Context.GetSystemService(Android.App.Activity.ActivityService) as Android.App.ActivityManager;
			Android.App.ActivityManager.MemoryInfo memoryInfo = new Android.App.ActivityManager.MemoryInfo();
			activityManager.GetMemoryInfo(memoryInfo);
			return memoryInfo.TotalMem / (1024 * 1024);
		}

		public void HideKeyboard()
		{
			var inputMethodManager = Xamarin.Forms.Forms.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
			if (inputMethodManager != null && Xamarin.Forms.Forms.Context is Activity) {
				var activity = Xamarin.Forms.Forms.Context as Activity;
				var token = activity.CurrentFocus == null ? null : activity.CurrentFocus.WindowToken;
				inputMethodManager.HideSoftInputFromWindow(token, 0);
			}
		}
	}
}

