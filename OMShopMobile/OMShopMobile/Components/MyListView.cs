using System;
using Xamarin.Forms;

namespace OMShopMobile
{
	public class MyListView : ListView
	{
		public static readonly BindableProperty HasSelectionProperty =
			BindableProperty.Create<MyListView, bool>(p => p.HasSelection, false);
		public bool HasSelection {
			get { return (bool)GetValue(HasSelectionProperty); }
			set { SetValue(HasSelectionProperty, value); }
		}
	}
}

