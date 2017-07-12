using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Serialization;

namespace OMShopMobile
{
	public class AppsLog
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("creat_date")]
		public DateTime CreatDate { get; set; }

		[JsonProperty("system_name")]
		public string SystemName { get; set; }

		[JsonProperty("device_model")]
		public string DeviceModel { get; set; }

		[JsonProperty("system_version")]
		public string SystemVersion { get; set; }

		[JsonProperty("exception_type")]
		public string ExceptionType { get; set; }

		[JsonProperty("stack_trace")]
		public string StackTrace { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("additional_data")]
		public string AdditionalData { get; set; }

		[JsonProperty("page_history")]
		public string PageHistory { get; set; }

		[JsonProperty("app_version")]
		public string AppVersion { get; set; }

		[JsonProperty("app_function")]
		public string AppFunction { get; set; }

		[JsonProperty("size_memory")]
		public string SizeMemory { get; set; }

		[JsonProperty("type_error")]
		public string TypeError { get; set; }

		[JsonProperty("user_id")]
		public int UserId { get; set; }

		[JsonProperty("user_key")]
		public string UseKey { get; set; }

		[JsonProperty("url")]
		public string UrlApp { get; set; }

		[JsonProperty("url_data")]
		public string UrlData { get; set; }

		[JsonProperty("url_method")]
		public string UrlMethod { get; set; }


		//public static List<AppsLog> GetLog(DateTime dateBegin, DateTime dateEnd, string exceptions)
		//{	
		//	string dates = string.Format("[\"{0}\", \"{1}\"]",dateBegin.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd"));
		//	Dictionary<string, string> arrParam = new Dictionary<string, string> { 
		//		{"dates", dates },
		//		{"exceptions", exceptions }
		//	}; 
		//	string url = WebRequestUtils.GetPathToAddParam(Constants.PathToLog, arrParam);
		//	ContentAndHeads contentAndHeads = WebRequestUtils.GetJsonAndHeads (url);

		//	string json = contentAndHeads.Content[0];
		//	List<AppsLog> arrLogs = new List<AppsLog>();
		//	arrLogs.AddRange (JsonConvert.DeserializeObject<List<AppsLog>> (json));

		//	return arrLogs;
		//}

		//public static void AddLog(AppsLog log)
		//{
		//	Dictionary<string, string> arrParam = new Dictionary<string, string> { 
		//		{"system_name", log.SystemName },
		//		{"device_model", log.DeviceModel },
		//		{"system_version", log.SystemVersion },
		//		{"exception_type", log.ExceptionType },
		//		{"stack_trace", log.StackTrace },
		//		{"message", log.Message },
		//		{"additional_data", log.AdditionalData },
		//	}; 
		//	string url = Constants.PathToLog;
		//	string data = string.Join ("&", arrParam.Select (x => x.Key + "=" + x.Value));
		//	WebRequestUtils.GetJsonAndHeads (url, "POST", data);
		//}

		public static async void AddLogAsync(AppsLog log)
		{
			Dictionary<string, string> arrParam = new Dictionary<string, string> {
				{"system_name", log.SystemName },
				{"device_model", log.DeviceModel },
				{"system_version", log.SystemVersion },
				{"exception_type", log.ExceptionType },
				{"stack_trace", log.StackTrace },
				{"message", log.Message },
				{"additional_data", log.AdditionalData },
			};
			string url = Constants.PathToLog;
			string data = string.Join("&", arrParam.Select(x => x.Key + "=" + x.Value));
			await WebRequestUtils.GetJsonAndHeadsAsync(url, "POST", data);
		}

		public static async void SendLog(Dictionary<string, string> appsLog)
		{
			string s = DictionaryToPostString(appsLog);
			string url = Constants.PathToLog;

			ContentAndHeads contentAndHeads = null;
			contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url, "POST", s, true);
			if (contentAndHeads.requestStatus != HttpStatusCode.OK && contentAndHeads.requestStatus != HttpStatusCode.Created) {
				throw new Exception();
			}
		}

		public static async void SendLog(AppsLog appsLog)
		{
			Dictionary<string, string> dicLog = new Dictionary<string, string>() {
				{ "system_name", appsLog.SystemName },
				{ "device_model", appsLog.DeviceModel },
				{ "system_version", appsLog.SystemVersion },
				{ "exception_type", appsLog.ExceptionType },
				{ "stack_trace", appsLog.StackTrace },
				{ "message", appsLog.Message },
				{ "additional_data", appsLog.AdditionalData },
				{ "page_history", appsLog.PageHistory },

				{ "app_version", appsLog.AppVersion },
				{ "app_function", appsLog.AppFunction },
				{ "size_memory", appsLog.SizeMemory },
				{ "type_error", appsLog.TypeError },
				{ "user_id", appsLog.UserId.ToString() },
				{ "user_key", appsLog.UseKey },
				{ "url", appsLog.UrlApp },
				{ "url_data", appsLog.UrlData },
				{ "url_method", appsLog.UrlMethod },
			};

			string data = DictionaryToPostString(dicLog);
			string url = Constants.PathToLog;

			ContentAndHeads contentAndHeads = null;
			contentAndHeads = await WebRequestUtils.GetJsonAndHeadsAsync(url, "POST", data, true);
			if (contentAndHeads.requestStatus != HttpStatusCode.OK && contentAndHeads.requestStatus != HttpStatusCode.Created) {
				throw new Exception();
			}
		}

		public static string DictionaryToPostString(Dictionary<string, string> postVariables)
		{
			string postString = "";
			foreach (KeyValuePair<string, string> pair in postVariables) {
				postString += System.Net.WebUtility.UrlEncode(pair.Key) + "=" +
					System.Net.WebUtility.UrlEncode(pair.Value) + "&";
			}
			return postString;
		}

		public class ExcludeContentKeyContractResolver : DefaultContractResolver
		{
			protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
			{
				IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
				return properties.Where(p => p.PropertyName != "id" && p.PropertyName != "creat_date").ToList();
			}
		}
	}
}