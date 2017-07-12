using System;

namespace OMShopMobile
{
	public class DeviceInfo
	{
		protected static DeviceInfo _instance;
		double width;
		double height;

		static DeviceInfo()
		{
			_instance = new DeviceInfo();
//			_instance.width = OnePage.mainPage.Width;
//			_instance.height = OnePage.mainPage.Height;
		}
		protected DeviceInfo()
		{
		}

		public static bool IsOrientationPortrait() 
		{
			return _instance.height > _instance.width;
		}

		public static void SetSize(double width, double height)
		{
			_instance.width = width;
			_instance.height = height;
		}
	}
}

