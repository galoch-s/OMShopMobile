using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OMShopMobile.Droid;
using OMShopMobile;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Graphics;
using Android.Text.Method;
using Android.Views;
using Android.Content.Res;

[assembly: ExportRenderer (typeof (MyEditor), typeof (MyEditorRenderer))]
namespace OMShopMobile.Droid
{
	public class MyEditorRenderer : EditorRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null) {
				MyEditor entity = e.NewElement as MyEditor;
				this.Control.Hint = entity.Placeholder;
				this.Control.SetHintTextColor (entity.PlaceholderColor.ToAndroid());

				ShapeDrawable shape = new ShapeDrawable(new RectShape());
				shape.Paint.Color = Xamarin.Forms.Color.Transparent.ToAndroid();
				shape.Paint.StrokeWidth = 5;
				shape.Paint.SetStyle(Paint.Style.Stroke);
				this.Control.SetBackground(shape);


				//Control.SetLines(4);
				//Control.VerticalScrollBarEnabled = true;
				//Control.MovementMethod = ScrollingMovementMethod.Instance;
				//Control.ScrollBarStyle = ScrollbarStyles.InsideInset;
				////Force scrollbars to be displayed
				//TypedArray a = Control.Context.Theme.ObtainStyledAttributes(new int[0]);
				//InitializeScrollbars(a);
				//a.Recycle();
			}
		}
	}
}

