using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace OMShopMobile.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			try {
			UIApplication.Main (args, null, "AppDelegate");
			}
			catch (Exception ex)
			{
//				#if !DEBUG
				string methodShow = null;
				string exClass = null;
				if (ex.TargetSite != null) {
					methodShow = ex.TargetSite.Name;
					exClass = ex.TargetSite.DeclaringType.FullName;
				}
				string userInfo = null;
				if (User.Singleton != null) {
					userInfo = "userID = " + User.Singleton.Id + ";\r\nmail=" + User.Singleton.Email + ";\r\nkey=" + User.Singleton.HashKey;
				}
				string pageHistory;
				if (OnePage.redirectApp != null)
					pageHistory = OnePage.redirectApp.GetHistoryToJson();
				else
					pageHistory = "нету";
				double totalRam = NSProcessInfo.ProcessInfo.PhysicalMemory / (1024 * 1024);

				AppsLog.SendLog(new Dictionary<string, string>() {
					{ "system_name", Device.OS.ToString() },
					{ "device_model", UIDevice.CurrentDevice.Name },
					{ "system_version", UIDevice.CurrentDevice.SystemVersion },
					{ "exception_type", ex.GetType ().ToString () },
					{ "stack_trace", ex.StackTrace },
					{ "message", ex.Message },
					{ "additional_data", "''" },
					{ "page_history", pageHistory },

					{ "app_version", App.Version },
					{ "app_function", exClass + "." + methodShow },
					{ "size_memory", totalRam.ToString("f2") },
					{ "type_error", "TypeApplication" },
					{ "user_id", User.Singleton == null ? "0" : User.Singleton.Id.ToString() },
					{ "user_key", User.Singleton?.HashKey },
					{ "url", "" },
					{ "url_data", "" },
					{ "url_method", "" },
				});

				//AppsLog.SendLog (new AppsLog {
				//	SystemName = Device.OS.ToString(),
				//	DeviceModel =  UIDevice.CurrentDevice.Name,
				//	SystemVersion = UIDevice.CurrentDevice.SystemVersion,
				//	ExceptionType = ex.GetType ().ToString (),
				//	StackTrace = ex.StackTrace,
				//	Message = ex.Message,
				//	//AdditionalData = "appVersion = " + appVersion + "; " + userInfo + ";\r\nmethod=" + exClass + "." + methodShow,
				//	AppVersion = appVersion.ToString(),
				//	AppFunction = exClass + "." + methodShow,
				//	SizeMemory = totalRam.ToString("f2"),
				//	TypeError = "TypeApplication",
				//	UserId = User.Singleton == null ? 0 : User.Singleton.Id,
				//	UseKey = User.Singleton?.HashKey,
				//	UrlApp = "",
				//	UrlData = "",
				//	UrlMethod = "",
				//}
				//);
//				#endif
			}
		}
	}
}

