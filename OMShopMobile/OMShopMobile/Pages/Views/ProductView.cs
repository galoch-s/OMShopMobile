using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using CustomLayouts.Controls;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Timers;
using System.Threading.Tasks;

namespace OMShopMobile
{
	public class ProductView : ContentView
	{
		#region Components 
		Label lblArticle;
		Image imgSklad;
		Image img;
		Label lblName;
		Label lblPriceOld;
		Label lblPrice;

		StackLayout layoutScrollSize;
		StackLayout layoutSize;
		MyScrollView scrollSizes;
		RelativeLayout relativeLayoutSize;
		Label lblSize;

		MyButton btnTableSize;
		MyLabel lblDescription;

		StackLayout layoutBottom;
		Button btnPlus;
		Button btnMinus;
		public Button btnOrder;
		#endregion
		Basket oldBasket;

		ScrollView scrollView;
		Grid mainGrid;
		Grid gridTime;
		Label lblTime;
		StackLayout layoutTitleTimer;
		StackLayout layoutContent;

		Button btnPred = null;
		MyEntry entCount;

		event EventHandler eventRefresh;
		public event EventHandler EventTableSizeClick;
		public TapGestureRecognizer ImageClick;

		bool isProducToOrder;
		Dictionary<int, string> SizeDictionary = new Dictionary<int, string>();

		string messageTimer;
		int countOrder;
		bool isInitialization = true;
		public static string MessageSize = "Пожалуйста выберите размер";
		public static Timer tmrCountProduct;

		Product ProductItem;
		KeyValuePair<int, string> selectSize;

		public ProductView()
		{
			lblArticle = new Label {
				HeightRequest = Utils.GetSize(23),
				FontSize = Utils.GetSize(11),
				VerticalTextAlignment = TextAlignment.Center,
				Margin = new Thickness(8, 0)
			};

			BoxView artLine = new BoxView();
			img = new Image();
			if (Device.OS == TargetPlatform.Android)
				img.HeightRequest = Utils.GetSize(300, 1);
			else
				img.HeightRequest = Utils.GetSize(300);

			ImageClick = new TapGestureRecognizer();
			img.GestureRecognizers.Add(ImageClick);
			imgSklad = new Image {
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Source = Device.OnPlatform("Catalog/sklad_", "Sklad", "Sklad"),
				HeightRequest = Utils.GetSize(18, 1),
				IsVisible = false
			};

			Grid imgGrid = new Grid {
				Padding = new Thickness(8, 0),
			};
			imgGrid.Children.Add(img, 0, 0);
			imgGrid.Children.Add(imgSklad, 0, 0);

			lblName = new Label { HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(8, 8) };

			lblPriceOld = new MyLabel {
				FontSize = Utils.GetSize(17),
				TextColor = ApplicationStyle.LabelColor,
				HorizontalOptions = LayoutOptions.Center,
				IsVisible = false,
				IsStrikeThrough = true
			};
			lblPrice = new Label {
				FontSize = Utils.GetSize(18),
				TextColor = ApplicationStyle.GreenColor,
				HorizontalOptions = LayoutOptions.Center,
			};
			StackLayout layoutPrice = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = Utils.GetSize(10),
				Children = {
					lblPriceOld,
					lblPrice
				}
			};

			InitSizes();

			btnTableSize = new MyButton {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				BackgroundColor = Color.Transparent,
				HeightRequest = Utils.GetSize(25),
				Text = "Таблица размеров",
				FontSize = Utils.GetSize(9),
				TextColor = ApplicationStyle.GreenColor,
				IsUnderline = true
			};
			btnTableSize.Clicked += (sender, e) => { EventTableSizeClick(sender, e); };

			lblDescription = new MyLabel {
				LineSpacing = 1.5f
			};

