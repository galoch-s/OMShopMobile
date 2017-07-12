using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Linq;
using System.Threading;

namespace OMShopMobile
{
//	X-Pagination-Total-Count
//	X-Pagination-Page-Count
//	X-Pagination-Current-Page
//	X-Pagination-Per-Page
	public static class WebRequestUtils
	{
		private static HttpWebResponse response;


		#region GetUrl
		public static string GetUrlPage (string url, int pageCurr, int itemCount)
		{
			string separator = url.Contains ("?") ? "&" : "?";
			string strPageCurr = separator + "page=" + pageCurr;
			string strItemCount = "&per-page=" + itemCount;
			url += strPageCurr + strItemCount;
			return url;
		}

		public static string GetUrl (string path, int pageCurr, int itemCount)
		{
			string url = GetUrl(path);
			string separator = url.Contains ("?") ? "&" : "?";
			string strPageCurr = separator + "page=" + pageCurr;
			string strItemCount = "&per-page=" + itemCount;
			url += strPageCurr + strItemCount;
			return url;
		}

		public static string GetUrl (string path, string expand, string advancedFilters, int pageCurr, int itemCount)
		{
			string url = GetUrl (path, expand, advancedFilters);
			string strPageCurr = "&page=" + pageCurr;
			string strItemCount = "&per-page=" + itemCount;
			url += strPageCurr + strItemCount;
			return url;
		}

		public static string GetUrl (string path, string expand, string advancedFilters, string advancedSort = "")
		{
			string url = GetUrl(path);
			string expandUrl = url.Contains ("?") ? "&" : "?";
			expandUrl += "expand=" + expand;
			if (advancedFilters != string.Empty)
				advancedFilters = "&advancedFilter=" + advancedFilters;
			if (advancedSort != string.Empty)
				advancedSort = "&advancedSort=" + advancedSort;
			url += expandUrl + advancedFilters + advancedSort;
			return url;
		}

		public static string GetUrl(string path, string expand, string advancedFilters, string advancedSort, int pageCurr, int itemCount)
		{
			string url = GetUrl(path, expand, advancedFilters, advancedSort);
			return GetUrlPage(url, pageCurr, itemCount);
		}

		public static string GetUrl (string path)
		{
			return Constants.HostWork + path;
		}

		public static string GetPathToAddParam(string path, Dictionary<string, string> arrParam)
		{
			string separator = path.Contains ("?") ? "&" : "?";
			return path + separator + string.Join ("&", arrParam.Select (x => x.Key + "=" + x.Value));
		}
		#endregion 

		//********** Methods Async *************

		public static async Task<List<string>> GetJsonsAllPageAsync(string url)
		{
			List<string> jsonsList = new List<string>();
			int pageCount = 0;
			int pageCurrent = 0;
			do {
				pageCurrent++;
				string paramPage = url.Contains("?") ? "&" : "?";
				paramPage += "page=" + pageCurrent;
				url += paramPage;

				jsonsList.Add(await GetJsonAsync(url));
				if (response == null)
					return null;
				int.TryParse(response.Headers[XPagination.PageCount], out pageCount);
				int.TryParse(response.Headers[XPagination.CurrentPage], out pageCurrent);
			} while (pageCurrent < pageCount);
			return jsonsList;
		}

		public static async Task<string> GetJsonAsync(string url)
		{
			#if DEBUG
			// Посчитать разницу по времени
			DateTime oldDate = DateTime.Now;
			Console.WriteLine("********************** url :" + url);
			#endif

			HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
			request.ContentType = "application/json";
			request.Method = "GET";
			request.Timeout = 100000;
			request.Headers.Add(Constants.HeaderAppKey, Constants.HeaderAppValue);
			try {
				request.Timeout = Constants.RequestTimeOut;
				using (response = await request.GetResponseAsync() as HttpWebResponse) {
					if (response.StatusCode != HttpStatusCode.OK)
						new Exception("Не удалось получить данные!");

					using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
						#if DEBUG
						DateTime newDate = DateTime.Now;
						TimeSpan ts = newDate - oldDate;
						Console.WriteLine("###################### Total minutes :" + ts.TotalMilliseconds + " url = " + url);
						#endif

						return await reader.ReadToEndAsync();
					}
				}
			} catch (WebException ex) {
				Console.WriteLine("*******  ERROR  *****");
				string messageFromServer = ex.Message;
				//ev.error = messageFromServer;
				if (ex.Response == null) {
					throw new Exception();
				}
				string result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

				SendLog(ex, result, request.Method, url, null);
				throw new Exception();
				//				OnePage.mainPage.DisplayMessage ("Не удалось подключиться к серверу!");
			} catch (Exception e) {
				Console.WriteLine("*******  ERROR  *****", e);
				OnePage.mainPage.DisplayMessage("Не удалось подключиться к серверу!");
			}
			return null;
		}

