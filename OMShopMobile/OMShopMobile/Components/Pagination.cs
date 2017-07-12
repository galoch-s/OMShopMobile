using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class Pagination : Grid
	{
		public TapGestureRecognizer tapGestureRecognizer;

		public int CurrentPage { get; set; }
		public int CountPage { get; set; }
		private int countLinkToPage = 7;

		private readonly string Separator;
		public int CountLinkToPage 
		{ 
			get { return countLinkToPage; }
			set 
			{ 
				if (value < 2)
					countLinkToPage = 2;
				if (value > 12)
					countLinkToPage = 12;
			}
		}
		public Pagination (string separator="...")
		{
			Separator = separator;
		}

		public Pagination (int currentPage, int countPage, string separator="..."):this (separator)
		{
			CurrentPage = currentPage;
			CountPage = countPage;

		}

		public void Clear()
		{
			this.Children.Clear ();
		}

		public void Show()
		{
			this.ColumnSpacing = 3;
			for (int i = 0; i < countLinkToPage + 1; i++) {
				this.ColumnDefinitions.Add(new ColumnDefinition { Width = Utils.GetSize(30) });
			}
			Children.Clear ();

			tapGestureRecognizer = new TapGestureRecognizer();

			Label lblFirst = new Label { 
				Text = 1.ToString(), 
				HorizontalTextAlignment = TextAlignment.Center, 
				VerticalTextAlignment = TextAlignment.Center,
				BackgroundColor = Color.White
			};
			lblFirst.GestureRecognizers.Add (tapGestureRecognizer);


			Label lblLast = new Label { 
				Text = CountPage.ToString(), 
				HorizontalTextAlignment = TextAlignment.Center, 
				VerticalTextAlignment = TextAlignment.Center,
				BackgroundColor = Color.White
			};
			lblLast.GestureRecognizers.Add (tapGestureRecognizer);


			Label lblSeparator1 = new Label { 
				Text = Separator, 
				HorizontalTextAlignment = TextAlignment.Center, 
				VerticalTextAlignment = TextAlignment.Center };
			Label lblSeparator2 = new Label { 
				Text = Separator, 
				HorizontalTextAlignment = TextAlignment.Center, 
				VerticalTextAlignment = TextAlignment.Center };

			int centerLink = CountLinkToPage / 2 + 1;


			if (CountPage <= CountLinkToPage) {
				AddLabelsList(1, CountPage);
//				for (int i = 1; i < CountPage; i++) {
//					Children.Add(new Label { Text = i.ToString(), XAlign = TextAlignment.Center });
//				}
			} else {
				if (CurrentPage <= centerLink) {	// Если текущая страница вначале
					AddLabelsList(1, CountLinkToPage - 1);
					Children.Add (lblSeparator1, Children.Count, 0);
					Children.Add (lblLast, Children.Count, 0);
				} else if (CurrentPage >= CountPage - centerLink + 1) {	// Если текущая страница вконце
					Children.Add (lblFirst, Children.Count, 0);
					Children.Add (lblSeparator1, Children.Count, 0);
					AddLabelsList(CountPage - (CountLinkToPage - 1) + 1, CountPage);

				} else {	// Если текущая страница в "середине"
					Children.Add (lblFirst, Children.Count, 0);
					Children.Add (lblSeparator1, Children.Count, 0);
					AddLabelsList(CurrentPage - centerLink/2, CurrentPage + centerLink/2);
					Children.Add (lblSeparator2, Children.Count, 0);
					Children.Add (lblLast, Children.Count, 0);
				}
			}
		}

		private void AddLabelsList(int begin, int end)
		{
			for (int i = begin; i <= end; i++) {
				Label lblPage = new Label { 
					Text = i.ToString(), 
					HorizontalTextAlignment = TextAlignment.Center, 
					BackgroundColor = Color.White,
				};
				lblPage.GestureRecognizers.Add(tapGestureRecognizer);

				if (i == CurrentPage) {
					BoxView linePage = new BoxView {
						HeightRequest = 1, 
						WidthRequest = 20,
						BackgroundColor = ApplicationStyle.RedColor,
						VerticalOptions = LayoutOptions.End,
						HorizontalOptions = LayoutOptions.Center,
					};

					Grid gridPage = new Grid ();
					gridPage.Children.Add (lblPage, 0, 0);
					gridPage.Children.Add (linePage, 0, 0);

					lblPage.TextColor = ApplicationStyle.RedColor;
					Children.Add (gridPage, Children.Count, 0);
				}
				else
					Children.Add (lblPage, Children.Count, 0);
			}
		}
	}
}