using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OMShopMobile.CustomControls
{
	public class CustomRadioButton : View
	{	
		/// <summary>
		/// The checked changed event.
		/// </summary>
		public EventHandler<EventArgs<bool>> CheckedChanged;
		/// <summary>
		/// Gets or sets a value indicating whether the control is checked.
		/// </summary>
		/// <value>The checked state.</value>


		public static readonly BindableProperty CheckedProperty =
			BindableProperty.Create<CustomRadioButton, bool>(p => p.Checked, false);
		public bool Checked
		{
			get	{ return this.GetValue<bool>(CheckedProperty); }
			set	{ this.SetValue(CheckedProperty, value);
				var eventHandler = this.CheckedChanged;
				if (eventHandler != null)
				{
					eventHandler.Invoke(this, value);
				}
			}
		}

		/// <summary>
		/// The default text property.
		/// </summary>
		public static readonly BindableProperty TextProperty =
			BindableProperty.Create<CustomRadioButton, string>(
				p => p.Text, string.Empty);
		public string Text
		{
			get	{ return this.GetValue<string>(TextProperty); }
			set	{ this.SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Identifies the TextColor bindable property.
		/// </summary>
		/// 
		/// <remarks/>
		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create<CustomRadioButton, Color>(
				p => p.TextColor, Color.Black);
		public Color TextColor
		{
			get	{ return (Color)base.GetValue(TextColorProperty); }
			set	{ base.SetValue(TextColorProperty, value); }
		}

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create<CustomRadioButton, int>(
				p => p.FontSize, 0);
		public int FontSize
		{
			get	{ return (int)base.GetValue(FontSizeProperty); }
			set	{ base.SetValue(FontSizeProperty, value); }
		}

		public int Id { get; set; }
	}
}