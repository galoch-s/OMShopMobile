using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using FFImageLoading.Forms;
using System.Text.RegularExpressions;
using System.Timers;
using System.Threading.Tasks;

namespace OMShopMobile
{
	public class OrderTemplate : ViewCell
	{
		StackLayout layoutMain;

		CachedImage img;
		Image imgClock;
		Image imgSklad;

		Label lblName;
		Label lblActualCount;
		Label lblArt;
		Label lblSize;
		MyEntry entCount;
		Label lblCount;
		MyLabel lblPriceOld;
		Label lblPrice;

		StackLayout actionLayout;
		Image imgDelete;
		StackLayout layoutImgDelete;
		bool isInitialization;
		int countOrder;

		Button btnPlus;
		Button btnMinus;

		DateTime oldDate;
		double countSecond = 1;
		int oldValue = -1;

		Basket BasketItem { get; set; }
		string imgPath;

		Dictionary<int, string> SizeDictionary = new Dictionary<int, string> ();
		bool isError = false;

		event EventHandler eventRefresh;
		TapGestureRecognizer tapImageClick;

		public OrderTemplate (EventHandler eventImageClick)
		{
			isInitialization = true;
			int heightIcoMedium = 18;
			int heightTempImg = 133;
			double scaleIco = (double)107 / heightTempImg;
			int heightIco = (int)Utils.GetSize(heightIcoMedium * scaleIco, 1);

			img = new CachedImage { 
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = Utils.GetSize(85),
				CacheDuration = TimeSpan.FromDays(30),
				RetryCount = 2,
				RetryDelay = 250,
				BitmapOptimizations = true,
				LoadingPlaceholder = "Zaglushka_",
			};

			imgClock = new Image {
				VerticalOptions = LayoutOptions.Start,
				Source = Device.OnPlatform("Catalog/clock_", "clock", "clock"),
				HeightRequest = heightIco,
				IsVisible = false
			};
			imgSklad = new Image {
				VerticalOptions = LayoutOptions.Start,
				Source = Device.OnPlatform("Catalog/sklad", "Sklad", "Sklad"),
				HeightRequest = heightIco,
				IsVisible = false
			};
			StackLayout icoLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.End,
				Padding = new Thickness(0, Utils.GetSize(2), Utils.GetSize(2), 0),
				Spacing = 2,
				Children = {
					imgClock,
					imgSklad
				}
			};

			Grid gridImg = new Grid {
				VerticalOptions = LayoutOptions.Center,
				HeightRequest = Utils.GetSize(85),
				WidthRequest = Utils.GetSize(57),
				Padding = new Thickness(0, 0, 8, 0)
			};
			gridImg.Children.Add(img, 0, 0);
			gridImg.Children.Add(icoLayout, 0, 0);

			tapImageClick = new TapGestureRecognizer ();
			tapImageClick.Tapped += (sender, e) => {
				OnePage.redirectApp.RedirectToPage (PageName.Image, false, false);
				OnePage.redirectApp.imageView.Show (Constants.PathToImage + imgPath, BasketItem.Article);
			};
			img.GestureRecognizers.Add (tapImageClick);

			lblName = new Label { 
				TextColor = ApplicationStyle.GreenColor,
				HeightRequest = Utils.GetSize(26),
				FontSize = Utils.GetSize(12),
			};
			lblActualCount = new Label {
				TextColor = ApplicationStyle.RedColor,
				FontSize = Utils.GetSize(11),
			};
			lblArt = new Label { 
				TextColor = ApplicationStyle.TextColor,
				FontSize = Utils.GetSize(12),
			};
			lblSize = new Label { 
				TextColor = ApplicationStyle.TextColor,
				FontSize = Utils.GetSize(12),
			};
			StackLayout layoutInfo = new StackLayout { 
				Spacing = Utils.GetSize(2),
				Padding = new Thickness(0, 0, 0, Utils.GetSize(5)),
				Children = {
					lblArt,
					lblSize
				}
			};
			lblPriceOld = new MyLabel {
				TextColor = ApplicationStyle.LabelColor,
				FontSize = Utils.GetSize(11),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.End,
				IsVisible = false,
				IsStrikeThrough = true
			};
			lblPrice = new Label { 
				TextColor = ApplicationStyle.TextColor,
				FontSize = Utils.GetSize(12),
				VerticalOptions = LayoutOptions.EndAndExpand,
			};
			StackLayout layoutPrice = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Children = {
					lblPrice,
					lblPriceOld
				}
			};