			btnOrder = new Button {
				Text = "В корзину",
				FontSize = Utils.GetSize(15),
				TextColor = Color.White,
				BackgroundColor = ApplicationStyle.RedColor,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BorderRadius = 0,
			};
			btnOrder.Clicked += BasketButtonClick;

			SetButtom();

			StackLayout layoutBody = new StackLayout {
				Spacing = 5,
				Padding = new Thickness(8, 5),
				Children = {
					btnTableSize,
					lblDescription
				}
			};

			Image imgClock = new Image {
				Source = Device.OnPlatform("Catalog/clock_", "clock", "clock"),
				HeightRequest = Utils.GetSize(18, 1),
			};
			MyLabel lblTitleTimer = new MyLabel {
				TextColor = ApplicationStyle.GreenColor,
				Text = "Расписание доступности товара:",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Utils.GetSize(14),
				IsUnderline = true,
			};
			layoutTitleTimer = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness(8, 0),
				Children = {
					imgClock,
					lblTitleTimer
				}
			};
			TapGestureRecognizer tapLayoutTitle = new TapGestureRecognizer();
			tapLayoutTitle.Tapped += OnSelectTimer;
			layoutTitleTimer.GestureRecognizers.Add(tapLayoutTitle);

			gridTime = new Grid {
				Padding = new Thickness(8, 0),
				ColumnSpacing = Utils.GetSize(30),
				RowSpacing = 0,
				IsVisible = false,
				ColumnDefinitions =
				{
					new ColumnDefinition { Width =GridLength.Auto },
					new ColumnDefinition { Width =GridLength.Auto },
				}
			};
			lblTime = new Label();

			layoutContent = new StackLayout {
				Padding = new Thickness(0, 0, 0, 8),
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0,
				Children = {
					lblArticle,
					artLine,
					imgGrid,
					lblName,
					layoutPrice,
					layoutSize,
					layoutBody,
					layoutTitleTimer,
					gridTime
				}
			};

			scrollView = new ScrollView {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = layoutContent
			};

			mainGrid = new Grid {
				RowSpacing = 0,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions =
				{
					new RowDefinition { Height = new GridLength(100, GridUnitType.Star) },
					new RowDefinition { Height = Utils.GetSize(50) },

				}
			};
			mainGrid.Children.Add(scrollView, 0, 0);
			mainGrid.Children.Add(layoutBottom, 0, 1);

			Content = mainGrid;

