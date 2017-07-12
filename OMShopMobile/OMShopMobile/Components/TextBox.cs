//using System;
//using Xamarin.Forms;
//using System.Collections.Generic;
//
//namespace OMShopMobile
//{
//	public class TextBox : StackLayout
//	{
//		public IControlExtention textBox;
//
//		public event EventHandler SelectedIndexChanged;
//
//		private BoxView boxView = new BoxView();
//
//
//		public static readonly BindableProperty TextProperty =
//			BindableProperty.Create<MyEntry, string>(p => p.Text, "");
//		public String Text
//		{
//			get { return (String)GetValue (TextProperty); }
//			set { SetValue(TextProperty, value); }
//		}
//		#region Property
//
//		public string Placeholder { 
//			get 
//			{ 
//				return textBox.Placeholder; 
//			}
//			set 
//			{ 
//				textBox.Placeholder = value; 
//			}
//		}
//
//		public DateTime Date {
//			get 
//			{ 
//				if (textBox is MyDatePicker)
//					return ((MyDatePicker)textBox).Date; 
//				return new DateTime();
//			}
//			set 
//			{ 
//				if (textBox is MyDatePicker)
//					((MyDatePicker)textBox).Date = value;
//			}
//		}
//
//		public IList<string> Items
//		{
//			get 
//			{
//				if (textBox is IPickerExtention)
//					return ((IPickerExtention)textBox).Items; 
//				return null;
//			}
//		}
//
//		public int SelectedIndex
//		{
//			get 
//			{
//				if (textBox is IPickerExtention)
//					return ((IPickerExtention)textBox).SelectedIndex;
//				return -1;
//			}
//			set 
//			{
//				if (textBox is IPickerExtention)
//					((IPickerExtention)textBox).SelectedIndex = value;
//			}
//		}
//
//		public string Format
//		{
//			get 
//			{ 
//				if (textBox is MyDatePicker)
//					return ((MyDatePicker)textBox).Format;
//				return null;
//			}
//			set 
//			{ 
//				if (textBox is MyDatePicker)
//					((MyDatePicker)textBox).Format = value;
//			}
//		}
//
//		public bool IsPassword
//		{
//			get 
//			{ 
//				if (textBox is MyEntry)
//					return ((MyEntry)textBox).IsPassword;
//				return false;
//			}
//			set 
//			{ 
//				if (textBox is MyEntry)
//					((MyEntry)textBox).IsPassword = value;
//			}
//		}
//
//		public bool IsEnabledField
//		{
//			get { return textBox.IsEnabled;	}
//			set { textBox.IsEnabled = value; }
//		}
//
//		public Color TextColor { 
//			get { return textBox.TextColor; }
//			set { textBox.TextColor = value; }
//		}
//		#endregion
//		protected override void OnBindingContextChanged ()
//		{
//			base.OnBindingContextChanged ();
//			if (textBox is MyEntry) {
//				((MyEntry)textBox).Text = Text;
//			}
//		}
//
//		void OnSelectIndexChanged (object sender, EventArgs e)
//		{
//			if (textBox is IPickerExtention && SelectedIndexChanged != null)
//				SelectedIndexChanged(sender, e);
//		}
//
//		public TextBox () : this (TypeFieldExtension.Entry)
//		{
//		}
//
//		public TextBox (TypeFieldExtension type)
//		{
//			HeightRequest = 43;
//
//			if (type == TypeFieldExtension.Entry)
//				textBox = new MyEntry ();
//			if (type == TypeFieldExtension.Picker)
//				textBox = new MyPicker ();
//			if (type == TypeFieldExtension.DatePicker)
//				textBox = new MyDatePicker ();
//			
//			textBox.XAlign = TextAlignment.Start;
//			textBox.VerticalOptions = LayoutOptions.FillAndExpand;
//
//			if (textBox is IPickerExtention)
//				((IPickerExtention)textBox).SelectedIndexChanged += OnSelectIndexChanged;
//			if (textBox is MyEntry) {
//				((MyEntry)textBox).TextChanged += OnTextChanged;
//			}
//			if (textBox is MyDatePicker) {
//				((MyDatePicker)textBox).Focused += OnFocused;;
//			}
//
//			boxView.BackgroundColor = Color.FromHex (ApplicationStyle.SpacingColor);
//			boxView.HeightRequest = 1;
//
//			StackLayout layoutEntry = new StackLayout { 
//				VerticalOptions = LayoutOptions.FillAndExpand,
//				Padding = new Thickness (8, 0),
//				Children = {
//					textBox as View
//				}
//			};
//			this.Spacing = 0;
//			this.Children.Add (layoutEntry);
//			this.Children.Add (boxView);
//		}
//
//		void OnFocused (object sender, FocusEventArgs e)
//		{
//			MyDatePicker entry = sender as MyDatePicker;
//			Text = entry.Date.ToString();
//		}
//
//		void OnTextChanged (object sender, TextChangedEventArgs e)
//		{
//			MyEntry entry = sender as MyEntry;
//			SetValue(TextProperty, entry.Text);
//		}
//	}
//}