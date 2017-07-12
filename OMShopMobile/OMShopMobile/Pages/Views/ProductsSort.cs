using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace OMShopMobile
{
	public class FilterParam {
		public ParamForSort paramForSort { get; set; }
		public int PriceBegin { get; set; }
		public int PriceEnd { get; set; }
		public int[] Sizes { get; set; }
	}

	public class ProductsSortView : ScrollView
	{
		ActivityIndicator indicator;
		public ListView sortList;
		StackLayout layoutSize;
		CellTemplate sizesCell;
		public ListView listSizes;
		StackLayout mainLayout;
		List<SizeCategory> SizeCategoryList;
		MyEntry entPriceBegin;
		MyEntry entPriceEnd;
		Button btnOk;

		bool isCurrentPage;
		Category _currentCategory;
		public Category CurrentCategory { 
			get
			{
				return _currentCategory;
			}
			set 
			{
				if (_currentCategory == value) return;
				isCurrentPage = false;
				_currentCategory = value;
			}
		}
		FilterParam filterParam = new FilterParam ();
		FilterParam filterParamDefault = new FilterParam ();

		event EventHandler eventRefresh;
		public EventHandler ClickFilterItem;

		public ProductsSortView (FilterParam _filterParam)
		{	
			filterParamDefault = _filterParam;
			VerticalOptions = LayoutOptions.FillAndExpand;

			indicator = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			Label lblSort = new Label {
				Text = "Сортировать по:",
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			StackLayout titleSortLayout = new StackLayout {
				BackgroundColor = ApplicationStyle.LineColor,
				HeightRequest = Utils.GetSize(22),
				Padding = new Thickness(8, 0),
				Children = {
					lblSort
				}
			};

			sortList = new ListView {
				ItemTemplate = new DataTemplate(typeof(SortItemCell)),
				ItemsSource = ParamSort.ParamsList,
				VerticalOptions = LayoutOptions.Start,
				RowHeight = Utils.GetSize(43),
			};
			sortList.SizeChanged += (sender, e) => {
				sortList.HeightRequest = (sortList.RowHeight + 0.5) * ParamSort.ParamsList.Count;
			};
			sortList.ItemTapped += OnClickSort;
			Sorted (sortList, _filterParam.paramForSort);

			Label lblFilter = new Label {
				Text = "Фильтровать по:",
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			StackLayout titleFilterLayout = new StackLayout {
				BackgroundColor = ApplicationStyle.LineColor,
//				VerticalOptions = LayoutOptions.Start,
				HeightRequest = Utils.GetSize(22),
				Padding = new Thickness(8, 0),
				Children = {
					lblFilter
				}	
			};
			sizesCell = new CellTemplate ("Размеру") {
				HeightRequest = Utils.GetSize(43)
			};
			TapGestureRecognizer tapOrderStatusList = new TapGestureRecognizer ();
			tapOrderStatusList.Tapped += OnSelectSize;;
			sizesCell.GestureRecognizers.Add (tapOrderStatusList);

			listSizes = new ListView { 
				ItemTemplate = new DataTemplate(typeof(RadioButtonItemCell)),
			};
			listSizes.ItemTapped += OnClickSize;

			layoutSize = new StackLayout {
				Spacing = 0,
				Children = {
					sizesCell,
					new BoxView (),
					listSizes
				}
			};

			entPriceBegin = new MyEntry { VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				Padding = new Thickness(5),
				BorderColor = ApplicationStyle.LineColor,
				BorderRadius = Utils.GetSize(3),
				BorderWidth = Utils.GetSize(1),
				HeightRequest = Utils.GetSize(26),
				WidthRequest = Utils.GetSize(80),
				Placeholder = "от",
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Keyboard = Keyboard.Numeric
			};
			entPriceEnd = new MyEntry { VerticalOptions = LayoutOptions.CenterAndExpand, 
				HorizontalTextAlignment = TextAlignment.Center,
				Padding = new Thickness(5),
				BorderColor = ApplicationStyle.LineColor,
				BorderRadius = Utils.GetSize(3),
				BorderWidth = Utils.GetSize(1),
				HeightRequest = Utils.GetSize(26),
				WidthRequest = Utils.GetSize(80),
				Placeholder = "до",
				Keyboard = Keyboard.Numeric
			};

			Label lblPriceBegin = new Label { VerticalOptions = LayoutOptions.CenterAndExpand, 
				TextColor = ApplicationStyle.TextColor,
				Text = "Цене"
			};

			Label lblPriceEnd = new Label { VerticalOptions = LayoutOptions.CenterAndExpand, 
				Text = " — "
			};

			StackLayout layoutPrice = new StackLayout { 
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness(16, 0, 24, 0),
				HeightRequest = Utils.GetSize(43),
				Children = {
					lblPriceBegin,
					entPriceBegin,
					lblPriceEnd,
					entPriceEnd,
				}
			};

			Button btnClear = new Button { 
				BackgroundColor = Color.Transparent,
				TextColor = ApplicationStyle.GreenColor,
				BorderColor = ApplicationStyle.GreenColor,
				BorderWidth = 1,
				WidthRequest = Utils.GetSize(150),
				Text = "СБРОСИТЬ ВСЕ",
			};
			btnClear.Clicked += OnClearClick;

			btnOk = new Button { 
				BackgroundColor = ApplicationStyle.RedColor,
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				WidthRequest = Utils.GetSize(150),
				Text = "ПРИМЕНИТЬ",
			};
			btnOk.Clicked += OnOkClick;

			StackLayout layoutBtn = new StackLayout {
				Padding = new Thickness(8, 16),
				HeightRequest = Utils.GetSize(35),
				Orientation = StackOrientation.Horizontal,
				Children = {
					btnClear,
					btnOk,
				} 
			};

			mainLayout = new StackLayout { 
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0,
				Children = {
					titleSortLayout,
					sortList,
					titleFilterLayout,
					layoutSize,
					layoutPrice,
					new BoxView (),
					layoutBtn,
				}
			};
			Content = mainLayout;
		}

		void OnClearClick (object sender, EventArgs e)
		{
			ClearFilter();
		}

		public void ClearFilter()
		{ 
			entPriceBegin.Text = "";
			entPriceEnd.Text = "";

			sortList.SelectedItem = null;
			Sorted(sortList, filterParamDefault.paramForSort);

			if (SizeCategoryList != null) {
				listSizes.SelectedItem = null;
				Sized(listSizes);
			}
		}

		void OnOkClick (object sender, EventArgs e)
		{
			int numBegin = 0;
			int numEnd = 0;
			int.TryParse(entPriceBegin.Text, out numBegin);
			int.TryParse(entPriceEnd.Text, out numEnd);

			filterParam.PriceBegin = numBegin;
			filterParam.PriceEnd = numEnd;

			ClickFilterItem (filterParam, e);
		}

		public void Show()
		{
			SetCategory ();
//			if (CurrentCategory.Description.ID < 1)
//				CurrentCategory = await Category.GetegoryByIDAsync(CurrentCategory.ID);
//
//			listSizes.IsVisible = false;
//			if (CurrentCategory != null && CurrentCategory.ParentId == 0) {
//				layoutSize.IsVisible = false;
//				return;
//			}
//			layoutSize.IsVisible = true;
//			Content = indicator;
//			SetSizes (CurrentCategory.ID);
		}

		async void SetCategory()
		{
			listSizes.IsVisible = false;
			if (isCurrentPage || CurrentCategory == null) {
				sizesCell.SetLeft();
				return;
			}

			layoutSize.IsVisible = false;

			
			Content = indicator;

			if (CurrentCategory.Description.ID < 1)
				CurrentCategory = await Category.GetegoryByIDAsync(CurrentCategory.ID);

			if (CurrentCategory != null && CurrentCategory.ParentId == 0) {
//				layoutSize.IsVisible = false;
				Content = mainLayout;
				return;
			}
			layoutSize.IsVisible = true;
			SetSizes (CurrentCategory.ID);

			isCurrentPage = true;
		}

		async void SetSizes(int categoryID)
		{
			try {
				SizeCategoryList = await SizeCategory.GetSizeCategoryAsync(categoryID);
				listSizes.ItemsSource = SizeCategoryList;
				if (listSizes.HeightRequest == -1 )
					listSizes.HeightRequest = (listSizes.RowHeight + 1) * SizeCategoryList.Count;
			}
			catch (Exception) {
				eventRefresh = null;
				eventRefresh += (sender, e) => { 
					Button content = sender as Button;
					content.IsEnabled = false;
					SetSizes (categoryID);
				};
				Content = OnePage.mainPage.ShowMessageError (eventRefresh);
				return;
			}
			Content = mainLayout;
		}

		async void OnSelectSize (object sender, EventArgs e)
		{
			CellTemplate orderStatusCell = sender as CellTemplate;
			if (listSizes.IsVisible) {
				await this.ScrollToAsync (0, 0, true);
				listSizes.IsVisible = false;
				orderStatusCell.SetLeft ();

			} else {
				listSizes.IsVisible = true;
//				await this.ScrollToAsync (listSizes, ScrollToPosition.End, true);
				orderStatusCell.SetDown ();
			}
		}

		void OnClickSort (object sender, ItemTappedEventArgs e)
		{
			ListView listView = sender as ListView;
			ParamForSort paramSort = (ParamForSort)listView.SelectedItem;
			Sorted (listView, paramSort);
		}

		public void Sorted (ListView listView, ParamForSort paramSort)
		{
//			ParamForSort paramSort = (ParamForSort)listView.SelectedItem;
			if (paramSort != null)
				paramSort.IsCheck = true;
			if (ParamSort.oldItem != null)
				ParamSort.oldItem.IsCheck = false;

			filterParam.paramForSort = paramSort;
			// ListView должен обновляться после изменения источника данных, но этого не происходит. Поэтому выход такой
			List<ParamForSort> tempList = new List<ParamForSort> ();
			foreach (ParamForSort item in ParamSort.ParamsList) {
				tempList.Add (new ParamForSort {
					Id = item.Id,
					FieldSort = item.FieldSort,
					Name = item.Name,
					IsDesc = item.IsDesc,
					IsCheck = item.Id == paramSort?.Id
				});
			}
			listView.ItemsSource = tempList;
			ParamSort.ParamsList = tempList;
		}

		void OnClickSize (object sender, ItemTappedEventArgs e)
		{	
			ListView listView = sender as ListView;
			Sized (listView);
		}

		void Sized (ListView listView)
		{
			SizeCategory sizeCategory = (SizeCategory)listView.SelectedItem;
			// ListView должен обновляться после изменения источника данных, но этого не происходит. Поэтому выход такой
			List<SizeCategory> tempList = new List<SizeCategory> ();
			foreach (SizeCategory item in SizeCategoryList) {
				if (sizeCategory != null)
					tempList.Add (new SizeCategory {    
						Id = item.Id,
						Name = item.Name,
						IsCheck = (item.Id == sizeCategory.Id) ? !sizeCategory.IsCheck : item.IsCheck
					});
				else
					tempList.Add (new SizeCategory {
						Id = item.Id,
						Name = item.Name,
						IsCheck = false
					});
			}
			filterParam.Sizes = tempList.Where (g => g.IsCheck).Select (g => g.Id).ToArray<int> ();
			listView.ItemsSource = tempList;
			SizeCategoryList = tempList;
		}
	}
}