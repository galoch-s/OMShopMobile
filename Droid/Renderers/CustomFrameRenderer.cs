using Android.Graphics;
using OMShopMobile;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ACanvas = Android.Graphics.Canvas;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(OMShopMobile.Droid.CustomFrameRenderer))]
namespace OMShopMobile.Droid
{
	public class CustomFrameRenderer : Xamarin.Forms.Platform.Android.FrameRenderer
	{
		CustomFrame element;

		public override void Draw(ACanvas canvas)
		{
			base.Draw(canvas);
			DrawOutline(canvas, canvas.Width, canvas.Height, 4f);//set corner radius
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			element = e.NewElement as CustomFrame;
			base.OnElementChanged(e);
		}

		void DrawOutline(ACanvas canvas, int width, int height, float cornerRadius)
		{
			using (var paint = new Paint { AntiAlias = true })
			using (var path = new Path())
			using (Path.Direction direction = Path.Direction.Cw)
			using (Paint.Style style = Paint.Style.Stroke)
			using (var rect = new RectF(0, 0, width, height)) {
				float rx = Forms.Context.ToPixels(cornerRadius);
				float ry = Forms.Context.ToPixels(cornerRadius);
				path.AddRoundRect(rect, rx, ry, direction);

				paint.StrokeWidth = element.OutlineWidth;  //set outline stroke
				paint.SetStyle(style);
				paint.Color = element.OutlineColor.ToAndroid();
				//paint.Color = Color.ParseColor("#A7AE22");//set outline color //_frame.OutlineColor.ToAndroid(); 
				canvas.DrawPath(path, paint);
			}
		}
	}
}