		public static async Task<ContentAndHeads> GetJsonsAndHeadsAllPageAsync(string url)
		{
			ContentAndHeads contentAndHeadsList = new ContentAndHeads ();
			contentAndHeadsList.Content = new List<string> ();

			int pageCount = 0;
			int pageCurrent = 0;
			do {
				pageCurrent++;
				string urlPage = url;
				string paramPage = url.Contains("?") ? "&" : "?";
				paramPage += "page=" + pageCurrent;
				paramPage += "&per-page=" + Constants.PerPageCategory;
				urlPage += paramPage;

				ContentAndHeads contentAndHeads = await GetJsonAndHeadsAsync(urlPage);
				if (contentAndHeads == null)
					return null;
				if (contentAndHeads.requestStatus != System.Net.HttpStatusCode.OK)
					throw new Exception();

				pageCount = contentAndHeads.countPage;
				pageCurrent = contentAndHeads.currentPage;
				if (contentAndHeadsList.Content != null && contentAndHeads.Content != null)
					contentAndHeadsList.Content.AddRange(contentAndHeads.Content);
			} while (pageCurrent < pageCount);
			return contentAndHeadsList;
		}

		public static async Task<ContentAndHeads> GetJsonAndHeadsAsync(string url, string method = "GET", string data = null, bool isLog = false, bool 
		                                                               isCancelable = false, bool isCatalog = false, bool isBanner = false)
		{
			//			if (!OnePage.mainPage.ValidInternetConnection ())
			//				return null;

#if DEBUG
			// Посчитать разницу по времени
			DateTime oldDate = DateTime.Now;
			Console.WriteLine("********************** method :" + method);
			Console.WriteLine("********************** url :" + url);
			if (method != "GET") {
				Console.WriteLine("********************** data :" + data);
			}
#endif
			CancelableRequest cancelableRequest = null;
			ContentAndHeads сontentAndHeads = new ContentAndHeads();

			HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
			request.ContentType = "application/json";
			request.Method = method;


			//OnePage.mainPage.appRequest.AbortAll();
			if (isCancelable) {
				OnePage.mainPage.appRequest.Add(cancelableRequest = new CancelableRequest { ParamRequest = request, IsProduct = isCatalog });
			}
			try {
			if (method != "GET") {
				request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
					if (data != null) {
						using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream())) {
							streamWriter.Write(data);
							streamWriter.Flush();
							streamWriter.Close();
						}

						//byte[] byteArray = Encoding.UTF8.GetBytes(data);
						//request.ContentLength = byteArray.Length;
						//Stream dataStream = request.GetRequestStream();
						//dataStream.Write(byteArray, 0, byteArray.Length);
						//dataStream.Close();
					}
			}

