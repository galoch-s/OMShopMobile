using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OMShopMobile;
using OMShopMobile.Droid;
using Android.Graphics;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using System.Threading;
using Android.Views.InputMethods;
using Android.App;
using Android.Views;
using Android.InputMethodServices;


[assembly: ExportRenderer (typeof (MyEntry), typeof (MyEntryRenderer))]
namespace OMShopMobile.Droid
{
	public class MyEntryRenderer : EntryRenderer
	{

		private bool _inititialized = false;

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			//(Forms.Context as Android.App.Activity).Window.SetSoftInputMode(Android.Views.SoftInput.AdjustNothing );

			base.OnElementChanged (e);

			//(Forms.Context as Android.App.Activity).Window.SetSoftInputMode(Android.Views.SoftInput.AdjustNothing );

			if (e.NewElement != null) {
				MyEntry Content = e.NewElement as MyEntry;
				SetCornerRadius (Content);

				Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "MyriadProRegular.ttf");
				TextView label = (TextView)Control;
				label.Typeface = font;

				ShapeDrawable shape = new ShapeDrawable (new RectShape ());
				shape.Paint.Color = Xamarin.Forms.Color.Transparent.ToAndroid ();
				shape.Paint.StrokeWidth = 5;
				shape.Paint.SetStyle (Paint.Style.Stroke);
				this.Control.SetBackground (shape);

				int paddingTop = Content.Padding.Top == 0 ? this.Control.PaddingTop : (int)Content.Padding.Top;
				int paddingBottom = Content.Padding.Bottom == 0 ? this.Control.PaddingBottom : (int)Content.Padding.Bottom;
				int paddingLeft = Content.Padding.Left == 0 ? this.Control.PaddingLeft : (int)Content.Padding.Left;
				int paddingRight = Content.Padding.Right == 0 ? this.Control.PaddingRight : (int)Content.Padding.Right;
				this.Control.SetPadding(paddingLeft, paddingTop, paddingRight, paddingBottom);


				if (Content != null)
					SetCornerRadius (Content);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			MyEntry Content = sender as MyEntry;
			if (e.PropertyName == MyEntry.BorderColorProperty.PropertyName) {
				SetCornerRadius (Content);
			}
		}

		private void SetCornerRadius(MyEntry Content)
		{
			GradientDrawable gd = new GradientDrawable();
			gd.SetColor(Content.BackgroundColor.ToAndroid());
			gd.SetCornerRadius(Content.BorderRadius);
			if (Content.BorderWidth > 0)
				gd.SetStroke(Content.BorderWidth, Content.BorderColor.ToAndroid());

			this.Control.SetBackground (gd);
		}


		//				if (!_inititialized) {
		//					this.Control.FocusChange += (sender, evt) => {
		//						if (evt.HasFocus) {
		//							ThreadPool.QueueUserWorkItem (s => {
		//								InputMethodManager imm = ((Android.Views.InputMethods.InputMethodManager)Context.
		//									GetSystemService (Android.Content.Context.InputMethodService));
		//								
		//								bool keyBoardVis = imm.IsActive;
		//								int f = imm.CurrentInputMethodSubtype.NameResId;//.IsActive;
		//
		//								var ddd = FindViewById(2131361830);
		//
		//
		//
		////								InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
		////								var currentFocus = Activity.CurrentFocus;
		////								if (currentFocus != null)
		////								{
		////									inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.ImplicitOnly);
		////								}
		////								Android.Views.MotionRange.
		//
		//
		//								Thread.Sleep (100); // For some reason, a short delay is required here.
		//								Xamarin.Forms.Device.BeginInvokeOnMainThread (
		//									() => ((Android.Views.InputMethods.InputMethodManager)Context.GetSystemService (Android.Content.Context.InputMethodService)).
		//								  	HideSoftInputFromWindow(WindowToken, HideSoftInputFlags.None));
		////									ShowSoftInput (this.Control, ShowFlags.Forced));
		////									ShowSoftInput (this.Control, Android.Views.InputMethods.ShowFlags.Implicit));
		////								InvokeOnMainThread(() => ((Android.Views.InputMethods.InputMethodManager)GetUIContext().GetSystemService(Android.Content.Context.InputMethodService)).ShowSoftInput(this.Control, Android.Views.InputMethods.ShowFlags.Implicit));
		//
		//
		//							});
		//						}
		//					};
		//					_inititialized = true;
		//				}
	}
}