			StackLayout infoLayout = new StackLayout {  
				Spacing = Utils.GetSize(2),
				Orientation = StackOrientation.Vertical,
				Children = {
					lblName,
					lblActualCount,
					layoutInfo,
					layoutPrice,
				}
			};

			btnPlus = new Button { 
				Style = ApplicationStyle.ButtonCountStyle,
				Text = "+",
			};
			btnPlus.Clicked += OnClickPlus;

			btnMinus = new Button { 
				Style = ApplicationStyle.ButtonCountStyle,
				Text = "–",
			};
			btnMinus.Clicked += OnClickMinus;

			entCount = new MyEntry { 
				WidthRequest = Utils.GetSize(40),
				HeightRequest = Utils.GetSize(30),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = ApplicationStyle.TextColor,
				Padding = new Thickness(1, 0)
			};
			entCount.TextChanged += OnCountChange;
			lblCount = new Label {
				WidthRequest = Utils.GetSize(40),
				HeightRequest = Utils.GetSize(30),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = ApplicationStyle.TextColor
			};

			actionLayout = new StackLayout {
				Spacing = 0,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Children = {
					btnPlus,
					entCount,
					lblCount,
					btnMinus,
				}
			};

			imgDelete = new Image {
				Source = Device.OnPlatform("Basket/dot_green_delete_.png", "dot_green_delete_.png", "dot_green_delete_.png"),
				HeightRequest = Utils.GetSize(24),
			};
			layoutImgDelete = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Padding = new Thickness(8),
				Children = {
					imgDelete
				}
			};

			TapGestureRecognizer tapDelete = new TapGestureRecognizer ();
			tapDelete.Tapped += ClickDelete;
			layoutImgDelete.GestureRecognizers.Add (tapDelete);

			StackLayout layout = new StackLayout {
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(0, 8, 8, 8),
				Orientation = StackOrientation.Horizontal,
				Children = {
					layoutImgDelete,
					gridImg,
					infoLayout,
					actionLayout
				}
			};

			layoutMain = new StackLayout { 
				Orientation = StackOrientation.Horizontal,
				Children = {
					layout
				}
			};
			View = layoutMain;
		}

		async void ClickDelete (object sender, EventArgs e)
		{
			if (!isError && !await OnePage.mainPage.DisplayMessageQuestion("Подтвердите действие", "Вы действительно хотите удалить этот товар?"))
				return;
			
			if (User.Singleton != null) {
				if (BasketItem.Quantity > 0)
					BasketItem.Quantity = -BasketItem.Quantity;
				try {
					await Basket.PushToBasketAsync (BasketItem);
				} catch (Exception) {
					eventRefresh = null;
					eventRefresh += (obj, evn) => { 
						Button content = obj as Button;
						content.IsEnabled = false;
						ClickDelete (sender, e);
					};
					OnePage.redirectApp.basketView.Content = OnePage.mainPage.ShowMessageError (eventRefresh); 
					isError = true;
					return;
				}
			} else {
				BasketDB basketDB;
				if (BasketItem.SizeValueId == null)
					basketDB = BasketDB.GetItemByID(BasketItem.ProductID);
				else
					basketDB = BasketDB.GetItem(BasketItem.ProductID, BasketItem.SizeValueId ?? 0);
				BasketDB.DeleteItem (basketDB.Id);
			}
			OnePage.redirectApp.RedirectToPage (PageName.Basket, true, true);

			if (isError)
				OnePage.redirectApp.basketView.Content = OnePage.redirectApp.basketView.mainGrid;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (BindingContext == null) return;
			BasketItem = BindingContext as Basket;

			float imgHeight = (float)Utils.GetSize((App.Density * Utils.GetSize(85)), 1);
			float imgWidth = (float)Utils.GetSize((App.Density * 60), 1);//57;

			imgPath = BasketItem.ProductImage;
			imgDelete.IsVisible = true;
			img.Source = new UriImageSource {
				Uri = new Uri(Constants.PathToPreviewImage + BasketItem.ProductImage + "&h=" + imgHeight + "&w=" + imgWidth),
				CachingEnabled = true,
				CacheValidity = new TimeSpan(60, 0, 0, 0)
			};
			imgClock.IsVisible = BasketItem.SchedulesList?.Count > 0 || BasketItem.IsSchedule;
			imgSklad.IsVisible = BasketItem.ProductExpress;

			if (BasketItem.PriceOld != 0) {
				lblPriceOld.Text = Math.Ceiling(BasketItem.PriceOld) + "р.";
				lblPriceOld.IsVisible = true;
			} else
				lblPriceOld.IsVisible = false;

			tapImageClick.CommandParameter = Constants.PathToImage + BasketItem.ProductImage;

			if (BasketItem.Quantity >= BasketItem.ProductActualQuantity || !BasketItem.ProductAvailable)
				btnPlus.IsEnabled = false;
			else
				btnPlus.IsEnabled = true;

			if (!(BasketItem.Quantity > 1) || BasketItem.ProductActualQuantity == 0 || !BasketItem.ProductAvailable)
				btnMinus.IsEnabled = false;

			lblName.Text = BasketItem.ProductName;// Utils.TruncateLongStringAtWord (BasketItem.ProductItem.productsDescription.Name, 40);
			lblArt.Text = "Артикул: " + BasketItem.Article;
			if (!string.IsNullOrEmpty(BasketItem.SizeName))
				lblSize.Text = "Размер: " + BasketItem.SizeName;
			else
				lblSize.Text = "Размер: - ";
			double price = BasketItem.Price;
			lblPrice.Text = Math.Ceiling(price) + "р.";

			if (BasketItem.Quantity > BasketItem.ProductActualQuantity && !BasketItem.IsHistoryProduct) {
				entCount.Text = BasketItem.ProductActualQuantity.ToString();
			} else {
				entCount.Text = BasketItem.Quantity.ToString();
			}
			if (BasketItem.IsHistoryProduct) {
				btnPlus.IsVisible = false;
				btnMinus.IsVisible = false;
				layoutImgDelete.IsVisible = false;
				entCount.IsVisible = false;
				lblCount.IsVisible = true;
				lblCount.Text = BasketItem.Quantity.ToString();
			} else {
				entCount.IsVisible = true;
				lblCount.IsVisible = false;
				if (BasketItem.ProductActualQuantity == 0 || !BasketItem.ProductAvailable) {
					layoutMain.BackgroundColor = ApplicationStyle.GrayEnabledColor;
					lblActualCount.Text = "Товар закончился";
					entCount.IsEnabled = false;
					entCount.Text = 0.ToString();
				} else if (BasketItem.Quantity > BasketItem.ProductActualQuantity) {
					layoutMain.BackgroundColor = ApplicationStyle.YellowBasketColor;
					lblActualCount.Text = "Осталось " + BasketItem.ProductActualQuantity + " из " + BasketItem.Quantity + " заказанных"; // "Остаток на складе " + BasketItem.ProductActualQuantity + "шт.";
				}
			}

			isInitialization = false;
		}

		void SetTimerEditCount(int countOrder, int productsMin, int productsUnits, MyEntry entCount)
		{
			Timer tmrCountProduct = new Timer(2000); // Секунда
			tmrCountProduct.Elapsed += (obj, evn) => {
				int countExcessProduct = (countOrder - productsMin) % productsUnits;
				if (countExcessProduct != 0) {
					countOrder -= countExcessProduct;
					Device.BeginInvokeOnMainThread(() => entCount.Text = countOrder.ToString());
				}
			};
			tmrCountProduct.AutoReset = false;
			tmrCountProduct.Enabled = true;
		}

		async void OnCountChanged()
		{
			int sec = (int)(countSecond * 1000);
			await Task.Delay(sec).ConfigureAwait(true);
			DateTime newDate = DateTime.Now;
			TimeSpan differenceTime = newDate - oldDate;
			if (differenceTime.TotalSeconds < countSecond)
				return;

			ProductView.StopTimer();
			int step;

			if (countOrder > BasketItem.ProductActualQuantity) {
				OnePage.mainPage.DisplayMessage("Максимальное количество " + BasketItem.ProductActualQuantity + " шт");
				countOrder = BasketItem.ProductActualQuantity;
				step = countOrder - oldValue;
				isInitialization = true;
				OnStepperValueChanged(step);
				entCount.Text = countOrder.ToString();
				return;
			}

			step = countOrder - oldValue;
			if (step == 0) {
				return;
			}
			OnStepperValueChanged(step);
			SetTimerEditCount(countOrder, BasketItem.ProductsQuantityOrderMin, BasketItem.ProductsQuantityOrderUnits, entCount);

			if (countOrder <= BasketItem.ProductActualQuantity) {
				layoutMain.BackgroundColor = Color.Transparent;
				lblActualCount.Text = " ";
			}
			oldValue = -1;
		}

		bool returnCountChange = false;
		void OnCountChange(object sender, TextChangedEventArgs e)
		{
			if (isInitialization || returnCountChange) {
				returnCountChange = false;
				return;
			}
			oldDate = DateTime.Now;

			ProductView.StopTimer();
			Regex rxNums = new Regex(@"^\d+$");
			MyEntry entCount = sender as MyEntry;
			// любые цифры
			if (entCount.Text != "" && !rxNums.IsMatch(entCount.Text)) {
				entCount.Text = e.OldTextValue;
				returnCountChange = true;
				return;
			}
			if (e.NewTextValue != "")
				countOrder = int.Parse(entCount.Text);

			int tempOldValue = -1;
			int.TryParse(e.OldTextValue, out tempOldValue);
			if (oldValue == -1) {
				if (BasketItem.Quantity > BasketItem.ProductActualQuantity)
					oldValue = BasketItem.Quantity;
				else
					oldValue = tempOldValue;
				//oldValue = tempOldValue;
			}

			OnCountChanged();
			isInitialization = false;
		}

		void OnClickPlus (object sender, EventArgs e)
		{
			int count;
			int.TryParse (entCount.Text, out count);
			if (count < BasketItem.ProductActualQuantity) {
				count += BasketItem.ProductsQuantityOrderUnits;
				entCount.Text = count.ToString();
			}
			if (count > BasketItem.ProductsQuantityOrderMin)
				btnMinus.IsEnabled = true;
			
			if (count >= BasketItem.ProductActualQuantity)
				btnPlus.IsEnabled = false;
		}

		void OnClickMinus (object sender, EventArgs e)
		{
			int count;
			int.TryParse (entCount.Text, out count);
			if (count > BasketItem.ProductsQuantityOrderMin) {
				count -= BasketItem.ProductsQuantityOrderUnits;
				entCount.Text = count.ToString();
			}
			if (count <= BasketItem.ProductsQuantityOrderMin)
				btnMinus.IsEnabled = false;
			
			if (count < BasketItem.ProductActualQuantity)
				btnPlus.IsEnabled = true;
		}

		private async void OnStepperValueChanged(int value)
		{
			btnPlus.IsEnabled = false;
			btnMinus.IsEnabled = false;

			if (!BasketItem.IsLocalBasket) {	// Товар в глобальной корзине
				Basket basketToServer = new Basket {
					Id = BasketItem.Id,
					ProductID = BasketItem.ProductID,
					SizeValueId = BasketItem.SizeValueId,
					Quantity = value,
					Comment = BasketItem.Comment
				};
				ContentAndHeads contentAndHeads;
				try {
					contentAndHeads = await Basket.PushToBasketAsync (basketToServer);
					isError = false;
				} catch (Exception) {
					eventRefresh = null;
					eventRefresh += (sender, e) => { 
						Button content = sender as Button;
						content.IsEnabled = false;
						OnStepperValueChanged(value);
					};
					OnePage.redirectApp.basketView.Content = OnePage.mainPage.ShowMessageError (eventRefresh); 
					isError = true;
					return;
				}
				if (contentAndHeads.requestStatus == System.Net.HttpStatusCode.OK)
					EditTotal (value);
					
			} else {    // Товар в локальной корзине
				BasketDB basketDB = new BasketDB {
					ProductID = BasketItem.ProductID,
					SizeID = BasketItem.SizeValueId ?? 0,
					Price = BasketItem.Price,
					Quantity = value
				};
				EditTotal (value);
				BasketDB.AddCount (basketDB);
				BasketItem.Quantity += value;
			}

			entCount.Text = BasketItem.Quantity.ToString ();
			if (BasketItem.Quantity < BasketItem.ProductActualQuantity) {
				btnPlus.IsEnabled = true;
			}
			if (BasketItem.Quantity > 1) {
				btnMinus.IsEnabled = true;
			}

			if (!isError) {
				OnePage.redirectApp.basketView.Content = OnePage.redirectApp.basketView.mainGrid;
			}
		}

		private void EditTotal (int count)
		{
			BasketView basketView = OnePage.redirectApp.basketView;
			double sum = count * BasketItem.Price;
			double total = basketView.total;
			total += sum;
			basketView.lblTotal.Text = total.ToString() + "р.";
			basketView.total = total;
		}
	}
}