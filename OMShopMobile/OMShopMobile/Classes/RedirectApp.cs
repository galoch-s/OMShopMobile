using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OMShopMobile
{
	public class RedirectApp
	{
		public CatalogView catalogView;
		public CatalogViewXaml catalogViewXaml;
		public BasketView basketView;
		public LoginView loginView;
		public SearchView searchView;
		public ImageView imageView;
		public MainPageView mainPageView;

		View newView = null;


		FilterParam filterParam;

//		private GridBtnLayuot predBtnLayuot;

		public event EventHandler EventRedirectPage;

		private List<History> HistoryList { get; set;}

		public RedirectApp ()
		{
			HistoryList = new List<History> ();
			loginView = new LoginView ();
			imageView = new ImageView ();
			//mainPageView = new MainPageView ();
		}

		public void RedirectToPage(PageName page, bool isHistory, bool IsRedirectToBack)
		{
			RedirectToPage(page, isHistory, IsRedirectToBack, true);
		}
		/// <summary>
		/// Метод перенаправляет на другую страницу и изменяет индикатор текущей страницы
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="isHistory">If set to <c>true</c> is history.</param>
		/// <param name="IsRedirectToBack">If set to <c>true</c> is redirect to back.</param>
		/// <param name="isLoadAllCategories">If set to <c>true</c> is load all categories.</param>
		public void RedirectToPage(PageName page, bool isHistory, bool IsRedirectToBack, bool isLoadAllCategories)
		{
			IKeyboardInteractions keyboardInteractions = DependencyService.Get<IKeyboardInteractions>();
			keyboardInteractions.HideKeyboard();

			StackLayout layoutContent = OnePage.bodyLayout;


			History history = GetCurrentTransition();
			if (history != null && history.Page == PageName.Order)
				CrearHistory();
			if (history != null && history.Page == PageName.Catalog && history.Step == HistoryStep.FilterProduct)
				DeleteLastTransition();

			//if (newView == catalogView && catalogView != null)
			//	catalogView.StopLoad();

			switch (page) {
				case PageName.Main:
					if (!isHistory && !AddTransition(page, "Главная"))
						return;
					ShowIndicator(layoutContent);
					//if (mainPageView == null) 
						mainPageView = new MainPageView();
					
					newView = mainPageView;
					break;
				case PageName.Info:
					if (!isHistory && !AddTransition(page, "Инфо"))
							return;
					ShowIndicator(layoutContent);
					newView = new InfoView();
					break;
				case PageName.Catalog:
					if (!isHistory && !AddTransition(page, "Каталог"))
							return;
					ShowIndicator(layoutContent);

					if (catalogView != null)
						catalogView.StopLoad();

					if (catalogView == null) {
						catalogView = new CatalogView(isLoadAllCategories);
						catalogView.filterParam = filterParam;
					}
					newView = catalogView;
					break;
				case PageName.Login:
					string text;
					if (User.Singleton == null) {
						text = "Вход";
					} else
						text = "Мой профиль";
					if (!isHistory && !AddTransition(page, text, IsRedirectToBack))
							return;
					if (newView != basketView)
						ShowIndicator(layoutContent);
					
					loginView.GotoPage(history.Step);
					newView = loginView;
					break;
				case PageName.Search:
					if (!isHistory && !AddTransition(page, "Поиск", IsRedirectToBack))
							return;
					ShowIndicator(layoutContent);

					newView = searchView;
					searchView.ClearText();
					break;
				case PageName.Basket:
					if (!isHistory && !AddTransition(page, "Корзина", false, true))
							return;
					ShowIndicator(layoutContent);

					basketView = new BasketView();
					newView = basketView;
					break;
				case PageName.Image:
					if (catalogView != null)
						filterParam = catalogView.filterParam;
					if (!isHistory && !AddTransition(page, "Просмотр", false, true))
							return;
					ShowIndicator(layoutContent);

					newView = imageView;
					break;
				default:
					break;
			}

			//if (catalogView != null)
			//	catalogView.StopLoad();

			Device.BeginInvokeOnMainThread(() => { 
				if (layoutContent.Children.Count > 0)
					layoutContent.Children.RemoveAt(0);
				layoutContent.Children.Insert(0, newView);
			});

		}

		void ShowIndicator(StackLayout layoutContent)
		{ 
			if (layoutContent.Children.Count > 0)
				//if (newView != basketView) {
					layoutContent.Children.RemoveAt(0);
					layoutContent.Children.Add(new ActivityIndicator {
						Color = Device.OnPlatform(Color.Black, Color.Gray, Color.Default),
						IsRunning = true,
						IsVisible = true,
						VerticalOptions = LayoutOptions.CenterAndExpand,
					});
				//}
		}

		public void ClickProductPage(int productPage)
		{
			History hist = GetCurrentTransition();
			hist.CurrentProductPage = productPage;
		}

		public void SetStatusBasket(PageName page)
		{
			//			if (User.Singleton != null && User.Singleton.BasketList != null) {
			//
			//				if (User.Singleton.BasketList.Count == 0) {
			//					if (predBtnLayuot != null && predBtnLayuot.PageName == PageName.Basket) {
			//						FileImageSource img = predBtnLayuot.Img.Source as FileImageSource;
			//						predBtnLayuot.Img.Source = img.File.Replace ("_fill", "");
			//					}
			//					if (itemNew.PageName == PageName.Basket) {
			//						FileImageSource img = itemNew.Img.Source as FileImageSource;
			//						itemNew.Img.Source = img.File.Replace ("_fill", "");
			//					}
			//				} else {
			//					if (predBtnLayuot != null && predBtnLayuot.PageName == PageName.Basket) {
			//						FileImageSource img = predBtnLayuot.Img.Source as FileImageSource;
			//						img.File = img.File.Replace ("_fill", "");
			//						predBtnLayuot.Img.Source = img.File + "_fill";
			//					}
			//					if (itemNew.PageName == PageName.Basket) {
			//						FileImageSource img = itemNew.Img.Source as FileImageSource;
			//						img.File = img.File.Replace ("_fill", "");
			//						itemNew.Img.Source = img.File + "_fill";
			//					}
			//				}
			//			} else {
			//				if (predBtnLayuot != null) {
			//					FileImageSource imgPred = predBtnLayuot.Img.Source as FileImageSource;
			//					imgPred.File = imgPred.File.Replace ("_fill", "");
			//				}
			//				FileImageSource img = itemNew.Img.Source as FileImageSource;
			//				img.File = img.File.Replace ("_fill", "");
			//			}
		}

		public void ClickHistoryPred(object s, EventArgs e)
		{
			BackToHistory ();
		}

		public bool BackToHistory()
		{	
			if (HistoryList.Count < 2)
				return false;
			
			History currentHistory = GetCurrentTransition ();
			//if (currentHistory.Step == HistoryStep.FilterProduct && currentHistory.Page == PageName.Catalog) {
			//	catalogView.CloseFilter();
			//	return true;
			//}
			History history = GetLastTransition ();



//			if (currentHistory.Step != HistoryStep.FilterProduct)
				if (EventRedirectPage != null)
					EventRedirectPage (history, null);

			GotoPage(currentHistory, history);
			return true;
		}

		void GotoPage(History currentHistory, History history)
		{
			if (history.Page == PageName.Search) {
				//if (currentHistory.Page != PageName.Catalog)
				//	RedirectToPage (history.Page, true, false, false);
				if (history.Step == HistoryStep.Default) {
					if (currentHistory.Page != PageName.Search)
						RedirectToPage(PageName.Search, true, false, false);
					if (history.ProductID == 0)
						searchView.ShowResult();
					else
						searchView.ShowProduct(history.ProductID);
				}
				if (history.Step == HistoryStep.TableSizes)
					searchView.GotoTableSize(true);
			} else if (history.Page == PageName.Login) {
				//				if (history.Step == HistoryStep.Default)
				//					RedirectToPage (history.Page, true, false);
				//				else {
				RedirectToPage(history.Page, true, false);
				loginView.GotoPage(history.Step);
				//				}
			} else if (history.Page == PageName.Order) {
				basketView.GoToOrder();
			} else {
				if (history.Page == PageName.Catalog) {
					if (currentHistory.Step == HistoryStep.FilterProduct && currentHistory.Page == PageName.Catalog && history.ContentCategory != null) {
						catalogView.CloseFilter();
					} else
					if (history.Step != HistoryStep.Default) {
						RedirectToPage(history.Page, true, false);
						catalogView.GotoTableSize(true, history.Step);
					} else if (history.ProductID != 0) {
						if (currentHistory.Page != PageName.Catalog) {
							RedirectToPage(history.Page, true, false, false);

						}
						catalogView.GoToProductAsync(history.CurrentPosition, history.ProductID, true);
					} else {
						//if (!string.IsNullOrEmpty(history.ImageSource)) {
						//	RedirectToPage(history.Page, true, false);
						//	catalogView.GoToImage(history.ImageSource, "23423", true);
						//} else {
							if (history.ContentCategory != null) {
								if (currentHistory.Page == PageName.Catalog) {
									if (currentHistory.ProductID != 0)
										catalogView.GoToCategory(history.ContentCategory, history.BreadCrumbs, true, false, history.CurrentProductPage);
									else
										catalogView.GoToCategory(history.ContentCategory, history.BreadCrumbs, true, true, history.CurrentProductPage);
								} else {
									RedirectToPage(history.Page, true, false, false);
									catalogView.GoToCategory(history.ContentCategory, history.BreadCrumbs, true, true, history.CurrentProductPage);
								}
							} else {
								RedirectToPage(history.Page, true, false);
								catalogView.GotoMainCategory();
							}
						//}
					}
				} else {
					RedirectToPage(history.Page, true, false);
				}
			}
		}

		#region AddTransition
		public bool AddTransition(PageName page, string currentPosition, bool isRedirectToBack, bool isNarrowTitle, Category category)
		{
			History history = new History (page, currentPosition, isRedirectToBack, isNarrowTitle, category, null);
			History currHistory = GetCurrentTransition ();
			if (currHistory != null && currHistory.HistoryEquals (history))
				return false;
			HistoryList.Add (history);
			if (EventRedirectPage != null)
				EventRedirectPage (history, null);
			return true;
		}

		public bool AddTransition(PageName page, string currentPosition, HistoryStep historyStep)
		{
			History history = new History (page, currentPosition, historyStep);
			History currHistory = GetCurrentTransition ();
			if (currHistory != null && currHistory.HistoryEquals (history))
				return false;
			HistoryList.Add (history);
			if (EventRedirectPage != null)
				EventRedirectPage (history, null);
			return true;
		}

		public bool AddTransition(PageName page, string currentPosition, bool isRedirectToBack = false, bool isNarrowTitle = false, HistoryStep historyStep = HistoryStep.Default)
		{
			return this.AddTransition (page, currentPosition, isRedirectToBack, isNarrowTitle, null);
		}

		public bool AddTransition(PageName page, int productID, string currentPosition)
		{
			History history = new History (page, productID, currentPosition);
			History currHistory = GetCurrentTransition ();
			if (currHistory != null && currHistory.HistoryEquals (history))
				return false;
			HistoryList.Add (history);
			if (EventRedirectPage != null)
				EventRedirectPage (history, null);
			return true;
		}

		//public bool AddTransition(PageName page, string imageSource, string currentPosition)
		//{
		//	History history = new History (page, imageSource, currentPosition);
		//	History currHistory = GetCurrentTransition ();
		//	if (currHistory != null && currHistory.HistoryEquals (history))
		//		return false;
		//	HistoryList.Add (history);
		//	if (EventRedirectPage != null)
		//		EventRedirectPage (history, null);
		//	return true;

		//}
		#endregion
		public History GetLastTransition()
		{
			HistoryList.RemoveAt (HistoryList.Count - 1);
			History hist = HistoryList [HistoryList.Count - 1];
			return hist;
		}

		public void DeleteLastTransition()
		{	
			HistoryList.RemoveAt (HistoryList.Count - 1);
			History history = HistoryList [HistoryList.Count - 1];
			OnePage.topView.lblPagePosition.Text = history.CurrentPosition;
		}

		public History GetCurrentTransition()
		{
			if (HistoryList != null && HistoryList.Count > 0)
				return HistoryList [HistoryList.Count - 1];
			return null;
		}

		public int CountHistory()
		{
			return HistoryList.Count;
		}

		public void CrearHistory()
		{
			HistoryList.RemoveRange (1, HistoryList.Count - 1);
		}

		public string GetHistoryToJson()
		{
			return History.GetHistoryToJson(HistoryList);
		}
	}
}

