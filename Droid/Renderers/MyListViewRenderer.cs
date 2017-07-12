using System;
using Android.Graphics.Drawables;
using OMShopMobile;
using OMShopMobile.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyListView), typeof(MyListViewRenderer))]
namespace OMShopMobile.Droid
{
	public class MyListViewRenderer : ListViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null) {
				MyListView entity = e.NewElement as MyListView;
				if (!entity.HasSelection)
					this.Control.SetSelector(Resource.Drawable.abc_list_divider_mtrl_alpha);
			}
		}
	}
}