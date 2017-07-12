using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMShopMobile
{
	public class TableSizeView : ContentView
	{
		event EventHandler eventRefresh;
		ListView listViewSize;
		WebView webTemp;
		HtmlWebViewSource htmlSource;
		SizeArticle sizeArticle;
		ActivityIndicator indicator;


		public TableSizeView ()
		{
			VerticalOptions = LayoutOptions.FillAndExpand;

			indicator = new ActivityIndicator {
				Color = Device.OnPlatform (Color.Black, Color.Gray, Color.Default),
				IsRunning = true,
				IsVisible = false,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
			Content = indicator;

			webTemp = new WebView ();
			//htmlSource = new HtmlWebViewSource { };
			//webTemp.Source = htmlSource;

			listViewSize = new ListView { 
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemTemplate = new DataTemplate(typeof(ListCellTemplate)),
			};
			listViewSize.ItemTapped += OnSizeClick;

			SetSizeList ();
		}

		public void GotoMain()
		{
			Content = listViewSize;
			//listViewSize.SelectedItem = null;
		}

		async void OnSizeClick (object sender, ItemTappedEventArgs e)
		{
			SizeArticle entity = e.Item as SizeArticle;
			htmlSource = new HtmlWebViewSource { };
			webTemp.Source = htmlSource;
			htmlSource.Html = entity.Description;

			await Task.Delay(100);
			listViewSize.SelectedItem = null;

			OnePage.redirectApp.AddTransition (PageName.Catalog, entity.Name, HistoryStep.TableSizeDescription);

			Content = webTemp;
		}

		async void SetSizeList()
		{
			List<SizeArticle> sizeArticleList = null;
			try {
				sizeArticleList = await SizeArticle.GetSizeArticleAsync ();
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (sender, e) => { 
					Button content = sender as Button;
					content.IsEnabled = false;
					SetSizeList ();
				};
				Content = OnePage.mainPage.ShowMessageError (eventRefresh);
				return;
			}

			listViewSize.ItemsSource = sizeArticleList;
			Content = listViewSize;
		}
	}
}

