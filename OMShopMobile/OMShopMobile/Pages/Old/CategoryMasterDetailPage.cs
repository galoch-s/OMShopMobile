//using System;
//using Xamarin.Forms;
//using FormsGallery;
//using System.Collections.Generic;
//
//namespace OMShopMobile
//{
//	public class CategoryMasterDetailPage : MasterDetailPage
//	{
//		public CategoryMasterDetailPage ()
//		{	
//			Label header = new Label
//			{
//				Text = "Список категорий",
//				Font = Font.BoldSystemFontOfSize(20),
//				HorizontalOptions = LayoutOptions.Center
//			};
//
//			// Assemble an array of NamedColor objects.
//			NamedColor[] namedColors = 
//			{
//				new NamedColor("Aqua", Color.Aqua),
//				new NamedColor("Black", Color.Black),
//				new NamedColor("Blue", Color.Blue),
//				new NamedColor("Fuschia", Color.Fuschia),
//				new NamedColor("Gray", Color.Gray),
//				new NamedColor("Green", Color.Green),
//				new NamedColor("Lime", Color.Lime),
//				new NamedColor("Maroon", Color.Maroon),
//				new NamedColor("Navy", Color.Navy),
//				new NamedColor("Olive", Color.Olive),
//				new NamedColor("Purple", Color.Purple),
//				new NamedColor("Red", Color.Red),
//				new NamedColor("Silver", Color.Silver),
//				new NamedColor("Teal", Color.Teal),
//				new NamedColor("White", Color.White),
//				new NamedColor("Yellow", Color.Yellow)
//			};
//
//			List<Category> categoriesList = Category.GetAllCategories();
//
//			// Create ListView for the master page.
//			ListView listView = new ListView
//			{
//				ItemTemplate = new DataTemplate (typeof(CategoryItemCell)),
//				ItemsSource = categoriesList,
//				RowHeight = 30,
//			};
//			//DataTemplate dataTemplate = new DataTemplate();
//
//
//			// Create the master page with the ListView.
//			this.Master = new ContentPage
//			{
//				Padding = new Thickness (0, 20, 0, 0),
//				Title = header.Text,
//				Content = new StackLayout
//				{
//					Children = 
//					{
//						header, 
//						listView
//					}
//				}
//			};
//
//			// Create the detail page using NamedColorPage and wrap it in a
//			// navigation page to provide a NavigationBar and Toggle button
//			this.Detail = new NavigationPage(new CategoryPage());
//
//			// Define a selected handler for the ListView.
//			listView.ItemSelected += (sender, args) =>
//			{
//				// Set the BindingContext of the detail page.
//				this.Detail.BindingContext = args.SelectedItem;
//
//				// Show the detail page.
//				this.IsPresented = false;
//			};
//
//			// Initialize the ListView selection.
//			listView.SelectedItem = categoriesList[0];
//		}
//
//		//private void 
//	}
//}
//
