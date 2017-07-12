using System;
using OMShopMobile;
using OMShopMobile.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyListView), typeof(MyListViewRenderer))]
namespace OMShopMobile.iOS
{
	public class MyListViewRenderer : ListViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null) {
				// Unsubscribe from event handlers and cleanup any resources
			}
			if (e.NewElement != null) {
				MyListView entity = e.NewElement as MyListView;
				if (!entity.HasSelection)
				// Configure the native control and subscribe to event handlers
					Control.AllowsSelection = false;
			}
		}
	}
}