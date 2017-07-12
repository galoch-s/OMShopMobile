using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class History
	{
		[JsonProperty("current_position")]
		public string CurrentPosition { get; set; }

		[JsonProperty("page")]
		public PageName Page { get; set;}

		[JsonProperty("content_category")]
		public Category ContentCategory { get; set; }

		[JsonIgnore]
		public List<Category> BreadCrumbs { get; set; }

		[JsonProperty("current_product_page")]
		public int CurrentProductPage { get; set; }

		[JsonProperty("product_id")]
		public int ProductID { get; set; }

		[JsonProperty("table_size_id")]
		public int TableSizeId { get; set; }

		[JsonProperty("image_source")]
		public string ImageSource { get; set; }

		[JsonProperty("is_redirect_to_back")]
		public bool IsRedirectToBack { get; set; }

		[JsonProperty("is_narrow_title")]
		public bool IsNarrowTitle { get; set; }

		[JsonProperty("step")]
		public HistoryStep Step { get; set; }

		public History(PageName page, string currentPosition) : this(page, currentPosition, false, false, null, null)
		{
		}

		public History(PageName page, string currentPosition, bool isRedirectToBack, bool isNarrowTitle, Category category, List<Category> breadCrumbs)
		{
			Page = page;
			CurrentPosition = currentPosition;
			IsRedirectToBack = isRedirectToBack;
			IsNarrowTitle = isNarrowTitle;
			ContentCategory = category;
			BreadCrumbs = breadCrumbs;
		}

		public History(PageName page, string currentPosition, HistoryStep historyStep)
		{
			Page = page;
			CurrentPosition = currentPosition;
			Step = historyStep;
		}

		public History(PageName page, int productID, string currentPosition)
		{
			Page = page;
			ProductID = productID;
			CurrentPosition = currentPosition;
		}

		//public History(PageName page, string imageSource, string currentPosition)
		//{
		//	Page = page;
		//	ImageSource = imageSource;
		//	CurrentPosition = currentPosition;
		//}

		public bool HistoryEquals(History history)
		{
			return CurrentPosition == history.CurrentPosition &&
				Page == history.Page &&
				ContentCategory == history.ContentCategory &&
				BreadCrumbs == history.BreadCrumbs &&
				ProductID == history.ProductID &&
				TableSizeId == history.TableSizeId &&
				ImageSource == history.ImageSource &&
				IsRedirectToBack == history.IsRedirectToBack &&
				IsNarrowTitle == history.IsNarrowTitle &&
				Step == history.Step;
		}


		public static string GetHistoryToJson(List<History> historyList)
		{	
			return JsonConvert.SerializeObject(historyList, Formatting.None,
							  new JsonSerializerSettings { ContractResolver = new ExcludeContentKeyContractResolver() });
		}

		public class ExcludeContentKeyContractResolver : DefaultContractResolver
		{
			protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
			{
				IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
				return properties.Where(p => p.PropertyName != "childrenCategoriesDesc" && 
				                        p.PropertyName != "categoriesDescription" &&
				                        p.PropertyName != "categories_image" &&
				                        p.PropertyName != "parent_id" &&
				                        p.PropertyName != "sort_order" &&
				                        p.PropertyName != "categories_status"
				                       ).ToList();
			}
		}

	}

//	public class UserHistory : StackLayout
//	{
//		private List<History> History { get; set;}
//		private Label lbPosition;
//
//		public TapGestureRecognizer ClickPred;
//
//		public UserHistory()
//		{
//			Style labelStyle = new Style (typeof(Label)) 
//			{
//				Setters = {
//					new Setter { Property = Label.VerticalOptionsProperty, Value = LayoutOptions.Center },
//					new Setter { Property = Label.FontSizeProperty, Value = (int)(16 * App.ScreenScale) }
//				}
//			};
//			HeightRequest = (int)(40 * App.ScreenScale);
//			ClickPred = new TapGestureRecognizer();
//			Label lblIconPred = new Label { 
//				Text = "\u2190", 
//				TextColor = Color.Black, 
//				FontAttributes = FontAttributes.Bold,
//				Style = labelStyle
//			};
//			lblIconPred.GestureRecognizers.Add (ClickPred);
//			Label lblTextPred = new Label { 
//				Text = "Назад", 
//				TextColor = Color.Black,
//				Style = labelStyle
//			};
//			lblTextPred.GestureRecognizers.Add (ClickPred);
//
//			History = new List<History> ();
//			Spacing = 1;
//			Orientation = StackOrientation.Horizontal;
//			Children.Add (lblIconPred);
//			Children.Add (lblTextPred);
//			lbPosition = new Label {
//				TextColor = Color.Black,
//				HorizontalOptions = LayoutOptions.CenterAndExpand,
//				Style = labelStyle
//			};
//			Children.Add (lbPosition);
//		}
//
//		public void AddTransition(GridBtnLayuot bntItem, string currentPosition, bool isRedirectToBack, Category category, List<Category> breadCrumbs)
//		{
//			lbPosition.Text = currentPosition;
//			History.Add (new History (bntItem, currentPosition, isRedirectToBack, category, breadCrumbs));
//		}
//
//		public void AddTransition(GridBtnLayuot bntItem, string currentPosition, bool isRedirectToBack = false)
//		{
//			this.AddTransition (bntItem, currentPosition, isRedirectToBack, null, null);
//		}
//
//		public void AddTransition(GridBtnLayuot bntItem, int productID, string currentPosition)
//		{
//			lbPosition.Text = currentPosition;
//			History.Add (new History (bntItem, productID, currentPosition));
//		}
//
//		public void AddTransition(GridBtnLayuot bntItem, string imageSource, string currentPosition)
//		{
//			lbPosition.Text = currentPosition;
//			History.Add (new History (bntItem, imageSource, currentPosition));
//		}
//
//		public History GetLastTransition()
//		{
//			History.RemoveAt (History.Count - 1);
//			History hist = History [History.Count - 1];
//			lbPosition.Text = hist.CurrentPosition;
//			return hist;
//		}
//
//		public History GetCurrentTransition()
//		{
//			return History [History.Count - 1];
//		}
//
//		public int CountHistory()
//		{
//			return History.Count;
//		}
//	}
}