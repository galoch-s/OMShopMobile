using System;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OMShopMobile
{
	public class EntryExtension : StackLayout
	{
		public MyEntry textBox = new MyEntry ();
		public Label lblPlaceHolder = new Label ();
		public event EventHandler TextFocuced;
		public event EventHandler TextUnFocuced;
		private BoxView boxView = new BoxView();
		private string text;


		public string Placeholder { 
			get { return textBox.Placeholder; }
			set 
			{ 
				textBox.Placeholder = value;
				lblPlaceHolder.Text = value;
			}
		}

		public Color TextColor { 
			get { return textBox.TextColor; }
			set { textBox.TextColor = value; }
		}

//		public TextAlignment XAlign { 
//			get { return textBox.XAlign; }
//			set { textBox.XAlign = value; }
//		}

//		public static readonly BindableProperty TextProperty =
//			BindableProperty.Create<EntryExtension, string>(p => p.Text, "");
//		public String Text
//		{
//			get { 
//				return (String)GetValue (TextProperty);
//			}
//			set {
//				SetValue(TextProperty, value);
//			}
//		}

		public bool IsPassword { 
			get { return textBox.IsPassword; }
			set { textBox.IsPassword = value; }
		}

		public Keyboard Keyboard { 
			get { return textBox.Keyboard; }
			set { textBox.Keyboard = value; }
		}

		protected override void OnBindingContextChanged ()
		{
			text = textBox.Text;
			base.OnBindingContextChanged ();
			if (text != null)
				textBox.Text = text;
		}

		public EntryExtension ()
		{
			textBox.HorizontalOptions = LayoutOptions.FillAndExpand;
			textBox.VerticalOptions = LayoutOptions.Center;
			textBox.HorizontalTextAlignment = TextAlignment.Start;
			textBox.BackgroundColor = Color.Transparent;


			textBox.Focused += (sender, e) => {
				if (TextFocuced != null)
					TextFocuced(sender, null);
			};
			textBox.Unfocused += (sender, e) => {
				if (TextUnFocuced != null)
					TextUnFocuced(sender, null);
			};
//				OnePage.mainPage.HeightRequest = 300;
//				((View)(((View)this.Parent).Parent)).HeightRequest = 300;



			lblPlaceHolder.VerticalOptions = LayoutOptions.CenterAndExpand;

			TextColor = Color.Black;
			HeightRequest = Utils.GetSize(43);
			Orientation = StackOrientation.Vertical;

			boxView.BackgroundColor = ApplicationStyle.LineColor;
//			boxView.HeightRequest = 1;

			StackLayout layoutEntry = new StackLayout { 
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness (8, 0),
				Children = {
					textBox,
					lblPlaceHolder,
				}
			};
//			this.VerticalOptions = LayoutOptions.StartAndExpand;
			this.Spacing = 0;
			this.Children.Add (layoutEntry);
			this.Children.Add (boxView);

			TapGestureRecognizer tapLayout = new TapGestureRecognizer();
			tapLayout.Tapped += OnTapLaoyut;
			this.GestureRecognizers.Add(tapLayout);
		}

		void OnTapLaoyut(object sender, EventArgs e)
		{
			textBox.Focus();
		}
	}
}

