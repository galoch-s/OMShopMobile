using System;
using Xamarin.Forms;
using System.Collections;
using System.Linq;

namespace CustomLayouts
{
	public interface ITabProvider
	{
		string ImageSource { get; set; }
	}

	public class PagerIndicatorDots : StackLayout
	{
		int dotCount = 1;
		int _selectedIndex;

		public Color SelectColor { get; set; }
		public Color UnSelectColor { get; set; }

		public double DotSize { get; set; }

		public PagerIndicatorDots()
		{
			HorizontalOptions = LayoutOptions.CenterAndExpand;
			VerticalOptions = LayoutOptions.Center;
			Orientation = StackOrientation.Horizontal;
			SelectColor = Color.Black;
		}

		void CreateDot()
		{
			//Make one button and add it to the dotLayout
			var dot = new Button {
				BorderRadius = Convert.ToInt32(DotSize/2),
				HeightRequest = DotSize,
				WidthRequest = DotSize,
				BackgroundColor = SelectColor
			};
			Children.Add(dot);
		}

		void CreateTabs()
		{
			foreach(var item in ItemsSource)
			{
				var tab = item as ITabProvider;
				var image = new Image {
					HeightRequest = 42,
					WidthRequest = 42,
					BackgroundColor = SelectColor,
					Source = tab.ImageSource,
				};
				Children.Add(image);
			}
		}

		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create<PagerIndicatorDots, IList> (
				pi => pi.ItemsSource,
				null,
				BindingMode.OneWay,
				propertyChanging: (bindable, oldValue, newValue) => {
				((PagerIndicatorDots)bindable).ItemsSourceChanging ();
			},
				propertyChanged: (bindable, oldValue, newValue) => {
				((PagerIndicatorDots)bindable).ItemsSourceChanged ();
			}
		);

		public IList ItemsSource {
			get {
				return (IList)GetValue(ItemsSourceProperty);
			}
			set {
				SetValue (ItemsSourceProperty, value);
			}
		}

		public static BindableProperty SelectedItemProperty =
			BindableProperty.Create<PagerIndicatorDots, object> (
				pi => pi.SelectedItem,
				null,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) => {
				((PagerIndicatorDots)bindable).SelectedItemChanged ();
			});

		public object SelectedItem {
			get {
				return GetValue (SelectedItemProperty);
			}
			set {
				SetValue (SelectedItemProperty, value);
			}
		}

		void ItemsSourceChanging ()
		{
			if (ItemsSource != null)
				_selectedIndex = ItemsSource.IndexOf (SelectedItem);
		}

		void ItemsSourceChanged ()
		{
			if (ItemsSource == null) return;

			// Dots *************************************
			Spacing = DotSize;
			var countDelta = ItemsSource.Count - Children.Count;

			if (countDelta > 0) {
				for (var i = 0; i < countDelta; i++) 
				{
					CreateDot();
				}
			} 
			else if (countDelta < 0) 
			{
				for (var i = 0; i < -countDelta; i++) 
				{
					Children.RemoveAt (0);
				}
			}
			//*******************************************
		}

		void SelectedItemChanged () {

			var selectedIndex = ItemsSource.IndexOf (SelectedItem);
			var pagerIndicators = Children.Cast<Button> ().ToList ();

			foreach(var pi in pagerIndicators)
			{
				UnselectDot(pi);
			}

			if(selectedIndex > -1)
			{
				SelectDot(pagerIndicators[selectedIndex]);
			}
		}

		void UnselectDot (Button dot)
		{
			dot.BackgroundColor = UnSelectColor;
		}

		void SelectDot (Button dot)
		{
			dot.BackgroundColor = SelectColor;
		}
	}
}

