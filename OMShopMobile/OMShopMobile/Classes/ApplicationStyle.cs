using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class ApplicationStyle
	{
		public static readonly Color TextColor = Color.FromHex("#333333");
		public static readonly Color PlaceholderColor = Color.FromHex("#C6C6C6");

		public static readonly Color RedColor = Color.FromHex("#E9516D");
		public static readonly Color GreenColor = Color.FromHex("#009F9C");
		public static readonly Color BlueColor = Color.FromHex("#007AFF");
		public static readonly Color YellowColor = Color.FromHex("#FFD517");

		public static readonly Color YellowBasketColor = Color.FromHex("#Fff9c4");
		public static readonly Color GrayEnabledColor = Color.FromHex("#Eceff1");


		public static readonly Color SpacingColor = Color.FromHex("#ECECEC");
		public static readonly Color LineColor = Color.FromHex("#E2E2E2");
		public static readonly Color ButtonColor = Color.FromHex("#F2F1EF");

		public static readonly Color LabelColor = Color.FromHex("#8E8E93");

		public ApplicationStyle ()
		{
			
		}

		public static Style TopLabelStyle = new Style (typeof(Label)) 
		{
			Setters = {
				new Setter { Property = Label.FontSizeProperty, Value = Utils.GetSize(10), },
				new Setter { Property = Button.TextColorProperty, Value = Color.Black },
			}
		};

		public static Style ButtonCountStyle = new Style (typeof(Button))
		{
			Setters = {
				new Setter { Property = Button.BackgroundColorProperty, Value = Color.Transparent },
				new Setter { Property = Button.TextColorProperty, Value = TextColor },
				new Setter { Property = Button.BorderWidthProperty, Value = 1 },
				new Setter { Property = Button.BorderColorProperty, Value = LineColor },
				new Setter { Property = Button.WidthRequestProperty, Value = Utils.GetSize(30) },
				new Setter { Property = Button.HeightRequestProperty, Value = Utils.GetSize(30) },
				new Setter { Property = Button.HorizontalOptionsProperty, Value = LayoutOptions.Center, },
				new Setter { Property = Button.FontSizeProperty, Value = Utils.GetSize(20) }
			}
		};

		public static Style ButtonSizeStyle = new Style (typeof(Button))
		{
			Setters = {
				new Setter { Property = Button.BackgroundColorProperty, Value = Color.Transparent },
				new Setter { Property = Button.TextColorProperty, Value = LabelColor },
				new Setter { Property = Button.BorderWidthProperty, Value = 1 },
				new Setter { Property = Button.BorderColorProperty, Value = LineColor },
				new Setter { Property = Button.HeightRequestProperty, Value = Utils.GetSize(30) },
				new Setter { Property = Button.HorizontalOptionsProperty, Value = LayoutOptions.StartAndExpand, },
				new Setter { Property = Button.FontSizeProperty, Value = Utils.GetSize(20) }
			}
		};

		public static Style ButtonSizeEndStyle = new Style (typeof(Button))
		{
			Setters = {
				new Setter { Property = Button.BackgroundColorProperty, Value = Color.Transparent },
				new Setter { Property = Button.TextColorProperty, Value = Color.Transparent },
//				new Setter { Property = Button.BorderWidthProperty, Value = 1 },
				new Setter { Property = Button.BorderColorProperty, Value = Color.Transparent },
				new Setter { Property = Button.HeightRequestProperty, Value = Utils.GetSize(30) },
				new Setter { Property = Button.WidthRequestProperty, Value = Utils.GetSize(40) },
//				new Setter { Property = Button.HorizontalOptionsProperty, Value = LayoutOptions.StartAndExpand, },
//				new Setter { Property = Button.FontSizeProperty, Value = 20 }
			}
		};
			
		public static void SetGlobalStyle()
		{
			Application.Current.Resources = new ResourceDictionary ();

			Style stackLayoutStyle = new Style (typeof(StackLayout)) {
				Setters = {
//					new Setter { Property = StackLayout.SpacingProperty, Value = 0 },
				}
			};
			Style buttonStyle = new Style (typeof(Button)) {
				Setters = {
					new Setter { Property = Button.FontFamilyProperty, Value = "Myriad Pro" },
					new Setter { Property = Button.BackgroundColorProperty, Value = RedColor },
					new Setter { Property = Button.TextColorProperty, Value = Color.FromHex("#FFF") },
					new Setter { Property = Button.FontSizeProperty, Value = Utils.GetSize(17) },
				}
			};
			Style myLabelStyle = new Style (typeof(MyLabel)) {
				Setters = {
					new Setter { Property = MyLabel.FontFamilyProperty, Value = "Myriad Pro" },
					new Setter { Property = Label.TextColorProperty, Value = LabelColor },
					new Setter { Property = MyLabel.FontSizeProperty, Value = Utils.GetSize(14), }
				}
			};
			Style labelStyle = new Style (typeof(Label)) {
				Setters = {
					new Setter { Property = Label.FontFamilyProperty, Value = "Myriad Pro" },
					new Setter { Property = Label.TextColorProperty, Value = LabelColor },
					new Setter { Property = Label.FontSizeProperty, Value = Utils.GetSize(14), }
				}
			};
			Style pickerStyle = new Style (typeof(MyPicker)) {
				Setters = {
					new Setter { Property = MyPicker.TextColorProperty, Value = TextColor },
					new Setter { Property = MyPicker.HeightRequestProperty, Value = Utils.GetSize(14) },
					new Setter { Property = MyPicker.FontSizeProperty, Value = Utils.GetSize(14) }
				}
			};
			Style entryStyle = new Style (typeof(Entry)) {
				Setters = {
					new Setter { Property = Entry.FontFamilyProperty, Value = "Myriad Pro" },
					new Setter { Property = Entry.TextColorProperty, Value = TextColor },
					new Setter { Property = Entry.PlaceholderColorProperty, Value = PlaceholderColor },
					new Setter { Property = Entry.FontSizeProperty, Value = Utils.GetSize(14) }
				}
			};
			Style myEntryStyle = new Style (typeof(MyEntry)) {
				Setters = {
					new Setter { Property = MyEntry.FontFamilyProperty, Value = "Myriad Pro" },
					new Setter { Property = MyEntry.TextColorProperty, Value = TextColor },
					new Setter { Property = MyEntry.PlaceholderColorProperty, Value = PlaceholderColor },
					new Setter { Property = MyEntry.FontSizeProperty, Value = Utils.GetSize(14) }
				}
			};
			Style listViewStyle = new Style (typeof(ListView)) {
				Setters = {
					new Setter { Property = ListView.RowHeightProperty, Value = Constants.HeightRowListView },
					new Setter { Property = ListView.SeparatorColorProperty, Value = LineColor },
				}
			};
			Style myDatePickerStyle = new Style (typeof(MyDatePicker)) {
				Setters = {
					new Setter { Property = MyDatePicker.TextColorProperty, Value = TextColor },
					new Setter { Property = MyDatePicker.PlaceholderColorProperty, Value = PlaceholderColor },
					new Setter { Property = MyDatePicker.FontSizeProperty, Value = Utils.GetSize(14) },
				}
			};

			Style searchBarStyle = new Style (typeof(SearchBar)) {
				Setters = {
					new Setter { Property = SearchBar.FontFamilyProperty, Value = "Myriad Pro" },
					new Setter { Property = SearchBar.FontSizeProperty, Value = Utils.GetSize(14) },
					new Setter { Property = SearchBar.TextColorProperty, Value = TextColor },

					new Setter { Property = SearchBar.BackgroundColorProperty, Value = ApplicationStyle.ButtonColor },
					new Setter { Property = SearchBar.CancelButtonColorProperty, Value = ApplicationStyle.GreenColor },
				}
			};

			Style mySearchBarStyle = new Style (typeof(MySearchBar)) {
				Setters = {
					new Setter { Property = MySearchBar.FontFamilyProperty, Value = "Myriad Pro" },
					new Setter { Property = MySearchBar.FontSizeProperty, Value = Utils.GetSize(14) },
					new Setter { Property = MySearchBar.TextColorProperty, Value = TextColor },

					new Setter { Property = MySearchBar.BackgroundColorProperty, Value = ApplicationStyle.ButtonColor },
					new Setter { Property = MySearchBar.CancelButtonColorProperty, Value = ApplicationStyle.GreenColor },
				}
			};

			Style boxViewStyle = new Style (typeof(BoxView)) {
				Setters = {
					new Setter { Property = BoxView.BackgroundColorProperty, Value = LineColor },
					new Setter { Property = BoxView.HeightRequestProperty, Value = Utils.GetSize(0.6) },//Math.Round ((0.5 * App.ScaleHeight / 1.5), 2) },
				}
			};

			Style textCellStyle = new Style (typeof(TextCell)) {
				Setters = {
					new Setter { Property = TextCell.TextColorProperty, Value = ApplicationStyle.TextColor, },

				}
			};

			// no Key specified, becomes an implicit style for ALL boxviews
			Application.Current.Resources.Add (stackLayoutStyle);
			Application.Current.Resources.Add (buttonStyle);
			Application.Current.Resources.Add (labelStyle);
			Application.Current.Resources.Add (myLabelStyle);
			Application.Current.Resources.Add (pickerStyle);
			Application.Current.Resources.Add (entryStyle);
			Application.Current.Resources.Add (myEntryStyle);
			Application.Current.Resources.Add (listViewStyle);
			Application.Current.Resources.Add (myDatePickerStyle);
			Application.Current.Resources.Add (searchBarStyle);
			Application.Current.Resources.Add (mySearchBarStyle);
			Application.Current.Resources.Add (boxViewStyle);
			Application.Current.Resources.Add (textCellStyle);
		}
	}
}

