using System;
using System.Collections.Concurrent;
using System.Net;

namespace OMShopMobile
{
	public class CancelableRequest
	{ 
		public HttpWebRequest ParamRequest { get; set; }
		public bool IsProduct { get; set; }
	}

	public class AppRequest
	{
		BlockingCollection<CancelableRequest> RequestList = new BlockingCollection<CancelableRequest>();

		public AppRequest()
		{
			
		}

		public void Add(CancelableRequest request)
		{
			lock (RequestList) {
				RequestList.Add(request);
			}
		}

		public void Remove(CancelableRequest request)
		{
			lock (RequestList) {
				RequestList.TryTake(out request);
			}
		}

		public bool AbortAll()
		{
			bool isCancelable = false;
			lock (RequestList) {
				if (RequestList == null) return isCancelable;
				foreach (var request in RequestList) {
					try {
						isCancelable = true;
						request.ParamRequest.Abort();
					} catch (WebException webEx) {
						if (webEx.Status != WebExceptionStatus.RequestCanceled) {

						}
					} catch (Exception ex) {

					}

				}
			}
			return isCancelable;
		}
	}
}

