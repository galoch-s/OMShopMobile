using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace OMShopMobile.CustomControls
{
	public class BindableRadioGroup: StackLayout
	{

		public List<CustomRadioButton> rads;

		private Color textColor;

		public BindableRadioGroup()
		{
			rads = new List<CustomRadioButton>();
		}

		public static BindableProperty TextColorProperty =
			BindableProperty.Create<BindableRadioGroup, Color>(o => o.TextColor, Color.Black, BindingMode.TwoWay, propertyChanged:OnTextColorChanged );
		public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create<BindableRadioGroup, IEnumerable>(o => o.ItemsSource, default(IEnumerable), propertyChanged: OnItemsSourceChanged);		
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public static BindableProperty SelectedIndexProperty =
			BindableProperty.Create<BindableRadioGroup, int>(o => o.SelectedIndex, default(int), BindingMode.TwoWay, propertyChanged:OnSelectedIndexChanged );
		public int SelectedIndex
		{
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public event EventHandler<int> CheckedChanged;


		private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
		{
			var radButtons = bindable as BindableRadioGroup;

			radButtons.rads.Clear();
			radButtons.Children.Clear();
			if (newvalue != null)
			{
				int radIndex = 0;
				foreach (var item in newvalue)
				{
					var rad = new CustomRadioButton();
					rad.Text = item.ToString();
					rad.Id = radIndex;  
					rad.TextColor = radButtons.textColor;
					rad.Margin = new Thickness(0);
					rad.HeightRequest = Utils.GetSize(43);
					rad.FontSize = Utils.GetSize(12);

					rad.CheckedChanged += radButtons.OnCheckedChanged;

					radButtons.rads.Add(rad);

					radButtons.Children.Add(rad);
					radButtons.Children.Add (new BoxView ());
					radIndex++;
				}
				if (radButtons.Children [radButtons.Children.Count - 1].GetType () == typeof(BoxView))
					radButtons.Children.RemoveAt (radButtons.Children.Count - 1);
			}
		}

		private void OnCheckedChanged(object sender, EventArgs<bool> e)
		{
			if (e.Value == false) return;

			var selectedRad = sender as CustomRadioButton;

			foreach (var rad in rads)
			{
				if(!selectedRad.Id.Equals(rad.Id))
				{
					rad.Checked = false;
				}
				else
				{
					if(CheckedChanged != null)
						CheckedChanged.Invoke(sender, rad.Id);
				}
			}
		}

		private static void OnSelectedIndexChanged(BindableObject bindable, int oldvalue, int newvalue)
		{
			if (newvalue == -1) return;

			var bindableRadioGroup = bindable as BindableRadioGroup;

			foreach (var rad in bindableRadioGroup.rads)
			{
				if (rad.Id == bindableRadioGroup.SelectedIndex)
				{
					rad.Checked = true;
				}
			}
		}

		private static void OnTextColorChanged(BindableObject bindable, Color oldvalue, Color newvalue)
		{
			if (newvalue == Color.Black) return;

			var bindableRadioGroup = bindable as BindableRadioGroup;
			bindableRadioGroup.textColor = newvalue;

			foreach (var rad in bindableRadioGroup.rads)
			{
				rad.TextColor = newvalue;
			}
		}
	}
}
