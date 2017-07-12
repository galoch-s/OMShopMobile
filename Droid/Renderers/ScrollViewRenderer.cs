using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OMShopMobile;
using OMShopMobile.Droid;
using Android.Graphics;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using System.Reflection;
using System.ComponentModel;

[assembly: ExportRenderer (typeof (MyScrollView), typeof (MyScrollViewRenderer))]
namespace OMShopMobile.Droid
{
	public class MyScrollViewRenderer : ScrollViewRenderer
	{
		HorizontalScrollView _scrollView;

		protected override void OnElementChanged (VisualElementChangedEventArgs e)
		{
			base.OnElementChanged (e);
			if(e.NewElement == null) return;

			e.NewElement.PropertyChanged += ElementPropertyChanged;
		}

		void ElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
			try {
				if (e.PropertyName == "Renderer") {
					if (this.ChildCount > 0) {
						_scrollView = (HorizontalScrollView)GetChildAt(0);
					}
					//_scrollView = (HorizontalScrollView)typeof(ScrollViewRenderer)
					//	.GetField ("hScrollView", BindingFlags.NonPublic | BindingFlags.Instance)
					//	.GetValue (this);
					_scrollView.HorizontalScrollBarEnabled = false;
				}

			} catch (ObjectDisposedException) {
			} catch (Exception) { 
			}
		}
	}
}

