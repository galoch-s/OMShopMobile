using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace OMShopMobile
{
	public class OrdersListView : ContentView
	{
		const int countColumn = 7;

		Grid gridNumberOrder;
		Grid gridScroll;

		List<Order> OrderList;
		ActivityIndicator indicator;
		StackLayout layoutGrid;
		StackLayout layoutMain;

		Pagination pagination;
		int currentPage = 1;
		int Status;

		public event EventHandler eventResult;

		public OrdersListView (int status = -1)
		{
			Status = status;

			indicator = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				IsVisible = true,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			gridNumberOrder = new Grid {
				WidthRequest = Utils.GetSize(80),
				VerticalOptions = LayoutOptions.Start,
				RowSpacing = 1,
				ColumnDefinitions = 
				{
					new ColumnDefinition { Width = Utils.GetSize(80) },
				}
			};
			if (Status == -1) {
				gridScroll = new Grid {
					VerticalOptions = LayoutOptions.Start,
					ColumnSpacing = 1,
					RowSpacing = 1,
					ColumnDefinitions = {
						new ColumnDefinition { Width = Utils.GetSize(70) },
						new ColumnDefinition { Width = Utils.GetSize(65) },
						new ColumnDefinition { Width = Utils.GetSize(50) },
						new ColumnDefinition { Width = Utils.GetSize(145) },
						new ColumnDefinition { Width = Utils.GetSize(130) },
					}
				};
			} else {
				gridScroll = new Grid {
					VerticalOptions = LayoutOptions.Start,
					ColumnSpacing = 1,
					RowSpacing = 1,
					ColumnDefinitions = 
					{
						new ColumnDefinition { Width = Utils.GetSize(70) },
						new ColumnDefinition { Width = Utils.GetSize(65) },
						new ColumnDefinition { Width = Utils.GetSize(50) },
						new ColumnDefinition { Width = Utils.GetSize(130) },
					}
				};
			}

			ScrollView scrollGrid = new ScrollView {
				Orientation = ScrollOrientation.Horizontal,
				Content = gridScroll
			};

			pagination = new Pagination ();

			layoutGrid = new StackLayout { 
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 1,
				Orientation = StackOrientation.Horizontal,
				Children = {
					gridNumberOrder,
					scrollGrid,
				}
			};
			layoutMain = new StackLayout{ 
				Children = {
					layoutGrid,
					pagination
				}
			};

			SetNumberOrder ();
			SetTitleGridScroll ();

			GetOrderAsync ();
			Content = indicator;
		}

		async void GetOrderAsync()
		{
			ContentAndHeads contentAndHeads;
			if (Status == -1)
				contentAndHeads = await Order.GetHistOrdersAsync (currentPage, 20);
			else
				contentAndHeads = await Order.GetHistOrdersToIDAsync (Status, currentPage, 20);

			OrderList = contentAndHeads.ContentList;

			if (contentAndHeads.countPage > 1) {
				SetPagination (pagination, contentAndHeads);
				pagination.IsVisible = true;
			} else {
				pagination.IsVisible = false;
			}

			SetGridBody ();
			Content = new ScrollView {
				Padding = new Thickness (8, 22, 0, 0),
				Orientation = ScrollOrientation.Vertical,
				Content = layoutMain
			};
		}

		private void SetPagination(Pagination pagination, ContentAndHeads contentAndHeads)
		{
			pagination.CurrentPage = contentAndHeads.currentPage;
			pagination.CountPage = contentAndHeads.countPage;
			pagination.Show ();
			pagination.tapGestureRecognizer.Tapped += ClickPage;
		}

		private void ClickPage(object s, EventArgs e)
		{
			Label lbl = s as Label;
			int numberPage = int.Parse (lbl.Text);

			currentPage = numberPage;
			Content = indicator;
			GetOrderAsync ();
		}

		void SetNumberOrder()
		{
			gridNumberOrder.Children.Add (GridUtils.GetTitleLabel("№ заказа", TextAlignment.Start), 0, 0);
		}

		void SetTitleGridScroll ()
		{
			List<string> strName = new List<string> {
				"Дата",
				"Сумма, р.",
				"Товар",
				"Статус заказа",
				"Состав заказа"
			};
			int i = 0;
			if (Status != -1)
				strName.RemoveRange(3, 1);
			
			foreach (var item in strName) {
				gridScroll.Children.Add (GridUtils.GetTitleLabel (item), i, 0);
				i++;
			}
		}

		void SetGridBody()
		{
			for (int i = 0; i < OrderList.Count; i++) {
				StackLayout layoutBodyLabel = GridUtils.GetBodyLabel ("смотреть", ApplicationStyle.GreenColor, true);

				TapGestureRecognizer tapBodyLabel = new TapGestureRecognizer ();
				tapBodyLabel.Tapped += OnTapBodyLabel;
				layoutBodyLabel.GestureRecognizers.Add (tapBodyLabel);
				tapBodyLabel.CommandParameter = i;

				gridNumberOrder.Children.Add (GridUtils.GetBodyLabel (OrderList [i].Number, TextAlignment.Start), 0, i + 1);
				if (Status == -1) {
					gridScroll.Children.Add (GridUtils.GetBodyLabel (OrderList [i].Date.ToString ("dd.MM.yy")), (int)ColunmHirtoryOrder.Date, i + 1);
					gridScroll.Children.Add (GridUtils.GetBodyLabel ((OrderList [i].Sum - OrderList[i].SumCoupon).ToString ()), (int)ColunmHirtoryOrder.Sum, i + 1);
					gridScroll.Children.Add (GridUtils.GetBodyLabel (OrderList [i].CountPosition.ToString ()), (int)ColunmHirtoryOrder.Count, i + 1);
					gridScroll.Children.Add (GridUtils.GetBodyLabel (OrderList [i].StatusString), (int)ColunmHirtoryOrder.StatusString, i + 1);
					gridScroll.Children.Add (layoutBodyLabel, (int)ColunmHirtoryOrder.Show, i + 1);
				} else {
					//gridScroll.Children.Add (GridUtils.GetBodyLabel (OrderList [i].StatusString), 0, i + 1);
					gridScroll.Children.Add(GridUtils.GetBodyLabel(OrderList[i].Date.ToString("dd.MM.yy")), (int)ColunmHirtoryOrder.Date, i + 1);
					gridScroll.Children.Add(GridUtils.GetBodyLabel(OrderList[i].Sum.ToString()), (int)ColunmHirtoryOrder.Sum, i + 1);
					gridScroll.Children.Add(GridUtils.GetBodyLabel(OrderList[i].CountPosition.ToString()), (int)ColunmHirtoryOrder.Count, i + 1);
					gridScroll.Children.Add (layoutBodyLabel, (int)ColunmHirtoryOrder.Show - 1, i + 1);
				}
			}
		}

		void OnTapBodyLabel (object sender, EventArgs e)
		{
			TappedEventArgs eTapped = e as TappedEventArgs;
			int index = (int)eTapped.Parameter;
			if (eventResult != null)
			eventResult (OrderList [index], null);
		}
	}
}