			VerticalOptions = LayoutOptions.FillAndExpand;
		}

		void SetButtom()
		{
			entCount = new MyEntry {
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = ApplicationStyle.GreenColor,
				FontAttributes = FontAttributes.Bold,
				FontSize = Utils.GetSize(18),
				Keyboard = Keyboard.Numeric
			};
			entCount.TextChanged += OnCountChange;

			btnPlus = new Button {
				HeightRequest = Utils.GetSize(49),
				WidthRequest = Utils.GetSize(49),
				BorderRadius = 0,
				TextColor = ApplicationStyle.GreenColor,
				BackgroundColor = ApplicationStyle.LineColor,
				Text = "+"
			};
			btnPlus.Clicked += OnPlusClick;

			btnMinus = new Button {
				HeightRequest = Utils.GetSize(49),
				WidthRequest = Utils.GetSize(49),
				BorderRadius = 0,
				TextColor = ApplicationStyle.GreenColor,
				BackgroundColor = ApplicationStyle.LineColor,
				Text = "-",
			};
			btnMinus.Clicked += OnMinusClick;

			StackLayout layoutButtomCount = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Children =  {
					btnPlus,
					entCount,
					btnMinus,
				}
			};
			Grid gridBottom = new Grid {

				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = Utils.GetSize(49),
				ColumnSpacing = 0,
				ColumnDefinitions =  {
					new ColumnDefinition { Width = new GridLength (50, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength (50, GridUnitType.Star) },
				},
				BackgroundColor = Color.White,
			};
			gridBottom.Children.Add(layoutButtomCount, 0, 0);
			gridBottom.Children.Add(btnOrder, 1, 0);

			layoutBottom = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0,
				Children =  {
					new BoxView (),
					gridBottom
				}
			};
		}

		public static int GetProductCount(Product product, int selectSizeId, Dictionary<int, string> sizeDictionary)
		{
			if (product.productsAttributes.Count > 0 && selectSizeId == -1) {
				//OnePage.mainPage.DisplayMessage (MessageSize);
				return -1;
			}
			int productCount;
			if (product.productsAttributes.Count > 0) {
				KeyValuePair<int, string> attributeKey = sizeDictionary.SingleOrDefault(g => g.Key == selectSizeId);
				if (attributeKey.Key == 0 && attributeKey.Value == null)
					return -1;
				ProductsAttributes attr = product.productsAttributes.SingleOrDefault(g => g.OptionValue.ID == attributeKey.Key);
				productCount = attr.Quantity;
			} else
				productCount = product.Quantity;

			return productCount;
		}

		void InitSizes()
		{
			layoutScrollSize = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

			scrollSizes = new MyScrollView {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = ScrollOrientation.Horizontal,
			};
			scrollSizes.Content = layoutScrollSize;

			relativeLayoutSize = new RelativeLayout() {
				HeightRequest = 43
			};

			relativeLayoutSize.Children.Add(scrollSizes,
			   	Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent((parent) => {
					return parent.Height;
				})
			   );

			relativeLayoutSize.Children.Add(
				new Image {
					HorizontalOptions = LayoutOptions.Start,
					Source = Device.OnPlatform("Catalog/Sizesfon_left_", "Sizesfon_left_", "Sizesfon_left_"),
					HeightRequest = Utils.GetSize(43)
				},
				Constraint.Constant(0),
				Constraint.Constant(0)
			);

			relativeLayoutSize.Children.Add(
				new Image {
					HorizontalOptions = LayoutOptions.EndAndExpand,
					Source = Device.OnPlatform("Catalog/Sizesfon_right_", "Sizesfon_right_", "Sizesfon_right_"),
					HeightRequest = Utils.GetSize(43)
				},
				Constraint.RelativeToParent((parent) => {
					return parent.Width - 40;
				}),
				Constraint.Constant(0)
			);

			lblSize = new Label {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				VerticalTextAlignment = TextAlignment.Center,
				FontSize = 14,
				Text = "Нет размеров",
			};

			layoutSize = new StackLayout {
				Padding = new Thickness(0, 8, 0, 0),
				Spacing = 0,
				HeightRequest = Utils.GetSize(45),
				Children = {
					new BoxView { HeightRequest = 1.5 },
					relativeLayoutSize,
					//gridLayoutSize,
					lblSize,
					new BoxView { HeightRequest = 1.5 },
				}
			};
		}

		void SetSize()
		{
			layoutScrollSize.Children.Clear();
			foreach (KeyValuePair<int, string> entity in SizeDictionary) {
				Size s = TextMeter.MeasureTextSize(entity.Value, Utils.GetSize(20), "MyriadProRegular.ttf");
				Button btn = new Button {
					Text = entity.Value,
					CommandParameter = entity.Key,
				};
				btn.WidthRequest = s.Width + btn.BorderWidth * 2 + Utils.GetSize(10);
				if (oldBasket?.SizeValueId == entity.Key) {
					btn.BackgroundColor = ApplicationStyle.GreenColor;
					btn.TextColor = Color.White;
					btnPred = btn;
					selectSize = entity;
				}
				btn.Style = ApplicationStyle.ButtonSizeStyle;
				btn.Clicked += OnSelectSize;
				layoutScrollSize.Children.Add(btn);
			}

			if (SizeDictionary.Count == 1) {
				selectSize = SizeDictionary.Single();
				layoutScrollSize.Children[0].BackgroundColor = ApplicationStyle.GreenColor;
				((Button)layoutScrollSize.Children[0]).TextColor = Color.White;
			} else {
				layoutScrollSize.Children.Insert(0, new Button { Style = ApplicationStyle.ButtonSizeEndStyle });
				layoutScrollSize.Children.Add(new Button { Style = ApplicationStyle.ButtonSizeEndStyle });
				scrollSizes.ScrollToAsync(20, 0, false);
			}
		}

		public void ShowProduct(Basket basket, Product product)
		{
			oldBasket = basket;
			Show(product);
			btnOrder.Text = "Изменить";
			OnePage.topView.HideLeftPanel();
		}

		public void ShowProduct(Product product)
		{
			oldBasket = null;
			Show(product);
		}

		void Show(Product product)
		{
			img.Source = null;
			isInitialization = true;
			ProductItem = product;
			ImageClick.CommandParameter = ProductItem;

			selectSize = new KeyValuePair<int, string>();
			btnOrder.Text = "В корзину";
			btnOrder.BackgroundColor = ApplicationStyle.RedColor;
			isProducToOrder = false;

			lblArticle.Text = "Артикул " + product.Article;
			lblName.Text = product.productsDescription.Name;
			lblDescription.Text = product.productsDescription.Description;
			lblPrice.Text = Math.Ceiling(product.Price).ToString("F0") + " р.";
			if (product.PriceOld != 0 && product.PriceOld > product.Price) {
				lblPriceOld.Text = Math.Ceiling(product.PriceOld) + "р.";
				lblPriceOld.IsVisible = true;
			} else
				lblPriceOld.IsVisible = false;

			img.Source = new UriImageSource {
				Uri = new Uri(Constants.PathToImage + product.Image),
				CachingEnabled = true,
				CacheValidity = new TimeSpan(60, 0, 0, 0)
			};
			imgSklad.IsVisible = product.Express;

			gridTime.IsVisible = false;
			if (product.SchedulesList != null && product.SchedulesList.Count > 0) {
				layoutTitleTimer.IsVisible = true;
				messageTimer = Utils.GetProductAvailabilitySchedule(product.SchedulesList, gridTime);
				lblTime.Text = messageTimer;
				messageTimer = "Данный товар доступен к оформлению в указанный ниже период. Он будет находиться в корзине и Вы сможете его оформить в доступное для этого время.\n" +
				"График доступности товара:\n" + messageTimer;
			} else {
				layoutTitleTimer.IsVisible = false;
			}

			SizeDictionary.Clear();
			for (int i = 0; i < product.productsAttributes.Count; i++) {
				if (product.productsAttributes[i].Quantity > 0)
					SizeDictionary.Add(product.productsAttributes[i].OptionValue.ID, product.productsAttributes[i].OptionValue.Value);
			}

			if (ProductItem.productsAttributes.Count > 0 && SizeDictionary.Count == 0) {
				entCount.IsEnabled = false;
				btnPlus.IsEnabled = false;
				btnMinus.IsEnabled = false;
			} else {
				entCount.IsEnabled = true;
				btnPlus.IsEnabled = true;
				btnMinus.IsEnabled = true;
			}
			SetSize();

			if (oldBasket != null)
				entCount.Text = oldBasket.Quantity.ToString();
			else
				entCount.Text = product.ProductsQuantityOrderMin.ToString();

			BtnIncrementEnabled(product.ProductsQuantityOrderMin);

			if (SizeDictionary.Count > 0) {
				btnTableSize.IsVisible = true;
				btnOrder.IsEnabled = true;
				relativeLayoutSize.IsVisible = true;
				lblSize.IsVisible = false;
				layoutSize.IsVisible = true;
			} else {
				if (product.productsAttributes.Count > 0) {
					btnTableSize.IsVisible = true;
					btnOrder.IsEnabled = false;
					lblSize.IsVisible = true;
					layoutSize.IsVisible = true;
				} else {
					btnTableSize.IsVisible = false;
					btnOrder.IsEnabled = true;
					lblSize.IsVisible = false;
					layoutSize.IsVisible = false;
				}
				relativeLayoutSize.IsVisible = false;
			}
			isInitialization = false;
		}

		async void OnSelectTimer(object sender, EventArgs e)
		{
			if (gridTime.IsVisible) {
				await scrollView.ScrollToAsync(0, 0, true);
				gridTime.IsVisible = false;
			} else {
				gridTime.IsVisible = true;
				await Task.Delay(10);
				await scrollView.ScrollToAsync(gridTime, ScrollToPosition.End, true);
			}
		}

		public static void StopTimer()
		{
			if (tmrCountProduct == null) return;
			tmrCountProduct.Stop();
			tmrCountProduct.Dispose();
		}

		public static void SetTimerEditCount(int countOrder, int productsMin, int productsUnits, MyEntry entCount)
		{
			tmrCountProduct = new Timer(2000); // Секунда
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
		void OnCountChange(object sender, TextChangedEventArgs e)
		{
			//SetTimerEditCount
			if (isInitialization) return;

			StopTimer();
			Regex rxNums = new Regex(@"^\d+$");
			// любые цифры
			if (e.NewTextValue != "" && !rxNums.IsMatch(e.NewTextValue)) {
				entCount.Text = e.OldTextValue;
				return;
			}
			if (e.NewTextValue != "")
				countOrder = int.Parse(entCount.Text);

			int maxCountProduct = GetProductCount(ProductItem, selectSize.Key, SizeDictionary);
			if (maxCountProduct == -1) {
				countOrder = ProductItem.ProductsQuantityOrderMin;
				entCount.Text = countOrder.ToString();
				return;
			}
			if (countOrder > maxCountProduct) {
				OnePage.mainPage.DisplayMessage("Максимальное количество " + maxCountProduct + " шт");
				countOrder = maxCountProduct;
				entCount.Text = countOrder.ToString();
			}
			SetTimerEditCount(countOrder, ProductItem.ProductsQuantityOrderMin, ProductItem.ProductsQuantityOrderUnits, entCount);
		}

		void OnPlusClick(object sender, EventArgs e)
		{
			int countP = int.Parse(entCount.Text);
			if (ProductItem.productsAttributes.Count > 0 && selectSize.Key == 0) {
				OnePage.mainPage.DisplayMessage(MessageSize);
			}
			if (countP < GetProductCount(ProductItem, selectSize.Key, SizeDictionary))
				countP += ProductItem.ProductsQuantityOrderUnits;
			entCount.Text = countP.ToString();
			BtnIncrementEnabled(countP);
		}

		void OnMinusClick(object sender, EventArgs e)
		{
			int countP = int.Parse(entCount.Text);
			if (ProductItem.productsAttributes.Count > 0 && selectSize.Key == 0) {
				OnePage.mainPage.DisplayMessage(MessageSize);
				return;
			}
			if (countP > ProductItem.ProductsQuantityOrderMin)
				countP -= ProductItem.ProductsQuantityOrderUnits;
			entCount.Text = countP.ToString();
			BtnIncrementEnabled(countP);
		}

		void BtnIncrementEnabled(int count)
		{
			if (count >= GetProductCount(ProductItem, selectSize.Key, SizeDictionary))
				btnPlus.IsEnabled = false;
			else
				btnPlus.IsEnabled = true;

			if (count <= ProductItem.ProductsQuantityOrderMin)
				btnMinus.IsEnabled = false;
			else
				btnMinus.IsEnabled = true;
		}

		void OnSelectSize(object sender, EventArgs e)
		{
			Button btn = sender as Button;
			int keySize = (int)btn.CommandParameter;
			selectSize = new KeyValuePair<int, string>(keySize, SizeDictionary[keySize]);
			int count = 0;

			btnOrder.BackgroundColor = ApplicationStyle.RedColor;
			isProducToOrder = false;

			if (btnPred != null) {
				btnPred.BackgroundColor = Color.Transparent;
				btnPred.TextColor = ApplicationStyle.LabelColor;
			}
			btn.BackgroundColor = ApplicationStyle.GreenColor;
			btn.TextColor = Color.White;
			btnPred = btn;

			if (oldBasket != null) {
				btnOrder.Text = "Изменить";

				int maxCountProduct;
				if (ProductItem.productsAttributes.Count == 0)
					maxCountProduct = ProductItem.Quantity;
				else
					maxCountProduct = ProductItem.productsAttributes.SingleOrDefault(g => g.OptionValue.ID == selectSize.Key).Quantity;

				if (oldBasket.Quantity > maxCountProduct)
					count = maxCountProduct;
				else
					count = oldBasket.Quantity;

			} else {
				btnOrder.Text = "В корзину";
				count = ProductItem.ProductsQuantityOrderMin;
			}
			entCount.Text = count.ToString();
			BtnIncrementEnabled(count);
		}

		private async void BasketButtonClick(object sender, EventArgs e)
		{
			Content = mainGrid;
			if (isProducToOrder) {
				OnePage.redirectApp.DeleteLastTransition();
				OnePage.redirectApp.RedirectToPage(PageName.Basket, false, true);
				return;
			} else if (ProductItem.SchedulesList?.Count > 0 && !Schedule.IsTimeOrder(ProductItem.SchedulesList))
				OnePage.mainPage.DisplayMessage(messageTimer);

			if (relativeLayoutSize.IsVisible && selectSize.Key == 0) {
				OnePage.mainPage.DisplayMessage(MessageSize);
				return;
			}
			Product product;
			try {
				product = await Product.GetProductsByIDAsync(ProductItem.ProductsID);
			} catch (Exception) {
				eventRefresh = null;
				eventRefresh += (obj, evn) => {
					Button content = sender as Button;
					content.IsEnabled = false;
					BasketButtonClick(sender, e);
				};
				Content = OnePage.mainPage.ShowMessageError(eventRefresh);
				return;
			}
			int maxCountProduct;
			if (product.productsAttributes.Count == 0)
				maxCountProduct = product.Quantity;
			else
				maxCountProduct = product.productsAttributes.SingleOrDefault(g => g.OptionValue.ID == selectSize.Key).Quantity;
			btnOrder.IsEnabled = false;

			await AddToBasket(maxCountProduct);


			btnOrder.Text = "Перейти в корзину";
			btnOrder.BackgroundColor = ApplicationStyle.GreenColor;
			isProducToOrder = true;

			OnePage.topView.RefreshCountProduct();
			entCount.Text = ProductItem.ProductsQuantityOrderMin.ToString();
			btnOrder.IsEnabled = true;
		}

		async Task AddToBasket(int maxCountProduct)
		{
			/// Если пользователь не зарегистрирован или товар из локальной корзины
			if (User.Singleton == null || (oldBasket != null && oldBasket.IsLocalBasket)) {
				BasketDB basketBD = new BasketDB {
					ProductID = ProductItem.ProductsID,
					Article = ProductItem.Article,
					ProductName = ProductItem.productsDescription.Name,
					SizeName = selectSize.Value,
					Image = ProductItem.Image,
					SizeID = selectSize.Key,
					Price = ProductItem.Price,
					Quantity = int.Parse(entCount.Text),
					ProductExpress = ProductItem.Express,
					IsSchedule = ProductItem.SchedulesList.Count > 0
				};
				/// Если мы изменяем размер товара в корзине
				if (oldBasket != null) {
					if (oldBasket.SizeValueId != selectSize.Key || oldBasket.Quantity != basketBD.Quantity) {
						/// Не дает добавить товаров больше чем они есть на складе
						if (basketBD.Quantity > maxCountProduct)
							basketBD.Quantity = maxCountProduct;
						BasketDB.Update(basketBD, oldBasket.ProductID, oldBasket.SizeValueId ?? 0);
					}
					OnePage.redirectApp.BackToHistory();
					return;
				} else {
					BasketDB basketProfile;
					if (basketBD.SizeID == 0)
						basketProfile = BasketDB.GetItemByID(basketBD.ProductID);
					else
						basketProfile = BasketDB.GetItem(basketBD.ProductID, basketBD.SizeID);

					if (basketProfile != null) {
						/// Не дает добавить товаров больше чем они есть на складе
						if (basketBD.Quantity + basketProfile.Quantity > maxCountProduct)
							basketBD.Quantity = maxCountProduct - basketProfile.Quantity;
					}
					BasketDB.AddCount(basketBD);
				}
			} else { /// Если залогинен
				Basket basket = new Basket {
					ProductID = ProductItem.ProductsID,
					SizeValueId = selectSize.Key,
					Quantity = int.Parse(entCount.Text)
				};
				/// Если мы изменяем размер товара в корзине
				if (oldBasket != null) {
					if (oldBasket.SizeValueId != selectSize.Key || oldBasket.Quantity != basket.Quantity) {
						/// Удаляем позицию товара из заказа
						oldBasket.Quantity = -oldBasket.Quantity;
						try {
							await Basket.PushToBasketAsync(oldBasket);
						} catch (Exception) {
							eventRefresh = null;
							eventRefresh += (obj, evn) => {
								Button content = obj as Button;
								content.IsEnabled = false;
								AddToBasket(maxCountProduct).Wait();
							};
							Content = OnePage.mainPage.ShowMessageError(eventRefresh);
							return;
						}
						/// Не дает добавить товаров больше чем они есть на складе
						if (basket.Quantity > maxCountProduct)
							basket.Quantity = maxCountProduct;
						try {
							await Basket.PushToBasketAsync(basket);
						} catch (Exception) {
							eventRefresh = null;
							eventRefresh += (obj, evn) => {
								Button content = obj as Button;
								content.IsEnabled = false;
								AddToBasket(maxCountProduct).Wait();
							};
							Content = OnePage.mainPage.ShowMessageError(eventRefresh);
							return;
						}
					}
					OnePage.redirectApp.BackToHistory();
					return;
				} else { /// Если перешли в товар НЕ из корзины
					List<Basket> basketProfileList;
					try {
						basketProfileList = await Basket.GetProductInBasketAsync();
					} catch (Exception) {
						eventRefresh = null;
						eventRefresh += (obj, evn) => {
							Button content = obj as Button;
							content.IsEnabled = false;
							AddToBasket(maxCountProduct).Wait();
						};
						Content = OnePage.mainPage.ShowMessageError(eventRefresh);
						return;
					}
					Basket basketProfile;
					if (basket.SizeValueId != null) {
						basketProfile = basketProfileList.SingleOrDefault(g => g.ProductID == basket.ProductID && g.SizeValueId == basket.SizeValueId);
					} else
						basketProfile = basketProfileList.SingleOrDefault(g => g.ProductID == basket.ProductID);

					if (basketProfile != null) {
						/// Не дает добавить товаров больше чем они есть на складе
						if (basket.Quantity + basketProfile.Quantity > maxCountProduct)
							basket.Quantity = maxCountProduct - basketProfile.Quantity;
					}
					try {
						await Basket.PushToBasketAsync(basket);
					} catch (Exception) {
						eventRefresh = null;
						eventRefresh += (obj, evn) => {
							Button content = obj as Button;
							content.IsEnabled = false;
							AddToBasket(maxCountProduct).Wait();
						};
						Content = OnePage.mainPage.ShowMessageError(eventRefresh);
						return;
					}
				}
			}
		}
	}
}