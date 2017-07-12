using System;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OMShopMobile
{
	public class DatePickerExtension : StackLayout
	{
		public MyDatePicker textBox = new MyDatePicker ();
		public Label lblPlaceHolder = new Label ();
		private BoxView boxView = new BoxView();

		public string Placeholder { 
			get { return textBox.Placeholder; }
			set 
			{ 
				textBox.Placeholder = value;
				if (!string.IsNullOrEmpty(value ))
					lblPlaceHolder.Text = value;
			}
		}

		public Color TextColor { 
			get { return textBox.TextColor; }
			set { textBox.TextColor = value; }
		}

		public TextAlignment HorizontalTextAlignment { 
			get { return textBox.HorizontalTextAlignment; }
			set { textBox.HorizontalTextAlignment = value; }
		}

		public string Format { 
			get { return textBox.Format; }
			set { textBox.Format = value; }
		}

		public static readonly BindableProperty TextProperty =
			BindableProperty.Create<DatePickerExtension, string>(p => p.Text, "");
		public String Text
		{
			get { 
				return (String)GetValue (TextProperty);
			}
			set {
				SetValue(TextProperty, value);
			}
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			Text = textBox.Date.ToString();
		}

		public DatePickerExtension ()
		{
			textBox.HorizontalOptions = LayoutOptions.FillAndExpand;
			textBox.VerticalOptions = LayoutOptions.FillAndExpand;
			textBox.HorizontalTextAlignment = TextAlignment.Start;
			textBox.Focused += (sender, e) => {
				Text = textBox.Date.ToString();
			};

			lblPlaceHolder.VerticalOptions = LayoutOptions.CenterAndExpand;

			TextColor = Color.Black;
			HeightRequest = Utils.GetSize(43);

			boxView.BackgroundColor = ApplicationStyle.SpacingColor;
			boxView.HeightRequest = 1;

			StackLayout layoutEntry = new StackLayout { 
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness (8, 0),
				Children = {
					textBox,
					lblPlaceHolder,
				}
			};
			this.Spacing = 0;
			this.Children.Add (layoutEntry);
			this.Children.Add (boxView);
		}
	}
}

