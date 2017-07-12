using System;

namespace OMShopMobile
{
	public class OperationEventArgs : EventArgs
	{
		public readonly bool IsError;
		public OperationEventArgs(bool isError)
		{
			IsError = isError;
		}
	}
}

