using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace OMShopMobile
{
	public class ContentAndHeads
	{
		public HttpStatusCode requestStatus;
		public WebExceptionStatus exceptionStatus;
		public List<string> Content;
		public List<Product> productsList;

		public int countPage;
		public int currentPage;

		public List<Order> ContentList;

		public APIException serverException;
	}

	public class ContentAndHeads<T>
	{
		public int countPage;
		public int currentPage;

		public string MessageError;
		public List<T> ContentList;
	}
}	