using OMShopMobile;
using OMShopMobile.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyScrollView), typeof(MyScrollViewRenderer))]
namespace OMShopMobile.iOS
{
	public class MyScrollViewRenderer : ScrollViewRenderer
	{
		public MyScrollViewRenderer()
		{	
			ShowsHorizontalScrollIndicator = false;
		}
	}
}