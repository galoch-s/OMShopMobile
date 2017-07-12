using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OMShopMobile;
using OMShopMobile.Droid;
using Android.Graphics;
using Android.Widget;
using Android.Views;

using G = Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Text;

[assembly: ExportRenderer (typeof (MySearchBar), typeof (MySearchBarRenderer))]
namespace OMShopMobile.Droid
{
	public class MySearchBarRenderer : SearchBarRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<SearchBar> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null) {
				MySearchBar Content = e.NewElement as MySearchBar;

				this.Control.SetBackgroundResource (Resource.Drawable.back_for_search);

				Android.Widget.SearchView searchView = (base.Control as Android.Widget.SearchView);

				int frameId = searchView.Context.Resources.GetIdentifier( "android:id/search_plate", null, null );
				Android.Views.View frameView = (searchView.FindViewById( frameId ) as Android.Views.View);
				frameView.SetBackgroundColor( Xamarin.Forms.Color.Transparent.ToAndroid() );
//				frameView.SetBackgroundResource(Resource.Drawable.textfield_searchview_holo_light);
				

				// Get native control (background set in shared code, but can use SetBackgroundColor here)
				searchView.SetInputType( InputTypes.ClassText | InputTypes.TextVariationNormal );

				// Access search textview within control
				int textViewId = searchView.Context.Resources.GetIdentifier( "android:id/search_src_text", null, null );
				EditText textView = (searchView.FindViewById( textViewId ) as EditText);
//				textView.SetBackgroundResource(Resource.Drawable.textfield_searchview_holo_light);

				// Set custom colors

//				textView.SetBackgroundColor( Android.Graphics.Color.White );
//				textView.SetTextColor( G.Color.Rgb( 32, 32, 32 ) );
//				textView.SetHintTextColor( G.Color.Rgb( 128, 128, 128 ) );

//				// Customize frame color
//				int frameId = searchView.Context.Resources.GetIdentifier( "android:id/search_plate", null, null );
//				Android.Views.View frameView = (searchView.FindViewById( frameId ) as Android.Views.View);
//				frameView.SetBackgroundColor( G.Color.Rgb( 96, 96, 96 ) );



//				SetBackgr (this.Control);
			}
		}


		void SetBackgr(ViewGroup viewGroup)
		{	
			for (int i = 0; i < viewGroup.ChildCount; i++) {
//				viewGroup.GetChildAt (i).SetBackgroundResource (Resource.Drawable.grd);
				ViewGroup v = viewGroup.GetChildAt (i)  as  ViewGroup;
				if (v != null) {
					GradientDrawable _normal = new Android.Graphics.Drawables.GradientDrawable ();
					_normal.SetColor(Android.Graphics.Color.White);
					v.SetBackground (_normal);
					v.SetBackgroundResource (Resource.Drawable.grd);
					SetBackgr (v);
				}
			}
		}

		void SetFone(SearchBar button)
		{
			GradientDrawable _normal, _pressed;
			// Create a drawable for the button's normal state
			_normal = new Android.Graphics.Drawables.GradientDrawable();
			_normal.SetColor(button.BackgroundColor.ToAndroid());
			_normal.SetStroke(30, Android.Graphics.Color.Red);
			_normal.SetCornerRadius(5);
			this.Control.SetBackground(_normal);
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "MyriadProRegular.ttf");
			Android.Widget.SearchView label = (Android.Widget.SearchView)Control;
			AutoCompleteTextView objAutoTextView = (AutoCompleteTextView)(((this.Control.
				GetChildAt (0)  as  ViewGroup).
				GetChildAt (2)  as  ViewGroup).
				GetChildAt (1)  as  ViewGroup).
				GetChildAt (0);
			objAutoTextView.Typeface = font;

//			MySearchBar Content = sender as MySearchBar;
			objAutoTextView.Typeface = font;
//			objAutoTextView.set (Color.White);
//			objAutoTextView.SetHighlightColor (Color.White);
//			objAutoTextView.SetHighlightColor (Color.White);
		}


//
//			// Get native control (background set in shared code, but can use SetBackgroundColor here)
//			Android.Widget.SearchView searchView = (base.Control as Android.Widget.SearchView);
//			searchView.SetBackgroundColor (Xamarin.Forms.Color.White.ToAndroid ());
////			searchView.SetInputType( InputTypes.ClassText | InputTypes.TextVariationNormal );
//
//			// Access search textview within control
////			int textViewId = searchView.Content.Resources.GetIdentifier( "android:id/search_src_text", null, null );
////			EditText textView = (searchView.FindViewById( textViewId ) as EditText);
//
//			AutoCompleteTextView textView = (AutoCompleteTextView)(((this.Control.GetChildAt (0)  as  ViewGroup).GetChildAt (2)  as  ViewGroup).GetChildAt (1)  as  ViewGroup).GetChildAt (0);
//
//			// Set custom colors
//			textView.SetBackgroundColor( Xamarin.Forms.Color.White.ToAndroid() );
//			textView.SetTextColor( Xamarin.Forms.Color.Black.ToAndroid() );
//			textView.SetHintTextColor( Xamarin.Forms.Color.Black.ToAndroid() );
//
//			// Customize frame color
//			int frameId = searchView.Context.Resources.GetIdentifier( "android:id/search_plate", null, null );
//			Android.Views.View frameView = (searchView.FindViewById( frameId ) as Android.Views.View);
//			frameView.SetBackgroundColor( Xamarin.Forms.Color.Blue.ToAndroid() );
//		}
	}
}