			//			if (!string.IsNullOrEmpty (header))
			//			{
			//				byte[] data = Encoding.UTF8.GetBytes(header);
			//				request.ContentLength = data.Length;
			//				using (var stream = request.GetRequestStream())
			//				{
			//					stream.Write(data, 0, data.Length);
			//				}
			//			}
			string _key;
			if (isBanner) {
				_key = Convert.ToBase64String(Encoding.UTF8.GetBytes("alex.gal:qazwsx"));
				request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + _key);
			}else if (isLog) {
				_key = Convert.ToBase64String(Encoding.UTF8.GetBytes("codetekteam:Samsung86GaL"));
				request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + _key);
			} else {
				if (User.Singleton != null && User.Singleton.IsAuth) {
					_key = Convert.ToBase64String(Encoding.UTF8.GetBytes(User.Singleton.Email + ":" + User.Singleton.HashKey));
					request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + _key);
				}
			}
			request.Headers.Add(Constants.HeaderAppKey, Constants.HeaderAppValue);

			int pageCount = 0;
			int pageCurrent = 0;

			request.Timeout = Constants.RequestTimeOut;


				using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse) {
					if (response.StatusCode != HttpStatusCode.OK)
						new Exception("Не удалось получить данные!");

					int.TryParse(response.Headers[XPagination.PageCount], out pageCount);
					int.TryParse(response.Headers[XPagination.CurrentPage], out pageCurrent);

					using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
						string str = await reader.ReadToEndAsync();


#if DEBUG
						DateTime newDate = DateTime.Now;
						TimeSpan ts = newDate - oldDate;
						Console.WriteLine("###################### Total minutes :" + ts.TotalMilliseconds + " url = " + url);
#endif

						сontentAndHeads.Content = new List<string> { str };
						сontentAndHeads.countPage = pageCount;
						сontentAndHeads.currentPage = pageCurrent;
						сontentAndHeads.requestStatus = response.StatusCode;
						return сontentAndHeads;
					}

				}
			} catch (WebException ex) {
				if (ex.Status != WebExceptionStatus.RequestCanceled) {
					if (ex.Status == WebExceptionStatus.ProtocolError) {
						HttpWebResponse response = ex.Response as HttpWebResponse;
						if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
							return сontentAndHeads;
						//if (response != null && response.StatusCode == HttpStatusCode.NotFound)
						//	return сontentAndHeads;
					}
					if (ex.Response == null) {
						сontentAndHeads.requestStatus = HttpStatusCode.InternalServerError;
						throw new Exception();
					}
					string result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
					сontentAndHeads.Content = new List<string> { result };

					SendLog(ex, result, method, url, data);
					Console.WriteLine("*******  ERROR  *****");
					string messageFromServer = ex.Message;
					//ev.error = messageFromServer;
					string title = "Ошибка";
					string message = "Не удалось подключиться к серверу!";


					Console.WriteLine("Web Error: " + result);
					message = "Не удалось подключиться к серверу!";
					message = "";
					JObject jobjectMess = null;
					APIException aPIException = null;
					try {
						aPIException = JsonConvert.DeserializeObject<APIException>(result);
						сontentAndHeads.serverException = aPIException;
						if (aPIException != null) {
							string mes = aPIException.Message.Replace("\\\"", "");
							jobjectMess = JObject.Parse(mes);
						}
					} catch (JsonReaderException) {
						jobjectMess = null;
					}
					if (jobjectMess != null) {
						try {
							title = (string)jobjectMess["errorPlace"];
							JObject errorData = (JObject)jobjectMess["errorData"];
							foreach (var o in errorData) {
								message += o.Value[0].Value<string>() + " ";
							}
							message = message.Trim();
						} catch {
							message = "Запрос не выполнился, попробуйте снова";
						}
					} else {
						if (aPIException != null && !string.IsNullOrEmpty(aPIException.Message)) {
							message = aPIException.Message;
						}
					}
					//var json = System.Json.JsonValue.Parse(message);
					// Выводит сообщение от АПИ
#if DEBUG
					Console.WriteLine("Error sql: " + title);
#else
				//OnePage.mainPage.DisplayMessage ("Произошла ошибка на сервере", title);
#endif
					//				}
				} else {
					сontentAndHeads.exceptionStatus = WebExceptionStatus.RequestCanceled;
					Console.WriteLine("WebExceptionStatus.RequestCanceled");
					сontentAndHeads.requestStatus = HttpStatusCode.OK;
					return сontentAndHeads;
				}
			} catch (FormatException e) {

			} catch (ObjectDisposedException e) { 
				Console.WriteLine("*******  ERROR ObjectDisposedException  *****", e);
				сontentAndHeads.requestStatus = HttpStatusCode.OK;
				return сontentAndHeads;
			} catch (Exception e) {
				Console.WriteLine("*******  ERROR  *****", e);
				//				OnePage.mainPage.DisplayMessage ("Не удалось подключиться к серверу!");
				OnePage.mainPage.ShowMessageError();
			} finally { 
				if (isCancelable)
					OnePage.mainPage.appRequest.Remove(cancelableRequest);
			}

			сontentAndHeads.requestStatus = HttpStatusCode.InternalServerError;
			return сontentAndHeads;
		}

		static void SendLog(WebException ex, string messResult, string method, string url, string data)
		{
			//			string method = request.Method;
			//			string url = request.RequestUri.OriginalString;
			//			string data = null;
			if (method == "GET") {
				data = null;
			}
			string message;
			if (ex.Response != null)
				message = messResult;
				//message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
			else
				message = ex.Message;

			string methodShow = null;
			string exClass = null;
			if (ex.TargetSite != null) {
				methodShow = ex.TargetSite.Name;
				exClass = ex.TargetSite.DeclaringType.FullName;
			}
			string pageHistory;
			if (OnePage.redirectApp != null)
				pageHistory = OnePage.redirectApp.GetHistoryToJson();
			else
				pageHistory = "нету";
			AppsLog log = new AppsLog {
				SystemName = Device.OS.ToString(),
				ExceptionType = ex.GetType().ToString(),
				StackTrace = ex.StackTrace,
				Message = message,
				AdditionalData = @"""  """,
				PageHistory = pageHistory,
				AppVersion = App.Version,
				AppFunction = exClass + "." + methodShow,
				TypeError = "TypeWeb",
				UserId = User.Singleton == null ? 0 : User.Singleton.Id,
				UseKey = User.Singleton?.HashKey,
				UrlApp = url,
				UrlMethod = method,
				UrlData = data
			};
			#if __ANDROID__
			var activityManager = Android.App.Application.Context.GetSystemService(Android.App.Activity.ActivityService) as Android.App.ActivityManager;
			Android.App.ActivityManager.MemoryInfo memoryInfo = new Android.App.ActivityManager.MemoryInfo();
			activityManager.GetMemoryInfo(memoryInfo);
			double totalUsed = memoryInfo.AvailMem / (1024 * 1024);
			double totalRam = memoryInfo.TotalMem / (1024 * 1024);

			log.DeviceModel = Android.OS.Build.Model;
			log.SystemVersion = Android.OS.Build.VERSION.Sdk;
			log.SizeMemory = totalUsed.ToString("f2") + "/" + totalRam.ToString("f2");
			#elif __IOS__
			log.DeviceModel = UIKit.UIDevice.CurrentDevice.Name;
			log.SystemVersion = UIKit.UIDevice.CurrentDevice.SystemVersion;
			log.SizeMemory = (Foundation.NSProcessInfo.ProcessInfo.PhysicalMemory / (1024 * 1024)).ToString("f2");
			#endif
			AppsLog.SendLog(log);
		}
	}
}