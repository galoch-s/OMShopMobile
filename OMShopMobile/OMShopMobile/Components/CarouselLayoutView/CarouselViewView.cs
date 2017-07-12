using System;
using Xamarin.Forms;
using CustomLayouts.Controls;
using CustomLayouts.ViewModels;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using OMShopMobile;

namespace CustomLayouts
{
	public class CarouselView : ContentView
	{
		View _tabs;
		RelativeLayout relativeLayout;
		CarouselLayout.IndicatorStyleEnum _indicatorStyle;
		DataTemplate _dataTemplate;

		public CarouselView(CarouselLayout.IndicatorStyleEnum indicatorStyle, BaseViewModel viewModel, DataTemplate dataTemplate)
		{
			_indicatorStyle = indicatorStyle;
			_dataTemplate = dataTemplate;
			BindingContext = viewModel;

			relativeLayout = new RelativeLayout 
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			CarouselLayout pagesCarousel = CreatePagesCarousel();
			StackLayout dots = CreatePagerIndicatorContainer();
			_tabs = CreateTabs();

			switch(pagesCarousel.IndicatorStyle)
			{
				case CarouselLayout.IndicatorStyleEnum.Dots:
					relativeLayout.Children.Add (pagesCarousel,
						Constraint.RelativeToParent ((parent) => { return parent.X; }),
						Constraint.RelativeToParent ((parent) => { return parent.Y; }),
						Constraint.RelativeToParent ((parent) => { return parent.Width; }),
						Constraint.RelativeToParent ((parent) => { return parent.Height; })
					);

					relativeLayout.Children.Add (dots, 
						Constraint.Constant (0),
						Constraint.RelativeToView (pagesCarousel, 
						(parent,sibling) => { return sibling.Height - Constants.HeightCaruselIndicator; }),
						Constraint.RelativeToParent (parent => parent.Width),
						Constraint.Constant (Constants.HeightCaruselIndicator)
					);
					break;
				case CarouselLayout.IndicatorStyleEnum.Tabs:
					var tabsHeight = 50;
					relativeLayout.Children.Add (_tabs, 
						Constraint.Constant (0),
						Constraint.RelativeToParent ((parent) => { return parent.Height - tabsHeight; }),
						Constraint.RelativeToParent (parent => parent.Width),
						Constraint.Constant (tabsHeight)
					);

					relativeLayout.Children.Add (pagesCarousel,
						Constraint.RelativeToParent ((parent) => { return parent.X; }),
						Constraint.RelativeToParent ((parent) => { return parent.Y; }),
						Constraint.RelativeToParent ((parent) => { return parent.Width; }),
						Constraint.RelativeToView (_tabs, (parent, sibling) => { return parent.Height - (sibling.Height); })
					);
					break;
				default:
					relativeLayout.Children.Add (pagesCarousel,
						Constraint.RelativeToParent ((parent) => { return parent.X; }),
						Constraint.RelativeToParent ((parent) => { return parent.Y; }),
						Constraint.RelativeToParent ((parent) => { return parent.Width; }),
						Constraint.RelativeToParent ((parent) => { return parent.Height; })
					);
					break;
				}

			Content = relativeLayout;
		}

		CarouselLayout CreatePagesCarousel ()
		{
			var carousel = new CarouselLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				IndicatorStyle = _indicatorStyle,
//				ItemTemplate = new DataTemplate(typeof(BannerTemplate))
				ItemTemplate = _dataTemplate
			};
			carousel.SetBinding(CarouselLayout.ItemsSourceProperty, "Pages");
			carousel.SetBinding(CarouselLayout.SelectedItemProperty, "CurrentPage", BindingMode.TwoWay);

			return carousel;
		}

		StackLayout CreatePagerIndicatorContainer()
		{
			return new StackLayout {
				Children = { CreatePagerIndicators() }
			};
		}

		PagerIndicatorDots CreatePagerIndicators()
		{
			PagerIndicatorDots pagerIndicator = new PagerIndicatorDots() { 
				DotSize = 7, 
				SelectColor = ApplicationStyle.GreenColor, 
				UnSelectColor = ApplicationStyle.LineColor,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
			pagerIndicator.SetBinding (PagerIndicatorDots.ItemsSourceProperty, "Pages");
			pagerIndicator.SetBinding (PagerIndicatorDots.SelectedItemProperty, "CurrentPage");
			return pagerIndicator;
		}

		View CreateTabsContainer()
		{
			return new StackLayout {
				Children = { CreateTabs() }
			};
		}

		View CreateTabs()
		{
			var pagerIndicator = new PagerIndicatorTabs() { HorizontalOptions = LayoutOptions.CenterAndExpand };
			pagerIndicator.RowDefinitions.Add(new RowDefinition() { Height = 50 });
			pagerIndicator.SetBinding (PagerIndicatorTabs.ItemsSourceProperty, "Pages");
			pagerIndicator.SetBinding (PagerIndicatorTabs.SelectedItemProperty, "CurrentPage");
			//			pagerIndicator.SetBinding (PagerIndicatorTabs.ColumnDefinitionsProperty, "Pages", BindingMode.Default, new SpacingConverter());
			return pagerIndicator;
		}
	}

	public class SpacingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var items = value as IEnumerable<Banner>;

			var collection = new ColumnDefinitionCollection();
			foreach(var item in items)
			{
				collection.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
			}
			return collection;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}