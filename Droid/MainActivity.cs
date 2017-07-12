using System;

using Android.App;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using Android.Content.PM;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Graphics;
using Android.Views.InputMethods;
using Android.Widget;

namespace OMShopMobile.Droid
{
	[Activity (Label = "OMShopMobile.Droid", Theme="@android:style/Theme.Holo.Light", Icon = "@drawable/icon", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait,
	           ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			App.DisplayWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
			App.DisplayHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);

			App.ScreenHeight = Resources.Configuration.ScreenHeightDp;
			App.ScreenWidth = Resources.Configuration.ScreenWidthDp;

			App.Density = Resources.DisplayMetrics.Density;

			var width = Resources.DisplayMetrics.WidthPixels;
			var height = Resources.DisplayMetrics.HeightPixels;
			var density = Resources.DisplayMetrics.Density;

			App.ScreenPanWidth = (width - 0.5f) / density;
			App.ScreenPanHeight = (height - 0.5f) / density;

			LoadApplication (new App ());

			AndroidEnvironment.UnhandledExceptionRaiser +=  HandleAndroidException;


			Context context = this.ApplicationContext;
			var appVersion = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
			App.Version = appVersion;

//			ViewGroup viewGroup = (ViewGroup) ((ViewGroup) this.FindViewById(Android.Resource.Id.Content)).GetChildAt(0);
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);


		}

		//public override bool DispatchTouchEvent(Android.Views.MotionEvent ev)
		//{
		//	if (ev.Action == MotionEventActions.Down) {
		//		Android.Views.View v = CurrentFocus;
		//		if (v != null && v.GetType() != typeof(EditText)) {
		//			Rect outRect = new Rect();
		//			v.GetGlobalVisibleRect(outRect);
		//			if (!outRect.Contains((int)ev.RawX, (int)ev.RawY)) {
		//				v.ClearFocus();
		//				InputMethodManager imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
		//				imm.HideSoftInputFromWindow(v.WindowToken, 0);
		//			}
		//		}
		//	}
		//	return base.DispatchTouchEvent(ev);
		//}

		public string GetMemory()
		{ 
			var activityManager = GetSystemService(Activity.ActivityService) as ActivityManager;
			ActivityManager.MemoryInfo memoryInfo = new ActivityManager.MemoryInfo();
			activityManager.GetMemoryInfo(memoryInfo);

			double totalUsed = memoryInfo.AvailMem / (1024 * 1024);
			double totalRam = memoryInfo.TotalMem / (1024 * 1024);

			return totalUsed.ToString("f2") + "/" + totalRam.ToString("f2");
		}

		void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
		{
//			#if !DEBUG
			string methodShow = null;
			string exClass = null;
			if (e.Exception.TargetSite != null) {
				methodShow = e.Exception.TargetSite.Name;
				exClass = e.Exception.TargetSite.DeclaringType.FullName;
			}
			string userInfo = null;
			if (User.Singleton != null) {
				userInfo = "userID = " + User.Singleton.Id + ";\r\nmail=" + User.Singleton.Email + ";\r\nkey=" + User.Singleton.HashKey;
			}

			var activityManager = GetSystemService(Activity.ActivityService) as ActivityManager;
			ActivityManager.MemoryInfo memoryInfo = new ActivityManager.MemoryInfo();
			activityManager.GetMemoryInfo(memoryInfo);

			double totalUsed = memoryInfo.AvailMem / (1024 * 1024);
			double totalRam = memoryInfo.TotalMem / (1024 * 1024);

			e.Handled = true;

			//AppsLog.SendLog(new AppsLog {
			//	SystemName = Device.OS.ToString(),
			//	DeviceModel = Android.OS.Build.Model,
			//	SystemVersion = Android.OS.Build.VERSION.Sdk,
			//	ExceptionType = e.Exception.GetType().ToString(),
			//	StackTrace = e.Exception.StackTrace,
			//	Message = e.Exception.Message,
			//	AdditionalData = "",

			//	AppVersion = appVersion,
			//	AppFunction = exClass + "." + methodShow,
			//	SizeMemory = totalUsed.ToString("f2") + "/" + totalRam.ToString("f2"),
			//	TypeError = "TypeApplication",
			//	UserId = User.Singleton == null ? 0 : User.Singleton.Id,
			//	UseKey = User.Singleton?.HashKey,
			//	UrlApp = "",
			//	UrlData = "",
			//	UrlMethod = "",
			//}
			//);
			string pageHistory;
			if (OnePage.redirectApp != null)
				pageHistory = OnePage.redirectApp.GetHistoryToJson();
			else
				pageHistory = "нету";
			AppsLog.SendLog(new Dictionary<string, string> () {
				{ "system_name", Device.OS.ToString() },
				{ "device_model", Android.OS.Build.Model },
				{ "system_version", Android.OS.Build.VERSION.Sdk },
				{ "exception_type", e.Exception.GetType().ToString() },
				{ "stack_trace", e.Exception.StackTrace },
				{ "message", e.Exception.Message },
				{ "additional_data", @""" """ },
				{ "page_history", pageHistory },

				{ "app_version", App.Version },
				{ "app_function", exClass + "." + methodShow },
				{ "size_memory", totalUsed.ToString("f2") + "/" + totalRam.ToString("f2") },
				{ "type_error", "TypeApplication" },
				{ "user_id", User.Singleton == null ? "0" : User.Singleton.Id.ToString() },
				{ "user_key", User.Singleton?.HashKey },
				{ "url", "" },
				{ "url_data", "" },
				{ "url_method", "" },
			});
		}
	}
}